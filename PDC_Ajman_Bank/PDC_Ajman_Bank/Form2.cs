using PDC_Ajman_Bank.Models;
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

namespace PDC_Ajman_Bank
{
    public partial class Form2 : Form
    {
        public string value = "";
        public int Checkstatusid;
        public Form2()
        {
            this.BackgroundImage = Properties.Resources.logo;
            this.BackgroundImageLayout = ImageLayout.Center;
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {

            SetStatusId();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                Logger.WriteLog("Reading CSV files.....to get the Check status Id ");
                string folderPath = folderBrowserDialog1.SelectedPath;
                string[] CSVFiles = Directory.GetFiles(folderPath, "*.CSV");
                Logger.WriteLog(CSVFiles.Length + " CSV files found");

                if (CSVFiles.Length == 0)
                {
                    MessageBox.Show("No text files found in the specified folder!");
                    Logger.WriteLog("No text files found in the specified folder!");
                    return;
                }
                else
                {
                    foreach (var csv in CSVFiles)
                    {
                        var rows = GetAllRowsFromCSV(csv);
                        var ishistory = StoreHistoryUser(rows);
                        if (ishistory)
                        {
                            MessageBox.Show("Record successfully Updated");
                            Logger.WriteLog("Done !");
                        }
                        else
                        {
                            MessageBox.Show("Error Occured During Processing .........");
                            Logger.WriteLog("Error Occured...");
                        }

                    }   
                }
            }
        }
        public List<string> GetAllRowsFromCSV(string filePath)
        {
            List<string> allRows = new List<string>();

            using (StreamReader reader = new StreamReader(filePath))
            {
                // Read each line (row) from the CSV file and add it to the list
                while (!reader.EndOfStream)
                {
                    string row = reader.ReadLine();
                    var columns = row.Split(',');
                    if (row.Contains("Codelines:"))
                    {
                        continue;
                    }

                    foreach (var item in columns)
                    {
                        if (item.Length == 30)
                        {
                            allRows.Add(item);
                        }
                    }
                        
                }
            }

            return allRows;
        }
        public bool StoreHistoryUser(List<string> r)
        {
            bool status = false;
            foreach(var micrs in r)
            {
                BeneficiaryTransaction beneficiary = new BeneficiaryTransaction();
                Dal dal = new Dal();
                beneficiary.Micr = micrs;
                beneficiary.ChkStatusId = Checkstatusid;
                Logger.WriteLog(" Geting Records of Specific MICR");
                var result = dal.GetByMICR(beneficiary);  // Geting Records of Specific MICR
                Logger.WriteLog("Updating CheckStatus Id from BeneficiaryTransaction table");
                var test = dal.UpdateCheckStatusBT(beneficiary); // Updating CheckStatus Id from BeneficiaryTransaction table
                BeneficiaryTransactionUser btuser = new BeneficiaryTransactionUser();
                btuser.BankId = result.BankId;
                btuser.CIF = result.CIF;
                btuser.ChkStatusId = result.ChkStatusId;
                btuser.PrintBatchNo = 2;
                btuser.TrDate = System.DateTime.Now;
                btuser.TrTime = System.DateTime.Now;
                btuser.CheckNo = result.CheckNo;
                btuser.UserId = Form1.UserName;
                btuser.Micr = result.Micr;
                btuser.PrintDate = System.DateTime.Now;
                Logger.WriteLog("Inserting history to BeneficiaryTransactionUser table");
                var BTresult = dal.DalBTUserInsertion(btuser); // Inserting history to BeneficiaryTransactionUser table 
                BTHistory bTHistory = new BTHistory();
                bTHistory.CheckNo = result.CheckNo;
                bTHistory.EntryDate = System.DateTime.Now;
                bTHistory.Micr = result.Micr;
                bTHistory.ChkStatusIdOld = result.ChkStatusId;
                bTHistory.ChkStatusIdNew = Checkstatusid;
                bTHistory.SerialNo = result.SerialNo;
                Logger.WriteLog("Inserting history to BTHistory Table table");
                var BTHResult = dal.DalBTHistoryInsertion(bTHistory); //Inserting history to BTHistory Table table
                if (test&& BTresult && BTHResult)
                {
                    status = true;
                    Logger.WriteLog("Successfully records Updated ..");
                }
                else
                {
                    status = false;
                    Logger.WriteLog("Erorr while Updating records");
                }
            }
            return status;
           
        }
        
        public void SetStatusId()
        {
            bool isChecked = radioButton1.Checked;
            if (isChecked)
                value = radioButton1.Text;
            else
                value = radioButton2.Text;
            if(value== "Normal Withdrawal")
            {
                Checkstatusid = 3;
            }
            else
            {
                Checkstatusid = 6;
            }
        }
        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 myForm = new Form1();
            this.Hide();
            myForm.ShowDialog();
            this.Close();
        }

        

        private void unscheduledWithdrawalToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
