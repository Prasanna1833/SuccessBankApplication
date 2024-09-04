namespace BankPratianCommon.ClassLibrary
{
    public class InvalidTransactionTypeException : ApplicationException
    {
            public InvalidTransactionTypeException(string message) : base(message) { }
        }
    }


