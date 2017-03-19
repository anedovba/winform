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
using System.Xml.Linq;

namespace TestEditor
{
    //новый тест
    public partial class CreateTest : Form
    {
        string fName;//имя файла
        XDocument xdoc;
        int y = 0;//координаты текст бокса с новым ответом
        int i = 7;
        List<TextBox> ans = new List<TextBox>();//
        public CreateTest()
        {
            InitializeComponent();
            y = button2.Bottom + 4;
            T = new Test();
        }
        //класс вопросов
        public class Questions
        {
            public string Quest { set; get; }
            public string Pic { set; get; }
            List<string> answ = new List<string>();
            public List<string> Answ
            {
                get { return answ; }
                set { answ = value; }
            }
            public int Right { set; get; }

        }
        //класс тест хранит вопросы
        public class Test
        {

            List<Questions> questions = new List<Questions>();
            public List<Questions> Question
            {
                get { return questions; }
                set { questions = value; }
            }

        }
        Test T;

        //закрыть без сохранения
        private void button5_Click(object sender, EventArgs e)
        {
            Close();
        }

        //сохранить весь тест
        private void button6_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "")
            {
                MessageBox.Show("Заполните все поля");
                return;
            }
            if (T.Question.Count == 0)
            {
                MessageBox.Show("В тесте нет ни одного вопроса");
                return;
            }
            try
            {
                Convert.ToInt32(textBox3.Text);
                Convert.ToInt32(textBox4.Text);
            }
            catch
            {
                MessageBox.Show("Неверный заполнено поле 'Время прохождения'");
                return;
            }
            //выбираем место для сохранения
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "xml files (*.xml)|*.xml||";
            sfd.InitialDirectory = @"D:\ШАГ\Project\WinForm\Exam\TestEditor\TestEditor\bin\Debug\Tests";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                fName = sfd.FileName;
                File.WriteAllText(fName, "");
            }
            xdoc = new XDocument();
            //создаем структуру
            XElement root = new XElement("test");
            XElement head = new XElement("head");
            head.Value = textBox1.Text;
            XElement description = new XElement("description");
            description.Value = textBox2.Text;
            XElement time = new XElement("time");
            XElement min = new XElement("min");
            min.Value = textBox3.Text;
            XElement sec = new XElement("sec");
            sec.Value = textBox4.Text;
            time.Add(min, sec);
            XElement qw = new XElement("qw");
            foreach (var item in T.Question)
            {
                XElement q = new XElement("q", new XAttribute("text", item.Quest), new XAttribute("src", item.Pic));
                int aa = 0;
                foreach (var i in item.Answ)
                {
                    XElement a;
                    if (aa == item.Right)
                        a = new XElement("a", new XAttribute("right", "yes"));
                    else
                    {
                        a = new XElement("a", new XAttribute("right", "no"));
                    }
                    a.Value = i;
                    q.Add(a);
                    aa++;
                }
                qw.Add(q);
            }
            XAttribute num = new XAttribute("num", T.Question.Count.ToString());
            qw.Add(num);
            root.Add(head, description, time, qw);
            xdoc.Add(root);
            if (fName != null)
                xdoc.Save(fName);
            Close();
        }
        //выбор картинки
        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = @"D:\ШАГ\Project\WinForm\Exam\TestEditor\TestEditor\bin\Debug";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                label7.Text = ofd.FileName;
            }
        }

        //добавить поле для нового ответа
        private void button2_Click(object sender, EventArgs e)
        {
            TextBox TB = new TextBox();
            TB.Location = new System.Drawing.Point(16, y);
            TB.Name = "textBox" + i.ToString();
            TB.Size = new System.Drawing.Size(536, 20);
            y += 24;
            i++;
            ans.Add(TB);
            Controls.Add(TB);
        }

        //Сохранить текущий ответ
        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox6.Text == "" || textBox5.Text == "")
            {
                MessageBox.Show("Заполните все поля");
                return;
            }
            try { Convert.ToInt32(textBox5.Text); }
            catch { MessageBox.Show("Неправильно заполнено поле номер ответа"); return; }
            if (ans.Count < 2) { MessageBox.Show("Недостаточное количество вариантов ответа"); return; }
            if (Convert.ToInt32(textBox5.Text) > ans.Count) { MessageBox.Show("Измените номер правильного ответа"); return; }
            foreach (var item in ans)
            {
                if (item.Text == "")
                {
                    MessageBox.Show("Не заполнен ответ");
                    return;
                }
            }
            Questions Q = new Questions();
            Q.Quest = textBox6.Text;
            foreach (var item in ans)
            {
                Q.Answ.Add(item.Text);
            }
            Q.Right = Convert.ToInt32(textBox5.Text) - 1;
            if (label7.Text == "Картинка не выбрана")
            {
                Q.Pic = "";
            }
            else
            {
                Q.Pic = Path.GetFileName(label7.Text);
            }
            T.Question.Add(Q);
            if (ans != null)
            {
                foreach (TextBox item in ans)
                {
                    item.Dispose();

                }
                ans.Clear();
            }
            y = button2.Bottom + 4;
            i = 7;
            textBox6.Text = "";
            textBox5.Text = "";
            label7.Text = "Картинка не выбрана";
            this.Refresh();
        }
    }
}
