using DataBaseTest.Models;
using System;

namespace DataBaseTest.Repos
{
    public interface ITransaction
    {
        public decimal Amount { get; }
        public bool IsIncoming { get; }
        public string Description { get; }
        public DateTime Date { get; }
        public IEntity Creditor { get; }
        public IEntity Debitor { get; }
        public TransactionType TransactionType { get; }
        public Guid TransactionGuid { get; }
        public int EmployeeId { get; }

        public ICurrency Currency { get; }
    }
}
