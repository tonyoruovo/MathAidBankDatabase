using DataBaseTest.Repos;
using System;
using System.Collections.Generic;

namespace DataBaseTest.Controllers
{
    public class SignedEmployee : IEmployee
    {
        private readonly IEmployee _implementor;
        public string Token { get; set; }
        public int Id { get; }

        public IPerson Person => _implementor.Person;

        public string Level => _implementor.Level;

        public string Position => _implementor.Position;

        public decimal Salary => _implementor.Salary;

        public ILoginCredentials Secret => _implementor.Secret;

        public IEnumerable<IEmployee> Group => _implementor.Group;

        public IEmployee Supervisor => _implementor.Supervisor;

        public IEmployee Superior => _implementor.Superior;

        public IEmployee Subordinate => _implementor.Subordinate;

        public DateTime HireDate => _implementor.HireDate;

        public Models.WorkingStatus WorkingStatus => _implementor.WorkingStatus;

        public IEducation Qualification => _implementor.Qualification;

        public IPerson Guarantor => _implementor.Guarantor;

        public IAccount Account => _implementor.Account;

        public SignedEmployee(IEmployee employeeToSign, int id)
        {
            _implementor = employeeToSign;
            this.Id = id;
        }
    }
}