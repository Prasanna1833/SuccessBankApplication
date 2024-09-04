
namespace BankPratianCommon.ClassLibrary
{
    // Factories
    public static class AccountFactory
    {
        public static IAccount CreateAccount(AccountType accountType)
        {
            switch (accountType)
            {
                case AccountType.SAVINGS:
                    return new SavingsAccount();
                case AccountType.CURRENT:
                    return new CurrentAccount();
                default:
                    throw new InvalidAccountTypeException("Invalid account type.");
            }
        }
    }
}


