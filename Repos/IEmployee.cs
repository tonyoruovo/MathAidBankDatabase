using DataBaseTest.Models;
using System;
using System.Collections.Generic;

namespace DataBaseTest.Repos
{
    public interface IEmployee
    {
        IPerson Person { get; }

        //string DatabaseId { get; }

        string Level { get; }
        string Position { get; }
        decimal Salary { get; }

        ILoginCredentials Secret { get; }

        IEnumerable<IEmployee> Group { get; }
        IEmployee Supervisor { get; }
        IEmployee Superior { get; }
        IEmployee Subordinate { get; }

        DateTime HireDate { get; }
        WorkingStatus WorkingStatus { get; }

        IEducation Qualification { get; }
        IPerson Guarantor { get; }
        IAccount Account { get; }
    }
}
