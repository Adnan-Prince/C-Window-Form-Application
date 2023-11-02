using Al_Ain_DataEntry.DAL;
using ClosedXML.Excel;
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

namespace Al_Ain_DataEntry
{
    public partial class Reports : Form
    {
        Dal dal = new Dal();

        public Reports()
        {
            InitializeComponent();
        }
        string username = "";
        string entrydate = "";
        DataTable data = null;
        private void btnfind_Click(object sender, EventArgs e)
        {
            username = textusername.Text;
            entrydate = dateTimePicker1.Value.ToString();
            data = dal.DalGenerateReport(username, entrydate);
            dataGridView1.DataSource = data;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataEntry myForm = new DataEntry();
            this.Hide();
            myForm.ShowDialog();
            this.Close();
        }
       
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        string basePath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\UserReports\\" +Login.UserName+"_Report.xlsx";
        
        private void button3_Click(object sender, EventArgs e)
        {
            DataTable dataTable = dal.DalGenerateReport(username, entrydate);
            ExportToExcel(dataTable, basePath);
        }
        public void ExportToExcel(DataTable dt, string filePath)
        {

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Report");
                var headerRow = worksheet.Row(1);
                headerRow.Style.Font.Bold = true;
                worksheet.PageSetup.Margins.Top = 0.25;
                worksheet.PageSetup.Margins.Bottom = 0.25;
                worksheet.PageSetup.Margins.Left = 0.25;
                worksheet.PageSetup.Margins.Right = 0.25;
                worksheet.PageSetup.Margins.Footer = 0.25;
                worksheet.PageSetup.Margins.Header = 0.25;
                // Add headers
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    worksheet.Cell(1, i + 1).Value = dt.Columns[i].ColumnName;
                }

                // Add data rows
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        var value = dt.Rows[i][j];
                        worksheet.Cell(i + 2, j + 1).Value = value != null ? value.ToString() : string.Empty;
                    }
                }

                workbook.SaveAs(filePath);
                MessageBox.Show("Report Success fully Generated and Saved ");
            }
        }
    }
}
