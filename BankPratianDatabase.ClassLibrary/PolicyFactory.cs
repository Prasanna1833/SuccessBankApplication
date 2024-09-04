
namespace BankPratianCommon.ClassLibrary
{
    //public class PolicyFactory
    //{
    //    private static PolicyFactory _instance;
    //    private readonly Dictionary<string, IPolicy> _policies = new Dictionary<string, IPolicy>();

    //    private PolicyFactory()
    //    {
    //        // In a real-world scenario, this would be loaded from a configuration file
    //        _policies["SAVINGS-REGULAR"] = new Policy(5000.0, 4.0);
    //        _policies["SAVINGS-GOLD"] = new Policy(25000.0, 4.25);
    //        _policies["SAVINGS-PREMIUM"] = new Policy(100000.0, 4.75);
    //        _policies["CURRENT-REGULAR"] = new Policy(25000.0, 2.0);
    //        _policies["CURRENT-GOLD"] = new Policy(100000.0, 2.25);
    //        _policies["CURRENT-PREMIUM"] = new Policy(300000.0, 2.75);
    //    }

    //    public static PolicyFactory GetInstance()
    //    {
    //        return _instance ??= new PolicyFactory();
    //    }

    //    public IPolicy CreatePolicy(string accType, string privilege)
    //    {
    //        string key = $"{accType}-{privilege}";
    //        if (!_policies.TryGetValue(key, out var policy))
    //            throw new InvalidPolicyTypeException($"Invalid policy type: {key}");
    //        return policy;
    //    }
    //}

    public class PolicyFactory
    {
        public static PolicyFactory _instance;
        private readonly Dictionary<string, IPolicy> _policies = new Dictionary<string, IPolicy>();

        private PolicyFactory()
        {
            LoadPoliciesFromFile("Policies.properties");
        }

        public static PolicyFactory GetInstance()
        {
            return _instance ??= new PolicyFactory();
        }

        public IPolicy CreatePolicy(string accType, string privilege)
        {
            string key = $"{accType}-{privilege}";
            if (!_policies.TryGetValue(key, out var policy))
                throw new InvalidPolicyTypeException($"Invalid policy type: {key}");
            return policy;
        }

        private void LoadPoliciesFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("The policies file could not be found.", filePath);
            }

            foreach (var line in File.ReadAllLines(filePath))
            {
                var parts = line.Split(':');
                if (parts.Length != 2)
                    throw new FormatException($"Invalid line format in policies file: {line}");

                var key = parts[0].Trim();
                var values = parts[1].Trim().Split(',');

                if (values.Length != 2)
                    throw new FormatException($"Invalid values format in policies file for key: {key}");

                if (double.TryParse(values[0], out var amount) &&
                    double.TryParse(values[1], out var rate))
                {
                    _policies[key] = new Policy(amount, rate);
                }
                else
                {
                    throw new FormatException($"Invalid amount or rate format in policies file for key: {key}");
                }
            }


        }
        public static Dictionary<string, IPolicy> GetPolicies()
        {
            return GetInstance()._policies;
        }
    }
}


