namespace BankPratianCommon.ClassLibrary
{
    public class InsufficientBalanceException : ApplicationException
    {
            public InsufficientBalanceException(string message) : base(message) { }
        }
    }


