using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;


namespace Text_TO_Excel_Exporter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string folderPath = folderBrowserDialog1.SelectedPath;
                string[] textFiles = Directory.GetFiles(folderPath, "*.txt");

                if (textFiles.Length == 0)
                {
                    MessageBox.Show("No text files found in the specified folder!");
                    return;
                }
                Dictionary<string, List<string[]>> dataDictionary = new Dictionary<string, List<string[]>>();

                foreach (string filePath in textFiles)
                {
                    // Read the content of the file
                    string data = File.ReadAllText(filePath);

                    // Split the data into rows and columns
                    string[] rows = data.Split('\n');
                    rows=rows.Take(rows.Length - 1).ToArray();
                    string[][] columns = new string[rows.Length][];
                    for (int i = 0; i < rows.Length; i++)
                    {
                        columns[i] = rows[i].Split(';');
                    }
                    // Use the header row as the key for the dictionary
                    string headerKey = string.Join("|", columns[0]);
                    
                    // Check if the header key already exists in the dictionary
                    if (dataDictionary.ContainsKey(headerKey))
                    {
                        // If the key exists, append the data to the existing list
                        dataDictionary[headerKey].AddRange(columns.Skip(1));
                    }
                    else
                    {
                        // If the key does not exist, create a new list and add the data
                        dataDictionary.Add(headerKey, columns.Skip(1).ToList());
                    }
                }
                
              
                foreach (var kvp in dataDictionary)
                {
                    // Create a new worksheet for each header key
                    var workbook = new XLWorkbook();
                    var worksheet = workbook.Worksheets.Add("Sheet 1");
                    var headerRow = worksheet.Row(1);
                    headerRow.Style.Font.Bold = true;
                    worksheet.PageSetup.Margins.Top = 0.25;
                    worksheet.PageSetup.Margins.Bottom = 0.25;
                    worksheet.PageSetup.Margins.Left = 0.25;
                    worksheet.PageSetup.Margins.Right = 0.25;
                    worksheet.PageSetup.Margins.Footer = 0.25;
                    worksheet.PageSetup.Margins.Header = 0.25;


                    // Split the header key to get individual header items
                    string[] headerItems = kvp.Key.Split('|');

                    // Populate the header row
                    for (int i = 0; i < headerItems.Length; i++)
                    {
                        worksheet.Cell(1, i + 1).Value = headerItems[i];
                    }

                    // Populate the data in the worksheet
                    for (int i = 0; i < kvp.Value.Count; i++)
                    {
                        for (int j = 0; j < kvp.Value[i].Length; j++)
                        {
                            if (kvp.Value[i][j].Length == 0 )
                            {
                                worksheet.Cell(i + 2, j + 1).Value = "Adnan";
                            }  
                            else
                            {
                                worksheet.Cell(i + 2, j + 1).Value = kvp.Value[i][j].Trim('"');
                                
                            }
                        }
                    }

                    // Auto-fit columns for better readability
                    worksheet.Columns().AdjustToContents();
                    workbook.SaveAs(folderPath + "\\TextToExcelFiles\\" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".xlsx");
                }
            }
        }
       
    }
}
