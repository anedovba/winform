using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace EXAM_TEST
{
    public partial class Examenator : Form
    {
        string fpath; // путь к файлу теста
        string fname; // файл теста
        XmlReader xmlReader;
        string qwet;     // вопрос

        // варианты ответа
        //string[] answ = new string[3];
        List<string> answ=new List<string>();
        List<test> qwetions = new List<test>();
        string pic;    // путь к файлу иллюстрации
        string sright; // правильный ответ
        int right; // правильный ответ (номер)
        int otv;   // выбранный ответ (номер)
        string sotv;
        int n;     // количество правильных ответов
        int nv;    // количество пройденых вопросов
        int mode;  // состояние программы:
        int countAns;//кол-во ответов
        int countQ;//общее  кол-во вопросов
        int timeM;// minutes
        int timeS;// seconds
        string ro;//номер правильного ответа в строке
        test q;
        public Examenator(string TestPath)
        {
            InitializeComponent();

            timer1.Interval = 1000;
            
            groupBox1.Visible = false;//скрываем таймер
            
            fpath = Application.StartupPath + "\\";
            fname =Path.GetFileName(TestPath);
           
            try
            {
              xmlReader = new XmlTextReader(TestPath); //Создаем xmlReader
              //xmlReader = new System.Xml.XmlTextReader(fpath + fname);
              xmlReader.Read();
                mode = 0;
                n = 0;            
                
           
            //загрузить заголовок теста
            this.showHead();
            //загрузить описание теста
          this.showDescription();
          this.getTime();
          
            }
            catch (Exception exc)
            {
                label1.Text = "Ошибка доступа к файлу  " +
                    TestPath;

                MessageBox.Show("Ошибка доступа к файлу.\n" +
                    TestPath + "\n",
                    "Экзаменатор",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                mode = 2;
            }

        }
        private void showHead()
        {
            // ищем узел <head>
            do xmlReader.Read();
            while (xmlReader.Name != "head");

            // считываем заголовок
            xmlReader.Read();

            // вывести название теста в заголовок окна
            this.Text = xmlReader.Value;

            // выходим из узла <head>
            xmlReader.Read();
        }
        // выводит описание теста
        private void showDescription()
        {
            // ищем узел <description>
            do
                xmlReader.Read();
            while (xmlReader.Name != "description");

            // считываем описание теста
            xmlReader.Read();

            // выводим описание теста
            label1.Text = xmlReader.Value;

            // выходим из узла <description>
            xmlReader.Read();

            // ищем узел вопросов <qw>
            do
                xmlReader.Read();
            while (xmlReader.Name != "qw");
            countQ = Convert.ToInt32(xmlReader.GetAttribute("num")); //Читаем атрибут узла <qw> 
            // входим внутрь узла
            xmlReader.Read();
        }
        private void getTime()
        {
           
            XDocument xdoc = XDocument.Load(fpath + fname);
            XElement root = xdoc.Element("test");
            XElement time = root.Element("time");
            label2.Text = time.Element("min").Value;
            label4.Text = time.Element("sec").Value;
        }
       
        // читает вопрос из файла теста
        private Boolean getQw()
        {
            countAns = 0;
           
            // считываем тэг <q>
            xmlReader.Read();

            if (xmlReader.Name == "q")
            {

                // здесь прочитан тэг <q>,
                // атрибут text которого содержит вопрос, а
                // атрибут src - имя файла иллюстрации.
              //  id = Convert.ToInt32(xmlReader.GetAttribute("id"));
                // извлекаем значение атрибутов:
                qwet = xmlReader.GetAttribute("text");
                pic = xmlReader.GetAttribute("src");
                if (!pic.Equals(string.Empty)) pic = fpath + pic;

                // входим внутрь узла
                xmlReader.Read();
                int i = 0;

                // считываем данные узла вопроса <q>
                while (xmlReader.Name != "q")
                {
                    xmlReader.Read();

                    // варианты ответа
                    if (xmlReader.Name == "a")
                    {

                        // запоминаем правильный ответ
                        if (xmlReader.GetAttribute("right") == "yes")
                        {
                            ro = i.ToString();//запоминаем номер ответа и переводим его в строку
                            right = i;
                           // MessageBox.Show(ro);
                           
                        }

                        // считываем вариант ответа
                        xmlReader.Read();
                        if (!(String.IsNullOrEmpty(ro)))//если ответ правильный считан
                        {
                            sright = xmlReader.Value;//запонминаем правильный ответ
                           // MessageBox.Show(sright);
                        }
                        ro = "";
                        answ.Add(xmlReader.Value);

                        // выходим из узла <a>
                        xmlReader.Read();
                        countAns++;
                        i++;
                        
                    }
                    for (int k = 0; k < answ.Count; k++)
                    {
                        string tmp = answ[0];
                        answ.RemoveAt(0);
                        answ.Insert(RND.Next(answ.Count), tmp);
                    }
                    q = new test(qwet, answ);                   
                    //qwetions.Add(q);
                }

                // выходим из узла вопроса <q>
                xmlReader.Read();
          
                return true;
            }
            // если считанный тэг не является
            // тэгом вопроса <q>
            else
                return false;
        }

        RadioButton[] rb;
        int y;
        // выводит вопрос и варианты ответа
        private void showQw()
        {
                 
            rb=new RadioButton[countAns];
            for (int i = 0; i < countAns; i++)
            {
                rb[i] = new RadioButton();
                rb[i].AutoSize = true;

                rb[i].Name = "radioButton" + i.ToString();
                rb[i].Size = new System.Drawing.Size(85, 17);
                rb[i].TabIndex = 3 + i;
                rb[i].TabStop = true;                
                rb[i].UseVisualStyleBackColor = true;
                rb[i].Click += new System.EventHandler(this.radioButton1_Click_1);

            }

            // выводим вопрос
            label1.Text = qwet;

            // иллюстрация
            if (pic.Length != 0)
            {
                try
                {
                    pictureBox1.Image =
                    new Bitmap(pic);

                    pictureBox1.Visible = true;
                    y = pictureBox1.Bottom + 16;
                    rb[0].Top = pictureBox1.Bottom + 16;
                   
                }
                catch
                {
                    if (pictureBox1.Visible)
                        pictureBox1.Visible = false;

                    label1.Text +=
                        "\n\n\nОшибка доступа к файлу " + pic + ".";
                    y = pictureBox1.Bottom + 8;
                    rb[0].Top = pictureBox1.Bottom + 8;
                   
                }
            }
            else
            {
                if (pictureBox1.Visible)
                    pictureBox1.Visible = false;
                y = label1.Bottom;
                rb[0].Top = label1.Bottom;
               
            }
            for (int i = 0; i < countAns; i++) // показать варианты ответа
            {
                y += 24;
                rb[i].Location = new System.Drawing.Point(25, y);
                rb[i].Text = answ[i];
                Controls.Add(rb[i]);
                rb[i].Visible = true;
                
            }

            button1.Enabled = false;
        }

        // щелчок на кнопке выбора ответа
        // функция обрабатывает событие Click
        // компонентов radioButton1 - radioButton3
        private void radioButton1_Click_1(object sender, EventArgs e)
        {
            for (int i = 0; i < rb.Length; i++)
            {
                if ((RadioButton)sender == rb[i])
                 //otv = i;
                 sotv = rb[i].Text;
               
            }         
            button1.Enabled = true;
        }
        


        // выводит оценку 
        private void showLevel()
        {
            // ищем узел <levels>
            do
                xmlReader.Read();
            while (xmlReader.Name != "levels");

            // входим внутрь узла
            xmlReader.Read();

            // читаем данные узла
            while (xmlReader.Name != "levels")
            {
                xmlReader.Read();

                if (xmlReader.Name == "level")
                    // n - кол-во правильных ответов,
                    // проверяем, попадаем ли в категорию
                    if (n >= System.Convert.ToInt32(
                        xmlReader.GetAttribute("score")))
                        break;
            }
            int mark = n * 12 / countQ;
            // выводим оценку
            label1.Text =
                "Тестирование завершено.\n" +
                "Всего вопросов: " + countQ.ToString() + ". " +
                "Вы ответили на: "+nv.ToString()+". \n"+
                "Правильных ответов: " + n.ToString() + ".\n" +
                "Baшa оценка " + mark.ToString()+" баллов.";
            timer1.Enabled=false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {


            timeM = (Convert.ToInt32(label2.Text));
            timeS = (Convert.ToInt32(label4.Text));

            timeS--;
            //label2.Text = timeM.ToString();
            label4.Text = timeS.ToString();
            if (timeS == -1)
            {
                timeM--; timeS = 59;
                label2.Text = timeM.ToString();
                label4.Text = timeS.ToString();
                if (timeM == -1)
                {
                    timer1.Enabled = false; ;
                    groupBox1.Visible = false;
                    this.showLevel();
                    MessageBox.Show("Тест провален. Время вышло!", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    this.Close(); // закрыть окно
                    LoadForm Lf = new LoadForm();
                    Lf.Close();
                    Application.Exit();

                }
            }
        }
       
        Random RND = new Random();
     
        
        private void button1_Click(object sender, EventArgs e)
        {
            
            
           // ShuflleList(qwetions);
            if(answ.Count!=0)answ.Clear();
            if (rb != null)
            {
                for (int i = 0; i < rb.Length; i++)
                {
                    rb[i].Dispose();
                }
            }



            //this.getQw();
            //qwetions.Add(q);
            //for (int k = 0; k < qwetions.Count; k++)
            //{
            //    test tmp = qwetions[0];
            //    qwetions.RemoveAt(0);
            //    qwetions.Insert(RND.Next(qwetions.Count), tmp);
            //}
            //qwetions = qwetions.OrderBy(v => RND.Next()).ToList();



            groupBox1.Visible = true;           
            timer1.Enabled = true;
            switch (mode)
            {
                case 0:        // начало работы программы
                
                     this.getQw();
                     //for (int k = 0; k < qwetions.Count; k++)
                     //{
                     //    test tmp = qwetions[0];
                     //    qwetions.RemoveAt(0);
                     //    qwetions.Insert(RND.Next(qwetions.Count), tmp);
                         
                     //}
                     this.showQw();
                     
                    

                    mode = 1;
                    button1.Enabled = true;
                    
                    break;

                case 1:
                    nv++;
                   
                    // правильный ли ответ выбран
                    if (sotv == sright) n++;
                   // if (otv == right) n++;
                    if (this.getQw()) this.showQw();
                    else
                    {
                        // больше вопросов нет
                       
                        pictureBox1.Visible = false;

                        // обработка и вывод результата
                        this.showLevel();

                        // следующий щелчок на кнопке Ok
                        // закроет окно программы
                        mode = 2;
                    }
                    break;
                case 2:   // завершение работы программы
                    this.Close();
                    Application.Exit();
                    
                    break;
            }
        }

        
    }
}
