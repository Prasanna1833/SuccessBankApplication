using BankPratianCommon.ClassLibrary;
using PratianBankDatabase.ClassLibrary;
namespace BankOfPratian.ConsoleApp
{
    public  static class ResultGenerator
    {
        //private static readonly Dictionary<string, IAccount> _accounts = new Dictionary<string, IAccount>();
        //private static readonly Lazy<AccountDBRepository> _repo = new Lazy<AccountDBRepository>(() => new AccountDBRepository());
        //private static readonly Lazy<TransactionDbRepository> _tRepo = new Lazy<TransactionDbRepository>(() => new TransactionDbRepository());
        //private static List<Account> accounts => _repo.Value.GetAccounts();
        //public static Dictionary<string, Dictionary<TransactionType, List<Transaction>>> transactions => _tRepo.Value.GetAllTransactions();
        //public void AddAccount(IAccount account)
        //{
        //    _accounts[account.AccNo] = account;
        //    Console.WriteLine($"Account {account.AccNo} added to Re   sultGenerator. Total accounts: {_accounts.Count}");
        //}

        public static void PrintAllLogTransactions()
        {
            #region old way
            //  var transactions = TransactionLog.GetTransactions();
            //var allTransactions = transactions.ToList();
            //if (transactions.Count == 0)
            //{
            //    Console.WriteLine("No transactions found.");
            //    return;
            //}

            //foreach (var accountTransactions in transactions)
            //{
            //    //PrintAccountTransactions(accountTransactions.Key, accountTransactions.Value);
            //    Console.WriteLine($"{accountTransactions.TransID}\t{accountTransactions.FromAccount}\t{accountTransactions.TranDate}\t{accountTransactions.TransactionType}\t{accountTransactions.Amount}");
            //}
            #endregion
            var allTransactions = TransactionLog.GetTransactions();
            Console.WriteLine("\n******************************   Report Starts   *************************************\n");

            foreach (var accountTransactions in allTransactions)
            {
                Console.WriteLine("-----------------------------------------------------------------------------------------------");
                Console.WriteLine($"Account No:{accountTransactions.Key}\n");

                foreach (var transactionType in accountTransactions.Value)
                {
                    Console.WriteLine($"Transaction Type:{transactionType.Key}");
                    Console.WriteLine();

                    Console.WriteLine("ID\t\tAccount No\t\t Date\t\t\tAmount\t\tStatus\n");

                    foreach (var transaction in transactionType.Value)
                    {
                        Console.WriteLine($"{transaction.TransID}\t\t{transaction.FromAccount}\t\t{transaction.TranDate}\t\t{transaction.Amount}\t\t{transaction.Status.ToString()}");
                    }
                    Console.WriteLine();
                }
            }
            Console.WriteLine("\n******************************   Report Ends   *************************************\n");
        }

        public static void PrintAllLogTransactions(string accountId)
        {
            //if (!TransactionLog.GetTransactions().TryGetValue(accountId, out var transactions))
            //{
            //    Console.WriteLine($"No transactions found for account: {accountId}");
            //    return;
            //}

            //PrintAccountTransactions(accountId, transactions);
            var accountTransactions = TransactionLog.GetTransactions(accountId);
            Console.WriteLine("\n******************************   Report Starts   *************************************\n");
            Console.WriteLine($"Account No:{accountId}\n");

            foreach (var transactionType in accountTransactions)
            {
                Console.WriteLine($"Transaction Type:{transactionType.Key}");
                Console.WriteLine();

                Console.WriteLine("ID\t\tAccount No\t\t Date\t\t\tAmount\t\tStatus\n");

                foreach (var transaction in transactionType.Value)
                {
                    Console.WriteLine($"{transaction.TransID}\t\t{transaction.FromAccount}\t\t{transaction.TranDate}\t\t{transaction.Amount}\t\t{transaction.Status.ToString()}");
                }
                Console.WriteLine();
            }
            Console.WriteLine("\n******************************   Report Ends   ****************************************\n");
        }

        public static void PrintAllLogTransactions(TransactionType transactionType)
        {
            //Console.WriteLine($"Transaction Type: {transactionType}");
            //bool transactionsFound = false;

            //foreach (var accountTransactions in TransactionLog.GetTransactions())
            //{
            //    if (accountTransactions.Value.TryGetValue(transactionType, out var transactions))
            //    {
            //        transactionsFound = true;
            //        foreach (var transaction in transactions)
            //        {
            //            PrintTransaction(accountTransactions.Key, transaction);
            //        }
            //    }
            //}

            //if (!transactionsFound)
            //{
            //    Console.WriteLine($"No transactions found for type: {transactionType}");
            //}
            var allTransactions = TransactionLog.GetTransactions();
            Console.WriteLine("\n******************************   Report Starts   *************************************\n");
            Console.WriteLine(transactionType.ToString());
            Console.WriteLine();
            Console.WriteLine("ID\t\tAccount No\t\t Date\t\t\tAmount\t\tStatus\n");
            foreach (var accountTransactions in allTransactions)
            {
                if (accountTransactions.Value.ContainsKey(transactionType))
                {
                    foreach (var transaction in accountTransactions.Value[transactionType])
                    {
                        Console.WriteLine($"{transaction.TransID}\t\t{transaction.FromAccount}\t\t{transaction.TranDate}\t\t{transaction.Amount}\t\t{transaction.Status.ToString()}");
                    }
                }
            }
            Console.WriteLine("\n******************************   Report Ends   ****************************************\n");
        }

        public static int GetTotalNoOfAccounts()
        {
            AccountDBRepository bankApplicationDbRepository = new AccountDBRepository();
            var allAccounts = bankApplicationDbRepository.GetAccounts();
            return allAccounts.Count;
        }

        public static void DisplayNoOfAccTypeWise()
        {
            AccountDBRepository bankApplicationDbRepository = new AccountDBRepository();
            //var accountTypes = _accounts.Values
            //    .GroupBy(acc => acc.GetAccType())
            //    .Select(group => new { AccountType = group.Key, Count = group.Count() });

            //Console.WriteLine("Account Type  No Of Accounts");
            //foreach (var type in accountTypes)
            //{
            //    Console.WriteLine($"{type.AccountType,-15} {type.Count}");
            //}
            var allAccounts = bankApplicationDbRepository.GetAccounts();
            var accountTypeCounts = allAccounts.GroupBy(acc => acc.GetAccType())
                                               .Select(group => new { AccType = group.Key, Count = group.Count() }).ToList();

            Console.WriteLine("\nAccount Type\tNo Of Accounts\n");
            foreach (var accTypeCount in accountTypeCounts)
            {
                Console.WriteLine($"{accTypeCount.AccType}\t\t{accTypeCount.Count}");
            }
        }

        public static void DispTotalWorthOfBank()
        {
            //double totalBalance = _accounts.Values.Sum(acc => acc.Balance);
            //Console.WriteLine($"Total balance available: Rs {totalBalance:N2}");
            AccountDBRepository bankApplicationDbRepository = new AccountDBRepository();
            List<Account> allAccounts = bankApplicationDbRepository.GetAccounts();
            double totalBalance = allAccounts.Sum(acc => acc.Balance);

            Console.WriteLine($"Total Worth of Asset Available in Bank is : Rs {totalBalance:N2}");
        }

        public static void DispPolicyInfo()
        {
            var policies = PolicyFactory.GetPolicies();

            //Console.WriteLine("Policy Type             Minimum Balance Rate Of Interest");
            //foreach (var policy in policies)
            //{
            //    Console.WriteLine($"{policy.Key,-25} {policy.Value.GetMinBalance(),-15:N2} {policy.Value.GetRateOfInterest(),-15:N2}");
            //}


            Console.WriteLine("\nPolicy Type\t  Minimum Balance\tRate Of Interest\n");
            foreach (var policy in policies)
            {
                Console.WriteLine($"{policy.Key}\t\t{policy.Value.GetMinBalance()}\t\t{policy.Value.GetRateOfInterest()}");
            }
        }

        public static void DisplayAllTransfers()
        {
            //PrintAllLogTransactions(TransactionType.TRANSFER);
            var allTransactions = TransactionLog.GetTransactions();
            Console.WriteLine("\nFrom\t\tTo\tDate\t\tAmount\n");

            foreach (var accountTransactions in allTransactions)
            {
                if (accountTransactions.Value.ContainsKey(TransactionType.TRANSFER))
                {
                    foreach (var transaction in accountTransactions.Value[TransactionType.TRANSFER])
                    {
                        Console.WriteLine($"{transaction.FromAccount}\t\t{"NA"}\t{transaction.TranDate.ToShortDateString()}\t{transaction.Amount}");
                    }
                }
            }
        }

        public static void DisplayAllWithdrawals()
        {
            // PrintAllLogTransactions(TransactionType.WITHDRAW);
            var allTransactions = TransactionLog.GetTransactions();
            Console.WriteLine("\nFrom\t\tDate\t\tAmount\n");

            foreach (var accountTransactions in allTransactions)
            {
                if (accountTransactions.Value.ContainsKey(TransactionType.WITHDRAW))
                {
                    foreach (var transaction in accountTransactions.Value[TransactionType.WITHDRAW])
                    {
                        Console.WriteLine($"{transaction.FromAccount}\t\t{transaction.TranDate.ToShortDateString()}\t{transaction.Amount}");
                    }
                }
            }
        }

        public static void DisplayAllDeposits()
        {
            // PrintAllLogTransactions(TransactionType.DEPOSIT);
            var allTransactions = TransactionLog.GetTransactions();
            Console.WriteLine("\nTo\t\tDate\t\tAmount\n");

            foreach (var accountTransactions in allTransactions)
            {
                if (accountTransactions.Value.ContainsKey(TransactionType.DEPOSIT))
                {
                    foreach (var transaction in accountTransactions.Value[TransactionType.DEPOSIT])
                    {
                        Console.WriteLine($"{transaction.FromAccount}\t\t{transaction.TranDate.ToShortDateString()}\t{transaction.Amount}");
                    }
                }
            }
        }

        public static void DisplayAllTransactionsForDay()
        {
            //var today = DateTime.Today;
            //bool transactionsFound = false;

            //foreach (var accountTransactions in TransactionLog.GetTransactions())
            //{
            //    foreach (var transactionType in accountTransactions.Value)
            //    {
            //        foreach (var transaction in transactionType.Value.Where(t => t.TranDate.Date == today))
            //        {
            //            transactionsFound = true;
            //            PrintTransaction(accountTransactions.Key, transaction);
            //        }
            //    }
            //}

            //if (!transactionsFound)
            //{
            //    Console.WriteLine("No transactions found for today.");
            //}
            var allTransactions = TransactionLog.GetTransactions();
            DateTime today = DateTime.Today;
            Console.WriteLine("\nFrom\tTo\t\tDate\t  Amount\n");

            foreach (var accountTransactions in allTransactions)
            {
                foreach (var transactionTypes in accountTransactions.Value)
                {
                    foreach (var transaction in transactionTypes.Value)
                    {
                        if (transaction.TranDate.Date == today)
                        {
                            string toAccount = transactionTypes.Key == TransactionType.WITHDRAW ? "---" : transaction.FromAccount.AccNo;
                            string fromAccount = transactionTypes.Key == TransactionType.DEPOSIT ? "---" : transaction.FromAccount.AccNo;
                            Console.WriteLine($"{fromAccount}\t{toAccount}\t    {transaction.TranDate.ToShortDateString()}\t   {transaction.Amount}");
                        }
                    }
                }
            }
        }

        //private static void PrintAccountTransactions(string accountId, Dictionary<TransactionType, List<Transaction>> transactions)
        //{
        //    Console.WriteLine($"Account: {accountId}");
        //    foreach (var transactionType in transactions)
        //    {
        //        Console.WriteLine($"  Transaction Type: {transactionType.Key}");
        //        foreach (var transaction in transactionType.Value)
        //        {
        //            PrintTransaction(accountId, transaction);
        //        }
        //    }
        //}

        //private static void PrintTransaction(string accountId, Transaction transaction)
        //{
        //    Console.WriteLine($"    Account: {accountId}, ID: {transaction.TransID}, Amount: {transaction.Amount:C}, Date: {transaction.TranDate}, Status: {transaction.Status}");
        //}
    }
    }


