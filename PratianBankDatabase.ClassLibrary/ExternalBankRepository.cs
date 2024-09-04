using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankOfPratian.ConsoleApp;

namespace PratianBankDatabase.ClassLibrary
{
    public class ExternalBankRepository
    {
        public bool Update(ExternalAccount account, double amount, string bank)
        {
            SqlConnection connection = new SqlConnection();
            string conStr = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
            connection.ConnectionString = conStr;
            string sqlInsert = $"insert into {bank} (AccId,amount) values (@accNo,@amount)";
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("accNo",account.AccNo);
            cmd.Parameters.AddWithValue("@amount", amount);
            cmd.CommandText = sqlInsert;
            cmd.Connection = connection;
            try
            {
                connection.Open();//open as late as possible
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                connection.Close();//close connection as soon as possible
            }
        }

    }
}
