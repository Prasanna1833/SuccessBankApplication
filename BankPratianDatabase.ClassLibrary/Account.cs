
namespace BankPratianCommon.ClassLibrary
{
    // Base Account class
    public abstract class Account : IAccount
    {
        public string AccNo { get;  set; }
        public string Name { get; set; }
        public string Pin { get; set; }
        public bool Active { get; set; }
        public DateTime DateOfOpening { get; set; }
        public double Balance { get; set; }
        public PrivilegeType PrivilegeType { get; set; }
        public IPolicy Policy { get; set; }

        protected Account(string accNoPrefix)
        {
            AccNo = accNoPrefix + IDGenerator.GenerateID();
        }

        public abstract string GetAccType();
        public virtual bool Open()
        {
            Active = true;
            DateOfOpening = DateTime.Now;
            return true;
        }

        public virtual bool Close()
        {
            Active = false;
            Balance = 0;
            return true;
        }

        public IPolicy GetPolicy() => Policy;
    }
}


