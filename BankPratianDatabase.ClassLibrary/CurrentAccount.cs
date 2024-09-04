
namespace BankPratianCommon.ClassLibrary
{
    public class CurrentAccount : Account
    {
        public CurrentAccount() : base("CUR") { }
        public override string GetAccType() => "Current";
    }
}


