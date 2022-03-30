using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DataBaseTest.Utilities;
#nullable enable
namespace DataBaseTest.Models
{
    public class AddressModel
    {
        public NationalityModel? CountryOfResidence { get; set; }
        public string? StreetAddress { get; set; }
        public string? ZIPCode { get; set; }
        public string? PMB { get; set; }

        [Key]
        public int? AddressModelId { get; set; }

        [ForeignKey(nameof(CountryOfResidence))]
        public int? CountryOfResidenceNationalityModelId { get; set; }
        //[ForeignKey(nameof(ContactModel.Address))]
        //public int AddressId { get; set; }

        //[ForeignKey(nameof(QualificationModel.Academy))]
        //public int AcademyId { get; set; }

        public override int GetHashCode()
        {
            var hash = -1;
            hash &= (CountryOfResidence == null ? 0 : CountryOfResidence.GetHashCode()) &
                (StreetAddress == null ? 0 : StreetAddress.GetHashCode()) &
                (ZIPCode == null ? 0 : ZIPCode.GetHashCode()) &
                (PMB == null ? 0 : PMB.GetHashCode());
            return hash;
        }

        public override bool Equals(object? obj)
        {
            if (obj is AddressModel a)
            {
                var isEqual = (CountryOfResidence == null ? a.CountryOfResidence == null : CountryOfResidence.Equals(a.CountryOfResidence))
                    && (StreetAddress == null ? a.StreetAddress == null : StreetAddress.CompareTo(a.StreetAddress) == 0)
                    && (ZIPCode == null ? a.ZIPCode == null : ZIPCode.CompareTo(a.ZIPCode) == 0)
                    && (PMB == null ? a.PMB == null : PMB.CompareTo(a.PMB) == 0);
                return isEqual;
            }
            return false;
        }
    }
}