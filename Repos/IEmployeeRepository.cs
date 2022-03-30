using DataBaseTest.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace DataBaseTest.Repos
{
    public interface IEmployeeRepository
    {
        #region  GETs
        //GETs
        Task<IEnumerable<IEmployee>> GetEmployees();
        Task<IEmployee> GetEmployee(int? id);

        Task<IName> GetName(int? id);
        Task<IName> GetSurname(int? id);
        Task<IName> GetNickname(int? id);
        Task<IName> GetMaidenName(int? id);
        Task<IEnumerable<IName>> GetOtherNames(int? id);
        Task<IEnumerable<IName>> GetTitles(int? id);

        Task<string> GetAccountNumber(int? id);

        Task<string> GetPersonalOnlineKey(int? id);
        Task<string> GetUsername(int? id);
        Task<string> GetPassword(int? id);

        Task<DateTime> GetHireDate(int? id);
        Task<WorkingStatus> GetWorkingStatus(int? id);
        Task<decimal> GetSalary(int? id);
        Task<string> GetLevel(int? id);
        Task<string> GetPosition(int? id);

        Task<IEnumerable<IEmployee>> GetGroup(int? id);
        Task<IEmployee> GetSupervisor(int? id);
        Task<IEmployee> GetSuperior(int? id);
        Task<IEmployee> GetSubordinate(int? id);

        Task<IReadOnlyDictionary<string, string>> GetGrades(int? id, string level, string certification);
        Task<IAddress> GetCertificateIssuer(int? id, string level, string certification);
        Task<string> GetGrade(int? id, string level, string certification, string course);

        Task<IPerson> GetGuarantor(int? id);

        Task<byte[]> GetFingerprint(int? id);
        Task<byte[]> GetSignature(int? id);
        Task<byte[]> GetPassport(int? id);
        Task<string> GetJobType(int? id);
        Task<string> GetIdNumberFor(int? id, string idType);
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

        Task<List<string>> GetTokens(int? id);

        #endregion GETs
        
        #region  POSTs
        //POSTs
        Task<PostResult<object>> AddEmployee(IEmployee e);

        Task<bool> SetName(int? id, IName @new);
        Task<bool> SetSurname(int? id, IName @new);
        Task<bool> SetNickname(int? id, IName @new);
        Task<bool> SetMaidenName(int? id, IName @new);
        Task<bool> AddOtherName(int? id, IName @new);
        Task<bool> AddTitle(int? id, string @new);

        Task<bool> SetLoginCredentials(int? id, ILoginCredentials @new);
        Task<bool> SetPersonalOnlineKey(int? id, ILoginCredentials @new);
        Task<bool> SetUsername(int? id, ILoginCredentials @new);
        Task<bool> SetPassword(int? id, ILoginCredentials @new);

        Task<bool> SetHireDate(int? id, DateTime @new);
        Task<bool> SetWorkingStatus(int? id, WorkingStatus @new);
        Task<bool> SetSalary(int? id, decimal @new);
        Task<bool> SetLevel(int? id, string @new);
        Task<bool> SetPosition(int? id, string @new);

        Task<bool> SetGroup(int? id, IEnumerable<IEmployee> @new);
        Task<bool> SetSupervisor(int? id, IEmployee @new);
        Task<bool> SetSuperior(int? id, IEmployee @new);
        Task<bool> SetSubordinate(int? id, IEmployee @new);

        Task<bool> SetGrades(int? id, string level, string certification, IDictionary<string, string> grades);
        Task<bool> SetCertificateIssuer(int? id, string level, string certification, IAddress @new);
        Task<bool> AddGrade(int? id, string level, string certification, string course, string grade);

        Task<bool> SetGuarantor(int? id, IPerson @new);

        Task<bool> SetFingerprint(int? id, byte[] @new);
        Task<bool> SetSignature(int? id, byte[] @new);
        Task<bool> SetPassport(int? id, byte[] @new);
        Task<bool> SetJobType(int? id, string @new);
        Task<bool> AddIdNumberFor(int? id, string idType, string number);
        Task<bool> SetSex(int? id, bool @new);
        Task<bool> SetUniqueTag(int? id, Guid @new);
        Task<bool> SetBirthDate(int? id, DateTime @new);

        Task<bool> SetCountryOfOrigin(int? id, IName @new);
        Task<bool> SetStateOfOrigin(int? id, IName @new);
        Task<bool> SetTown_CityOfOrigin(int? id, IName @new);
        Task<bool> SetLGAOfOrigin(int? id, string @new);
        Task<bool> SetLanguage(int? id, string @new);

        Task<bool> SetNextOfKin(int? id, IPerson @new);

        Task<bool> SetMaritalStatus(int? id, MaritalStatus @new);

        Task<bool> SetBusinessName(int? id, IName bussinesName);
        Task<bool> AddBusinessEmail(int? id, [EmailAddress] string @new);
        Task<bool> AddBusinessMobile(int? id, [Phone] string @new);
        Task<bool> AddBusinessSocialMedia(int? id, [Url] string @new);
        Task<bool> AddBusinessWebsite(int? id, [Url] string @new);
        Task<bool> SetBusinessStreetAddress(int? id, string @new);
        Task<bool> SetBusinessZIPCode(int? id, string @new);
        Task<bool> SetBusinessPMB(int? id, string @new);

        Task<bool> SetBusinessCountryOfResidence(int? id, IName @new);
        Task<bool> SetBusinessStateOfResidence(int? id, IName @new);
        Task<bool> SetBusinessTown_CityOfResidence(int? id, IName @new);
        Task<bool> SetBusinessLGAOfResidence(int? id, string @new);

        Task<bool> SetDisplayName(int? id, IName bussinesName);
        Task<bool> AddEmail(int? id, [EmailAddress] string @new);
        Task<bool> AddMobile(int? id, [Phone] string @new);
        Task<bool> AddSocialMedia(int? id, [Url] string @new);
        Task<bool> AddWebsite(int? id, [Url] string @new);
        Task<bool> SetStreetAddress(int? id, string @new);
        Task<bool> SetZIPCode(int? id, string @new);
        Task<bool> SetPMB(int? id, string @new);

        Task<bool> SetCountryOfResidence(int? id, IName @new);
        Task<bool> SetStateOfResidence(int? id, IName @new);
        Task<bool> SetTown_CityOfResidence(int? id, IName @new);
        Task<bool> SetLGAOfResidence(int? id, string @new);

        #endregion POSTs

        #region PUTs
        //PUTs
        Task<bool> UpdateName(int? id, IName old, IName @new);
        Task<bool> UpdateSurname(int? id, IName old, IName @new);
        Task<bool> UpdateNickname(int? id, IName old, IName @new);
        Task<bool> UpdateMaidenName(int? id, IName old, IName @new);
        Task<bool> UpdateOthername(int? id, IName old, IName @new);
        Task<bool> UpdateTitle(int? id, string old, string @new);

        Task<bool> UpdateAccountNumber(int? id, string old, string @new);

        Task<bool> UpdatePersonalOnlineKey(int? id, ILoginCredentials old, ILoginCredentials @new);
        Task<bool> UpdateUsername(int? id, ILoginCredentials old, ILoginCredentials @new);
        Task<bool> UpdatePassword(int? id, ILoginCredentials old, ILoginCredentials @new);

        Task<bool> UpdateHireDate(int? id, DateTime old, DateTime @new);
        Task<bool> UpdateWorkingStatus(int? id, WorkingStatus old, WorkingStatus @new);
        Task<bool> UpdateSalary(int? id, decimal old, decimal @new);
        Task<bool> UpdateLevel(int? id, string old, string @new);
        Task<bool> UpdatePosition(int? id,  string old, string @new);

        Task<bool> UpdateGroup(int? id, IEnumerable<IEmployee> old, IEnumerable<IEmployee> @new);
        Task<bool> UpdateSupervisor(int? id, IEmployee old, IEmployee @new);
        Task<bool> UpdateSuperior(int? id, IEmployee old, IEmployee @new);
        Task<bool> UpdateSubordinate(int? id, IEmployee old, IEmployee @new);

        Task<bool> UpdateCertificateIssuer(int? id, string level, string certification, IAddress old, IAddress @new);
        Task<bool> UpdateGrades(int? id, string level, string certification, IDictionary<string, string> old, IDictionary<string, string> grades);
        Task<bool> UpdateGrade(int? id, string level, string certification, string course, string oldGrade, string newGrade);

        Task<bool> UpdateGuarantor(int? id, IPerson old, IPerson @new);

        Task<bool> UpdateFingerprint(int? id, byte[] old, byte[] @new);
        Task<bool> UpdateSignature(int? id, byte[] old, byte[] @new);
        Task<bool> UpdatePassport(int? id, byte[] old, byte[] @new);
        Task<bool> UpdateJobType(int? id,  string old, string @new);
        Task<bool> UpdateIdNumberFor(int? id, string idType, string oldNumber, string newNumber);
        Task<bool> UpdateSex(int? id, bool old, bool @new);
        Task<bool> UpdateUniqueTag(int? id, Guid old, Guid @new);
        Task<bool> UpdateBirthDate(int? id, DateTime old, DateTime @new);

        Task<bool> UpdateCountryOfOrigin(int? id, IName old, IName @new);
        Task<bool> UpdateStateOfOrigin(int? id, IName old, IName @new);
        Task<bool> UpdateTown_CityOfOrigin(int? id, IName old, IName @new);
        Task<bool> UpdateLGAOfOrigin(int? id, string old, string @new);
        Task<bool> UpdateLanguage(int? id, string old, string @new);

        Task<bool> UpdateNextOfKin(int? id, IPerson old, IPerson @new);

        Task<bool> UpdateMaritalStatus(int? id, MaritalStatus old, MaritalStatus @new);

        Task<bool> UpdateBusinessName(int? id, IName old, IName @new);
        Task<bool> UpdateBusinessEmail(int? id, [EmailAddress] string old, [EmailAddress] string @new);
        Task<bool> UpdateBusinessMobile(int? id, [Phone] string old, [Phone] string @new);
        Task<bool> UpdateBusinessSocialMedia(int? id, [Url] string old, [Url] string @new);
        Task<bool> UpdateBusinessWebsite(int? id, [Url] string old, [Url] string @new);
        Task<bool> UpdateBusinessStreetAddress(int? id, string old, string @new);
        Task<bool> UpdateBusinessZIPCode(int? id, string old, string @new);
        Task<bool> UpdateBusinessPMB(int? id, string old, string @new);

        Task<bool> UpdateBusinessCountryOfResidence(int? id, IName old, IName @new);
        Task<bool> UpdateBusinessStateOfResidence(int? id, IName old, IName @new);
        Task<bool> UpdateBusinessTown_CityOfResidence(int? id, IName old, IName @new);
        Task<bool> UpdateBusinessLGAOfResidence(int? id, string old, string @new);

        Task<bool> UpdateDisplayName(int? id, IName old, IName @new);
        Task<bool> UpdateEmail(int? id, [EmailAddress] string old, [EmailAddress] string @new);
        Task<bool> UpdateMobile(int? id, [Phone] string old, [Phone] string @new);
        Task<bool> UpdateSocialMedia(int? id, [Url] string old, [Url] string @new);
        Task<bool> UpdateWebsite(int? id, [Url] string old, [Url] string @new);
        Task<bool> UpdateStreetAddress(int? id, string old, string @new);
        Task<bool> UpdateZIPCode(int? id, string old, string @new);
        Task<bool> UpdatePMB(int? id, string old, string @new);

        Task<bool> UpdateCountryOfResidence(int? id, IName old, IName @new);
        Task<bool> UpdateStateOfResidence(int? id, IName old, IName @new);
        Task<bool> UpdateTown_CityOfResidence(int? id, IName old, IName @new);
        Task<bool> UpdateLGAOfResidence(int? id, string old, string @new);
        #endregion PUTs

        //Verify
        Task<bool> ValidatePassword(int? Id, ILoginCredentials password);
        Task<bool> ValidateAndChangePassword(int? Id, ILoginCredentials oldValue, ILoginCredentials newValue, object hashSource);
        Task<IEmployee> Verify(int? id, ILoginCredentials loginCredential);
        Task<bool> AddToken(int? id, string @new);
        Task<bool> InvalidateToken(int? id, string @new);
        Task<bool> InvalidateTokens(int? id);

        #region DELETEs
        //DELETE
        Task<IEmployee> Remove(int? id);

        Task<IName> RemoveOtherName(int? id, IName val);
        Task<string> RemoveTitle(int? id, string val);

        Task<string> RemoveId(int? id, string val);

        Task<string> RemoveGrade(int? id, string certification, string course);

        Task<string> RemoveBusinessEmail(int? id, [EmailAddress]string val);
        Task<string> RemoveBusinessMobile(int? id, [Phone] string val);
        Task<string> RemoveBusinessSocialMedia(int? id, [Url] string val);
        Task<string> RemoveBusinessWebsites(int? id, [Url] string val);

        Task<string> RemoveEmail(int? id, [EmailAddress] string val);
        Task<string> RemoveMobile(int? id, [Phone] string val);
        Task<string> RemoveSocialMedia(int? id, [Url] string val);
        Task<string> RemoveWebsite(int? id, [Url] string val);
        #endregion DELETEs
    }
}