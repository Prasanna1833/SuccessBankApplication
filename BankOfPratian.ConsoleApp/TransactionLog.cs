using BankPratianCommon.ClassLibrary;
using PratianBankDatabase.ClassLibrary;
namespace BankOfPratian.ConsoleApp
{
    // Transaction Log
    public static class TransactionLog
    {
        private static readonly Dictionary<string, Dictionary<TransactionType, List<Transaction>>> _transactionLog = new Dictionary<string, Dictionary<TransactionType, List<Transaction>>>();
        static TransactionLog()
        {
            TransactionDbRepository dbTransactionRepo = new TransactionDbRepository();
            _transactionLog = dbTransactionRepo.GetAllTransactions();
        }

        public static Dictionary<string, Dictionary<TransactionType, List<Transaction>>> GetTransactions()
        {
            //if (_transactionLog.Count == 0)
            //    throw new TransactionNotFoundException("No transactions found.");
            return _transactionLog;
        }

        public static Dictionary<TransactionType, List<Transaction>> GetTransactions(string accNo)
        {
            //if (!_transactionLog.TryGetValue(accNo, out var transactions))
            //    throw new TransactionNotFoundException($"No transactions found for account {accNo}.");
            //return transactions;
            if (!_transactionLog.ContainsKey(accNo))
                throw new TransactionNotFoundException("No transactions found");

            return _transactionLog[accNo];
        }

        public static List<Transaction> GetTransactions(string accNo, TransactionType type)
        {
            //var transactions = GetTransactions(accNo);
            //if (!transactions.TryGetValue(type, out var typeTransactions))
            //    throw new InvalidTransactionTypeException($"Invalid transaction type {type} for account {accNo}.");
            //return typeTransactions;
            if (!_transactionLog.ContainsKey(accNo) || !_transactionLog[accNo].ContainsKey(type))
                throw new InvalidTransactionTypeException("Invalid transaction type");

            return _transactionLog[accNo][type];
        }

        public static void LogTransaction(string accNo, TransactionType type, Transaction transaction)
        {
            //if (!_transactionLog.ContainsKey(accNo))
            //    _transactionLog[accNo] = new Dictionary<TransactionType, List<Transaction>>();

            //if (!_transactionLog[accNo].ContainsKey(type))
            //    _transactionLog[accNo][type] = new List<Transaction>();

            //_transactionLog[accNo][type].Add(transaction);
            if (!_transactionLog.ContainsKey(accNo))
            {
                _transactionLog[accNo] = new Dictionary<TransactionType, List<Transaction>>();
            }

            if (!_transactionLog[accNo].ContainsKey(type))
            {
                _transactionLog[accNo][type] = new List<Transaction>();
            }

            _transactionLog[accNo][type].Add(transaction);
        }
    }
}


