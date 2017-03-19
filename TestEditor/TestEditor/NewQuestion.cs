using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace TestEditor
{
    //добавление нового вопроса в редакторе
    public partial class NewQuestion : Form
    {
        int y = 0;//координата техт бокса
        int i = 2;//переменная для нового имени нового текстбокса
        List<TextBox> ans=new List<TextBox>();//список текстбоксов с ответами
        public NewQuestion()
        {
            InitializeComponent();
            y = button2.Bottom + 4;
        }

        public string Name2//вопрос
        {
            get { return textBox1.Text; }
        }
        public string PicPath//изображение 
        {
            get { return label2.Text; }
        }
        public List<TextBox> Ans//список ответов
        {
            get { return ans; }
        }
        public int Right2//правильный ответ
        {
            get { return rig; }
        }
        //добавляем картинку
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = @"D:\ШАГ\Project\WinForm\Exam\TestEditor\TestEditor\bin\Debug";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                label2.Text = ofd.FileName;               
            }

        }
       //добавляем новый текст бокс для добавления варианта ответа
        private void button2_Click(object sender, EventArgs e)
        {
            TextBox TB = new TextBox();
            TB.Location = new System.Drawing.Point(16, y);
            TB.Name = "textBox"+i.ToString();
            TB.Size = new System.Drawing.Size(536, 20);
            y += 24;
            i++;
            ans.Add(TB);
            Controls.Add(TB);
        }
        int rig;
        //сохранить 
        private void button4_Click(object sender, EventArgs e)
        {
            //проверка на правильность передачи данных
            try { rig = Convert.ToInt32(textBox2.Text); }
                catch { MessageBox.Show("Неправильный формат правильного варианта ответа");
                return;
                }
            if (textBox2.Text == "")
            {
                
                MessageBox.Show("Заполните поле 'Номер правильного ответа'!!!");
            }
            else
            {
                
                Close();
            }
           
        }

        //отмена добавления
        private void button3_Click(object sender, EventArgs e)
            
        {
            DialogResult = DialogResult.Cancel;
            try { rig = Convert.ToInt32(textBox2.Text); }
            catch { Close(); }
           
        }
    }
}
