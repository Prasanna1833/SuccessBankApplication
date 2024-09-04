
namespace BankPratianCommon.ClassLibrary
{
    // External Bank Service implementations
    public class ICICIBankService : IExternalBankService
    {
        public bool Deposit(string accId, double amount)
        {
            // ICICI Bank specific deposit logic
            Console.WriteLine($"Depositing {amount} to ICICI account {accId}");
            return true;
        }
    }
}


