using System.Net.Http.Headers;
using System.Transactions;
using BankPratianCommon.ClassLibrary;
using PratianBankDatabase.ClassLibrary;
using Transaction = BankPratianCommon.ClassLibrary.Transaction;
namespace BankOfPratian.ConsoleApp
{
    // Managers
    public class AccountManager
    {
        private readonly PolicyFactory _policyFactory;
        private readonly AccountPrivilegeManager _privilegeManager;
        //private readonly DatabaseManager _databaseManager;

        public AccountManager(PolicyFactory policyFactory, AccountPrivilegeManager privilegeManager)
        {
            _policyFactory = policyFactory;
            _privilegeManager = privilegeManager;
           // _databaseManager = databaseManager;
        }

        public IAccount FindAccount(string accountNo)
        {
            AccountDBRepository repo = new AccountDBRepository();
            return repo.GetAccoountByID(accountNo);
        }

        public void FindTransactionForAccount(string accNo)
        {
            AccountDBRepository repo = new AccountDBRepository();
            repo.FindAllTransactions(accNo);
        }

        public IAccount CreateAccount(string name, string pin, double balance, PrivilegeType privilegeType, AccountType accType)
        {
            var account = AccountFactory.CreateAccount(accType);
            account.Name = name;
            account.Pin = pin;
            account.PrivilegeType = privilegeType;
            account.Policy = _policyFactory.CreatePolicy(accType.ToString(), privilegeType.ToString());
            if (balance < account.Policy.GetMinBalance())
                throw new MinBalanceNeedsToBeMaintainedException("Initial balance does not meet minimum balance requirement.");
            account.Balance = balance;
            if (!account.Open())
                throw new UnableToOpenAccountException("Failed to open the account.");
            // _databaseManager.AddAccount(account);
           // ResultGenerator.AddAccount(account);
            // inserting into the database 
            AccountDBRepository Accountrepo = new AccountDBRepository();
            Accountrepo.Create(account);
            return account;
        }


        public bool Withdraw(IAccount account, double amount, string pin)
        {
            AccountDBRepository Accountrepo = new AccountDBRepository();
            TransactionDbRepository transactionRepo = new TransactionDbRepository();
            if (!account.Active)
                throw new InactiveAccountException("Account is inactive.");
            if (account.Pin != pin)
                throw new InvalidPinException("Invalid PIN.");
            //if (account.Balance - amount < account.Policy.GetMinBalance())
            //    throw new MinBalanceNeedsToBeMaintainedException("Withdrawal would breach minimum balance requirement.");
            account.Balance -= amount;
            var transaction = new BankPratianCommon.ClassLibrary.Transaction(account, amount, TransactionType.WITHDRAW);
            // _databaseManager.AddTransaction(transaction, account.AccNo);

           // TransactionLog.LogTransaction(account.AccNo, TransactionType.WITHDRAW, transaction);
            // database updation
            Accountrepo.DeductAmount(account.AccNo, account.Balance);
            transactionRepo.Create(transaction);
            return true;
        }

        public bool Deposit(IAccount account, double amount)
        {
            AccountDBRepository Accountrepo = new AccountDBRepository();
            TransactionDbRepository TransactionRepo = new TransactionDbRepository();
            if (!account.Active)
                throw new InactiveAccountException("Account is inactive.");
            account.Balance += amount;
            var transaction = new BankPratianCommon.ClassLibrary.Transaction(account, amount, TransactionType.DEPOSIT);
            //TransactionLog.LogTransaction(account.AccNo, TransactionType.DEPOSIT, transaction);
            //_databaseManager.AddTransaction(transaction, account.AccNo);
            // database updation  
            Accountrepo.AddAmount(account.AccNo, account.Balance);
            TransactionRepo.Create(transaction);
            return true;
        }


        public bool TransferFunds(IAccount fromAccount, IAccount toAccount, double amount, string pin)
        {
            AccountDBRepository repo = new AccountDBRepository();
            TransactionDbRepository transRepo = new TransactionDbRepository();
            if (amount > _privilegeManager.GetDailyLimit(fromAccount.PrivilegeType))
                throw new DailyLimitExceededException("Transfer amount exceeds daily limit.");
            if (!fromAccount.Active || !toAccount.Active)
                throw new InactiveAccountException("Either or both Account is inactive.");
            if (fromAccount.Pin != pin)
                throw new InvalidPinException("Invalid PIN.");
            int transactionId = IDGenerator.GenerateID();
            fromAccount.Balance -= amount;
            toAccount.Balance += amount;
            var transfer = new Transfer(fromAccount, toAccount, amount) { TransID = transactionId };
            BankPratianCommon.ClassLibrary.Transaction transacation = new Transaction(fromAccount, amount, TransactionType.TRANSFER);
            //TransactionLog.LogTransaction(fromAccount.AccNo, TransactionType.TRANSFER, transfer);
            //_databaseManager.AddTransaction(transfer, fromAccount.AccNo);
            // updation of database
            repo.TransferFunds(fromAccount.AccNo, toAccount.AccNo, fromAccount.Balance, toAccount.Balance);
            transRepo.Create(transacation);
             return true;
        }

        public bool ExternalTransfer(ExternalTransfer extTransfer)
        {
            AccountDBRepository repo = new AccountDBRepository();
            TransactionDbRepository transRepo = new TransactionDbRepository();
            ExternalBankRepository extRepo = new ExternalBankRepository();
            if (extTransfer.Amount > _privilegeManager.GetDailyLimit(extTransfer.FromAccount.PrivilegeType))
                throw new DailyLimitExceededException("Transfer amount exceeds daily limit.");
            if (!extTransfer.FromAccount.Active )
                throw new InactiveAccountException("Either or both Account is inactive.");
            if (extTransfer.FromAccount.Pin != extTransfer.FromAccPin)
                throw new InvalidPinException("Invalid PIN.");
            extTransfer.FromAccount.Balance -= extTransfer.Amount;
            //TransactionLog.LogTransaction(extTransfer.FromAccount.AccNo, TransactionType.EXTERNALTRANSFER, extTransfer);
            Transaction transaction = (Transaction)extTransfer;
            transaction.TransactionType = TransactionType.EXTERNALTRANSFER;
            transRepo.Create(transaction);
            //ExternalBankServiceFactory externalBankServiceFactory = ExternalBankServiceFactory.GetInstance();
            //Console.WriteLine("hi from external factory");
            //step 2. generate bankserice of the extenal bank using bankcode
           // IExternalBankService externalBankService = externalBankServiceFactory.GetBankService(extTransfer.ToExternalAcc.BankCode);
            //step 3.call the deposit function of the extenalbank
            // externalBankService.Deposit(extTransfer.ToExternalAcc.AccNo, extTransfer.Amount);
            extRepo.Update(extTransfer.ToExternalAcc, extTransfer.Amount, extTransfer.ToExternalAcc.BankName);
            repo.DeductAmount(extTransfer.FromAccount.AccNo, extTransfer.FromAccount.Balance);
            return true;
        }
    }
}


