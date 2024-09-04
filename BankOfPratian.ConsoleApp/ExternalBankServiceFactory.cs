using BankPratianCommon.ClassLibrary;
namespace BankOfPratian.ConsoleApp
{
    // External Bank Service Factory
    public class ExternalBankServiceFactory
    {
        private static ExternalBankServiceFactory _instance;
        private readonly Dictionary<string, IExternalBankService> _serviceBankPool = new Dictionary<string, IExternalBankService>();

        private ExternalBankServiceFactory()
        {
            LoadBankServices("serviceBanks.properties");
        }

        public static ExternalBankServiceFactory GetInstance()
        {
            return _instance ??= new ExternalBankServiceFactory();
        }

        private void LoadBankServices(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("The service banks file could not be found.", filePath);
            }

            foreach (var line in File.ReadAllLines(filePath))
            {
                var parts = line.Split(':');
                if (parts.Length != 2)
                    throw new FormatException($"Invalid line format in service banks file: {line}");

                var bankCode = parts[0].Trim();
                var className = parts[1].Trim();

                var bankService = (IExternalBankService)Activator.CreateInstance(Type.GetType(className) ?? throw new InvalidOperationException($"Class not found: {className}"));
                _serviceBankPool[bankCode] = bankService;
            }
        }

        public IExternalBankService GetBankService(string bankCode)
        {
            Console.WriteLine("fetching");
            if (!_serviceBankPool.TryGetValue(bankCode, out var service))
                throw new InvalidBankCodeException($"Invalid bank code: {bankCode}");
            Console.WriteLine("fetched");

            return service;
        }
    }
}


