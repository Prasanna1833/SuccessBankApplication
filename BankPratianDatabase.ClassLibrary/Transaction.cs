
namespace BankPratianCommon.ClassLibrary
{
    // Transaction classes
    public class Transaction
    {
        public int TransID { get; set; }
        public IAccount FromAccount { get; set; }
        public DateTime TranDate { get; set; }
        public double Amount { get; set; }
        public TransactionStatus Status { get; set; }
        public TransactionType TransactionType { get; set; }
        public Transaction() 
        {
            Status = TransactionStatus.CLOSE;
        }
        public Transaction(IAccount fromAccount, double amount, TransactionType type = default)
        {
            TransID = IDGenerator.GenerateID();
            FromAccount = fromAccount;
            TranDate = DateTime.Now;
            Amount = amount;
            Status = TransactionStatus.CLOSE;
            TransactionType = type;
        }
    }
}


