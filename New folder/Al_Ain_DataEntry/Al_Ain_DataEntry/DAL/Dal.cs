using Al_Ain_DataEntry.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Al_Ain_DataEntry.DAL
{
    public class Dal
    {
        readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public bool DALCompleteBatch(DataEntries dataEntries)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "Update DataEntries set IsComplete = @IsComplete where BoxNumber = @BoxNumber";
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {

                        cmd.Parameters.AddWithValue("@BoxNumber", dataEntries.BoxNumber);
                        cmd.Parameters.AddWithValue("@IsComplete", dataEntries.IsComplete);
                        int n = cmd.ExecuteNonQuery();
                        con.Close();
                        if (n > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool DALInsertEntry(DataEntries dataEntries)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO DataEntries (BoxNumber, FileBarcode,Bagnumber,UserName,EntryDate, BranchName, Date, SubmissionDate) VALUES (@BoxNumber, @FileBarcode,@BagNumber,@UserName, @EntryDate, @BranchName, @Date, @SubmissionDate)";
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@BoxNumber", dataEntries.BoxNumber);
                        cmd.Parameters.AddWithValue("@FileBarcode", dataEntries.FileBarcode);
                        cmd.Parameters.AddWithValue("@UserName", dataEntries.UserName);
                        cmd.Parameters.AddWithValue("@BagNumber", dataEntries.BagNumber);
                        cmd.Parameters.AddWithValue("@EntryDate", dataEntries.EntryDate);
                        cmd.Parameters.AddWithValue("@BranchName", dataEntries.BranchName);
                        cmd.Parameters.AddWithValue("@Date", dataEntries.Date);
                        cmd.Parameters.AddWithValue("@SubmissionDate", dataEntries.SubmissionDate);
                        int n = cmd.ExecuteNonQuery();
                        con.Close();
                        if (n > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string ValidateUser(Users users)
        {
            string Name = null;

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT UserName From  TblUsers WHERE UserId = @userid  AND Password = @pas", con))
                    {
                        cmd.Parameters.AddWithValue("@userid", users.UserId);
                        cmd.Parameters.AddWithValue("@pas", users.Password);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Name = (string)reader["UserName"];
                            }
                        }
                    }
                }
                return Name;

            }
            catch (Exception ex)
            {
                
                return Name;
            }
        }




        //public List<Branches> DALGetBranchName()
        //{
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(connectionString))
        //        {

        //            string query = "select * from Branchtbl order by BranchName";
        //            con.Open();

        //            using (SqlCommand cmd = new SqlCommand(query, con))
        //            {
        //                using (SqlDataReader reader = cmd.ExecuteReader())
        //                {
        //                    List<Branches> chequelist = new List<Branches>();
        //                    while (reader.Read())
        //                    {
        //                        Branches cheque = new Branches
        //                        {
        //                            BranchCode = (int)reader["BranchCode"],
        //                            BranchName = (string)reader["BranchName"],
        //                        };
        //                        chequelist.Add(cheque);
        //                    }
        //                    return chequelist;
        //                }

        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}
        public DataTable DalGenerateReport(string username, string entrydate)
        {

            // Construct the base SQL query
            string queryString = "SELECT * FROM VUGetAllDataEntries WHERE 1=1";

            // Add conditions based on selected values
            if (!string.IsNullOrEmpty(username))
            {
                queryString += " AND UserName = @username";
            }
            
            if (!string.IsNullOrEmpty(entrydate))
            {
                queryString += " AND CAST(EntryDate AS DATE) = CAST(@entrydate AS DATE)";
            }
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    // Add parameters to the query
                    if (!string.IsNullOrEmpty(username))
                    {
                        command.Parameters.AddWithValue("@username", username);
                    }
                    if (!string.IsNullOrEmpty(entrydate))
                    {
                        DateTime dateTime = DateTime.Parse(entrydate);
                        command.Parameters.AddWithValue("@entrydate", entrydate);
                    }
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }
            return dataTable;
        }
        public DataTable DALGetBranchName()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "select * from Branchtbl order by BranchName";
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Columns.Add("BranchCode", typeof(int));
                            dataTable.Columns.Add("BranchName", typeof(string));

                            while (reader.Read())
                            {
                                dataTable.Rows.Add(reader["BranchCode"],reader["BranchName"]);
                            }

                            return dataTable;
                        }
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string DalValidateBoxNo(DataEntries dataEntries)
        {
            string boxnumber = null;

            try
            {

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT * From  DataEntries WHERE BoxNumber = @BoxNumber", con))
                    {
                        cmd.Parameters.AddWithValue("@boxnumber", dataEntries.BoxNumber);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                boxnumber = (string)reader["BoxNumber"];
                            }
                        }
                    }
                }
                return boxnumber;
            }
            catch (Exception ex)
            {
                return null;
                throw;
            }
        }
        public string DalValidateFileBarcode(DataEntries dataEntries)
        {
            string filebarcode = null;

            try
            {

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT * From  DataEntries WHERE FileBarcode = @FileBarcode", con))
                    {
                        cmd.Parameters.AddWithValue("@FileBarcode", dataEntries.FileBarcode);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                filebarcode = (string)reader["FileBarcode"];
                            }
                        }
                    }
                }
                return filebarcode;
            }
            catch (Exception ex)
            {
                return null;
                throw;
            }

        }
    }
}
