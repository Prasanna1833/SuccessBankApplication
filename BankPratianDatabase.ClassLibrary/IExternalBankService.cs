namespace BankPratianCommon.ClassLibrary
{
    // external bank services
    public interface IExternalBankService
    {
        bool Deposit(string accId, double amount);
    }
}


