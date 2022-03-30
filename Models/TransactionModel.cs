using DataBaseTest.Repos;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using static DataBaseTest.Utilities;
#nullable enable
namespace DataBaseTest.Models
{
    public class TransactionModel
    {
        public override int GetHashCode()
        {
            return -1 & Hash(Amount) &
                (IsIncoming ? 1 : 0) & Date.GetHashCode() &
                Hash(Creditor) & Hash(Debitor) & TransactionType.GetHashCode() & TransactionGuid.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            if (obj is TransactionModel t)
            {
                return Amount == t.Amount
                    && IsIncoming == t.IsIncoming
                    && Date == t.Date
                    && IsEqual(Creditor, t.Creditor)
                    && IsEqual(Debitor, t.Debitor)
                    && TransactionGuid == t.TransactionGuid
                    && TransactionType == t.TransactionType;
            }
            return false;
        }

        [Key]
        public int? TransactionModelId { get; set; }

        [ForeignKey(nameof(Creditor))]
        public int? CreditorEntityModelId { get; set; }
        [ForeignKey(nameof(Debitor))]
        public int? DebitorEntityModelId { get; set; }
        [ForeignKey(nameof(AccountModel.Transactions))]
        public int? AccountModelId { get; set; }
        //[ForeignKey(nameof(AccountModel.Transactions))]
        //public int TransactionsId { get; set; }

        //[Column(TypeName = "decimal(28, 4)")]
        public decimal Amount { get; set; }
        public bool IsIncoming { get; set; }
        public string? Description { get; set; }
        //[Column(TypeName = "DateTime2")]
        public DateTime Date { get; set; }
        public EntityModel? Creditor { get; set; }
        public EntityModel? Debitor { get; set; }
        public TransactionType TransactionType { get; set; }
        public Guid TransactionGuid { get; set; }
        public int? EmployeeId { get; set; }
        [JsonIgnore]
        public ICurrency? Currency { get; set; }
    }
}