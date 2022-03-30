using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DataBaseTest.Utilities;
#nullable enable
namespace DataBaseTest.Models
{
    public class EntityModel
    {
        public override int GetHashCode()
        {
            var hash = -1;
            hash &= (Name == null ? 0 : Name.GetHashCode()) & (Contact == null ? 0 : Contact.GetHashCode());
            return hash;
        }

        public override bool Equals(object? obj)
        {
            if (obj is EntityModel e)
            {
                var isEquals = (Name == null ? e.Name == null : Name.Equals(e.Name))
                    && (Contact == null ? e.Contact == null : Contact.Equals(e.Contact));
                return isEquals;
            }
            return false;
        }

        public NameModel? Name { get; set; }
        public ContactModel? Contact { get; set; }

        [Key]
        public int? EntityModelId { get; set; }

        [ForeignKey(nameof(Name))]
        public int? NameModelIdId { get; set; }
        [ForeignKey(nameof(Contact))]
        public int? ContactModelIdId { get; set; }
        //[ForeignKey(nameof(PersonModel.Entity))]
        //public int EntityId { get; set; }
        //[ForeignKey(nameof(PersonModel.OfficialEntity))]
        //public int OfficialEntityId { get; set; }

        //[ForeignKey(nameof(CardModel.Brand))]
        //public int BrandId { get; set; }

        //[ForeignKey(nameof(TransactionModel.Creditor))]
        //public int CreditorId { get; set; }
        //[ForeignKey(nameof(TransactionModel.Debitor))]
        //public int DebitorId { get; set; }
    }
}
