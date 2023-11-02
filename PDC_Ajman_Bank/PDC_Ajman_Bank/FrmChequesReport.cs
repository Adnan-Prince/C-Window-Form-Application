using PDC_Ajman_Bank.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDC_Ajman_Bank
{
    public partial class FrmChequesReport : Form
    {
        public FrmChequesReport()
        {
            InitializeComponent();
            GetBankName();
            comb_bank.SelectedIndex = -1; 
            GetChequeTypes();
            comb_cheques.SelectedIndex = -1;
            
        }
        Dal dal = new Dal();
        private void GetBankName()
        {
            List<Bank> lst_ = dal.DALGetBank();
            if (lst_ != null)
            {
                comb_bank.DataSource = lst_;
                comb_bank.ValueMember = "BankId";
                comb_bank.DisplayMember = "BankName";
            }
            else
            {
                MessageBox.Show("Please Fill the Bank name");
            }
        }
        private void GetChequeTypes()
        {
            List<ChequeTypes> lst_= dal.DALGetChequeTypes();
            if (lst_ != null)
            {
                comb_cheques.DataSource = lst_;
                comb_cheques.ValueMember = "ID";
                comb_cheques.DisplayMember= "ChequeType";

            }
            else
            {
                MessageBox.Show("Please Fill the Cheque Type");
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Form2 myForm = new Form2();
            this.Hide();
            myForm.ShowDialog();
            this.Close();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
                string bank = comb_bank.Text;
                string cheque = comb_cheques.Text;
                string duedate;
                if (checkBox1.Checked)
                {
                    duedate = picker_duedate.Value.ToString();
                }
                else
                {
                    duedate ="";
                }
                string beneAccount = text_acct.Text.ToString().Trim(' ');
                string chequenumber = text_chequeNumber.Text.ToString().Trim(' ');

                ChequesPresentedToBank RC = new ChequesPresentedToBank();
            try
            {

                DataTable dt = dal.DalGenerateReportFinal(beneAccount, bank, cheque, chequenumber, duedate);
                RC.SetDataSource(dt);
                RC.SetParameterValue("username", Form1.UserName);
                crystalReportViewer1.ReportSource = RC;
                crystalReportViewer1.Refresh();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {

            string bank = "";
            bool isInsert = false;
            bool isDelete = false;
            var bankobj = comb_bank.SelectedValue;
            if (bankobj != null)
            {
                 bank = comb_bank.SelectedValue.ToString();
            }
            else
            {
                 bank = "";
            }
            string cheque = comb_cheques.Text;
            string duedate;
            if (checkBox1.Checked)
            {
                duedate = picker_duedate.Value.ToString();
            }
            else
            {
                duedate = "";
            }
            string beneAccount = text_acct.Text.ToString().Trim(' ');
            string chequenumber = text_chequeNumber.Text.ToString().Trim(' ');
            var result = dal.DalbeneficiaryTransaction(beneAccount, bank, cheque, chequenumber, duedate);
            if (result == null)
            {
                MessageBox.Show("Report is Empty");
            }

            foreach (var item in result)
            {
                item.DepositeDate = System.DateTime.Now;
                isInsert = dal.DalInsertBeneficiaryTransactionSave(item);
            }
            if (isInsert)
            {
                isDelete = dal.DalDeletebeneficiaryTransaction(beneAccount, bank, cheque, chequenumber, duedate);
            }
            else
            {
                MessageBox.Show("Record is not inserted in BeneficiaryTransactionSave table ");
            }
            if (isDelete)
            {
                crystalReportViewer1.ReportSource = null;
                crystalReportViewer1.Refresh();
                MessageBox.Show("Report is clearing .........");
            }
            else
            {
                MessageBox.Show("Report does not contain ay record");
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void FrmChequesReport_Load(object sender, EventArgs e)
        {

        }
    }
}
