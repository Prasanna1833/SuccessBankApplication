namespace BankPratianCommon.ClassLibrary
{
    public class AccountDoesNotExistException : ApplicationException
    {
        public AccountDoesNotExistException(string message) : base(message) { }
    }
}


