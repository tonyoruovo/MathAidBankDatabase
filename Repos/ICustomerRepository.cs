using DataBaseTest.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace DataBaseTest.Repos
{
    public interface ICustomerRepository
    {
        #region GETs
        /*Bulk*/
        Task<IEnumerable<ICustomer>> GetCustomers();
        Task<ICustomer> GetCustomer(int? id);
        Task<IEnumerable<IAccount>> GetAccounts(int? id);
        Task<IAccount> GetAccount(int? id, string accountNumber);
        Task<IEnumerable<ICard>> GetCards(int? id, string accountNumber);
        Task<ICard> GetCard(int? id, string accountNumber, ILoginCredentials cardCredential);

        /*Non-bulk*/
        Task<IName> GetName(int? id);
        Task<IName> GetSurname(int? id);
        Task<IName> GetNickname(int? id);
        Task<IName> GetMaidenName(int? id);
        Task<IEnumerable<IName>> GetOtherNames(int? id);
        Task<IEnumerable<IName>> GetTitles(int? id);

        Task<IEnumerable<ITransaction>> GetTransactions(int? id, string accountNumber);
        Task<ITransaction> GetTransaction(int? id, string accountNumber, Guid transactionId);

        Task<IPerson> GetGuarantor(int? id, string accountNumber);

        Task<string> GetPersonalOnlineKey(int? id, string accountNumber, ILoginCredentials password);
        Task<string> GetUsername(int? id, string accountNumber, ILoginCredentials password);
        Task<string> GetPassword(int? id, string accountNumber, ILoginCredentials password);
        Task<IName> GetBrandName(int? id, string accountNumber, ILoginCredentials password);
        Task<string> GetBrandPhone(int? id, string accountNumber, ILoginCredentials password);
        Task<string> GetBrandEmail(int? id, string accountNumber, ILoginCredentials password);
        Task<string> GetBrandSocialMedia(int? id, string accountNumber, ILoginCredentials password);
        Task<string> GetBrandWebsite(int? id, string accountNumber, ILoginCredentials password);
        //Task<IAddress> GetBrandAddress(int? id, string accountNumber, ILoginCredentials password);
        Task<string> GetBrandStreetAddress(int? id, string accountNumber, ILoginCredentials password);
        Task<string> GetBrandZIPCode(int? id, string accountNumber, ILoginCredentials password);
        Task<string> GetBrandPMB(int? id, string accountNumber, ILoginCredentials password);
        //Task<INationality> GetBrandCountryOfResidence(int? id, string accountNumber, ILoginCredentials password);
        Task<IName> GetBrandCountryOfResidence(int? id, string accountNumber, ILoginCredentials password);
        Task<IName> GetBrandStateOfResidence(int? id, string accountNumber, ILoginCredentials password);
        Task<string> GetBrandLGAOfResidence(int? id, string accountNumber, ILoginCredentials password);
        Task<string> GetBrandLanguage(int? id, string accountNumber, ILoginCredentials password);
        Task<IName> GetBrandCityOfResidence(int? id, string accountNumber, ILoginCredentials password);
        //Task<INationality> GetBrandCountryOfUse(int? id, string accountNumber, ILoginCredentials password, INationality @new);
        Task<IName> GetBrandCountryOfUse(int? id, string accountNumber, ILoginCredentials password);
        Task<IName> GetBrandStateOfUse(int? id, string accountNumber, ILoginCredentials password);
        Task<string> GetBrandLGAOfUse(int? id, string accountNumber, ILoginCredentials password);
        Task<string> GetBrandLanguageOfUse(int? id, string accountNumber, ILoginCredentials password);
        Task<IName> GetBrandCityTownOfUse(int? id, string accountNumber, ILoginCredentials password);
        Task<ICurrency> GetCurrencyOfUse(int? id, string accountNumber, ILoginCredentials password);
        Task<CardType> GetCardType(int? id, string accountNumber, ILoginCredentials password);
        Task<DateTime> GetIssuedDate(int? id, string accountNumber, ILoginCredentials password);
        Task<DateTime> GetExpiryDate(int? id, string accountNumber, ILoginCredentials password);
        Task<decimal> GetIssuedCost(int? id, string accountNumber, ILoginCredentials password);
        Task<decimal> GetMonthlyRate(int? id, string accountNumber, ILoginCredentials password);
        Task<decimal> GetWithdrawalLimit(int? id, string accountNumber, ILoginCredentials password);

        Task<string> GetBVN(int? id);
        Task<DateTime> GetEntryDate(int? id);

        //Task<ILoginCredentials> GetLoginCredential(int? id, ILoginCredentials @new);
        Task<string> GetPersonalOnlineKey(int? id, ILoginCredentials password);
        Task<string> GetUsername(int? id, ILoginCredentials password);
        Task<string> GetPassword(int? id, ILoginCredentials password);

        Task<string> GetIdentification(int? id, string type);
        Task<byte[]> GetFingerprint(int? id);
        Task<byte[]> GetSignature(int? id);
        Task<byte[]> GetPassport(int? id);
        Task<string> GetJobType(int? id);
        Task<bool> GetSex(int? id);
        Task<Guid> GetUniqueTag(int? id);
        Task<DateTime> GetBirthDate(int? id);

        Task<IName> GetCountryOfOrigin(int? id);
        Task<IName> GetStateOfOrigin(int? id);
        Task<IName> GetTown_CityOfOrigin(int? id);
        Task<string> GetLGAOfOrigin(int? id);
        Task<string> GetLanguage(int? id);

        Task<IPerson> GetNextOfKin(int? id);

        Task<MaritalStatus> GetMaritalStatus(int? id);

        Task<IName> GetBusinessName(int? id);
        Task<IEnumerable<string>> GetBusinessEmails(int? id);
        Task<IEnumerable<string>> GetBusinessMobiles(int? id);
        Task<IEnumerable<string>> GetBusinessSocialMedia(int? id);
        Task<IEnumerable<string>> GetBusinessWebsites(int? id);
        Task<string> GetBusinessStreetAddress(int? id);
        Task<string> GetBusinessZIPCode(int? id);
        Task<string> GetBusinessPMB(int? id);

        Task<IName> GetBusinessCountryOfResidence(int? id);
        Task<IName> GetBusinessStateOfResidence(int? id);
        Task<IName> GetBusinessTown_CityOfResidence(int? id);
        Task<string> GetBusinessLGAOfResidence(int? id);

        Task<IName> GetDisplayName(int? id);
        Task<IEnumerable<string>> GetEmails(int? id);
        Task<IEnumerable<string>> GetMobiles(int? id);
        Task<IEnumerable<string>> GetSocialMedia(int? id);
        Task<IEnumerable<string>> GetWebsites(int? id);
        Task<string> GetStreetAddress(int? id);
        Task<string> GetZIPCode(int? id);
        Task<string> GetPMB(int? id);

        Task<IName> GetCountryOfResidence(int? id);
        Task<IName> GetStateOfResidence(int? id);
        Task<IName> GetTown_CityOfResidence(int? id);
        Task<string> GetLGAOfResidence(int? id);
        //Task<int?> GetId(IFullName name);

        #endregion GETs

        #region POSTs
        Task<PostResult<object>> AddCustomer(ICustomer @new);

        Task<bool> SetFullName(int? id, IFullName @new);
        Task<bool> SetName(int? id, IName @new);
        Task<bool> SetSurname(int? id, IName @new);
        Task<bool> SetNickname(int? id, IName @new);
        Task<bool> SetMaidenName(int? id, IName @new);
        Task<bool> AddOtherName(int? id, IName @new);
        Task<bool> AddTitle(int? id, string @new);

        //Account
        Task<bool> AddAccount(int? id, IAccount @new);
        Task<bool> SetBalance(int? id, string accountNumber, decimal @new);
        Task<bool> SetPercentageIncrease(int? id, string accountNumber, decimal @new);
        Task<bool> SetPercentageDecrease(int? id, string accountNumber, decimal @new);
        Task<bool> SetDebt(int? id, string accountNumber, decimal @new);
        Task<bool> SetDebitLimit(int? id, string accountNumber, decimal @new);
        Task<bool> SetCreditLimit(int? id, string accountNumber, decimal @new);
        Task<bool> SetSmallestBalance(int? id, string accountNumber, decimal @new);
        Task<bool> SetLargestBalance(int? id, string accountNumber, decimal @new);
        Task<bool> SetSmallestTransferIn(int? id, string accountNumber, decimal @new);
        Task<bool> SetLargestTransferIn(int? id, string accountNumber, decimal @new);
        Task<bool> SetSmallestTransferOut(int? id, string accountNumber, decimal @new);
        Task<bool> SetLargestTransferOut(int? id, string accountNumber, decimal @new);
        Task<bool> SetEntryDate(int? id, string accountNumber, DateTime @new);
        Task<bool> SetCurrency(int? id, string accountNumber, ICurrency @new);
        Task<bool> AddMessageNumber(int? id, string accountNumber, [Phone] string @new);
        Task<bool> AddEmail(int? id, string accountNumber, [EmailAddress] string @new);
        Task<bool> AddStatus(int? id, string accountNumber, AccountStatusInfo @new);
        Task<bool> SetCreditIntervalLimit(int? id, string accountNumber, TimeSpan @new);
        Task<bool> SetDebitIntervalLimit(int? id, string accountNumber, TimeSpan @new);
        Task<bool> AddTransaction(int? id, string accountNumber, ITransaction @new);
        Task<bool> SetType(int? id, string accountNumber, AccountType @new);
        Task<bool> SetGuarantor(int? id, string accountNumber, IPerson @new);

        Task<bool> AddCard(int? id, string accountNumber, ICard @new);
        Task<bool> SetLoginCredentials(int? id, string accountNumber, ICard card, ILoginCredentials @new);
        Task<bool> SetPersonalOnlineKey(int? id, string accountNumber, ICard card, ILoginCredentials @new);
        Task<bool> SetUsername(int? id, string accountNumber, ICard card, ILoginCredentials @new);
        Task<bool> SetPassword(int? id, string accountNumber, ICard card, ILoginCredentials @new);
        Task<bool> SetBrand(int? id, string accountNumber, ILoginCredentials password, IEntity @new);
        Task<bool> SetBrandName(int? id, string accountNumber, ILoginCredentials password, IName @new);
        Task<bool> SetBrandPhone(int? id, string accountNumber, ILoginCredentials password, [Phone] string @new);
        Task<bool> SetBrandEmail(int? id, string accountNumber, ILoginCredentials password, [EmailAddress] string @new);
        Task<bool> SetBrandSocialMedia(int? id, string accountNumber, ILoginCredentials password, [Url] string @new);
        Task<bool> SetBrandWebsite(int? id, string accountNumber, ILoginCredentials password, [Url] string @new);
        Task<bool> SetBrandAddress(int? id, string accountNumber, ILoginCredentials password, IAddress @new);
        Task<bool> SetBrandStreetAddress(int? id, string accountNumber, ILoginCredentials password, string @new);
        Task<bool> SetBrandZIPCode(int? id, string accountNumber, ILoginCredentials password, string @new);
        Task<bool> SetBrandPMB(int? id, string accountNumber, ILoginCredentials password, string @new);
        Task<bool> SetBrandCountryOfResidence(int? id, string accountNumber, ILoginCredentials password, INationality @new);
        Task<bool> SetBrandCountryOfResidence(int? id, string accountNumber, ILoginCredentials password, IName @new);
        Task<bool> SetBrandStateOfResidence(int? id, string accountNumber, ILoginCredentials password, IName @new);
        Task<bool> SetBrandLGAOfResidence(int? id, string accountNumber, ILoginCredentials password, string @new);
        Task<bool> SetBrandLanguage(int? id, string accountNumber, ILoginCredentials password, string @new);
        Task<bool> SetBrandCityOfResidence(int? id, string accountNumber, ILoginCredentials password, IName @new);
        Task<bool> SetBrandCountryOfUse(int? id, string accountNumber, ILoginCredentials password, INationality @new);
        Task<bool> SetBrandCountryOfUse(int? id, string accountNumber, ILoginCredentials password, IName @new);
        Task<bool> SetBrandStateOfUse(int? id, string accountNumber, ILoginCredentials password, IName @new);
        Task<bool> SetBrandLGAOfUse(int? id, string accountNumber, ILoginCredentials password, string @new);
        Task<bool> SetBrandLanguageOfUse(int? id, string accountNumber, ILoginCredentials password, string @new);
        Task<bool> SetBrandCityTownOfUse(int? id, string accountNumber, ILoginCredentials password, IName @new);
        Task<bool> SetCurrencyOfUse(int? id, string accountNumber, ILoginCredentials password, ICurrency @new);
        Task<bool> SetCardType(int? id, string accountNumber, ILoginCredentials password, CardType @new);
        Task<bool> SetIssuedDate(int? id, string accountNumber, ILoginCredentials password, DateTime @new);
        Task<bool> SetExpiryDate(int? id, string accountNumber, ILoginCredentials password, DateTime @new);
        Task<bool> SetIssuedCost(int? id, string accountNumber, ILoginCredentials password, decimal @new);
        Task<bool> SetMonthlyRate(int? id, string accountNumber, ILoginCredentials password, decimal @new);
        Task<bool> SetWithdrawalLimit(int? id, string accountNumber, ILoginCredentials password, decimal @new);

        Task<bool> SetBVN(int? id, string @new);
        Task<bool> SetEntryDate(int? id, DateTime @new);

        Task<bool> SetLoginCredential(int? id, ILoginCredentials @new);
        Task<bool> SetPersonalOnlineKey(int? id, ILoginCredentials @new);
        Task<bool> SetUsername(int? id, ILoginCredentials @new);
        Task<bool> SetPassword(int? id, ILoginCredentials @new);

        Task<bool> SetPerson(int? id, IPerson @new);
        Task<bool> AddIdentification(int? id, string type, string @new);
        Task<bool> SetFingerprint(int? id, byte[] @new);
        Task<bool> SetSignature(int? id, byte[] @new);
        Task<bool> SetPassport(int? id, byte[] @new);
        Task<bool> SetJobType(int? id, string @new);
        Task<bool> SetSex(int? id, bool @new);
        Task<bool> SetUniqueTag(int? id, Guid @new);
        Task<bool> SetBirthDate(int? id, DateTime @new);

        Task<bool> SetCountryOfOrigin(int? id, INationality @new);
        Task<bool> SetCountryOfOrigin(int? id, IName @new);
        Task<bool> SetStateOfOrigin(int? id, IName @new);
        Task<bool> SetTown_CityOfOrigin(int? id, IName @new);
        Task<bool> SetLGAOfOrigin(int? id, string @new);
        Task<bool> SetLanguage(int? id, string @new);

        Task<bool> SetNextOfKin(int? id, IPerson @new);

        Task<bool> SetMaritalStatus(int? id, MaritalStatus @new);

        Task<bool> SetOfficialEntity(int? id, IEntity @new);
        Task<bool> SetBusinessName(int? id, IName @ew);
        Task<bool> SetOfficialContact(int? id, IContact @new);
        Task<bool> AddBusinessEmail(int? id, [EmailAddress] string @new);
        Task<bool> AddBusinessMobile(int? id, [Phone] string @new);
        Task<bool> AddBusinessSocialMedia(int? id, [Url] string @new);
        Task<bool> AddBusinessWebsite(int? id, [Url] string @new);
        Task<bool> SetOfficialAddress(int? id, IAddress @new);
        Task<bool> SetBusinessStreetAddress(int? id, string @new);
        Task<bool> SetBusinessZIPCode(int? id, string @new);
        Task<bool> SetBusinessPMB(int? id, string @new);

        Task<bool> SetOfficalNationality(int? id, INationality @new);
        Task<bool> SetBusinessCountryOfResidence(int? id, IName @new);
        Task<bool> SetBusinessStateOfResidence(int? id, IName @new);
        Task<bool> SetBusinessTown_CityOfResidence(int? id, IName @new);
        Task<bool> SetBusinessLGAOfResidence(int? id, string @new);

        Task<bool> SetEntity(int? id, IEntity @new);
        Task<bool> SetDisplayName(int? id, IName @new);
        Task<bool> SetContact(int? id, IContact @new);
        Task<bool> AddEmail(int? id, [EmailAddress] string @new);
        Task<bool> AddMobile(int? id, [Phone] string @new);
        Task<bool> AddSocialMedia(int? id, [Url] string @new);
        Task<bool> AddWebsite(int? id, [Url] string @new);
        Task<bool> SetAddress(int? id, IAddress @new);
        Task<bool> SetStreetAddress(int? id, string @new);
        Task<bool> SetZIPCode(int? id, string @new);
        Task<bool> SetPMB(int? id, string @new);

        Task<bool> SetNationality(int? id, INationality @new);
        Task<bool> SetCountryOfResidence(int? id, IName @new);
        Task<bool> SetStateOfResidence(int? id, IName @new);
        Task<bool> SetTown_CityOfResidence(int? id, IName @new);
        Task<bool> SetLGAOfResidence(int? id, string @new);

        #endregion POSTs

        #region PUTs
        //Task<bool> UpdateFullName(int? id, IFullName @new);
        Task<bool> UpdateName(int? id, IName old, IName @new);
        Task<bool> UpdateSurname(int? id, IName old, IName @new);
        Task<bool> UpdateNickname(int? id, IName old, IName @new);
        Task<bool> UpdateMaidenName(int? id, IName old, IName @new);
        Task<bool> UpdateOthername(int? id, IName old, IName @new);
        Task<bool> UpdateTitle(int? id, string old, string @new);

        //Account
        //Task<bool> AddAccount(int? id, IAccount @new);
        Task<bool> UpdateNumber(int? id, string old, string @new);
        Task<bool> UpdateBalance(int? id, string accountNumber, decimal old, decimal @new);
        Task<bool> UpdatePercentageIncrease(int? id, string accountNumber, decimal old, decimal @new);
        Task<bool> UpdatePercentageDecrease(int? id, string accountNumber, decimal old, decimal @new);
        Task<bool> UpdateDebt(int? id, string accountNumber, decimal old, decimal @new);
        Task<bool> UpdateDebitLimit(int? id, string accountNumber, decimal old, decimal @new);
        Task<bool> UpdateCreditLimit(int? id, string accountNumber, decimal old, decimal @new);
        Task<bool> UpdateSmallestBalance(int? id, string accountNumber, decimal old, decimal @new);
        Task<bool> UpdateLargestBalance(int? id, string accountNumber, decimal old, decimal @new);
        Task<bool> UpdateSmallestTransferIn(int? id, string accountNumber, decimal old, decimal @new);
        Task<bool> UpdateLargestTransferIn(int? id, string accountNumber, decimal old, decimal @new);
        Task<bool> UpdateSmallestTransferOut(int? id, string accountNumber, decimal old, decimal @new);
        Task<bool> UpdateLargestTransferOut(int? id, string accountNumber, decimal old, decimal @new);
        Task<bool> UpdateEntryDate(int? id, string accountNumber, DateTime old, DateTime @new);
        Task<bool> UpdateCurrency(int? id, string accountNumber, ICurrency old, ICurrency @new);
        Task<bool> UpdateMessageNumber(int? id, string accountNumber, [Phone] string old, [Phone] string @new);
        Task<bool> UpdateEmail(int? id, string accountNumber, [EmailAddress] string old, [EmailAddress] string @new);
        //Task<bool> UpdateStatus(int? id, string accountNumber, AccountStatusInfo old, AccountStatusInfo @new);
        Task<bool> UpdateCreditIntervalLimit(int? id, string accountNumber, TimeSpan old, TimeSpan @new);
        Task<bool> UpdateDebitIntervalLimit(int? id, string accountNumber, TimeSpan old, TimeSpan @new);
        //Task<bool> AddTransaction(int? id, string accountNumber, ITransaction @new);
        Task<bool> UpdateType(int? id, string accountNumber,  AccountType old, AccountType @new);
        Task<bool> UpdateGuarantor(int? id, string accountNumber,  IPerson old, IPerson @new);

        //Task<bool> AddCard(int? id, string accountNumber, ICard @new);
        //Task<bool> UpdateLoginCredentials(int? id, string accountNumber, ILoginCredentials @new);
        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="accountNumber"></param>
        /// <param name="password">This value should also contain the old personal online key</param>
        /// <param name="new"></param>
        /// <returns></returns>
        Task<bool> UpdatePersonalOnlineKey(int? id, string accountNumber, ILoginCredentials password, ILoginCredentials @new);
        Task<bool> UpdateUsername(int? id, string accountNumber, ILoginCredentials password, ILoginCredentials @new);
        Task<bool> UpdatePassword(int? id, string accountNumber, ILoginCredentials old, ILoginCredentials @new);
        //Task<bool> UpdateBrand(int? id, string accountNumber, ILoginCredentials password, IEntity @new);
        Task<bool> UpdateBrandName(int? id, string accountNumber, ILoginCredentials password, IName old, IName @new);
        Task<bool> UpdateBrandPhone(int? id, string accountNumber, ILoginCredentials password, [Phone] string old, [Phone] string @new);
        Task<bool> UpdateBrandEmail(int? id, string accountNumber, ILoginCredentials password, [EmailAddress] string old, [EmailAddress] string @new);
        Task<bool> UpdateBrandSocialMedia(int? id, string accountNumber, ILoginCredentials password, [Url] string old,  [Url] string @new);
        Task<bool> UpdateBrandWebsite(int? id, string accountNumber, ILoginCredentials password, [Url] string old, [Url] string @new);
        //Task<bool> UpdateBrandAddress(int? id, string accountNumber, ILoginCredentials password, IAddress @new);
        Task<bool> UpdateBrandStreetAddress(int? id, string accountNumber, ILoginCredentials password, string old, string @new);
        Task<bool> UpdateBrandZIPCode(int? id, string accountNumber, ILoginCredentials password, string old, string @new);
        Task<bool> UpdateBrandPMB(int? id, string accountNumber, ILoginCredentials password, string old, string @new);
        //Task<bool> UpdateBrandCountryOfResidence(int? id, string accountNumber, ILoginCredentials password, INationality @new);
        Task<bool> UpdateBrandCountryOfResidence(int? id, string accountNumber, ILoginCredentials password, IName old, IName @new);
        Task<bool> UpdateBrandStateOfResidence(int? id, string accountNumber, ILoginCredentials password,  IName old, IName @new);
        Task<bool> UpdateBrandLGAOfResidence(int? id, string accountNumber, ILoginCredentials password, string old, string @new);
        Task<bool> UpdateBrandLanguage(int? id, string accountNumber, ILoginCredentials password, string old, string @new);
        Task<bool> UpdateBrandCityOfResidence(int? id, string accountNumber, ILoginCredentials password,  IName old, IName @new);
        //Task<bool> UpdateBrandCountryOfUse(int? id, string accountNumber, ILoginCredentials password, INationality @new);
        Task<bool> UpdateBrandCountryOfUse(int? id, string accountNumber, ILoginCredentials password,  IName old, IName @new);
        Task<bool> UpdateBrandStateOfUse(int? id, string accountNumber, ILoginCredentials password,  IName old, IName @new);
        Task<bool> UpdateBrandLGAOfUse(int? id, string accountNumber, ILoginCredentials password, string old, string @new);
        Task<bool> UpdateBrandLanguageOfUse(int? id, string accountNumber, ILoginCredentials password, string old, string @new);
        Task<bool> UpdateBrandCityTownOfUse(int? id, string accountNumber, ILoginCredentials password,  IName old, IName @new);
        Task<bool> UpdateCurrencyOfUse(int? id, string accountNumber, ILoginCredentials password, ICurrency old, ICurrency @new);
        Task<bool> UpdateCardType(int? id, string accountNumber, ILoginCredentials password, CardType old, CardType @new);
        Task<bool> UpdateIssuedDate(int? id, string accountNumber, ILoginCredentials password, DateTime old, DateTime @new);
        Task<bool> UpdateExpiryDate(int? id, string accountNumber, ILoginCredentials password, DateTime old, DateTime @new);
        Task<bool> UpdateIssuedCost(int? id, string accountNumber, ILoginCredentials password, decimal old,  decimal @new);
        Task<bool> UpdateMonthlyRate(int? id, string accountNumber, ILoginCredentials password, decimal old, decimal @new);
        Task<bool> UpdateWithdrawalLimit(int? id, string accountNumber, ILoginCredentials password, decimal old, decimal @new);

        Task<bool> UpdateBVN(int? id, string old, string @new);
        Task<bool> UpdateEntryDate(int? id, DateTime old, DateTime @new);

        //Task<bool> UpdateLoginCredential(int? id, ILoginCredentials @new);
        Task<bool> UpdatePersonalOnlineKey(int? id, ILoginCredentials password, ILoginCredentials @new);
        Task<bool> UpdateUsername(int? id, ILoginCredentials password, ILoginCredentials @new);
        Task<bool> UpdatePassword(int? id, ILoginCredentials old, ILoginCredentials @new);

        //Task<bool> UpdatePerson(int? id, IPerson @new);
        Task<bool> UpdateIdentification(int? id, string type, string old, string @new);
        Task<bool> UpdateFingerprint(int? id, byte[] old, byte[] @new);
        Task<bool> UpdateSignature(int? id, byte[] old, byte[] @new);
        Task<bool> UpdatePassport(int? id,byte[] old, byte[] @new);
        Task<bool> UpdateJobType(int? id, string old, string @new);
        Task<bool> UpdateSex(int? id, bool old, bool @new);
        Task<bool> UpdateUniqueTag(int? id, Guid old, Guid @new);
        Task<bool> UpdateBirthDate(int? id, DateTime old, DateTime @new);

        //Task<bool> UpdateCountryOfOrigin(int? id, INationality @new);
        Task<bool> UpdateCountryOfOrigin(int? id, IName old, IName @new);
        Task<bool> UpdateStateOfOrigin(int? id, IName old, IName @new);
        Task<bool> UpdateTown_CityOfOrigin(int? id, IName old, IName @new);
        Task<bool> UpdateLGAOfOrigin(int? id, string old, string @new);
        Task<bool> UpdateLanguage(int? id, string old, string @new);

        //Task<bool> UpdateNextOfKin(int? id, IPerson @new);

        Task<bool> UpdateMaritalStatus(int? id, MaritalStatus old, MaritalStatus @new);

        //Task<bool> UpdateOfficialEntity(int? id, IEntity @new);
        Task<bool> UpdateBusinessName(int? id, IName old, IName @new);
        //Task<bool> UpdateOfficialContact(int? id, IContact @new);
        Task<bool> UpdateBusinessEmail(int? id, [EmailAddress]string old, [EmailAddress] string @new);
        Task<bool> UpdateBusinessMobile(int? id, [Phone]string old, [Phone] string @new);
        Task<bool> UpdateBusinessSocialMedia(int? id, [Url] string old, [Url] string @new);
        Task<bool> UpdateBusinessWebsite(int? id,  [Url] string old, [Url] string @new);
        //Task<bool> UpdateOfficialAddress(int? id, IAddress @new);
        Task<bool> UpdateBusinessStreetAddress(int? id, string old, string @new);
        Task<bool> UpdateBusinessZIPCode(int? id, string old, string @new);
        Task<bool> UpdateBusinessPMB(int? id, string old, string @new);

        //Task<bool> UpdateOfficalNationality(int? id, INationality @new);
        Task<bool> UpdateBusinessCountryOfResidence(int? id, IName old, IName @new);
        Task<bool> UpdateBusinessStateOfResidence(int? id, IName old, IName @new);
        Task<bool> UpdateBusinessTown_CityOfResidence(int? id, IName old, IName @new);
        Task<bool> UpdateBusinessLGAOfResidence(int? id, string old, string @new);

        //Task<bool> UpdateEntity(int? id, IEntity @new);
        Task<bool> UpdateDisplayName(int? id, IName old, IName @new);
        //Task<bool> UpdateContact(int? id, IContact @new);
        Task<bool> UpdateEmail(int? id, [EmailAddress] string old, [EmailAddress] string @new);
        Task<bool> UpdateMobile(int? id,  [Phone] string old, [Phone] string @new);
        Task<bool> UpdateSocialMedia(int? id,  [Url] string old, [Url] string @new);
        Task<bool> UpdateWebsite(int? id,  [Url] string old, [Url] string @new);
        //Task<bool> UpdateAddress(int? id, IAddress @new);
        Task<bool> UpdateStreetAddress(int? id, string old, string @new);
        Task<bool> UpdateZIPCode(int? id, string old, string @new);
        Task<bool> UpdatePMB(int? id, string old, string @new);

        //Task<bool> UpdateNationality(int? id, INationality @new);
        Task<bool> UpdateCountryOfResidence(int? id, IName old, IName @new);
        Task<bool> UpdateStateOfResidence(int? id, IName old, IName @new);
        Task<bool> UpdateTown_CityOfResidence(int? id, IName old, IName @new);
        Task<bool> UpdateLGAOfResidence(int? id, string old, string @new);
        #endregion PUTs

        //DELETE
        Task<bool> RemoveAccount(int? id, IAccount acc);

        Task<bool> ValidatePassword(int? id, ILoginCredentials password);
        Task<bool> ValidateCardPassword(int? id, string num, string cardId, ILoginCredentials password);
        Task<bool> ValidateAndChangePassword(int? id, ILoginCredentials oldValue, ILoginCredentials newValue);
        Task<bool> ValidateAndChangeCardPassword(int? id, string num, string cardId, ILoginCredentials oldValue, ILoginCredentials newValue);
        Task<ICustomer> Verify(int? id, ILoginCredentials pass);
        Task<bool> AddToken(int? id, string @new);
        Task<bool> InvalidateToken(int? id, string @new);
        Task<bool> InvalidateTokens(int? id);
    }
}
