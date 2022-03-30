using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json;
using static DataBaseTest.Utilities;
#nullable enable
namespace DataBaseTest.Models
{
    public class EducationModel
    {
        public override int GetHashCode()
        {
            return -1 & Hash(Primary) & Hash(Secondary) & Hash(PrimaryTertiary) & CombineHash(Others);
        }

        public override bool Equals(object? obj)
        {
            if (obj is EducationModel e)
            {
                return IsEqual(Primary, e.Primary)
                    && IsEqual(Secondary, e.Secondary)
                    && IsEqual(PrimaryTertiary, e.PrimaryTertiary)
                    && IsEqual(Others, e.Others);
            }
            return false;
        }

        public QualificationModel? Primary { get; set; }
        public QualificationModel? Secondary { get; set; }
        public QualificationModel? PrimaryTertiary { get; set; }

        //[NotMapped]
        public List<QualificationModel>? Others
        {
            //get => JsonSerializer.Deserialize<List<QualificationModel>>(_others);
            //set
            //{
            //    _others = JsonSerializer.Serialize<List<QualificationModel>>(value);
            //}
            get; set;
        }

        [Key]
        public int? EducationModelId { get; set; }

        [ForeignKey(nameof(Primary))]
        public int? PrimaryQualificationModelId { get; set; }
        [ForeignKey(nameof(Secondary))]
        public int? SecondaryQualificationModelId { get; set; }
        [ForeignKey(nameof(PrimaryTertiary))]
        public int? PrimaryTertiaryQualificationModelId { get; set; }
        //[ForeignKey(nameof(EmployeeModel.Qualification))]
        //public int QualificationId { get; set; }

        //public string _others { get; set; }
    }

    public class QualificationModel
    {
        public override int GetHashCode()
        {
            return -1 & Hash(Academy) & Hash(Certification) & CombineHash(Grades);
        }

        public override bool Equals(object? obj)
        {
            if (obj is QualificationModel q)
            {
                if (Grades != null && q.Grades != null)
                    foreach (var grade in Grades)
                    {
                        if (!q.Grades.Contains(grade)) return false;
                    }
                else if (q.Grades == null && Grades != null)
                    return false;
                else if (Grades == null && q.Grades != null)
                    return false;

                return (Certification == null ? q.Certification == null : Certification.CompareTo(q.Certification) == 0)
                    && IsEqual(Academy, q.Academy);
            }
            return false;
        }

        public Dictionary<string, string>? Grades
        {
            get;
            set;
        }

        public string? Certification { get; set; }

        public AddressModel? Academy { get; set; }

        [Key]
        public int? QualificationModelId { get; set; }

        [ForeignKey(nameof(Academy))]
        public int? AcademyAddressModelId { get; set; }
        //[ForeignKey(nameof(EducationModel.Primary))]
        //public int PrimaryId { get; set; }
        //[ForeignKey(nameof(EducationModel.Secondary))]
        //public int SecondaryId { get; set; }
        //[ForeignKey(nameof(EducationModel.PrimaryTertiary))]
        //public int PrimaryTertiaryId { get; set; }
        //[ForeignKey(nameof(EducationModel.Others))]
        //public int OthersId { get; set; }

        //public string? _grades { get; set; } //= JsonSerializer.Serialize(new Dictionary<string, string>());
    }
}