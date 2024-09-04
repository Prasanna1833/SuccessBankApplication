using NLog;
using BankPratianCommon.ClassLibrary;
using PratianBankDatabase.ClassLibrary;
namespace BankOfPratian.ConsoleApp
{
    class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
       // private static Dictionary<string, IAccount> accounts = new Dictionary<string, IAccount>();
        private static AccountManager accountManager;
       // private static ExternalTransferService externalTransferService;

        static void Main(string[] args)
        {
            SetupDependencies();
           // externalTransferService.Start();

            while (true)
            {
                try
                {
                    DisplayMenu();
                    string choice = Console.ReadLine();
                    ProcessChoice(choice);
                }
                catch (Exception ex)
                {
                    LogError(ex);
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }

        private static void SetupDependencies()
        {
            var policyFactory = PolicyFactory.GetInstance();
            var privilegeManager = new AccountPrivilegeManager();
            accountManager = new AccountManager(policyFactory, privilegeManager);
           // externalTransferService = new ExternalTransferService();
        }

        private static void DisplayMenu()
        {
            Console.WriteLine("\n===== Banking System Menu =====");
            Console.WriteLine("1.  Open a new account");
            Console.WriteLine("2.  Close an account");
            Console.WriteLine("3.  Deposit money");
            Console.WriteLine("4.  Withdraw money");
            Console.WriteLine("5.  Transfer funds");
            Console.WriteLine("6.  External transfer");
            Console.WriteLine("7.  Check account balance");
            Console.WriteLine("8.  View transaction history");
            Console.WriteLine("9.  Print all transactions");
            Console.WriteLine("10. Display all transfers");
            Console.WriteLine("11. Display all withdrawals");
            Console.WriteLine("12. Display all deposits");
            Console.WriteLine("13. Get total number of accounts");
            Console.WriteLine("14. Get number of accounts by type");
            Console.WriteLine("15. Total worth of the bank");
            Console.WriteLine("16. Display Policy Info");
            Console.WriteLine("17. Display all transactions for today");
            Console.WriteLine("18. Exit");
            Console.Write("Enter your choice (1-18): ");
        }

        private static void ProcessChoice(string choice)
        {
            switch (choice)
            {
                case "1": OpenNewAccount(); break;
                case "2": CloseAccount(); break;
                case "3": DepositMoney(); break;
                case "4": WithdrawMoney(); break;
                case "5": TransferFunds(); break;
                case "6": ExternalTransfer(); break;
                case "7": CheckAccountBalance(); break;
                case "8": ViewTransactionHistory(); break;
                case "9": ResultGenerator.PrintAllLogTransactions(); break;
                case "10": ResultGenerator.DisplayAllTransfers(); break;
                case "11": ResultGenerator.DisplayAllWithdrawals(); break;
                case "12": ResultGenerator.DisplayAllDeposits(); break;
                case "13": int totalAccount = ResultGenerator.GetTotalNoOfAccounts();
                    Console.WriteLine($"Total number of accounts : {totalAccount}");
                    break;
                case "14": ResultGenerator.DisplayNoOfAccTypeWise(); break;
                case "15": ResultGenerator.DispTotalWorthOfBank(); break;
                case "16": ResultGenerator.DispPolicyInfo(); break;
                case "17": ResultGenerator.DisplayAllTransactionsForDay(); break;
                case "18": Exit(); break;
                default: Console.WriteLine("Invalid choice. Please try again."); break;
            }
        }

        private static void OpenNewAccount()
        {
            Console.Write("Enter name: ");
            string name = Console.ReadLine();
            Console.Write("Enter PIN: ");
            string pin = Console.ReadLine();
            Console.Write("Enter initial balance: ");
            double balance = double.Parse(Console.ReadLine());
            Console.Write("Enter privilege type (REGULAR/GOLD/PREMIUM): ");
            PrivilegeType privilegeType = (PrivilegeType)Enum.Parse(typeof(PrivilegeType), Console.ReadLine(), true);
            Console.Write("Enter account type (SAVINGS/CURRENT): ");
            AccountType accType = (AccountType)Enum.Parse(typeof(AccountType), Console.ReadLine(), true);

            IAccount newAccount = accountManager.CreateAccount(name, pin, balance, privilegeType, accType);
           // accounts[newAccount.AccNo] = newAccount;
            Console.WriteLine($"Account created successfully. Account number: {newAccount.AccNo}");
        }

        private static void CloseAccount()
        {
            Console.Write("Enter account number: ");
            string accNo = Console.ReadLine();
            IAccount account = accountManager.FindAccount(accNo);
            if (account != null)
            {
                account.Close();
               // accounts.Remove(accNo);
                Console.WriteLine("Account closed successfully.");
            }
            else
            {
                Console.WriteLine("Account not found.");
            }
        }

        private static void DepositMoney()
        {
            Console.Write("Enter account number: ");
            string accNo = Console.ReadLine();
            IAccount account = accountManager.FindAccount(accNo);
            if (account != null)
            {
                Console.Write("Enter amount to deposit: ");
                double amount = double.Parse(Console.ReadLine());
                accountManager.Deposit(account, amount);
                Console.WriteLine("Deposit successful.");
            }
            else
            {
                Console.WriteLine("Account not found.");
            }
        }

        private static void WithdrawMoney()
        {
            Console.Write("Enter account number: ");
            string accNo = Console.ReadLine();
            IAccount account = accountManager.FindAccount(accNo);
            if (account != null)
            {
                Console.Write("Enter amount to withdraw: ");
                double amount = double.Parse(Console.ReadLine());
                Console.Write("Enter PIN: ");
                string pin = Console.ReadLine();
                accountManager.Withdraw(account, amount, pin);
                Console.WriteLine("Withdrawal successful.");
            }
            else
            {
                Console.WriteLine("Account not found.");
            }
        }

        private static void TransferFunds()
        {
            Console.Write("Enter source account number: ");
            string fromAccNo = Console.ReadLine();
            IAccount fromAcc = accountManager.FindAccount(fromAccNo);
            Console.Write("Enter destination account number: ");
            string toAccNo = Console.ReadLine();
            IAccount toAcc = accountManager.FindAccount(toAccNo);
            if (fromAcc != null && toAcc != null)
            {
                Console.Write("Enter amount to transfer: ");
                double amount = double.Parse(Console.ReadLine());
                Console.Write("Enter PIN: ");
                string pin = Console.ReadLine();
                accountManager.TransferFunds(fromAcc, toAcc, amount, pin);
                Console.WriteLine("Transfer successful.");
            }
            else
            {
                Console.WriteLine("One or both accounts not found.");
            }
        }

        private static void ExternalTransfer()
        {
            Console.Write("Enter source account number: ");
            string fromAccNo = Console.ReadLine();
            IAccount account = accountManager.FindAccount(fromAccNo);
            if (account != null)
            {
                Console.Write("Enter external account number: ");
                string toExternalAcc = Console.ReadLine();
                Console.Write("Enter bank code: ");
                string bankCode = Console.ReadLine();
                Console.Write("Enter amount to transfer: ");
                double amount = double.Parse(Console.ReadLine());
                Console.Write("Enter PIN: ");
                string pin = Console.ReadLine();
                ExternalAccount extAcc = new ExternalAccount
                {
                    AccNo = toExternalAcc,
                    BankCode = bankCode,
                    BankName = $"{bankCode}Bank"
                };
                ExternalTransfer extTransfer = new ExternalTransfer
                (
                     account,
                     amount,
                   extAcc,
                     pin

                );
                accountManager.ExternalTransfer(extTransfer);
                Console.WriteLine("External transfer initiated successfully.");
            }
            else
            {
                Console.WriteLine("Account not found.");
            }
        }

        private static void CheckAccountBalance()
        {
            Console.Write("Enter account number: ");
            string accNo = Console.ReadLine();
            IAccount account = accountManager.FindAccount(accNo);
            if (account != null)
            {
                Console.WriteLine($"Current balance: {account.Balance}");
            }
            else
            {
                Console.WriteLine("Account not found.");
            }
        }

        private static void ViewTransactionHistory()
        {
            //Console.Write("Enter account number: ");
            //string accNo = Console.ReadLine();
            //IAccount account = accountManager.FindAccount(accNo);
            //if (account != null)
            //{
            //    var transactions = accountManager.FindTransactionForAccount(accNo);
            //    foreach (var typeTransactions in transactions)
            //    {
            //        Console.WriteLine($"Transaction Type: {typeTransactions.Key}");
            //        foreach (var transaction in typeTransactions.Value)
            //        {
            //            Console.WriteLine($"  Amount: {transaction.Amount:C}, Date: {transaction.TranDate}, Status: {transaction.Status}");
            //        }
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("Account not found.");
            //}
        }

        private static void Exit()
        {
            //externalTransferService.Stop();
            Console.WriteLine("Thank you for using our banking system. Goodbye!");
            Environment.Exit(0);
        }

        private static void LogError(Exception ex, string userId = null, string action = null, string customMessage = null)
        {
            var logEvent = new LogEventInfo(LogLevel.Error, Logger.Name, ex.Message)
            {
                Exception = ex
            };
            logEvent.Properties["UserId"] = userId;
            logEvent.Properties["Action"] = action;
            logEvent.Properties["CustomMessage"] = customMessage;

            Logger.Log(logEvent);
        }
    }
}


