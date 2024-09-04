
namespace BankPratianCommon.ClassLibrary
{
    // Policy class
    public class Policy : IPolicy
    {
        private readonly double _minBalance;
        private readonly double _rateOfInterest;

        public Policy(double minBalance, double rateOfInterest)
        {
            _minBalance = minBalance;
            _rateOfInterest = rateOfInterest;
        }

        public double GetMinBalance() => _minBalance;
        public double GetRateOfInterest() => _rateOfInterest;
    }
}


