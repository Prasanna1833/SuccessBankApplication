namespace BankPratianCommon.ClassLibrary
{
    public class TransactionNotFoundException : ApplicationException
    {
            public TransactionNotFoundException(string message) : base(message) { }
        }
    }


