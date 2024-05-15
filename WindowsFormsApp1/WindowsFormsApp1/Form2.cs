using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
  
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            this.Focus();
            this.FormBorderStyle = FormBorderStyle.None; // кнопки вверху формы
        }

        private void button1_Click(object sender, EventArgs e)
        {   
            this.Close();
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
 
                
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Вы действительно хотите закрыть игру?", "Подтверждение удаления", MessageBoxButtons.OKCancel);

            if (res == DialogResult.OK)
            {
                Application.Exit();
            }
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
           
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
