using DataBaseTest.Models;
using System;
using System.Collections.Generic;

namespace DataBaseTest.Repos
{
    /// <summary>
    /// Add Account type and ICard interface for debit and credit card.
    /// We may also add a decrement and increment interval or just add
    /// a listener to the account that adds decrement and increment
    /// values to the balance at various interval such as by the second,
    /// minute, hourly, daily, 3-day intervals, 5-days interval, weekly,
    /// monthly, quaterly, 6-months interval and annually
    /// 
    /// Add a percentageUpdate property to prevent abrupt large sums going out or in unnoticed
    /// </summary>
    public interface IAccount
    {
        string Number { get; }

        decimal Balance { get; }
        decimal PercentageIncrease { get; }
        decimal PercentageDecrease { get; }
        decimal Debt { get; }
        decimal DebitLimit { get; }
        decimal CreditLimit { get; }
        decimal SmallestBalance { get; }
        decimal SmallestTransferIn { get; }
        decimal SmallestTransferOut { get; }
        decimal LargestBalance { get; }
        decimal LargestTransferIn { get; }
        decimal LargestTransferOut { get; }

        DateTime EntryDate { get; }
        ICurrency Currency { get; }

        IEnumerable<string> SMSAlertList { get; }
        IEnumerable<string> EmailAlertList { get; }
        Queue<AccountStatusInfo> Statuses { get; }
        IEnumerable<ICard> Cards { get; }

        public TimeSpan CreditIntervalLimit { get; }
        public TimeSpan DebitIntervalLimit { get; }
        public IEnumerable<ITransaction> Transactions { get; }
        public AccountType Type { get; }

        public IPerson Guarantor { get; }

        public bool Credit(string jsonDetails);
        public bool Debit(string jsonDetails);
    }
}
