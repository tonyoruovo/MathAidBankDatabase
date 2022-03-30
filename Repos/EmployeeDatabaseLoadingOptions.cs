using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBaseTest.Repos
{
    public enum EmployeeDatabaseLoadingOptions
    {

        Person,
        /// <summary>
        /// Will never load the password field
        /// </summary>
        Secret,
        /// <summary>
        /// Will load all fields
        /// </summary>
        Guarantor,
        /// <summary>
        /// Will Load all fields
        /// </summary>
        Qualification,
        /// <summary>
        /// Will not load contact
        /// </summary>
        Entity,
        /// <summary>
        /// Will not load contact
        /// </summary>
        OfficialEntity,
        /// <summary>
        /// Will load all fields
        /// </summary>
        FullName,
        /// <summary>
        /// Will load all fields
        /// </summary>
        CountryOfOrigin,
        /// <summary>
        /// Will load all fields
        /// </summary>
        NextOfKin,
        /// <summary>
        /// Will load all fields
        /// </summary>
        Contact
    }
}
