using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DataBaseTest.Utilities;
using System.Text.Json;
#nullable enable
namespace DataBaseTest.Models
{
    public class FullNameModel
    {
        public override int GetHashCode()
        {
            return -1 & (Name == null ? 0 : Name.GetHashCode()) &
                (Surname == null ? 0 : Surname.GetHashCode()) &
                (Title == null ? 0 : Title.GetHashCode()) &
                (Nickname == null ? 0 : Nickname.GetHashCode()) &
                (Hash(MaidenName)) &
                CombineHash(OtherNames);
        }

        public override bool Equals(object? obj)
        {
            if (obj is FullNameModel f)
            {
                return (Name == null ? f.Name == null : Name.Equals(f.Name))
                    && (Surname == null ? f.Surname == null : Surname.Equals(f.Surname))
                    && (Title == null ? f.Title == null : Title.CompareTo(f.Title) == 0)
                    && (Nickname == null ? f.Nickname == null : Nickname.Equals(f.Nickname))
                    && IsEqual(OtherNames, f.OtherNames);
            }
            return false;
        }

        public string? Title { get; set; }
        public NameModel? Name { get; set; }
        public NameModel? Surname { get; set; }
        public NameModel? Nickname { get; set; }
        public NameModel? MaidenName { get; set; }

        //[NotMapped]
        public List<NameModel>? OtherNames
        {
            get;// => _otherNames == null ? new() :  JsonSerializer.Deserialize<List<NameModel>>(_otherNames);
            set;
            //{
            //    _otherNames = JsonSerializer.Serialize(value);
            //}
        }

        [Key]
        public int? FullNameModelId { get; set; }

        [ForeignKey(nameof(Name))]
        public int? NameModelId { get; set; }
        [ForeignKey(nameof(Surname))]
        public int? SurnameNameModelId { get; set; }
        [ForeignKey(nameof(Nickname))]
        public int? NicknameNameModelId { get; set; }
        [ForeignKey(nameof(MaidenName))]
        public int? MaidenNameNameModelId { get; set; }

        //[ForeignKey(nameof(PersonModel.FullName))]
        //public int PersonNameId { get; set; }

        //private string? _otherNames { get; set; }// = JsonSerializer.Serialize(new List<NameModel>());
    }
}