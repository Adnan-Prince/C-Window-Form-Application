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
    public partial class FrmBackUpReport : Form
    {
        Dal dal = new Dal();
        public FrmBackUpReport()
        {
            InitializeComponent();
            
            GetChequeTypes();
            com_cheqtype.SelectedIndex = -1;
        }

        private void btn_find_Click(object sender, EventArgs e)
        {
            
            string cheque = com_cheqtype.Text;
            string duedate;
            if (checkBox1.Checked)
            {
                duedate = dateTimePicker1.Value.ToString();
            }
            else
            {
                duedate = "";
            }
            string beneAccount = text_accnt.Text.ToString().Trim(' ');
            string chequenumber = text_cheqno.Text.ToString().Trim(' ');

            Backuplistofcheques RC = new Backuplistofcheques();
            try
            {

                DataTable dt = dal.DalGenerateReportBackup(beneAccount, cheque, chequenumber, duedate);
                RC.SetDataSource(dt);
                RC.SetParameterValue("userName", Form1.UserName);
                crystalReportViewer1.ReportSource = RC;
                crystalReportViewer1.Refresh();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void GetChequeTypes()
        {
            List<ChequeTypes> lst_ = new List<ChequeTypes>();
            List<string> lst_cheques = new List<string>();
            lst_ = dal.DALGetChequeTypes();
            if (lst_ != null)
            {
                foreach (var item in lst_)
                {
                    lst_cheques.Add(item.ChequeType);
                }
                com_cheqtype.DataSource = lst_cheques;
            }
            else
            {
                MessageBox.Show("Please Fill the Cheque Type");
            }
        }

        private void btn_back_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            this.Hide();
            form2.ShowDialog();
            this.Close();
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
    }
}
