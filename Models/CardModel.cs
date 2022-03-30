using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using static DataBaseTest.Utilities;
#nullable enable
namespace DataBaseTest.Models
{
	//[DataContract]
    public class CardModel
    {
        public override int GetHashCode()
        {
            return -1 & Hash(Secret) & Hash(Brand) & Hash(CountryOfUse) & Hash(Currency) & Hash(Type) &
                Hash(IssuedDate) & Hash(ExpiryDate);
        }

        public override bool Equals(object? obj)
        {
            if (obj is CardModel c)
                return IsEqual(Secret, c.Secret) && IsEqual(Brand, c.Brand) && IsEqual(CountryOfUse, c.CountryOfUse)
                    && IsEqual(Currency, c.Currency) && IsEqual(Type, c.Type) && IsEqual(IssuedDate, c.IssuedDate)
                    && IsEqual(ExpiryDate, c.ExpiryDate);
            return false;
        }

        //[DataMember]
        [Key]
        public int? CardModelId { get; set; }

        [ForeignKey(nameof(Secret))]
        public int? SecretLoginCredentialModelId { get; set; }
        [ForeignKey(nameof(Brand))]
        public int? BrandEntityModelId { get; set; }
        [ForeignKey(nameof(CountryOfUse))]
        public int? CountryOfUseNationalityModelId { get; set; }
        [ForeignKey(nameof(AccountModel.Cards))]
        public int? AccountModelId { get; set; }
        //[ForeignKey(nameof(AccountModel.Cards))]
        //public int CardsId { get; set; }

        //[DataMember]
        public LoginCredentialModel? Secret { get; set; }
		//[DataMember]
        public EntityModel? Brand { get; set; }
		//[DataMember]
        public NationalityModel? CountryOfUse { get; set; }
		//[DataMember]
        public CardType Type { get; set; }
		//[DataMember]
        //[Column(TypeName = "DateTime2")]
        public DateTime IssuedDate { get; set; }
		//[DataMember]
        //[Column(TypeName = "DateTime2")]
        public DateTime ExpiryDate { get; set; }
		//[DataMember]
        //[Column(TypeName = "decimal(28, 4)")]
        public decimal IssuedCost { get; set; }
		//[DataMember]
        //[Column(TypeName = "decimal(28, 4)")]
        public decimal MonthlyRate { get; set; }
		//[DataMember]
        //[Column(TypeName = "decimal(28, 4)")]
        public decimal WithdrawalLimit { get; set; }
        //[NotMapped]
        [JsonIgnore]
        public ICurrency? Currency {
            //get => _currency == null ? null : JsonSerializer.Deserialize<Currencies.MediumOfExchange>(_currency/* ?? throw new ApplicationException($"{nameof(_currency)} is null. No valid saved Json object found for stored {nameof(_currency)}")*/);
            //set { _currency = JsonSerializer.Serialize((Currencies.MediumOfExchange?)value); }
            get;
            set;
        }

        //[DataMember]
        //public string _currency { get; set; } = JsonSerializer.Serialize(Currencies.TryParse('N', 'G', 'N'));
    }
}