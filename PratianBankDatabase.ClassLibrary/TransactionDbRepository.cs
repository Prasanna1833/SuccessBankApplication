using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankPratianCommon.ClassLibrary;

namespace PratianBankDatabase.ClassLibrary
{
    public class TransactionDbRepository
    {
        public void Create(Transaction transaction)
        {
            string dbProvider = ConfigurationManager.ConnectionStrings["default"].ProviderName;
            DbProviderFactories.RegisterFactory(dbProvider, SqlClientFactory.Instance);
            DbProviderFactory factory = DbProviderFactories.GetFactory(dbProvider);
            IDbConnection conn = factory.CreateConnection();
            string connStr = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
            conn.ConnectionString = connStr;

            string sqlInsert = "INSERT INTO Transactions VALUES (@transid, @transactiontype, @accno, @transdate, @amount)";

            IDbCommand cmd = conn.CreateCommand();
            IDbDataParameter p1 = cmd.CreateParameter();
            p1.ParameterName = "@transid";
            p1.Value = transaction.TransID;
            cmd.Parameters.Add(p1);

            IDbDataParameter p2 = cmd.CreateParameter();
            p2.ParameterName = "@transactiontype";
            p2.Value = transaction.TransactionType.ToString();
            cmd.Parameters.Add(p2);

            IDbDataParameter p3 = cmd.CreateParameter();
            p3.ParameterName = "@accno";
             p3.Value = transaction.FromAccount.AccNo;
            //p3.Value = transaction.FromAccount;
            cmd.Parameters.Add(p3);

            IDbDataParameter p4 = cmd.CreateParameter();
            p4.ParameterName = "@transdate";
            p4.Value = transaction.TranDate;
            cmd.Parameters.Add(p4);

            IDbDataParameter p5 = cmd.CreateParameter();
            p5.ParameterName = "@amount";
            p5.Value = transaction.Amount;
            cmd.Parameters.Add(p5);

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

        public Dictionary<string, Dictionary<TransactionType, List<Transaction>>> GetAllTransactions()
        {
            AccountDBRepository _accountRepository = new AccountDBRepository();
            //AccountDBRepository repo = new AccountDBRepository();
            //Dictionary<string, Dictionary<TransactionType, List<Transaction>>> TransDictionary = new Dictionary<string, Dictionary<TransactionType, List<Transaction>>>();
            //SqlConnection conn = new SqlConnection();
            //string connStr = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
            //conn.ConnectionString = connStr;
            //string sqlString = "SELECT * FROM Transactions ORDER BY AccNo,transactionType";
            //SqlCommand cmd = new SqlCommand();
            //cmd.CommandText = sqlString;
            //cmd.Connection = conn;
            //try
            //{
            //    conn.Open();
            //    SqlDataReader reader = cmd.ExecuteReader();

            //    while (reader.Read())
            //    {
            //        int transId = reader.GetInt32(reader.GetOrdinal("transId"));
            //        string fromAccount = reader.GetString(reader.GetOrdinal("accNo")); 
            //        DateTime tranDate = reader.GetDateTime(reader.GetOrdinal("tansDate"));
            //        double amount = reader.GetDouble(reader.GetOrdinal("amount"));
            //        TransactionType transType = Enum.Parse<TransactionType>(reader.GetString(reader.GetOrdinal("transactionType")));

            //        IAccount account = repo.GetAccoountByID(fromAccount);
            //        if (!TransDictionary.ContainsKey(fromAccount))
            //        {
            //            TransDictionary[fromAccount] = new Dictionary<TransactionType, List<Transaction>>();
            //        }

            //        if (!TransDictionary[fromAccount].ContainsKey(transType))
            //        {
            //            TransDictionary[fromAccount][transType] = new List<Transaction>();
            //        }
            //        Transaction transaction = new Transaction 
            //        { 
            //            Amount = amount,
            //            TransactionType = transType,
            //            FromAccount = account,
            //            TranDate = tranDate,
            //            TransID = transId
            //        };
            //        TransDictionary[fromAccount][(transType)].Add(transaction);
            //    }
            //    return TransDictionary;
            //}
            //catch(Exception e)
            //{
            //    throw e;
            //}
            //finally
            //{
            //    conn.Close();
            //}
            Dictionary<string, Dictionary<TransactionType, List<Transaction>>> transDictionary = new Dictionary<string, Dictionary<TransactionType, List<Transaction>>>();
            string connStr = ConfigurationManager.ConnectionStrings["default"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM Transactions ORDER BY AccNo,transactionType", conn))
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int transId = reader.GetInt32(reader.GetOrdinal("transId"));
                        string fromAccount = reader.GetString(reader.GetOrdinal("accNo"));
                        DateTime tranDate = reader.GetDateTime(reader.GetOrdinal("transDate"));
                        double amount = reader.GetDouble(reader.GetOrdinal("amount"));
                        TransactionType transType = Enum.Parse<TransactionType>(reader.GetString(reader.GetOrdinal("transactionType")));

                        IAccount account = _accountRepository.GetAccoountByID(fromAccount);

                        if (!transDictionary.ContainsKey(fromAccount))
                        {
                            transDictionary[fromAccount] = new Dictionary<TransactionType, List<Transaction>>();
                        }

                        if (!transDictionary[fromAccount].ContainsKey(transType))
                        {
                            transDictionary[fromAccount][transType] = new List<Transaction>();
                        }

                        Transaction transaction = new Transaction
                        {
                            Amount = amount,
                            TransactionType = transType,
                            FromAccount = account,
                            TranDate = tranDate,
                            TransID = transId
                        };
                        transDictionary[fromAccount][transType].Add(transaction);
                    }
                }
            }
            return transDictionary;

        }
    }
}