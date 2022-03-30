using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBaseTest.Repos
{

    public interface IJWTSettings
    {
        public string SecretKey { get; }
        public string ValidIssuer { get; }
        public string ValidAudience { get; }
        public string Role { get; }
    }

    public class EmployeeJWTSettings : IJWTSettings
    {
        public string SecretKey { get; set; }
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public string Role { get; } = "Operator";
    }

    public class EmployeeAdminJWTSettings : IJWTSettings
    {
        public string SecretKey { get; set; }
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public string Role { get; } = "Maintenance";
    }

    public class AdminAdminJWTSettings : IJWTSettings
    {
        public string SecretKey { get; set; }
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public string Role { get; } = "Security";
    }

    public class VIPCustomerJWTSettings : IJWTSettings
    {
        public string SecretKey { get; set; }
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public string Role { get; } = "Administrator";
    }

    public class CustomerJWTSettings : IJWTSettings
    {
        public string SecretKey { get; set; }
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public string Role { get; } = "User";
    }

    public class JWTSettings : IJWTSettings
    {
        public string SecretKey { get; set; }
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public string Role { get; } = "Visitor";
    }
}
