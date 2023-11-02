using PDC_Ajman_Bank.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDC_Ajman_Bank
{
    public partial class Form1 : Form
    {
        public static string UserName;
        public Form1()
        {
            
            this.BackgroundImage = Properties.Resources.logo;
            this.BackgroundImageLayout = ImageLayout.Center;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Users users = new Users();
            Dal dal = new Dal();
            users.UserId = textBox1.Text;
            UserName = textBox1.Text;
            users.Password = textBox2.Text;
            string result = dal.ValidateUser(users);
            if (result != null)
            {
                Logger.WriteLog("User " + UserName + " is logged on.");
                Form2 myForm = new Form2();
                this.Hide();
                myForm.ShowDialog();
                this.Close();
                
            }
            else
            {
                MessageBox.Show("Invalid User Name or Password !");
                textBox1.Text = "";
                textBox2.Text = "";
                Logger.WriteLog("Invalid User Name or Password !");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
