using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankPratianCommon.ClassLibrary;
using System.Configuration;
using System.Data.SqlClient;
using System.Transactions;
using BankOfPratian.ConsoleApp;
namespace PratianBankDatabase.ClassLibrary
{
    public class AccountDBRepository
    {
        public void Create(IAccount account)
        {
            string dbProvider = ConfigurationManager.ConnectionStrings["default"].ProviderName;
            DbProviderFactories.RegisterFactory(dbProvider, SqlClientFactory.Instance);
            DbProviderFactory factory = DbProviderFactories.GetFactory(dbProvider);
            IDbConnection conn = factory.CreateConnection();
            string connStr = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
            conn.ConnectionString = connStr;
            // sql injection
            //string sqlInsert = $"insert into contacts values ('{contact.Name}','{contact.Mobile}','{contact.Email}','{contact.Location}')";
            //string sqlInsert = $"insert into contacts values (@name,@mobile,@email,@loc)";
            string sqlInsert = "INSERT INTO Accounts VALUES (@accno, @name, @pin, @active, @date, @balance, @privilege, @accType)";

            IDbCommand cmd = conn.CreateCommand();
            IDbDataParameter p1 = cmd.CreateParameter();
            p1.ParameterName = "@accno";
            p1.Value = account.AccNo;
            cmd.Parameters.Add(p1);

            IDbDataParameter p2 = cmd.CreateParameter();
            p2.ParameterName = "@name";
            p2.Value = account.Name;
            cmd.Parameters.Add(p2);


            IDbDataParameter p3 = cmd.CreateParameter();
            p3.ParameterName = "@pin";
            p3.Value = account.Pin;
            cmd.Parameters.Add(p3);

            IDbDataParameter p4 = cmd.CreateParameter();
            p4.ParameterName = "@active";
            p4.Value = account.Active;
            cmd.Parameters.Add(p4);


            IDbDataParameter p5 = cmd.CreateParameter();
            p5.ParameterName = "@date";
            p5.Value = account.DateOfOpening;
            cmd.Parameters.Add(p5);

            IDbDataParameter p6 = cmd.CreateParameter();
            p6.ParameterName = "@balance";
            p6.Value = account.Balance;
            cmd.Parameters.Add(p6);

            IDbDataParameter p7 = cmd.CreateParameter();
            p7.ParameterName = "@privilege";
            p7.Value = account.PrivilegeType.ToString().Trim();
            cmd.Parameters.Add(p7);

            IDbDataParameter p8 = cmd.CreateParameter();
            p8.ParameterName = "@accType";
            p8.Value = account.GetAccType();
            cmd.Parameters.Add(p8);

            //cmd.Parameters.AddWithValue("@mobile", contact.Mobile);
            //cmd.Parameters.AddWithValue("@email",contact.Email);
            //cmd.Parameters.AddWithValue("@loc", contact.Location);

            cmd.CommandText = sqlInsert;
            cmd.Connection = conn;
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                return;
            }
            finally
            {
                conn.Close();
            }
        }

        public void DeductAmount(string accNo, double  amount)
        {
            SqlConnection conn = new SqlConnection();
            string conStr = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
            conn.ConnectionString = conStr;
            string sqlString = $"update Accounts set balance = @balance  where accno = @accno";
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@balance", amount);
            cmd.Parameters.AddWithValue("@accno",accNo);
            cmd.CommandText = sqlString;
            cmd.Connection = conn;
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                return;
            }
            finally
            {
                conn.Close();
            }
        }

        public List<Account> GetAccounts()
        {
            List<Account> accounts = new List<Account>();
            string connStr = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sqlString = "SELECT * FROM Accounts";
                SqlCommand cmd = new SqlCommand(sqlString, conn);
                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
         
                            string accountType = reader["AccType"].ToString();
                            Account account = null;
                       
                            if (accountType == "Savings")
                            {
                                account = new SavingsAccount();
                            }
                            else if (accountType == "Current")
                            {
                                account = new CurrentAccount();
                            }
                           
                            if (account != null)
                            {
                                account.AccNo = reader["AccNo"].ToString();
                                account.Name = reader["Name"].ToString();
                                account.Pin = reader["Pin"].ToString();
                                account.Active = Convert.ToBoolean(reader["Active"]);
                                account.DateOfOpening = Convert.ToDateTime(reader["DateOfOpening"]);
                                account.Balance = Convert.ToDouble(reader["Balance"]);
                                account.PrivilegeType = (PrivilegeType)Enum.Parse(typeof(PrivilegeType), reader["PrivilegeType"].ToString());
                                // Set other fields as necessary
                                accounts.Add(account);
                            }
                        }
                    }
                }
                finally
                {
                    conn.Close();
                }
            }

            return accounts;
        }


        public void AddAmount(string accNo, double amount)
        {
            SqlConnection conn = new SqlConnection();
            string connStr = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
            conn.ConnectionString = connStr;
            string sqlString = $"update Accounts set balance = @balance where accno = @accno";
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@balance",amount);
            cmd.Parameters.AddWithValue("@accno", accNo);
            cmd.CommandText = sqlString;
            cmd.Connection = conn;
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                return;
            }
            finally
            {
                conn.Close();
            }
        }

        public IAccount GetAccoountByID(string accNo)
        {
            IAccount account = null;
            SqlConnection conn = new SqlConnection();
            string connStr = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
            conn.ConnectionString = connStr;
            string sqlString = "select * from accounts where accno = @accountno";
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@accountno",accNo);
            cmd.CommandText=sqlString;
            cmd.Connection = conn;
            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string accType = reader["AccType"].ToString();
                    account = AccountFactory.CreateAccount((AccountType)Enum.Parse(typeof(AccountType), accType,true));
                    account.AccNo = reader["AccNo"].ToString();
                    account.Name = reader["Name"].ToString();
                    account.Pin = reader["Pin"].ToString();
                    account.Active = Convert.ToBoolean(reader["Active"]);
                    account.Balance = Convert.ToDouble(reader["Balance"]);
                    account.DateOfOpening = Convert.ToDateTime(reader["DateOfOpening"]);
                    account.PrivilegeType = (PrivilegeType)Enum.Parse(typeof(PrivilegeType), reader["PrivilegeType"].ToString(),true);
                }
                //if(account != null)
                //{
                //    PolicyFactory factory = PolicyFactory.GetInstance();
                //    IPolicy policy =  factory.CreatePolicy(account.GetAccType(),account.PrivilegeType.ToString());
                //    account.Policy = policy;
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return account;
        }

        public void TransferFunds(string fromAccNo, string toAccNo, double fromBalance, double toBalance)
        {
            SqlConnection conn = new SqlConnection();
            string sqlConn = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
            conn.ConnectionString = sqlConn;
            string deposit = $"update Accounts set balance = @balance where accno = @accno ";
            SqlCommand cmd1 = new SqlCommand();
            cmd1.Parameters.AddWithValue("@balance", toBalance);
            cmd1.Parameters.AddWithValue("@accno", toAccNo);
            cmd1.CommandText = deposit;
            cmd1.Connection = conn;
            string withdraw = $"update Accounts set balance = @balance where accno = @accno";
            SqlCommand cmd2 = new SqlCommand();
            cmd2.Parameters.AddWithValue("@balance", fromBalance);
            cmd2.Parameters.AddWithValue("@accno", fromAccNo);
            cmd2.CommandText = deposit;
            cmd2.Connection = conn;
            conn.Open();
            SqlTransaction trans = conn.BeginTransaction();
            cmd1.Transaction = trans;
            cmd2.Transaction = trans;
            try
            {
                cmd1.ExecuteNonQuery();
                cmd2.ExecuteNonQuery();
                trans.Commit();
            }
            catch(Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public void FindAllTransactions(string accNo)
        {
            IAccount account = null;
            SqlConnection conn = new SqlConnection();
            string connStr = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
            conn.ConnectionString = connStr;
            string sqlString = "select * from Transactions where accno = @accNo";
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@accNo", accNo);
            cmd.CommandText = sqlString;
            cmd.Connection = conn;
            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string accType = reader["AccType"].ToString();
                    account = AccountFactory.CreateAccount((AccountType)Enum.Parse(typeof(AccountType), accType, true));
                    account.AccNo = reader["AccNo"].ToString();
                    account.Name = reader["Name"].ToString();
                    account.Pin = reader["Pin"].ToString();
                    account.Active = Convert.ToBoolean(reader["Active"]);
                    account.Balance = Convert.ToDouble(reader["Balance"]);
                    account.DateOfOpening = Convert.ToDateTime(reader["DateOfOpening"]);
                    account.PrivilegeType = (PrivilegeType)Enum.Parse(typeof(PrivilegeType), reader["PrivilegeType"].ToString(), true);
                }
                //if(account != null)
                //{
                //    PolicyFactory factory = PolicyFactory.GetInstance();
                //    IPolicy policy =  factory.CreatePolicy(account.GetAccType(),account.PrivilegeType.ToString());
                //    account.Policy = policy;
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
          
        }
    }
}
