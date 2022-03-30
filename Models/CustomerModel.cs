using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DataBaseTest.Utilities;
#nullable enable
namespace DataBaseTest.Models
{
    public class CustomerModel
    {
        public override int GetHashCode()
        {
            var hash = -1;
            hash &= (Person == null ? 0 : Person.GetHashCode()) & (BVN ?? "0").GetHashCode() &
                EntryDate.GetHashCode();
            return hash;
        }

        public override bool Equals(object? obj)
        {
            if (obj is CustomerModel c)
            {
                var val = EntryDate == c.EntryDate;
                val = val && (BVN == null ? c.BVN == null : BVN.CompareTo(c.BVN) == 0) && (Person == null ? c.Person == null : Person.Equals(c.Person));
                return val;
            }
            return false;
        }

        public PersonModel? Person { get; set; }
        public LoginCredentialModel? LoginCredential { get; set; }

        [Key]
        public int? CustomerModelId { get; set; }

        [ForeignKey(nameof(Person))]
        public int? PersonModelId { get; set; }
        [ForeignKey(nameof(LoginCredential))]
        public int? LoginCredentialModelId { get; set; }
        //[NotMapped]
        public List<AccountModel>? Accounts
        {
            //get => JsonSerializer.Deserialize<List<AccountModel>>(_accounts);
            //set
            //{
            //    _accounts = JsonSerializer.Serialize<List<AccountModel>>(value);
            //}
            get;
            set;
        }
        public string? BVN { get; set; }
        //[Column(TypeName = "DateTime2")]
        public DateTime EntryDate { get; set; }

        //public string _accounts { get; set; }
    }
}
