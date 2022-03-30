using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using static DataBaseTest.Utilities;
#nullable enable
namespace DataBaseTest.Models
{
    public class EmployeeModel
    {
        public override int GetHashCode()
        {
            var hashCode = -1 & HireDate.GetHashCode() & WorkingStatus.GetHashCode() & (((object)(Person == null ? Int32.Parse("0") : Person)).GetHashCode()); //for null properties
            hashCode &= Level == null ? 0 : Level.GetHashCode();
            return hashCode;
        }

        public override bool Equals(object? obj)
        {
            if (obj is EmployeeModel e)
            {
                var isEquals = HireDate == e.HireDate && WorkingStatus == e.WorkingStatus;
                isEquals = isEquals && (Person == null ? e.Person == null : Person.Equals(e.Person));
                return isEquals;
            }
            return false;
        }

        public PersonModel? Person { get; set; }
        public PersonModel? Guarantor { get; set; }
        public LoginCredentialModel? Secret { get; set; }
        public EducationModel? Qualification { get; set; }

        public string? Level { get; set; }
        public string? Position { get; set; }
        //[Column(TypeName = "decimal(18, 2)")]
        public decimal Salary { get; set; }
        //public string Username { get; set; }
        //public string Password { get; set; }//should be encrypted

        //[NotMapped]
        public List<EmployeeModel>? Group
        {
            get;
            set;
        }
        //[NotMapped]
        public EmployeeModel? Supervisor
        {
            get;
            set;
        }
        //[NotMapped]
        public EmployeeModel? Superior
        {
            get;set;
        }
        //[NotMapped]
        public EmployeeModel? Subordinate
        {
            get;set;
        }
        //[NotMapped]
        //[JsonIgnore]
        public AccountModel? Account
        {
            get;
            set;
        }

        //[Column(TypeName = "DateTime2")]
        public DateTime HireDate { get; set; }
        public WorkingStatus WorkingStatus { get; set; }


        [Key]
        public int? EmployeeModelId { get; set; }

        [ForeignKey(nameof(Person))]
        public int? PersonModelId { get; set; }
        [ForeignKey(nameof(Guarantor))]
        public int? GuarantorPersonModelId { get; set; }
        [ForeignKey(nameof(Secret))]
        public int? SecretLoginCredentialModelId { get; set; }
        [ForeignKey(nameof(Qualification))]
        public int? QualificationEducationModelId { get; set; }

        //public string? _group { get; set; }
        //public string? _supervisor { get; set; }
        //public string? _superior { get; set; }
        //public string? _subordinate { get; set; }
        //public string _guarantor { get; set; }
        //public string? _account { get; set; }
    }
}
