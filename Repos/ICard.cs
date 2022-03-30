using System;

namespace DataBaseTest.Repos
{
    public interface ICard
    {
        /// <summary>
        /// This represents the id number at the front of the card (represented by
        /// the property <c>PersonalOnlineKey</c>), the signature at the back of the
        /// card (represented by the property <c>Username</c>) and the user password
        /// for access (represented by the property <c>Password</c>).
        /// </summary>
        public ILoginCredentials Secret { get; }
        /// <summary>
        /// Brands should only have one email, one password and one website in it's <c>IContact</c> object
        /// </summary>
        public IEntity Brand { get; }
        public INationality CountryOfUse { get; }
        public Models.ICurrency Currency { get; }
        public Models.CardType Type { get; }
        public DateTime IssuedDate { get; }
        public DateTime ExpiryDate { get; }
        public decimal IssuedCost { get; }
        public decimal MonthlyRate { get; }
        public decimal WithdrawalLimit { get; }
    }
}
