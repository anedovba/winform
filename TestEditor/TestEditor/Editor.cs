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

namespace TestEditor
{
    public partial class Editor : Form
    {

        string fpath; // путь к файлу теста
        string fname; // файл теста
        XDocument xdoc;//новый объект файла теста
        public XDocument Xdoc//свойство для получения доступа к  объекту файла теста
        {
            get { return xdoc; }
            set { xdoc = value; }
        }
        XmlReader xmlReader;
        string qwet;     // вопрос

        // варианты ответа
        //string[] answ = new string[3];
        List<string> answ = new List<string>();//список ответов
        string pic;    // путь к файлу иллюстрации
        int countQ;//количество вопросов
        int right; // правильный ответ (номер)
        //класс для сохранения вопросов
        public class Questions
        {
            public string Quest { set; get; }//сам вопрос
            List<string> answ = new List<string>();//список ответов
            public List<string> Answ
            {
                get { return answ; }
                set { answ = value; }
            }
            public int Right { set; get; }//порядковый номер правильного ответа 

        }
        //класс тест для сохранения всех ответов
        public class Test
        {
            public string Head { set; get; }//название теста
            public string Description { set; get; }//описание теста
            public int Min { set; get; }//время на сдачу
            public int Sec { set; get; }
            List<Questions> questions = new List<Questions>();//список вопросов
            public List<Questions> Question
            {
                get { return questions; }
                set { questions = value; }
            }

        }
        Test T;

        public Editor(string TestPath)
        {
            InitializeComponent();
            label8.Visible = false;
            fpath = Application.StartupPath + "\\";
            fname = TestPath;

            try
            {
                xdoc = XDocument.Load(fpath + fname);//загружаем файл для дальнейшего редавтирования
                xmlReader = new System.Xml.XmlTextReader(fpath + fname);//считываем файл
                xmlReader.Read();
                //загрузить заголовок теста
                this.showHead();
                //загрузить описание теста
                this.showDescription();
                //загружаем время
                this.showTime();
                //получаем вопросы
                this.getQw();
                //показываем вопросы
                this.showQw();
                //создаем объект тест
                T = new Test();
                //показываем форму с загруженными данными
                this.Show();
            }
            catch (Exception exc)
            {
                label1.Text = "Ошибка доступа к файлу" +
                    TestPath;

                MessageBox.Show("Ошибка доступа к файлу.\n" +
                    TestPath + "\n",
                    "Экзаменатор",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                this.Close();
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
            textBox1.Text = xmlReader.Value;

            // выходим из узла <head>
            xmlReader.Read();
        }
        private void showDescription()
        {
            // ищем узел <description>
            do
                xmlReader.Read();
            while (xmlReader.Name != "description");

            // считываем описание теста
            xmlReader.Read();

            // выводим описание теста
            textBox2.Text = xmlReader.Value;

            // выходим из узла <description>
            xmlReader.Read();


        }
        private void showTime()
        {
            // ищем узел <min>
            do
                xmlReader.Read();
            while (xmlReader.Name != "min");

            // считываем описание теста
            xmlReader.Read();

            // выводим описание теста
            textBox3.Text = xmlReader.Value;

            // выходим из узла <min>
            xmlReader.Read();
            // ищем узел <sec>
            do
                xmlReader.Read();
            while (xmlReader.Name != "sec");

            // считываем описание теста
            xmlReader.Read();

            // выводим описание теста
            textBox4.Text = xmlReader.Value;

            // выходим из узла <sec>
            xmlReader.Read();
            // ищем узел вопросов <qw>
            do
                xmlReader.Read();
            while (xmlReader.Name != "qw");

            // входим внутрь узла
            xmlReader.Read();
        }
        private Boolean getQw()
        {
            countQ = 0;
            // считываем тэг <q>
            xmlReader.Read();

            if (xmlReader.Name == "q")
            {

                // здесь прочитан тэг <q>,
                // атрибут text которого содержит вопрос, а
                // атрибут src - имя файла иллюстрации.

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
                        // countQ++;
                        // запоминаем правильный ответ
                        if (xmlReader.GetAttribute("right") == "yes")
                            right = i;

                        // считываем вариант ответа
                        xmlReader.Read();
                        //if (i < countQ) 
                        answ.Add(xmlReader.Value);

                        // выходим из узла <a>
                        xmlReader.Read();
                        countQ++;
                        i++;
                    }
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

        RadioButton[] rb;//содаем массив радиобаттонов для динамического отображения ответов
        int y;//координата для размещения радиобаттонов
        // выводит вопрос и варианты ответа
        private void showQw()
        {
            
            rb = new RadioButton[countQ];
            for (int i = 0; i < countQ; i++)
            {
                rb[i] = new RadioButton();
                rb[i].AutoSize = true;
                rb[i].Name = "radioButton" + i.ToString();
                rb[i].Size = new System.Drawing.Size(85, 17);
                rb[i].TabIndex = 3 + i;
                rb[i].TabStop = true;
                rb[i].UseVisualStyleBackColor = true;
            }

            // выводим вопрос
            textBox5.Text = qwet;

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
                    label8.Text =
                        "\n\n\nОшибка доступа к файлу " + pic + ".";
                    label8.Visible = true;
                    y = pictureBox1.Bottom + 8;
                    rb[0].Top = pictureBox1.Bottom + 8;
                }
            }
            else
            {
                pictureBox1.Visible = false;
                y = textBox5.Bottom + 4;
                rb[0].Top = textBox5.Bottom + 4;
            }
            for (int i = 0; i < countQ; i++) // показать варианты ответа
            {
                y += 24;
                rb[i].Location = new System.Drawing.Point(25, y);
                rb[i].Text = answ[i];
                Controls.Add(rb[i]);
                rb[i].Visible = true;
            }
            rb[right].Checked = true;//правильный ответ чекд
        }
        //закрыть редактор без сохранения
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //сохранить изменения и перейти к след вопросу
        private void button1_Click(object sender, EventArgs e)
        {
            //сохраняем все в объект тест
            T.Head = textBox1.Text;
            T.Description = textBox2.Text;
            T.Min = Convert.ToInt32(textBox3.Text);
            T.Sec = Convert.ToInt32(textBox4.Text);
            Questions Q = new Questions();
            Q.Quest = textBox5.Text;
            foreach (var item in answ)
            {
                Q.Answ.Add(item);
            }
            for (int i = 0; i < rb.Length; i++)
            {
                if (rb[i].Checked == true) right = i;
            }
            Q.Right = right;
            T.Question.Add(Q);
            answ.Clear();
            if (rb != null)
            {
                for (int i = 0; i < rb.Length; i++)
                {
                    rb[i].Dispose();
                }
            }
            if (this.getQw()) this.showQw();
            else
            {
                MessageBox.Show("Вопросов больше нет, все изменения сохранены");
                //сохраняем данные объекта тест в файл
                XElement root = xdoc.Element("test");
                root.Element("head").Value = textBox1.Text;
                root.Element("description").Value = textBox2.Text;
                XElement time = root.Element("time");
                time.Element("min").Value = textBox3.Text;
                time.Element("sec").Value = textBox4.Text;
                XElement qw = root.Element("qw");
                qw.Attribute("num").Value = T.Question.Count.ToString();
                int i = 0;
                foreach (XElement q in qw.Elements("q").ToList())
                {
                    try
                    {
                        q.Attribute("text").Value = T.Question[i].Quest;
                        int aaa = 0;
                        foreach (XElement a in q.Elements("a").ToList())
                        {
                            if (aaa == T.Question[i].Right)
                                a.Attribute("right").Value = "yes";
                            else a.Attribute("right").Value = "no";
                            aaa++;
                        }
                        i++;
                    }
                    catch { }
                    finally { }
                }
                xmlReader.Close();//закрываем чтение что б получить доступ для сохранение файла
                xdoc.Save(fpath + fname);
                this.Close();
            }
        }
        //удалить вопрос
        private void button4_Click(object sender, EventArgs e)
        {
            //считываем файл
            XElement root = xdoc.Element("test");
            XElement qw = root.Element("qw");
            foreach (XElement q in qw.Elements("q").ToList())
            {
                if (q.Attribute("text").Value == textBox5.Text)//ищем вопрос
                {
                    foreach (XElement a in q.Elements("a").ToList())
                    {
                        if (a.Value == answ[0])//проверка что это тот вопрос - совпадает и первый ответ
                        {
                            q.Remove();//удаляем вопрос
                            if (rb != null)//очищаем от радиобатонов
                            {
                                for (int i = 0; i < rb.Length; i++)
                                {
                                    rb[i].Dispose();
                                }
                            }
                            if (this.getQw()) this.showQw();//загружаем следующий вопрос
                            else // если других вопросов нет
                            {
                                MessageBox.Show("Вопросов больше нет");
                                button1_Click(sender, e);//сохраняем все изменения
                            }
                        }
                        return;
                    }
                }
            }
        }

        //добавить новый вопрос
        private void button3_Click(object sender, EventArgs e)
        {
            NewQuestion NQ = new NewQuestion();//новая форма
            if (NQ.ShowDialog() == DialogResult.Cancel) { return; }
            //получаем данные после закрытия новой формы
            string fName = "";
            if (Path.GetFileName(NQ.PicPath) != "Картинка не выбрана")
                fName = Path.GetFileName(NQ.PicPath);
      
            XElement root = xdoc.Element("test");
            XElement qw = root.Element("qw");
            int r = NQ.Right2 - 1;
            int i = 0;
            //создаем новый вопрос
            XElement q = new XElement("q", new XAttribute("text", NQ.Name2), new XAttribute("src", fName));
            //создаем ответы
            XElement[] a = new XElement[NQ.Ans.Count];
            foreach (var item in NQ.Ans)
            {

                if (i == r)
                    a[i] = new XElement("a", new XAttribute("right", "yes"));
                else
                {
                    a[i] = new XElement("a", new XAttribute("right", "no"));

                }
                a[i].Value = item.Text;
                i++;
            }
            //добавляем ответы к вопросу
            for (int j = 0; j < NQ.Ans.Count; j++)
            {
                q.Add("", a[j]);
            }
            //добавляем вопрос к тестам
            qw.Add("", q);
            MessageBox.Show("Новый вопрос добавлен");
            //удаляем форму создания нового вопроса
            NQ.Dispose();
        }
    }
}
