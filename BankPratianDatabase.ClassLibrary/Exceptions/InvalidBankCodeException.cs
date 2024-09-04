
    namespace BankOfPratian.ConsoleApp
    {
    [Serializable]
    public class InvalidBankCodeException : Exception
    {
        //public InvalidBankCodeException()
        //{
        //}

        //public InvalidBankCodeException(string? message) : base(message)
        //{
        //}

        public InvalidBankCodeException(string? message = null, Exception? innerException = null) : base(message, innerException)
        {
        }
    }
}