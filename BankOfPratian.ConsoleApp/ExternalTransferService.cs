//using BankPratianCommon.ClassLibrary;
//namespace BankOfPratian.ConsoleApp
//{
//    // AccountManager


//    // External Transfer Service
//    public class ExternalTransferService
//    {
//        private CancellationTokenSource _cancellationTokenSource;

//        public void Start()
//        {
//            _cancellationTokenSource = new CancellationTokenSource();
//            Task.Run(() => Run(_cancellationTokenSource.Token), _cancellationTokenSource.Token);
//        }

//        public void Stop()
//        {
//            _cancellationTokenSource.Cancel();
//        }

//        private async Task Run(CancellationToken cancellationToken)
//        {
//            while (!cancellationToken.IsCancellationRequested)
//            {
//                try
//                {
//                    foreach (var accountTransactions in TransactionLog.GetTransactions())
//                    {
//                        foreach (var transaction in accountTransactions.Value[TransactionType.EXTERNALTRANSFER].Where(t => t.Status == TransactionStatus.OPEN))
//                        {
//                            var externalTransfer = (ExternalTransfer)transaction;
//                            var externalService = ExternalBankServiceFactory.GetInstance().GetBankService(externalTransfer.ToExternalAcc);

//                            if (externalService.Deposit(externalTransfer.ToExternalAcc, externalTransfer.Amount))
//                            {
//                                externalTransfer.FromAccount.Balance -= externalTransfer.Amount;
//                                externalTransfer.Status = TransactionStatus.CLOSE;
//                                // Update the account and transaction logs as needed
//                            }
//                        }
//                    }
//                }
//                catch (Exception ex)
//                {
//                    // Log or handle exceptions as needed
//                }

//                await Task.Delay(TimeSpan.FromHours(1), cancellationToken); // Check every hour
//            }
//        }
//    }
//    }


