using Al_Ain_DataEntry.DAL;
using Al_Ain_DataEntry.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Al_Ain_DataEntry
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        public static string UserName;
        private void btnlogin_Click(object sender, EventArgs e)
        {
            Users users = new Users();
            Dal dal = new Dal();
            users.UserId = textusername.Text;
            UserName = textusername.Text;
            users.Password = textPassword.Text;
            string result = dal.ValidateUser(users);
            if (result != null)
            {
                DataEntry myForm = new DataEntry();
                this.Hide();
                myForm.ShowDialog();
                this.Close();

            }
            else
            {
                MessageBox.Show("Invalid User Name or Password !");
                textusername.Text = "";
                textPassword.Text = "";
            }
        }
    }
}
