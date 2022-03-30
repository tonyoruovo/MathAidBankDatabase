using System;
using System.ComponentModel.DataAnnotations;
#nullable enable
namespace DataBaseTest.Models
{
    public class NameModel : IComparable<NameModel>
    {
        public override int GetHashCode()
        {
            return -1 & (Name == null ? 0 : Name.GetHashCode());
        }

        public override bool Equals(object? obj)
        {
            if (obj is NameModel n)
                return Name == null ? n.Name == null : Name.CompareTo(n.Name) == 0;
            return false;
        }

#pragma warning disable CS8629 // Nullable value type may be null.
        public int CompareTo(NameModel? other)
        {
            var val = Name?.CompareTo(other?.Name);

            return val == null ? Name == null && other?.Name != null ? 1
                : other?.Name == null && Name != null ? -1 : Name == null && other?.Name == null ? 0
                : val.Value : val.Value;
        }
#pragma warning restore CS8629 // Nullable value type may be null.

        //[Required(AllowEmptyStrings = false)]
        //[MinLength(3)]
        public string? Name { get; set; }

        [Key]
        public int? NameModelId { get; set; }

        //[ForeignKey(nameof(FullNameModel.Name))]
        //public int NameId { get; set; }
        //[ForeignKey(nameof(FullNameModel.Surname))]
        //public int SurnameId { get; set; }
        //[ForeignKey(nameof(FullNameModel.OtherNames))]
        //public int OthernamesId { get; set; }
        //[ForeignKey(nameof(FullNameModel.Nickname))]
        //public int NicknameId { get; set; }
        //[ForeignKey(nameof(FullNameModel.MaidenName))]
        //public int MaidenNameId { get; set; }

        //[ForeignKey(nameof(EntityModel.Name))]
        //public int EntityNameId { get; set; }

        //[ForeignKey(nameof(NationalityModel.CountryName))]
        //public int CountryNameId { get; set; }
        //[ForeignKey(nameof(NationalityModel.State))]
        //public int StateId { get; set; }
        //[ForeignKey(nameof(NationalityModel.CityTown))]
        //public int CityTownId { get; set; }
    }
}