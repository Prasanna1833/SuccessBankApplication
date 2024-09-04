using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using BankPratianCommon.ClassLibrary;
namespace BankOfPratian.ConsoleApp
{
    public class DatabaseManager
    {
    //    private readonly string _connectionString;

    //    public DatabaseManager(string connectionString)
    //    {
    //        _connectionString = connectionString;
    //    }

    //    public void AddAccount(IAccount account)
    //    {
    //        using (var connection = new SqlConnection(_connectionString))
    //        {
    //            connection.Open();
    //            var query = @"INSERT INTO Accounts  
    //                      VALUES (@AccNo, @Name, @Pin, @Active, @DateOfOpening, @Balance, @PrivilegeType, @AccType)";

    //            using (var command = new SqlCommand(query, connection))
    //            {
    //                command.Parameters.AddWithValue("@AccNo", account.AccNo);
    //                command.Parameters.AddWithValue("@Name", account.Name);
    //                command.Parameters.AddWithValue("@Pin", account.Pin);
    //                command.Parameters.AddWithValue("@Active", account.Active);
    //                command.Parameters.AddWithValue("@DateOfOpening", account.DateOfOpening);
    //                command.Parameters.AddWithValue("@Balance", account.Balance);
    //                command.Parameters.AddWithValue("@PrivilegeType", account.PrivilegeType.ToString());
    //                command.Parameters.AddWithValue("@AccType", account.GetAccType());

    //                command.ExecuteNonQuery();
    //            }
    //            connection.Close();
    //        }
    //    }

    //    public void AddTransaction(Transaction transaction, string accNo)
    //    {
    //        using (var connection = new SqlConnection(_connectionString))
    //        {
    //            connection.Open();
    //            var query = @"INSERT INTO Transactions VALUES (@TransactionType, @AccNo, @TransDate, @Amount)";

    //            using (var command = new SqlCommand(query, connection))
    //            {
    //                command.Parameters.AddWithValue("@TransactionType", transaction.GetType());
    //                command.Parameters.AddWithValue("@AccNo", accNo);
    //                command.Parameters.AddWithValue("@TransDate", transaction.TranDate);
    //                command.Parameters.AddWithValue("@Amount", transaction.Amount);

    //                command.ExecuteNonQuery();
    //            }
    //            connection.Close();
    //        }
    //    }
    }
}
