using BankPratianCommon.ClassLibrary;
namespace BankOfPratian.ConsoleApp
{
    public class ExternalTransfer : Transaction
        {
            public ExternalAccount ToExternalAcc { get; set; }
            public string FromAccPin { get; set; }

            public ExternalTransfer(IAccount fromAccount, double amount, ExternalAccount toExternalAcc, string fromAccPin)
                : base(fromAccount, amount)
            {
                ToExternalAcc = toExternalAcc;
                FromAccPin = fromAccPin;
                Status = TransactionStatus.CLOSE;
            }
        }
    }


