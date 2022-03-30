using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DataBaseTest.Utilities;
#nullable enable
namespace DataBaseTest.Models
{
    public class ContactModel
    {
        public override int GetHashCode()
        {
            return -1 & (Address == null ? 0 : Address.GetHashCode()) &
                (Emails == null ? 0 : Emails.GetHashCode()) & (SocialMedia == null ? 0 : SocialMedia.GetHashCode()) &
                (Mobiles == null ? 0 : Mobiles.GetHashCode()) & (Websites == null ? 0 : Websites.GetHashCode());
        }

        public override bool Equals(object? obj)
        {
            if (obj is ContactModel c)
            {
                var isEqual = IsEqual(Emails, c.Emails) && IsEqual(SocialMedia, c.SocialMedia)
                    && IsEqual(Mobiles, c.Mobiles) && IsEqual(Websites, c.Websites);
                isEqual = isEqual && (Address == null ? c.Address == null : Address.Equals(c.Address));
                return isEqual;
            }
            return false;
        }

        public List<string>? Emails
        {
            get; set;
        }

        public List<string>? SocialMedia
        {
            get; set;
        }

        public List<string>? Mobiles
        {
            get; set;
        }

        public List<string>? Websites
        {
            get;set;
        }
        // This is the Home address
        public AddressModel? Address { get; set; }

        [Key]
        public int? ContactModelId { get; set; }

        [ForeignKey(nameof(Address))]
        public int? AddressModelId { get; set; }
        //[ForeignKey(nameof(EntityModel.Contact))]
        //public int ContactId { get; set; }

        //public string? _emails { get; set; }// = JsonSerializer.Serialize(new List<string>());
        //public string? _socialMedia { get; set; }// = JsonSerializer.Serialize(new List<string>());
        //public string? _mobiles { get; set; }// = JsonSerializer.Serialize(new List<string>());
        //public string? _websites { get; set; }// = JsonSerializer.Serialize(new List<string>());
    }
}