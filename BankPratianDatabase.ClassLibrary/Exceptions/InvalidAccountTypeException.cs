namespace BankPratianCommon.ClassLibrary
{
    // Custom Exceptions
    public class InvalidAccountTypeException : ApplicationException
        {
            public InvalidAccountTypeException(string message) : base(message) { }
        }
    }


