using DataBaseTest.Repos;
using System;
using System.Collections.Generic;

namespace DataBaseTest.Controllers
{
    public class SignedCustomer : ICustomer
    {
        public string Token { get; set; }
        public int Id { get; }

        public IPerson Person => _implementor.Person;

        public string BVN => _implementor.BVN;

        public DateTime EntryDate => _implementor.EntryDate;

        public ILoginCredentials LoginCredentials => _implementor.LoginCredentials;

        public IEnumerable<IAccount> Accounts => _implementor.Accounts;

        private ICustomer _implementor;

        public SignedCustomer(ICustomer customer, int id)
        {
            _implementor = customer;
            Id = id;
        }
    }
}
