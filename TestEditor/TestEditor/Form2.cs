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

namespace TestEditor
{
    //форма загрузки теста
    public partial class Form2 : Form
    {
        // System.Xml.XmlReader xmlThemeRead;
        DirectoryInfo testsDirectory = new DirectoryInfo("Tests"); //Создаем объект сообтветствующий папке Tests
        public Form2()
        {
            InitializeComponent();
            comboBox1.Items.AddRange(testsDirectory.GetDirectories()); //Добавление подпапок из директории Tests
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)//Заполнение listBox1 при выборе пункта comboBox
        {

            DirectoryInfo testsDir = new DirectoryInfo("Tests\\" + comboBox1.Text); //Создем объект соответствующий выбраной папке
            listBox1.Items.Clear(); //Очищаем listBox1

            foreach (FileInfo file in testsDir.GetFiles())
            {
                listBox1.Items.Add(Path.GetFileNameWithoutExtension(file.FullName));
            }

            //button1.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)//загрузка теста
        {
            string xmlPath = "Tests\\" + comboBox1.Text + "\\" + listBox1.Text + ".xml"; //Сохраняем путь к xml - файлу

            Editor exam = new Editor(xmlPath); //Передаем во 2ую форму путь к тесту 
            bool bFound = false;
            foreach (Form f in Application.OpenForms)
                bFound = f.Name == "exam";
            if (bFound)
            {
                exam.Show();
            }
            this.Visible = false; //Скрываем первую форму

        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
