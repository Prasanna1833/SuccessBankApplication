using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankPratianCommon.ClassLibrary
{
    public interface IAccount
    {
        string AccNo { get; set; }
        string Name { get; set; }
        string Pin { get; set; }
        bool Active { get; set; }
        DateTime DateOfOpening { get; set; }
        double Balance { get; set; }
        PrivilegeType PrivilegeType { get; set; }
        IPolicy Policy { get; set; }

        string GetAccType();
        bool Open();
        bool Close();
        IPolicy GetPolicy();
    }
}
