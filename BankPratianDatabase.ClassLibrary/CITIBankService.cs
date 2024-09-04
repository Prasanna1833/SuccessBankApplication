
namespace BankPratianCommon.ClassLibrary
{
    public class CITIBankService : IExternalBankService
    {
        public bool Deposit(string accId, double amount)
        {
            // CITI Bank specific deposit logic
            Console.WriteLine($"Depositing {amount} to CITI account {accId}");
            return true;
        }
    }
}


