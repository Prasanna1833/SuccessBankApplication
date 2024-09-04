using BankPratianCommon.ClassLibrary;
namespace BankOfPratian.ConsoleApp
{
        public class Transfer : Transaction
        {
            public IAccount ToAccount { get; }
            public Transfer(IAccount fromAccount, IAccount toAccount, double amount): base(fromAccount, amount)
            {
                ToAccount = toAccount;
            }
        }
}


