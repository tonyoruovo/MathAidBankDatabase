using DataBaseTest.Repos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using static DataBaseTest.Utilities;
#nullable enable
namespace DataBaseTest.Models
{
    /// <summary>
    /// Since this class is serialized and deserialized in JSON, it does not need
    /// The attribute 'NotMapped' and the extra properties to handle serializations.
	///
	/// The <c>DataContract</c> tells the compiler that the Json formatter should not
	/// serialize all the fields and properties of this class by default, but rather
	/// should only serialize fields and properties with the <c>DataMember</c>
	/// attribute.
    /// </summary>
	//[DataContract]
    public class AccountModel
    {
        protected virtual bool Credit()
        {
            return false;
        }

        protected virtual bool Debit()
        {
            return false;
        }

        public override int GetHashCode()
        {
            var hash = -1 & Balance.GetHashCode() & EntryDate.GetHashCode() & Type.GetHashCode();
            hash &= (Number == null ? 0 : Number.GetHashCode()) &
                (Currency == null ? 0 : Currency.GetHashCode()) &
                (Cards == null ? 0 : CombineHash(Cards));
            return hash;
        }

        public override bool Equals(object? obj)
        {
            if (obj is AccountModel a)
            {
                return IsEqual(Number, a.Number)
                    && Balance == a.Balance
                    && EntryDate == a.EntryDate
                    && Type == a.Type
                    && IsEqual(Currency, a.Currency)
                    && IsEqual(Cards, a.Cards);
            }
            return false;
        }
        //[DataMember]
        [Key]
        public int AccountModelId { get; set; }

        [ForeignKey(nameof(Guarantor))]
        public int? GuarantorPersonModelId { get; set; }
        [ForeignKey(nameof(CustomerModel.Accounts))]
        public int? CustomerModelId { get; set; }
        //[ForeignKey(nameof(CustomerModel.Accounts))]
        //public int CustomerAccountsId { get; set; }

        //[DataMember]
        public string Number { get; set; } = new string('0', 10);
        //[DataMember]
        //[Column(TypeName = "DateTime2")]
        public DateTime EntryDate { get; set; }
		//[DataMember]
        //[Column(TypeName = "decimal(28, 4)")]
        public decimal Balance { get; set; }
		//[DataMember]
        //[Column(TypeName = "decimal(28, 4)")]
        public decimal PercentageIncrease { get; set; }
		//[DataMember]
        //[Column(TypeName = "decimal(28, 4)")]
        public decimal PercentageDecrease { get; set; }
		//[DataMember]
        //[Column(TypeName = "decimal(28, 4)")]
        public decimal Debt { get; set; }
		//[DataMember]
        //[Column(TypeName = "decimal(28, 4)")]
        public decimal DebitLimit { get; set; }
		//[DataMember]
        //[Column(TypeName = "decimal(28, 4)")]
        public decimal CreditLimit { get; set; }
		//[DataMember]
        public AccountType Type { get; set; }
		//[DataMember]
        public List<TransactionModel>? Transactions { get; set; }
		//[DataMember]
        public List<CardModel>? Cards { get; set; }
		//[DataMember]
        public TimeSpan CreditIntervalLimit { get; set; }
		//[DataMember]
        public TimeSpan DebitIntervalLimit{ get; set; }
		//[DataMember]
        //[Column(TypeName = "decimal(28, 4)")]
        public decimal SmallestBalance { get; set; }
		//[DataMember]
        //[Column(TypeName = "decimal(28, 4)")]
        public decimal SmallestTransferIn { get; set; }
		//[DataMember]
        //[Column(TypeName = "decimal(28, 4)")]
        public decimal SmallestTransferOut { get; set; }
		//[DataMember]
        //[Column(TypeName = "decimal(28, 4)")]
        public decimal LargestBalance { get; set; }
		//[DataMember]
        //[Column(TypeName = "decimal(28, 4)")]
        public decimal LargestTransferIn { get; set; }
		//[DataMember]
        //[Column(TypeName = "decimal(28, 4)")]
        public decimal LargestTransferOut { get; set; }
        //[NotMapped]
		[JsonIgnore]
        public ICurrency? Currency
        {
            //get => JsonSerializer.Deserialize<Currencies.MediumOfExchange>(_currency/* ?? throw new ApplicationException($"{nameof(_currency)} is null. No valid saved Json object found for stored {nameof(_currency)}")*/);
            //set{
            //    _currency = JsonSerializer.Serialize((Currencies.MediumOfExchange?)value);
            //}
            get;
            set;
        }
        //[NotMapped]
        public List<string>? SMSAlertList
        {
            //get => _smsAlertList == null ? new() : JsonSerializer.Deserialize<List<string>>(_smsAlertList/* ?? throw new ApplicationException($"{nameof(_smsAlertList)} is null. No valid saved Json object found for stored {nameof(_smsAlertList)}")*/);
            //set
            //{
            //    _smsAlertList = JsonSerializer.Serialize(value);
            //}
            get;
            set;
        }
        //[NotMapped]
        public List<string>? EmailAlertList
        {
            //get => _emailAlertList == null ? new() : JsonSerializer.Deserialize<List<string>>(_emailAlertList/* ?? throw new ApplicationException($"{nameof(_emailAlertList)} is null. No valid saved Json object found for stored {nameof(_emailAlertList)}")*/);
            //set
            //{
            //    _emailAlertList = JsonSerializer.Serialize(value);
            //}
            get;
            set;
        }
        /// <summary>
        /// Status History
        /// </summary>
        //[NotMapped]
        public Queue<AccountStatusInfo>? Statuses
        {
            //get => _statuses == null ? new() : JsonSerializer.Deserialize<Queue<AccountStatusInfo>>(_statuses/* ?? throw new ApplicationException($"{nameof(_statuses)} is null. No valid saved Json object found for stored {nameof(_statuses)}")*/);
            //set
            //{
            //    _statuses = JsonSerializer.Serialize(value);
            //}
            get;
            set;
        }
        //[NotMapped]
        
        //[NotMapped]

		//[DataMember]
        public PersonModel? Guarantor { get; set; }
        //Address EntryBranch { get; set; }
        //[DataMember]
        //public string _currency { get; set; } = JsonSerializer.Serialize(Currencies.TryParse('N', 'G', 'N' ));
        //[DataMember]
        //public string? _smsAlertList { get; set; }
        //[DataMember]
        //public string? _emailAlertList { get; set; }
        //[DataMember]
        //public string? _statuses { get; set; }
        //public string _creditIntervalLimit { get; set; }
        //public string _debitIntervalLimit { get; set; }
        //public string _transactions { get; set; }
        //public string _cards { get; set; }
    }
}