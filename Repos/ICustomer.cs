using System;
using System.Collections.Generic;

namespace DataBaseTest.Repos
{
    public interface ICustomer
    {
        IPerson Person { get; }

        string BVN { get; }
        DateTime EntryDate { get; }
        ILoginCredentials LoginCredentials { get; }
        IEnumerable<IAccount> Accounts { get; }

        IAccount this[string number]
        {
            get
            {
                foreach (var account in Accounts)
                {
                    if (account.Number.Equals(number))
                        return account;
                }
                return null;
            }
        }
    }
}
