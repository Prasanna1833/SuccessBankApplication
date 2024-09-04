using BankPratianCommon.ClassLibrary;
namespace BankOfPratian.ConsoleApp
{
    public class AccountPrivilegeManager
    {
        private static readonly Dictionary<PrivilegeType, double> DailyLimits = new Dictionary<PrivilegeType, double>
        {
            { PrivilegeType.REGULAR, 100000.0 },
            { PrivilegeType.GOLD, 200000.0 },
            { PrivilegeType.PREMIUM, 300000.0 }
        };

        public double GetDailyLimit(PrivilegeType privilegeType)
        {
            if (!DailyLimits.TryGetValue(privilegeType, out double limit))
                throw new InvalidPrivilegeTypeException($"Invalid privilege type: {privilegeType}");
            return limit;
        }
    }
}


