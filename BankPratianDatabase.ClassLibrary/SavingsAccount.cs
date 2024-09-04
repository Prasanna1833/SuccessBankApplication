
namespace BankPratianCommon.ClassLibrary
{
    // Concrete Account classes
    public class SavingsAccount : Account
    {
        public SavingsAccount() : base("SAV") { }
        public override string GetAccType() => "Savings";
    }
}


