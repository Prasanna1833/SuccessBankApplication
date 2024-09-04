namespace BankPratianCommon.ClassLibrary
{
    public class DailyLimitExceededException : ApplicationException
    {
        public DailyLimitExceededException(string message) : base(message) { }
    }
}


