using PDC_Ajman_Bank.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDC_Ajman_Bank
{
    class Dal
    {
        readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public string ValidateUser(Users users)
        {
            string Name = null;
            
            try
            {
               
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using(SqlCommand cmd = new SqlCommand("SELECT UserName From  TblUser WHERE UserId = @userid  AND Password = @pas", con))
                    {
                        cmd.Parameters.AddWithValue("@userid", users.UserId);
                        cmd.Parameters.AddWithValue("@pas", users.Password);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Name  = (string)reader["UserName"];   
                            }
                        }
                    }
                }
                return Name;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return Name;
            }
        }
        public bool UpdateCheckStatusBT(BeneficiaryTransaction bt)
        {
            
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("UPDATE TblBeneficiaryTransaction SET ChkStatusId = @ChkStatusId,StoreOutDate =@StoreOutDate WHERE Micr = @Micr; ", con))
                    {
                        cmd.Parameters.AddWithValue("@ChkStatusId", bt.ChkStatusId);
                        cmd.Parameters.AddWithValue("@StoreOutDate", bt.StoreOutDate);
                        cmd.Parameters.AddWithValue("@Micr", bt.Micr);
                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public DataTable DalGenerateReportFinal(string beneAccount,string Bank,string chequetype,string chequenumber,string duedat)
        {
            
            // Construct the base SQL query
            string queryString = "SELECT * FROM VUListOfChequesPresentedToBankFinal WHERE 1=1";

            // Add conditions based on selected values
            if (!string.IsNullOrEmpty(Bank))
            {
                queryString += " AND BankName = @BankName";
            }
            if (!string.IsNullOrEmpty(beneAccount))
            {
                queryString += " AND BeneficiaryAccountNo = @BeneficiaryAccountNo";
            }
            if (!string.IsNullOrEmpty(chequetype))
            {
                queryString += " AND ChecqueType = @ChecqueType";
            }
            if (!string.IsNullOrEmpty(duedat))
            {
                queryString += " AND CAST(DueDate AS DATE) = CAST(@DueDate AS DATE)";
            }
            if (!string.IsNullOrEmpty(chequenumber))
            {
                queryString += " AND CheckNo = @CheckNo";
            }
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    // Add parameters to the query
                    if (!string.IsNullOrEmpty(Bank))
                    {
                        command.Parameters.AddWithValue("@BankName", Bank);
                    }
                    if (!string.IsNullOrEmpty(beneAccount))
                    {
                        command.Parameters.AddWithValue("@BeneficiaryAccountNo", beneAccount);
                    }
                    if (!string.IsNullOrEmpty(chequetype))
                    {
                        command.Parameters.AddWithValue("@ChecqueType", chequetype);
                    }
                    if (!string.IsNullOrEmpty(duedat))
                    {
                        DateTime dateTime = DateTime.Parse(duedat);
                        command.Parameters.AddWithValue("@DueDate", dateTime);
                    }
                    if (!string.IsNullOrEmpty(chequenumber))
                    {
                        command.Parameters.AddWithValue("@CheckNo", chequenumber);
                    }

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }
            return dataTable;
        }
        public DataTable DalGenerateReportBackup(string beneAccount, string chequetype, string chequenumber, string printdate)
        {

            // Construct the base SQL query
            string queryString = "SELECT * from VUBackUpLisofChequesPresentedToBank WHERE 1=1";

            // Add conditions based on selected values
            
            if (!string.IsNullOrEmpty(beneAccount))
            {
                queryString += " AND BeneficiaryAccountNo = @BeneficiaryAccountNo";
            }
            if (!string.IsNullOrEmpty(chequetype))
            {
                queryString += " AND ChecqueType = @ChecqueType";
            }
            if (!string.IsNullOrEmpty(printdate))
            {
                queryString += " AND CAST(DepositeDate AS DATE) = CAST(@DepositeDate AS DATE)";
            }
            if (!string.IsNullOrEmpty(chequenumber))
            {
                queryString += " AND CheckNo = @CheckNo";
            }
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    // Add parameters to the query
                   
                    if (!string.IsNullOrEmpty(beneAccount))
                    {
                        command.Parameters.AddWithValue("@BeneficiaryAccountNo", beneAccount);
                    }
                    if (!string.IsNullOrEmpty(chequetype))
                    {
                        command.Parameters.AddWithValue("@ChecqueType", chequetype);
                    }
                    if (!string.IsNullOrEmpty(printdate))
                    {
                        DateTime dateTime = DateTime.Parse(printdate);
                        command.Parameters.AddWithValue("@DepositeDate", dateTime);
                    }
                    if (!string.IsNullOrEmpty(chequenumber))
                    {
                        command.Parameters.AddWithValue("@CheckNo", chequenumber);
                    }

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }
            return dataTable;
        }
        public BeneficiaryTransaction GetByMICR(BeneficiaryTransaction userTrans)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM TblBeneficiaryTransaction WHERE Micr = @Micr";
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Micr", userTrans.Micr.ToString());
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                BeneficiaryTransaction beneficiary = new BeneficiaryTransaction
                                {
                                    CIF = reader["CIF"].ToString(),
                                    CheckNo = reader["CheckNo"].ToString(),
                                    BankId = reader["BankId"].ToString(),
                                    SerialNo = reader["SerialNo"] == DBNull.Value ? 0 : (long)reader["SerialNo"],
                                    InstAmt = reader["InstAmt"] == DBNull.Value ? null : (double?)reader["InstAmt"],
                                    EntryDate = reader["EntryDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["EntryDate"],
                                    Micr = reader["Micr"].ToString(),
                                    ChkStatusId = reader["ChkStatusId"] == DBNull.Value ? 0 : (int)reader["ChkStatusId"],
                                    BeneficiaryAccountNo = reader["BeneficiaryAccountNo"].ToString(),
                                    CityId = reader["CityId"] == DBNull.Value ? 0 : (int)reader["CityId"],
                                    ChecqueType = reader["ChecqueType"].ToString(),
                                    StoreInDate = reader["StoreInDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["StoreInDate"],
                                    DueDate = reader["DueDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["DueDate"],
                                    StoreOutDate = reader["StoreOutDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["StoreOutDate"],
                                    CBC = reader["CBC"].ToString(),
                                    BeneficiaryBarcode = reader["BeneficiaryBarcode"].ToString(),
                                };
                                return beneficiary;
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
               // MessageBox.Show(ex.Message);
                return null;
            }
        }
        //public BeneficiaryTransaction GetByMICR(BeneficiaryTransaction userTrans)
        //{
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(connectionString))
        //        {

        //            string query = " Select * from TblBeneficiaryTransaction where Micr =@Micr ";
        //            con.Open();
        //            using (SqlCommand cmd = new SqlCommand(query, con))
        //            {
        //                cmd.Parameters.AddWithValue("@Micr", userTrans.Micr.ToString());
        //                using (SqlDataReader reader = cmd.ExecuteReader())
        //                {
        //                    if (reader.Read())
        //                    {
        //                        BeneficiaryTransaction beneficiary = new BeneficiaryTransaction
        //                        {
        //                            CIF = reader["CIF"].ToString(),
        //                            CheckNo = reader["CheckNo"].ToString(),
        //                            BankId = reader["BankId"].ToString(),
        //                            SerialNo = (long)reader["SerialNo"],
        //                            InstAmt = reader["InstAmt"] == DBNull.Value ? null : (double?)reader["InstAmt"],
        //                            EntryDate = (DateTime)reader["EntryDate"],
        //                            Micr = reader["Micr"].ToString(),
        //                            ChkStatusId = (int)reader["ChkStatusId"],
        //                            BeneficiaryAccountNo = reader["BeneficiaryAccountNo"].ToString(),
        //                            CityId = (int)reader["CityId"],
        //                            ChecqueType = reader["ChecqueType"].ToString(),   
        //                            StoreInDate = (DateTime)reader["StoreInDate"],
        //                            DueDate = (DateTime)reader["DueDate"],
        //                            StoreOutDate = (DateTime)reader["StoreOutDate"],
        //                            CBC = reader["CBC"].ToString(),
        //                            BeneficiaryBarcode= reader["BeneficiaryBarcode"].ToString(),
        //                        };
        //                        return beneficiary;
        //                    }

        //                }
        //            }
        //        }
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //        return null;
        //    }
        //}
        public List<BeneficiaryTransaction> DalbeneficiaryTransaction(string beneAccount, string Bank, string chequetype, string chequenumber, string duedat)
        {
            // Construct the base SQL query
            string queryString = "SELECT * FROM BeneficiaryTransaction WHERE 1=1";

            // Add conditions based on selected values
            if (!string.IsNullOrEmpty(Bank))
            {
                queryString += " AND BankId = @BankId";
            }
            if (!string.IsNullOrEmpty(beneAccount))
            {
                queryString += " AND BeneficiaryAccountNo = @BeneficiaryAccountNo";
            }
            if (!string.IsNullOrEmpty(chequetype))
            {
                queryString += " AND ChecqueType = @ChecqueType";
            }
            if (!string.IsNullOrEmpty(duedat))
            {
                queryString += " AND CAST(DueDate AS DATE) = CAST(@DueDate AS DATE)";
            }
            if (!string.IsNullOrEmpty(chequenumber))
            {
                queryString += " AND CheckNo = @CheckNo";
            }
           
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    // Add parameters to the query
                    if (!string.IsNullOrEmpty(Bank))
                    {
                        command.Parameters.AddWithValue("@BankId", Bank);
                    }
                    if (!string.IsNullOrEmpty(beneAccount))
                    {
                        command.Parameters.AddWithValue("@BeneficiaryAccountNo", beneAccount);
                    }
                    if (!string.IsNullOrEmpty(chequetype))
                    {
                        command.Parameters.AddWithValue("@ChecqueType", chequetype);
                    }
                    if (!string.IsNullOrEmpty(duedat))
                    {
                        DateTime dateTime = DateTime.Parse(duedat);
                        command.Parameters.AddWithValue("@DueDate", dateTime);
                    }
                    if (!string.IsNullOrEmpty(chequenumber))
                    {
                        command.Parameters.AddWithValue("@CheckNo", chequenumber);
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        List<BeneficiaryTransaction> BTlist = new List<BeneficiaryTransaction>();
                        while (reader.Read())
                        {
                            BeneficiaryTransaction beneficiary = new BeneficiaryTransaction
                            {
                                SerialNo = (long)reader["SerialNo"],
                                BeneficiaryAccountNo = (string)reader["BeneficiaryAccountNo"],
                                InstAmt = reader["InstAmt"] == DBNull.Value ? null : (double?)reader["InstAmt"],
                                EntryDate = (DateTime)reader["EntryDate"],
                                DueDate = (DateTime)reader["DueDate"],
                                CheckNo = (string)reader["CheckNo"],
                                BankId = (string)reader["BankId"],
                                CityId = (int)reader["CityId"],
                                ChecqueType = (string)reader["ChecqueType"],
                            };
                            BTlist.Add(beneficiary);
                        }
                        return BTlist;
                    }
                }
            }
        }
        public bool DalDeletebeneficiaryTransaction(string beneAccount, string Bank, string chequetype, string chequenumber, string duedat)
        {
            try
            {
                string queryString = "DELETE FROM BeneficiaryTransaction WHERE 1=1";

                // Add conditions based on selected values
                if (!string.IsNullOrEmpty(Bank.ToString()))
                {
                    queryString += " AND BankId = @BankId";
                }
                if (!string.IsNullOrEmpty(beneAccount))
                {
                    queryString += " AND BeneficiaryAccountNo = @BeneficiaryAccountNo";
                }
                if (!string.IsNullOrEmpty(chequetype.ToString()))
                {
                    queryString += " AND ChecqueType = @ChecqueType";
                }
                if (!string.IsNullOrEmpty(duedat))
                {
                    queryString += " AND CAST(DueDate AS DATE) = CAST(@DueDate AS DATE)";
                }
                if (!string.IsNullOrEmpty(chequenumber))
                {
                    queryString += " AND CheckNo = @CheckNo";
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(queryString, connection))
                    {
                        // Add parameters to the query
                        if (!string.IsNullOrEmpty(Bank))
                        {
                            command.Parameters.AddWithValue("@BankId", Bank);
                        }
                        if (!string.IsNullOrEmpty(beneAccount))
                        {
                            command.Parameters.AddWithValue("@BeneficiaryAccountNo", beneAccount);
                        }
                        if (!string.IsNullOrEmpty(chequetype))
                        {
                            command.Parameters.AddWithValue("@ChecqueType", chequetype);
                        }
                        if (!string.IsNullOrEmpty(duedat))
                        {
                            DateTime dateTime = DateTime.Parse(duedat);
                            command.Parameters.AddWithValue("@DueDate", dateTime);
                        }
                        if (!string.IsNullOrEmpty(chequenumber))
                        {
                            command.Parameters.AddWithValue("@CheckNo", chequenumber);
                        }

                        int n = command.ExecuteNonQuery();
                        connection.Close();
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
                MessageBox.Show(ex.Message);
                return false;
            }
            
        }
        public List<City> DALGetCity()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {

                    string query = "Select * from TblCity";
                    con.Open();
                    
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            List<City> citylst = new List<City>();
                            while (reader.Read())
                            {
                                City city = new City
                                {
                                    CityID = (int)reader["CityID"],
                                    CityName = (string)reader["CityName"],
                                    SName = (string)reader["SName"],
                                    
                                };
                                citylst.Add(city);
                            }
                            return citylst;
                        }
                        
                    }
                }
               
            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<Bank> DALGetBank()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {

                    string query = "Select BankId, BankName from TblBank";
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            List<Bank> banklist = new List<Bank>();
                            while (reader.Read())
                            {
                                Bank bank = new Bank
                                {
                                    BankId = (string)reader["BankId"],
                                    BankName = (string)reader["BankName"],
                                };
                                banklist.Add(bank);
                            }
                            return banklist;
                        }

                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<ChequeTypes> DALGetChequeTypes()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {

                    string query = "Select * from TblChequeTypes";
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            List<ChequeTypes> chequelist = new List<ChequeTypes>();
                            while (reader.Read())
                            {
                                ChequeTypes cheque = new ChequeTypes
                                {
                                    ID = (int)reader["ID"],
                                    ChequeType = (string)reader["ChequeType"],
                                };
                                chequelist.Add(cheque);
                            }
                            return chequelist;
                        }

                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
       
        public bool DalBTUserInsertion(BeneficiaryTransactionUser userTrans)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO TblBeneficiaryTransactionUser (CIF, CheckNo, BankId,UserId, TrDate, TrTime, ChkStatusId, PrintBatchNo, PrintDate ,Micr ) VALUES (@CIF, @CheckNo, @BankId,@UserId, @TrDate, @TrTime, @ChkStatusId, @PrintBatchNo, @PrintDate ,@Micr) ";
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@CIF", userTrans.CIF);
                        cmd.Parameters.AddWithValue("@CheckNo", userTrans.CheckNo);
                        cmd.Parameters.AddWithValue("@BankId", userTrans.BankId);
                        cmd.Parameters.AddWithValue("@UserId", userTrans.UserId);
                        cmd.Parameters.AddWithValue("@TrDate", userTrans.TrDate);
                        cmd.Parameters.AddWithValue("@TrTime", userTrans.TrTime);
                        cmd.Parameters.AddWithValue("@ChkStatusId", userTrans.ChkStatusId);
                        cmd.Parameters.AddWithValue("@PrintBatchNo", userTrans.PrintBatchNo);
                        cmd.Parameters.AddWithValue("@PrintDate", userTrans.PrintDate);
                        cmd.Parameters.AddWithValue("@Micr", userTrans.Micr);
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
        public bool DalBTHistoryInsertion(BTHistory bT)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO BTHistory (SerialNo, EntryDate, CheckNo, Micr, ChkStatusIdOld, ChkStatusIdNew) VALUES (@SerialNo, @EntryDate, @CheckNo, @Micr, @ChkStatusIdOld, @ChkStatusIdNew)";
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        
                        cmd.Parameters.AddWithValue("@SerialNo", bT.SerialNo);
                        cmd.Parameters.AddWithValue("@EntryDate", bT.EntryDate);
                        cmd.Parameters.AddWithValue("@CheckNo", bT.CheckNo);
                        cmd.Parameters.AddWithValue("@ChkStatusIdOld", bT.ChkStatusIdOld);
                        cmd.Parameters.AddWithValue("@ChkStatusIdNew", bT.ChkStatusIdNew);
                        cmd.Parameters.AddWithValue("@Micr", bT.Micr);
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
        public bool DalInsertBeneficiaryTransaction(BeneficiaryTransaction bT)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO BeneficiaryTransaction (BeneficiaryAccountNo, CheckNo, DueDate,EntryDate, InstAmt,BankId,CityId,ChecqueType) VALUES ( @BeneficiaryAccountNo, @CheckNo, @DueDate,@EntryDate, @InstAmt,@BankId,@CityId,@ChecqueType)";
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@BeneficiaryAccountNo", bT.BeneficiaryAccountNo);
                        cmd.Parameters.AddWithValue("@CheckNo", bT.CheckNo);
                        cmd.Parameters.AddWithValue("@DueDate", bT.DueDate);
                        cmd.Parameters.AddWithValue("@InstAmt", bT.InstAmt);
                        cmd.Parameters.AddWithValue("@BankId", bT.BankId);
                        cmd.Parameters.AddWithValue("@CityId", bT.CityId);
                        cmd.Parameters.AddWithValue("@EntryDate", bT.EntryDate);
                        cmd.Parameters.AddWithValue("@ChecqueType", bT.ChecqueType);
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
        public bool DalInsertBeneficiaryTransactionSave(BeneficiaryTransaction bT)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO BeneficiaryTransactionSave (BeneficiaryAccountNo, CheckNo, DueDate,EntryDate, InstAmt,BankId,CityId,ChecqueType,DepositeDate) VALUES ( @BeneficiaryAccountNo, @CheckNo, @DueDate,@EntryDate, @InstAmt,@BankId,@CityId,@ChecqueType,@DepositeDate)";
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@BeneficiaryAccountNo", bT.BeneficiaryAccountNo);
                        cmd.Parameters.AddWithValue("@CheckNo", bT.CheckNo);
                        cmd.Parameters.AddWithValue("@DueDate", bT.DueDate);
                        cmd.Parameters.AddWithValue("@InstAmt", bT.InstAmt);
                        cmd.Parameters.AddWithValue("@BankId", bT.BankId);
                        cmd.Parameters.AddWithValue("@CityId", bT.CityId);
                        cmd.Parameters.AddWithValue("@EntryDate", bT.EntryDate);
                        cmd.Parameters.AddWithValue("@ChecqueType", bT.ChecqueType);
                        cmd.Parameters.AddWithValue("@DepositeDate", bT.DepositeDate);
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
    }
}
