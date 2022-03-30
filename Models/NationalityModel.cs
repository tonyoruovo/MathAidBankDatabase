using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DataBaseTest.Utilities;
#nullable enable
namespace DataBaseTest.Models
{
    public class NationalityModel
    {
        public override int GetHashCode()
        {
            return -1 & (CountryName == null ? 0 : CountryName.GetHashCode()) &
                (Language == null ? 0 : Language.GetHashCode()) &
                (State == null ? 0 : State.GetHashCode()) &
                (LGA == null ? 0 : LGA.GetHashCode()) &
                (CityTown == null ? 0 : CityTown.GetHashCode());
        }

        public override bool Equals(object? obj)
        {
            if (obj is NationalityModel n)
            {
                return (CountryName == null ? n.CountryName == null : CountryName.Equals(n.CountryName))
                    && (Language == null ? n.Language == null : Language.CompareTo(n.Language) == 0)
                    && (State == null ? n.State == null : State.Equals(n.State))
                    && (LGA == null ? n.LGA == null : LGA.CompareTo(n.LGA) == 0)
                    && (CityTown == null ? n.CityTown == null : CityTown.Equals(n.CityTown));
            }
            return false;
        }

        public NameModel? CountryName { get; set; }
        public string? Language { get; set; }
        public NameModel? State { get; set; }
        public string? LGA { get; set; }
        public NameModel? CityTown { get; set; }

        [Key]
        public int? NationalityModelId { get; set; }

        [ForeignKey(nameof(CountryName))]
        public int? CountryNameNameModelId { get; set; }
        [ForeignKey(nameof(State))]
        public int? StateNameModelId { get; set; }
        [ForeignKey(nameof(CityTown))]
        public int? CityTownNameModelId { get; set; }

        //[ForeignKey(nameof(PersonModel.CountryOfOrigin))]
        //public int CountryOfOriginId { get; set; }

        //[ForeignKey(nameof(AddressModel.CountryOfResidence))]
        //public int CountryOfResidenceId { get; set; }

        //[ForeignKey(nameof(CardModel.CountryOfUse))]
        //public int CountryOfUseId { get; set; }
    }
}