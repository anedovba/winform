using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestEditor
{
    public partial class Form1 : Form
    {
        //главная форма
        public Form1()
        {
            InitializeComponent();
        }

        //если нажали выход - закрываем приложение
        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
           
        }
        //если нажали редактор тестов - открываем форму для загрузки теста
        private void button2_Click(object sender, EventArgs e)
        {
            Form2 load = new Form2();
            load.Show();
        }
        //если нажали новый тест - открываем форму создания нового теста 
        private void button1_Click(object sender, EventArgs e)
        {
            CreateTest ct = new CreateTest();
            ct.ShowDialog();
        }
    }
}
