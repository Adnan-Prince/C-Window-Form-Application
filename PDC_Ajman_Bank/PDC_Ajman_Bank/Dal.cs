using PDC_Ajman_Bank.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            catch (Exception)
            {
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
                    using (SqlCommand cmd = new SqlCommand("UPDATE TblBeneficiaryTransaction SET ChkStatusId = @ChkStatusId WHERE Micr = @Micr; ", con))
                    {
                        cmd.Parameters.AddWithValue("@ChkStatusId", bt.ChkStatusId);
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

        public  BeneficiaryTransaction GetByMICR(BeneficiaryTransaction userTrans)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    
                    string query = " Select * from TblBeneficiaryTransaction where Micr =@Micr ";
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Micr", userTrans.Micr);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                BeneficiaryTransaction beneficiary = new BeneficiaryTransaction
                                {
                                    CIF=(string)reader["CIF"],
                                    CheckNo = (string)reader["CheckNo"],
                                    BankId=(string)reader["BankId"],
                                    SerialNo = (long)reader["SerialNo"],
                                    InstAmt = reader["InstAmt"] == DBNull.Value ? null : (double?)reader["InstAmt"],
                                    EntryDate = (DateTime)reader["EntryDate"], 
                                    Micr = (string)reader["Micr"],
                                    ChkStatusId=(int)reader["ChkStatusId"],
                                };
                                return beneficiary;
                            }

                        }
                    }
                }
                return null;
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

    }
}
