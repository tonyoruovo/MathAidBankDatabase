using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using static DataBaseTest.Utilities;
#nullable enable
namespace DataBaseTest.Models
{
    public class PersonModel
    {
        public override int GetHashCode()
        {
            var hash = -1 & UniqueTag.GetHashCode() & _birthDate.GetHashCode() & MaritalStatus.GetHashCode();
            hash &= (Entity == null ? 0 : Entity.GetHashCode()) &
                (FullName == null ? 0 : FullName.GetHashCode()) &
                (CountryOfOrigin == null ? 0 : CountryOfOrigin.GetHashCode()) &
                Hash(OfficialEntity) & Hash(Passport) & Hash(Signature) & Hash(FingerPrint);
            return hash;
        }

        public override bool Equals(object? obj)
        {
            if (obj is PersonModel p)
            {
                var equals = UniqueTag == p.UniqueTag && _birthDate == p._birthDate && MaritalStatus == p.MaritalStatus;
                equals = equals && (Entity == null ? p.Entity == null : Entity.Equals(p.Entity)) &&
                    (FullName == null ? p.FullName == null : FullName.Equals(p.FullName)) &&
                    (CountryOfOrigin == null ? p.CountryOfOrigin == null : CountryOfOrigin.Equals(p.CountryOfOrigin))
                    && IsEqual(OfficialEntity, p.OfficialEntity)
                    && IsEqual(Passport, p.Passport) && IsEqual(Signature, p.Signature)
                    && IsEqual(FingerPrint, p.FingerPrint);
                return equals;
            }
            return false;
        }

        public EntityModel? Entity { get; set; }
        public EntityModel? OfficialEntity { get; set; }
        public FullNameModel? FullName { get; set; }
		
        public NationalityModel? CountryOfOrigin { get; set; }
        public Guid UniqueTag { get; set; }
        //[Column(TypeName = "DateTime2")]
        [NotMapped]
        public DateTime BirthDate { get => _birthDate; set => SetDateOfBirth(value); }
        public MaritalStatus MaritalStatus { get; set; }
        public bool IsMale { get; set; }
        public string? JobType { get; set; }
        public byte[]? Passport { get; set; }
        public byte[]? Signature { get; set; }
        public byte[]? FingerPrint { get; set; }
        //public byte[] FacialData { get; set; }
        //public byte[] RetinaData { get; set; }
        //public byte[] DNA { get; set; }

        //[NotMapped]
        public PersonModel? NextOfKin
        {
            get;set;
        }
        //[NotMapped]
        public Dictionary<string, string>? Identification
        {
            get;set;
        }
        [Key]
        public int? PersonModelId { get; set; }

        [ForeignKey(nameof(CountryOfOrigin))]
        public int? CountryOfOriginNationalityModelId { get; set; }
        [ForeignKey(nameof(FullName))]
        public int? FullNameModelId { get; set; }
        [ForeignKey(nameof(OfficialEntity))]
        public int? OfficialEntityEntityModelId { get; set; }
        [ForeignKey(nameof(Entity))]
        public int? EntityModelId { get; set; }
        //[ForeignKey(nameof(EmployeeModel.Person))]
        //public int EmployeePersonId { get; set; }

        //[ForeignKey(nameof(EmployeeModel.Guarantor))]
        //public int EmployeeGuarantorId { get; set; }

        //[ForeignKey(nameof(PersonModel.NextOfKin))]
        //public int NextOfKinId { get; set; }

        //[ForeignKey(nameof(AccountModel.Guarantor))]
        //public int AccountGuarantorId { get; set; }

        //[ForeignKey(nameof(CustomerModel.Person))]
        //public int CustomerPersonId { get; set; }

        public void SetDateOfBirth(DateTime dateOfBirth)
        {
            _birthDate = dateOfBirth;
        }

        public byte Age() => (byte)Years(_birthDate, DateTime.Today);

        private static int Years(DateTime start, DateTime end)
        {
            return (end.Year - start.Year - 1) +
            (((end.Month > start.Month) ||
            ((end.Month == start.Month)
            && (end.Day >= start.Day)))
            ? 1 : 0);
        }

        //public string? _identification { get; set; }// = JsonSerializer.Serialize(new Dictionary<string, string>());
        //public string? _nextOfKin { get; set; }

        private DateTime _birthDate;
    }
}