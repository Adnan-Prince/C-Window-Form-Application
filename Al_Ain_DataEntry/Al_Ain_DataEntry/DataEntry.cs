using Al_Ain_DataEntry.DAL;
using Al_Ain_DataEntry.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Al_Ain_DataEntry
{
    public partial class DataEntry : Form
    {
        private static int counter = 0;
        bool status = false;
        Dal dal = new Dal();
        public DataEntry()
        {
            InitializeComponent();
            GetBranchNames();
            combobranchname.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            combobranchname.AutoCompleteSource = AutoCompleteSource.ListItems;
            combobranchname.SelectedIndex = -1;
            textcount.Enabled=false;
        }

        private void DataEntry_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        

        private void button2_Click(object sender, EventArgs e)
        {
            clearFields();
        }

        private void btncomplete_Click(object sender, EventArgs e)
        {
            dataEntry.IsComplete = true;
            textboxno.Enabled= true; 
            dataEntry.BoxNumber=textboxno.Text;
            var result = dal.DALCompleteBatch(dataEntry);
            if (result)
            {
                MessageBox.Show("Batch Completed Successfully!");
                combobranchname.SelectedIndex = -1;
                textboxno.Clear();
                clearFields();
                counter = 0;
                textcount.Text = counter.ToString();
                textboxno.Focus();
            }
            else
            {
                MessageBox.Show("Error Occured in Batch..");
            }
            
        }
        DataEntries dataEntry = new DataEntries();
        private void btnNext_Click(object sender, EventArgs e)
        {
            
            dataEntry.BoxNumber = textboxno.Text;
            dataEntry.FileBarcode = textfilebarcode.Text;
            dataEntry.BagNumber=textBagnumber.Text;
            dataEntry.BranchName = combobranchname.Text;
            dataEntry.UserName = Login.UserName;
            dataEntry.EntryDate = System.DateTime.Now;
            dataEntry.SubmissionDate = textsubdate.Text;
            dataEntry.Date = textdate.Text;
            textcount.Text = counter.ToString();
            
            if (addmore)
            {
                InsertRecord(dataEntry);
                combobranchname.SelectedIndex = -1;
            }
            else
            {
                chackFields();
                combobranchname.SelectedIndex = -1;
                if (status)
                {
                    InsertRecord(dataEntry);
                }
            }
            textfilebarcode.Focus();
        }
        private void chackFields()
        {
            if (textboxno.Text == "")
            {
                errorProvider1.SetError(textboxno, "Please Enter Valid Box Number");
                status = false;
            }
            if (textBagnumber.Text == "")
            {
                errorProvider1.SetError(textBagnumber, "Please Enter Valid Bag Number");
                status = false;
            }
            if (textdate.Text == "")
            {
                errorProvider1.SetError(textdate, "Please Enter Valid Date");
                status = false;
            }
            if (textsubdate.Text == "")
            {
                errorProvider1.SetError(textsubdate, "Please Enter Valid Submission data");
                status = false;
            }
            if (combobranchname.Text == "")
            {
                errorProvider1.SetError(combobranchname, "Please Select Branch Name");
                status = false;
            }
            if (textfilebarcode.Text == "" || textfilebarcode.Text.Length != 11)
            {
                errorProvider1.SetError(textfilebarcode, "Please Enter Valid File Barcode");
                status = false;
            }
            if (textboxno.Text.Length ==7 && textfilebarcode.Text.Length==11 && textBagnumber.Text.Length!=0 && textsubdate.Text.Length != 0 && textdate.Text.Length != 0 && combobranchname.Text.Length !=0)
            {
                if (textboxno.Text.Substring(0, 2).ToUpper() == "AP")
                {
                    status = true;
                }
                else
                {
                    errorProvider1.SetError(textboxno, "Please Enter Box Number With AP ");
                    status = false;
                }
                
            }
        }
        #region GetBranche's Name
        private void GetBranchNames()
        {
            List<Branches> lst_ = dal.DALGetBranchName();
            if (lst_ != null)
            {
                combobranchname.DataSource = lst_;
                combobranchname.ValueMember = "BranchCode";
                combobranchname.DisplayMember = "BranchName";

            }
            else
            {
                MessageBox.Show("Please Fill the Cheque Type");
            }
        }
        #endregion
        private void InsertRecord(DataEntries dataEntries)
        {
            if (!addmore)
            {
                if (counter == 0)
                {
                    if (!(ValidateBoxno(dataEntries) && ValidateFileBarcode(dataEntries)))
                    {
                        var result = dal.DALInsertEntry(dataEntries);
                        if (result)
                        {
                            clearFields();
                            counter = counter + 1;
                            textcount.Text = counter.ToString();
                        }
                        else
                        {
                            MessageBox.Show("Record is not inserted");
                        }
                    }
                    else
                    {
                        clearFields();
                        combobranchname.SelectedIndex = -1;
                        MessageBox.Show("Box Number or File barcode already exists");
                        textboxno.Enabled = false;
                        textboxno.Focus();
                    }

                }
                else
                {
                    if (!ValidateFileBarcode(dataEntries))
                    {
                        var result = dal.DALInsertEntry(dataEntries);
                        if (result)
                        {
                            clearFields();
                            counter = counter + 1;
                            textcount.Text = counter.ToString();
                        }
                        else
                        {
                            MessageBox.Show("Record is not inserted");
                        }
                    }
                    else
                    {
                        textfilebarcode.Clear();
                        MessageBox.Show("FileBarcode Already Exists");
                    }

                }

            }
            else
            {
                var result = dal.DALInsertEntry(dataEntries);
                if (result)
                {
                    clearFields();
                    counter = counter + 1;
                    textcount.Text = counter.ToString();
                }
                else
                {
                    MessageBox.Show("Record is not inserted");
                }

            }
           
        }
        private void InsertRecordAddMore(DataEntries dataEntries)
        {
           
            var result = dal.DALInsertEntry(dataEntries);
            if (result)
            {
                combobranchname.SelectedIndex = -1;
            }
            else
                {
                MessageBox.Show("Add more is not Inserted");
            }
        }
        private bool ValidateFileBarcode(DataEntries dataEntries)
        {
            try
            {
                var result = dal.DalValidateFileBarcode(dataEntries);
                if (result != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool ValidateBoxno(DataEntries dataEntries)
        {
            try
            {
                var result = dal.DalValidateBoxNo(dataEntries);
                if (result != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void clearFields()
        {
            textBagnumber.Clear();
            textfilebarcode.Clear();
            textdate.Clear();
            textsubdate.Clear();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void textboxno_TextChanged(object sender, EventArgs e)
        {
            
            if(textboxno.Text.Length == 7)
            {
                errorProvider1.Clear();
                textfilebarcode.Focus();
                textboxno.Enabled = false;
            }
        }

        private void textfilebarcode_TextChanged(object sender, EventArgs e)
        { 
            if (textfilebarcode.Text.Length == 11)
            {
                errorProvider1.Clear();
                textBagnumber.Focus();         
            }
        }
        public static bool addmore = false;
        private void button1_Click(object sender, EventArgs e)
        {
            DataEntries dataEntries = new DataEntries();
            dataEntries.BoxNumber = textboxno.Text;
            dataEntries.FileBarcode = textfilebarcode.Text;
            dataEntries.BagNumber = textBagnumber.Text;
            dataEntries.BranchName = combobranchname.Text;
            dataEntries.UserName = Login.UserName;
            dataEntries.EntryDate = System.DateTime.Now;
            dataEntries.SubmissionDate = textsubdate.Text;
            dataEntries.Date = textdate.Text;
            textcount.Text = counter.ToString();
            InsertRecordAddMore(dataEntries);
            addmore = true;
        }

        private void textBagnumber_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.Clear();
        }
        private void combobranchname_KeyDown(object sender, KeyEventArgs e)
        {
            combobranchname.DroppedDown = false;
        }
        private void combobranchname_SelectedIndexChanged(object sender, EventArgs e)
        {
            errorProvider1.Clear();
        }

        private void textdate_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.Clear();
        }

        private void textsubdate_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.Clear();
        }
    }
}
