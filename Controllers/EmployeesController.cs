using DataBaseTest.Repos;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using static DataBaseTest.Repos.Builders;
using static DataBaseTest.Controllers.FormTitles;
using static DataBaseTest.Utilities;
using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;
using DataBaseTest.Handlers;
using static DataBaseTest.Controllers.CustomersController;

namespace DataBaseTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = Policy3)]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _employees;
        //private readonly EmployeeJWTSettings _jwtSettings;
        private readonly IAccountService _accountService;

        public EmployeesController(IEmployeeRepository employees)
        {
            _employees = employees;
            //_jwtSettings = options.Value;
            _accountService = new AccountService(15);
        }

        [HttpPost("login/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromRoute] int id, [FromBody] object x)
        {
            ILoginCredentials credential;
            try
            {
                var jsonObject = JsonSerializer.Serialize(x);
                credential = JsonSerializer.Deserialize<CustomersController.LoginModel>(jsonObject);
                if (credential == null)
                    return BadRequest(new BadHttpRequestException("The input type is not the same as expected"));
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex);
            }
            catch (JsonException ex)
            {
                return BadRequest(ex);
            }
            catch (NotSupportedException ex)
            {
                return BadRequest(ex);
            }

            var val = await _employees.Verify(id, credential);
                if (val == default)
                return NotFound();

            SignedEmployee emp = new(val, id);

            var jwt = _accountService.SignIn(id, credential, GetJWTSettings(Int32.Parse(emp.Level)));
            await _employees.AddToken(id, $"{jwt.RefreshToken}:{jwt.AccessToken}");
            return Ok(jwt);
        }

        [HttpPost("{id}/logout")]
        public async Task<IActionResult> LogoutAsync([FromRoute] int id, [FromBody] object x)
        {
            if (!_accountService.HasValidToken())
                return Forbid("Illegal access detected");
            TokenRef tref;
            try
            {
                var jsonObject = JsonSerializer.Serialize(x);
                ILoginCredentials credential = JsonSerializer.Deserialize<LoginModel>(jsonObject);
                if (credential == null)
                    return BadRequest(new BadHttpRequestException("The input type is not the same as expected"));
                await _employees.InvalidateTokens(id);
                x = await _accountService.Signout(credential);
                jsonObject = JsonSerializer.Serialize(x);
                tref = JsonSerializer.Deserialize<TokenRef>(jsonObject);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex);
            }
            catch (JsonException ex)
            {
                return BadRequest(ex);
            }
            catch (NotSupportedException ex)
            {
                return BadRequest(ex);
            }

            return Ok(tref);
        }

        /// <summary>
        /// Password must not be lesser than 6 characters and must not be greater than 18. Any value
        /// can be used as a password character with the exception of  "\uF71A" (private use character)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var employees = await _employees.GetEmployees();
            if (!employees.Any())
                return NotFound();
            return Ok(employees);
        }

        /*Since only one parameter in the route there is no need for type specification such as int*/
        [HttpGet("{id}")]
        [Authorize(Policy = Policy2)]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var emp = await _employees.GetEmployee(id);
            if (emp == null) return NotFound();
            return Ok(emp);
        }

        [HttpGet("{id}/password")]
        [Authorize(Policy = Policy1)]
        public async Task<IActionResult> GetPassword([FromRoute]int id)
        {
            try
            {
                var val = await _employees.GetPassword(id);
                if (val == default) return StatusCode(502, "Security exception. Try again later!");
                return Ok(val);
            }
            catch (ApplicationNullReferenceException ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            IFormCollection form;
            try
            {
                form = HttpContext.Request.Form;
            }
            catch (Exception)
            {
                return BadRequest();
            }
            var formFile = form.Files;

            var employeeBuilder = new EmployeeBuilder();

            //Person
            var personBuilder = new PersonBuilder();
            var uidList = new List<int>();// for Guid
            //var secretBuilder = new StringBuilder();// for secret

            //Full name
            var fnb = new FullNameBuilder();
            var n = form[FirstName].ToString().Trim();
            fnb.SetName(new NameBuilder().SetName(n).Build());
            n = form[Nickname].ToString().Trim();
            fnb.SetNickname(new NameBuilder().SetName(n).Build());
            n = form[Surname].ToString().Trim();
            fnb.SetSurname(new NameBuilder().SetName(n).Build());
            n = form[MaidenName].ToString().Trim();
            fnb.SetMaidenName(new NameBuilder().SetName(n).Build());
            n = form[Title].ToString().Trim();
            fnb.SetTitle(n);
            var na = form[OtherNames].ToString().Split(",",
                StringSplitOptions.TrimEntries & StringSplitOptions.RemoveEmptyEntries)
                .Select(s => new NameBuilder().SetName(s).Build()).ToList();
            fnb.AddOthers(na);
            var fullName = fnb.Build();
            if (fullName == null) // This means that all its fields are null
                throw new NullReferenceException("all Fields in full name cannot be null");

            personBuilder.SetFullName(fullName);
            uidList.Add(fullName.GetHashCode());

            //Nation of Origin
            //Please find a way to validate these values
            // maybe a Google Map API?
            var nationBuilder = new NationalityBuilder();
            n = form[CountryOfOrigin].ToString().Trim();
            nationBuilder.SetCountryName(new NameBuilder().SetName(n).Build());
            n = form[StateOfOrigin].ToString().Trim();
            nationBuilder.SetState(new NameBuilder().SetName(n).Build());
            n = form[LocalGovernmentOfOrigin].ToString().Trim();
            nationBuilder.SetLGA(n);
            n = form[Language].ToString().Trim();
            nationBuilder.SetLanguage(n);
            n = form[CityTownOfOrigin].ToString().Trim();
            nationBuilder.SetCityTown(new NameBuilder().SetName(n).Build());
            var nationality = nationBuilder.Build();
            if (nationality == null) // This means that all its fields are null
                throw new NullReferenceException("all Fields in place of origin cannot be null");
            personBuilder.SetCountryOfOrigin(nationality);
            uidList.Add(nationality.GetHashCode());

            //Entity
            var entityBuilder = new EntityBuilder();
            n = form[DisplayName].ToString().Trim();
            entityBuilder.SetName(new NameBuilder().SetName(n).Build());

            //Contact
            var contactBuilder = new ContactBuilder();

            //Address
            var addressBuilder = new AddressBuilder();

            //Nation of Residence
            nationBuilder.Clear();
            n = form[CountryOfResidence].ToString().Trim();
            nationBuilder.SetCountryName(new NameBuilder().SetName(n).Build());
            n = form[StateOfResidence].ToString().Trim();
            nationBuilder.SetState(new NameBuilder().SetName(n).Build());
            n = form[LocalGovernmentOfResidence].ToString().Trim();
            nationBuilder.SetLanguage(form[Language].ToString().Trim());
            nationBuilder.SetLGA(n);
            n = form[CityTownOfResidence].ToString().Trim();
            nationBuilder.SetCityTown(new NameBuilder().SetName(n).Build());
            nationality = nationBuilder.Build();
            if (nationality == null) // This means that all its fields are null
                throw new NullReferenceException("all Fields in place of residence cannot be null");
            addressBuilder.SetCountryOfResidence(nationality);

            //Please validate the following lines to ensure the user does not fabricate values
            n = form[HomeAddress].ToString().Trim();
            addressBuilder.SetStreetAddress(n);
            n = form[HomeAddressZipCode].ToString().Trim();
            addressBuilder.SetZipCode(n);
            n = form[HomeAddressPMB].ToString().Trim();
            addressBuilder.SetPMB(n);
            var address = addressBuilder.Build();
            if (address == null) // This means that all its fields are null
                throw new NullReferenceException("all Fields in address cannot be null");
            contactBuilder.SetAddress(address);
            uidList.Add(address.GetHashCode());

            /*
             * Each of the emails, phone numbers, websites and social media should have their own separate textfield in the form.
             * For example, if a user has 3 emails, then each of them should be in thier own textbox.
             * 
             * Also Please find a way to validate these values so as to prevent fabrication
             */
            contactBuilder.AddEmails(GetMultipleValues(PersonalEmail_, form));
            contactBuilder.AddSocialMedia(GetMultipleValues(PersonalSocialMedia_, form));
            contactBuilder.AddMobiles(GetMultipleValues(PersonalMobile_, form));
            contactBuilder.AddWebsite(GetMultipleValues(PersonalWebsite_, form));

            var contact = contactBuilder.Build();
            if (contact == null) // This means that all its fields are null
                throw new NullReferenceException("all Fields in contact cannot be null");
            entityBuilder.SetContact(contact);

            var entity = entityBuilder.Build();
            if (entity == null) // This means that all its fields are null
                throw new NullReferenceException("all Fields in entity cannot be null");
            personBuilder.SetEntity(entity);

            //Next of kin
            var nokBuilder = new PersonBuilder();
            entityBuilder.Clear();
            //This is for the next of kin IPerson property "Entity"
            n = form[NextOfKinRelationship].ToString().Trim();
            entityBuilder.SetName(new NameBuilder().SetName(n).Build());
            contactBuilder.Clear();
            contactBuilder.AddMobiles(GetMultipleValues(NextOfKinMobile_, form));
            contactBuilder.AddSocialMedia(GetMultipleValues(NextOfKinSocialMedia_, form));
            contactBuilder.AddEmails(GetMultipleValues(NextOfKinEmail_, form));
            addressBuilder.Clear();
            nationBuilder.Clear();
            n = form[NextOfKinCountryOfResidence].ToString().Trim();
            nationBuilder.SetCountryName(new NameBuilder().SetName(n).Build());
            nationBuilder.SetLanguage(form[NextOfKinLanguage].ToString().Trim());
            n = form[NextOfKinStateOfResidence].ToString().Trim();
            nationBuilder.SetState(new NameBuilder().SetName(n).Build());
            nationBuilder.SetLGA(form[NextOfKinLocalGovernmentOfResidence].ToString().Trim());
            n = form[NextOfKinCityTownOfResidence].ToString().Trim();
            nationBuilder.SetCityTown(new NameBuilder().SetName(n).Build());
            addressBuilder.SetCountryOfResidence(nationBuilder.Build());
            n = form[NextOfKinHomeAddress].ToString().Trim();
            addressBuilder.SetStreetAddress(n);
            addressBuilder.SetPMB(form[NextOfKinHomeAddressPMB].ToString().Trim());
            addressBuilder.SetZipCode(form[NextOfKinHomeAddressZipCode].ToString().Trim());
            contactBuilder.SetAddress(addressBuilder.Build());
            entityBuilder.SetContact(contactBuilder.Build());
            nokBuilder.SetEntity(entityBuilder.Build());
            //This is for the next of kin IPerson property "OfficialEntity"
            addressBuilder.Clear();
            nationBuilder.Clear();
            entityBuilder.Clear();
            contactBuilder.Clear();
            n = form[NextOfKinCompanyName].ToString().Trim();
            entityBuilder.SetName(new NameBuilder().SetName(n).Build());
            n = form[NextOfKinCompanyCountryOfResidence].ToString().Trim();
            nationBuilder.SetCountryName(new NameBuilder().SetName(n).Build());
            nationBuilder.SetLanguage(form[NextOfKinLanguage].ToString().Trim());
            n = form[NextOfKinCompanyStateOfResidence].ToString().Trim();
            nationBuilder.SetState(new NameBuilder().SetName(n).Build());
            nationBuilder.SetLGA(form[NextOfKinCompanyLGAOfResidence]);
            n = form[NextOfKinCompanyCityTownResidence].ToString().Trim();
            nationBuilder.SetCityTown(new NameBuilder().SetName(n).Build());
            addressBuilder.SetCountryOfResidence(nationBuilder.Build());
            n = form[NextOfKinCompanyAddress].ToString().Trim();
            addressBuilder.SetStreetAddress(n);
            addressBuilder.SetPMB(form[NextOfKinCompanyAddressPMB].ToString().Trim());
            addressBuilder.SetZipCode(form[NextOfKinCompanyAddressZipCode].ToString().Trim());
            contactBuilder.SetAddress(addressBuilder.Build());
            contactBuilder.AddEmails(GetMultipleValues(NextOfKinCompanyEmail_, form));
            contactBuilder.AddSocialMedia(GetMultipleValues(NextOfKinCompanySocialMedia_, form));
            contactBuilder.AddMobiles(GetMultipleValues(NextOfKinCompanyMobile_, form));
            contactBuilder.AddWebsite(GetMultipleValues(NextOfKinCompanyWebsite_, form));
            entityBuilder.SetContact(contactBuilder.Build());
            nokBuilder.SetOfficialEntity(entityBuilder.Build());
            //Next of Kin Country of Origin
            nationBuilder.Clear();
            n = form[NextOfKinCountryOfOrigin].ToString().Trim();
            nationBuilder.SetCountryName(new NameBuilder().SetName(n).Build());
            n = form[NextOfKinStateOfOrigin].ToString().Trim();
            nationBuilder.SetState(new NameBuilder().SetName(n).Build());
            n = form[NextOfKinLGAOfOrigin].ToString().Trim();
            nationBuilder.SetLGA(n);
            n = form[NextOfKinCityTownOfOrigin].ToString().Trim();
            nationBuilder.SetCityTown(new NameBuilder().SetName(n).Build());
            nationBuilder.SetLanguage(form[NextOfKinLanguage].ToString().Trim());
            nokBuilder.SetCountryOfOrigin(nationBuilder.Build());
            //Next of kin name
            fnb.Clear();
            n = form[NextOfKinFirstName].ToString().Trim();
            fnb.SetName(new NameBuilder().SetName(n).Build());
            n = form[NextOfKinSurname].ToString().Trim();
            fnb.SetSurname(new NameBuilder().SetName(n).Build());
            nokBuilder.SetFullName(fnb.Build());
            nokBuilder.SetUniqueTag(Guid.NewGuid());
            nokBuilder.SetMaritalStatus(GetMStatus(form, NextOfKinMaritalStatus));
            nokBuilder.SetSex(form[NextOfKinSex].ToString().Trim().CompareTo("Male") == 0);
            nokBuilder.SetJobType(form[NextOfKinJobType].ToString().Trim());
            var nok = nokBuilder.Build();
            personBuilder.SetNextOfKin(nok);
            uidList.Add(nok.GetHashCode());

            //This value is suspicious. What if the value is an empty string?
            var bDate = GetDate(form[PersonalBirthYear].ToString().Trim(), form[PersonalBirthMonth].ToString().Trim(), form[PersonalBirthDay].ToString().Trim());
            var mStatus = GetMStatus(form, MaritalStatus);
            var sex = form[Sex].ToString().Trim().CompareTo("Male") == 0;
            personBuilder.SetMaritalStatus(mStatus);
            personBuilder.SetSex(sex);
            personBuilder.SetBirthDate(bDate);
            uidList.Add((int)mStatus);
            uidList.Add(sex ? 1 : 0);
            uidList.Add(bDate.Day);
            uidList.Add(bDate.Month);
            uidList.Add(bDate.Year);
            uidList.Add(bDate.Millisecond);
            uidList.Add(bDate.GetHashCode());
            personBuilder.AddIdentifications(GetIds(form, PersonalIdType_, PersonalIdNumber_));

            //GUID
            var uid = new Guid(
                    uidList[0], (short)uidList[1], (short)uidList[2], (byte)uidList[3], (byte)uidList[4],
                     (byte)uidList[5], (byte)uidList[6], (byte)uidList[7], (byte)uidList[8], (byte)uidList[9],
                      (byte)uidList[10]
                );
            personBuilder.SetUniqueTag(uid);

            //Biometrics
            //Passport
            var file = formFile[Passport];
            if (file != null)
            {
                var fileSize = file.Length;
                if (file.Length >= 0)
                {
                    try
                    {
                        //byte[] buffer = new byte[fileSize];
                        //using var f = file.OpenReadStream();
                        //f.Read(buffer, 0, (int)fileSize);
                        var stream = new MemoryStream((int)fileSize);
                        await file.CopyToAsync(stream);
                        var buffer = stream.ToArray();

                        personBuilder.SetPassport(buffer);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            //Signature
            file = formFile[Signature];
            if (file != null)
            {
                var fileSize = file.Length;
                if (file.Length > 0)
                {
                    try
                    {
                        //byte[] buffer = new byte[fileSize];
                        //using var f = file.OpenReadStream();
                        //f.Read(buffer, 0, (int)fileSize);
                        var stream = new MemoryStream((int)fileSize);
                        await file.CopyToAsync(stream);
                        var buffer = stream.ToArray();

                        personBuilder.SetSignature(buffer);
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }
            //Finger print
            file = formFile[FingerPrint];
            if (file != null)
            {
                var fileSize = file.Length;
                if (file.Length > 0)
                {
                    try
                    {
                        //byte[] buffer = new byte[fileSize];
                        //using var f = file.OpenReadStream();
                        //f.Read(buffer, 0, (int)fileSize);
                        var stream = new MemoryStream((int)fileSize);
                        await file.CopyToAsync(stream);
                        var buffer = stream.ToArray();

                        personBuilder.SetFingerPrint(buffer);
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }

            var person = personBuilder.Build();
            employeeBuilder.SetPerson(person);
            //Guarantor
            var guarantorBuilder = new PersonBuilder();
            contactBuilder.Clear();
            addressBuilder.Clear();
            nationBuilder.Clear();
            entityBuilder.Clear();
            //This for the guarantor's IPerson property "Entity"
            n = form[GuarantorRelationship].ToString().Trim();
            entityBuilder.SetName(new NameBuilder().SetName(n).Build());
            contactBuilder.AddMobiles(GetMultipleValues(GuarantorPrivateMobile_, form));
            contactBuilder.AddSocialMedia(GetMultipleValues(GuarantorPrivateSocialMedia_, form));
            contactBuilder.AddEmails(GetMultipleValues(GuarantorPrivateEmail_, form));
            n = form[GuarantorCountryOfResidence].ToString().Trim();
            nationBuilder.SetCountryName(new NameBuilder().SetName(n).Build());
            nationBuilder.SetLanguage(form[GuarantorLanguage].ToString().Trim());
            n = form[GuarantorStateOfResidence].ToString().Trim();
            nationBuilder.SetState(new NameBuilder().SetName(n).Build());
            nationBuilder.SetLGA(form[GuarantorLGAOfResidence].ToString().Trim());
            n = form[GuarantorCityTownOfResidence].ToString().Trim();
            nationBuilder.SetCityTown(new NameBuilder().SetName(n).Build());
            addressBuilder.SetCountryOfResidence(nationBuilder.Build());
            n = form[GuarantorHomeAddress].ToString().Trim();
            addressBuilder.SetStreetAddress(n);
            addressBuilder.SetPMB(form[GuarantorHomePMB].ToString().Trim());
            addressBuilder.SetZipCode(form[GuarantorHomeZipCode].ToString().Trim());
            contactBuilder.SetAddress(addressBuilder.Build());
            entityBuilder.SetContact(contactBuilder.Build());
            guarantorBuilder.SetEntity(entityBuilder.Build());

            //This for the guarantor's IPerson property "OfficalEntity"
            contactBuilder.Clear();
            addressBuilder.Clear();
            nationBuilder.Clear();
            entityBuilder.Clear();
            n = form[GuarantorCompanyName].ToString().Trim();
            entityBuilder.SetName(new NameBuilder().SetName(n).Build());
            n = form[GuarantorCompanyCountryOfResidence].ToString().Trim();
            nationBuilder.SetCountryName(new NameBuilder().SetName(n).Build());
            nationBuilder.SetLanguage(form[GuarantorLanguage].ToString().Trim());
            n = form[GuarantorCompanyStateOfResidence].ToString().Trim();
            nationBuilder.SetState(new NameBuilder().SetName(n).Build());
            nationBuilder.SetLGA(form[GuarantorCompanyLGAOfResidence]);
            n = form[GuarantorCompanyCityTownOfResidence].ToString().Trim();
            nationBuilder.SetCityTown(new NameBuilder().SetName(n).Build());
            addressBuilder.SetCountryOfResidence(nationBuilder.Build());
            n = form[GuarantorCompanyAddress].ToString().Trim();
            addressBuilder.SetStreetAddress(n);
            addressBuilder.SetPMB(form[GuarantorCompanyAddressPMB].ToString().Trim());
            addressBuilder.SetZipCode(form[GuarantorCompanyAddressZipCode].ToString().Trim());
            contactBuilder.SetAddress(addressBuilder.Build());
            contactBuilder.AddEmails(GetMultipleValues(GuarantorCompanyEmail_, form));
            contactBuilder.AddSocialMedia(GetMultipleValues(GuarantorCompanySocialMedia_, form));
            contactBuilder.AddMobiles(GetMultipleValues(GuarantorCompanyMobile_, form));
            contactBuilder.AddWebsite(GetMultipleValues(GuarantorCompanyWebsite_, form));
            entityBuilder.SetContact(contactBuilder.Build());
            guarantorBuilder.SetOfficialEntity(entityBuilder.Build());

            //Guarantor name
            fnb.Clear();
            n = form[GuarantorFirstName].ToString().Trim();
            fnb.SetName(new NameBuilder().SetName(n).Build());
            n = form[GuarantorSurname].ToString().Trim();
            fnb.SetSurname(new NameBuilder().SetName(n).Build());
            n = form[GuarantorTitle].ToString().Trim();
            fnb.SetTitle(n);
            n = form[GuarantorNickname].ToString().Trim();
            fnb.SetNickname(new NameBuilder().SetName(n).Build());
            na = form[GuarantorOtherNames].ToString().Split(",",
                StringSplitOptions.TrimEntries & StringSplitOptions.RemoveEmptyEntries)
                .Select(s => new NameBuilder().SetName(s).Build()).ToList();
            fnb.AddOthers(na);
            guarantorBuilder.SetFullName(fnb.Build());

            //Guarantor Country of Origin
            nationBuilder.Clear();
            n = form[GuarantorCountryOfOrigin].ToString().Trim();
            nationBuilder.SetCountryName(new NameBuilder().SetName(n).Build());
            n = form[GuarantorStateOfOrigin].ToString().Trim();
            nationBuilder.SetState(new NameBuilder().SetName(n).Build());
            n = form[GuarantorLGAOfOrigin].ToString().Trim();
            nationBuilder.SetLGA(n);
            n = form[GuarantorCityTownOfOrigin].ToString().Trim();
            nationBuilder.SetCityTown(new NameBuilder().SetName(n).Build());
            nationBuilder.SetLanguage(form[GuarantorLanguage].ToString().Trim());
            guarantorBuilder.SetCountryOfOrigin(nationBuilder.Build());

            //Guarantor Misc.
            guarantorBuilder.SetSex(form[GuarantorSex].ToString().Trim().CompareTo("Male") == 0);
            guarantorBuilder.SetBirthDate(GetDate(form[GuarantorBirthYear].ToString().Trim(), form[GuarantorBirthMonth].ToString().Trim(), form[GuarantorBirthDay].ToString().Trim()));
            guarantorBuilder.SetMaritalStatus(GetMStatus(form, GuarantorMaritalStatus));
            guarantorBuilder.SetJobType(form[GuarantorJobType].ToString().Trim());
            guarantorBuilder.AddIdentifications(GetIds(form, GuarantorIdType_, GuarantorIdNumber_));
            guarantorBuilder.SetUniqueTag(Guid.NewGuid());

            employeeBuilder.SetGuarantor(guarantorBuilder.Build());

            //Account
            var accountBuilder = new AccountBuilder();
            //Should validate number length
            accountBuilder.SetNumber(form[PersonalAcctNumber].ToString().Trim());
            accountBuilder.SetCurrency(Models.Currencies.TryParse(566)); // Naira account
            var acct = accountBuilder.Build();
            if (acct == null) // This means that all its fields are null
                throw new NullReferenceException("all Fields in account cannot be null");
            employeeBuilder.SetAccount(acct);

            //Education
            var eduBuilder = new EducationBuilder();
            var qBuilder = new QualificationBuilder();
            //Tertiary
            n = form[TertiaryCertName].ToString().Trim();
            qBuilder.SetCertification(n);
            qBuilder.AddGrades(GetMap(form, CourseNameAndLevel_, TertiaryScore_));
            addressBuilder.Clear();
            nationBuilder.Clear();
            n = form[TertiaryCountryOfResidence].ToString().Trim();
            nationBuilder.SetCountryName(new NameBuilder().SetName(n).Build());
            n = form[TertiaryStateOfResidence].ToString().Trim();
            nationBuilder.SetState(new NameBuilder().SetName(n).Build());
            n = form[TertiaryLGAOfResidence].ToString().Trim();
            nationBuilder.SetLGA(n);
            n = form[TertiaryCityTownOfResidence].ToString().Trim();
            nationBuilder.SetCityTown(new NameBuilder().SetName(n).Build());
            addressBuilder.SetCountryOfResidence(nationBuilder.Build());
            n = form[TertiaryAddress].ToString().Trim();
            addressBuilder.SetStreetAddress(n);
            addressBuilder.SetPMB(form[TertiaryAddressPMB].ToString().Trim());
            addressBuilder.SetZipCode(form[TertiaryAddressZipCode].ToString().Trim());
            qBuilder.SetAcademy(addressBuilder.Build());
            eduBuilder.SetPrimaryTertiary(qBuilder.Build());
            //Primary
            qBuilder.Clear();
            n = form[PrimaryCertName].ToString().Trim();
            qBuilder.SetCertification(n);
            qBuilder.AddGrades(GetMap(form, PrimarySubjectName_, PrimaryScore_));
            addressBuilder.Clear();
            nationBuilder.Clear();
            n = form[PrimaryCountryOfResidence].ToString().Trim();
            nationBuilder.SetCountryName(new NameBuilder().SetName(n).Build());
            n = form[PrimaryStateOfResidence].ToString().Trim();
            nationBuilder.SetState(new NameBuilder().SetName(n).Build());
            n = form[PrimaryLGAOfResidence].ToString().Trim();
            nationBuilder.SetLGA(n);
            n = form[PrimaryCityTownOfResidence].ToString().Trim();
            nationBuilder.SetCityTown(new NameBuilder().SetName(n).Build());
            addressBuilder.SetCountryOfResidence(nationBuilder.Build());
            n = form[PrimaryAddress].ToString().Trim();
            addressBuilder.SetStreetAddress(n);
            addressBuilder.SetPMB(form[PrimaryAddressPMB].ToString().Trim());
            addressBuilder.SetZipCode(form[PrimaryAddressZipCode].ToString().Trim());
            qBuilder.SetAcademy(addressBuilder.Build());
            eduBuilder.SetPrimary(qBuilder.Build());
            //Secondary
            qBuilder.Clear();
            n = form[SecondaryCertName].ToString().Trim();
            qBuilder.SetCertification(n);
            qBuilder.AddGrades(GetMap(form, SecondarySubjectName_, SecondaryScore_));
            addressBuilder.Clear();
            nationBuilder.Clear();
            n = form[SecondaryCountryOfResidence].ToString().Trim();
            nationBuilder.SetCountryName(new NameBuilder().SetName(n).Build());
            n = form[SecondaryStateOfResidence].ToString().Trim();
            nationBuilder.SetState(new NameBuilder().SetName(n).Build());
            n = form[SecondaryLGAOfResidence].ToString().Trim();
            nationBuilder.SetLGA(n);
            n = form[SecondaryCityTownOfResidence].ToString().Trim();
            nationBuilder.SetCityTown(new NameBuilder().SetName(n).Build());
            addressBuilder.SetCountryOfResidence(nationBuilder.Build());
            n = form[SecondaryAddress].ToString().Trim();
            addressBuilder.SetStreetAddress(n);
            addressBuilder.SetPMB(form[SecondaryAddressPMB].ToString().Trim());
            addressBuilder.SetZipCode(form[SecondaryAddressZipCode].ToString().Trim());
            qBuilder.SetAcademy(addressBuilder.Build());
            eduBuilder.SetSecondary(qBuilder.Build());
            employeeBuilder.SetQualification(eduBuilder.Build());

            employeeBuilder.SetLevel(form[Level].ToString().Trim());
            employeeBuilder.SetPosition(form[Position].ToString().Trim());
            bDate = GetDate(form[HireYear].ToString().Trim(), form[HireMonth].ToString().Trim(), form[HireDay].ToString().Trim());
            employeeBuilder.SetHireDate(bDate);
            employeeBuilder.SetWorkingStatus((Models.WorkingStatus)Int32.Parse(form[WorkingStatus].ToString().Trim()));

            var loginBuilder = new LoginCredentialsBuilder();
            var uidString = uid.ToString();
            var rawHex = uidString[^6..];
            //var decimalString = rawHex;//Encoding.UTF8.GetString(Convert.FromHexString(rawHex));
            //Console.WriteLine("uidString: " + uidString + ", rawHex: " + rawHex);
            var id = UInt32.Parse(rawHex, NumberStyles.HexNumber);
            loginBuilder.SetPersonalOnlineKey(id.ToString());
            loginBuilder.SetUsername(person.Entity.Name.Name);
            /*
             * Please impose additional requirement on the front end to improve strength
             * Additional requirements such as length, compulsory inclusion of symbols and numerals etc
             */
            var s = form[Secret].ToString().Trim();
            n = s;
            loginBuilder.SetPassword(n);
            employeeBuilder.SetLoginCredentials(loginBuilder.Build());//Password will be null here
            try
            {
                var x = await _employees.AddEmployee(employeeBuilder.Build());
                if (x.Successful)
                {
                    /*This is where the password is added*/
                    //Console.WriteLine("PersonalOnlineKey: " + id + ", Username: " + person.Entity.Name.Name + ", Password: " + n);
                    //await _employees.ValidateAndChangePassword(x.Id, null, loginBuilder.Build(), employeeBuilder.Build());
                    UpdateLookup(id, $"name={person.Entity.Name.Name},dbid={x.Id}");
                    return CreatedAtAction(nameof(Get), new { id, controller = "employees" }, employeeBuilder.Build());
                }
                else return Conflict();
            }
            finally
            {
                form = null;
            }

            //return CreatedAtAction(nameof(GetEmployee), new { id = id, controller = "employees"}, employeeBuilder.Build());
            //Console.WriteLine(Utilities.EmployeeIdDictionary);
        }

        private static IJWTSettings GetJWTSettings(int level)
        {
            IConfiguration config = new ConfigurationBuilder()
                                        .SetBasePath(Directory.GetCurrentDirectory())
                                        .AddJsonFile("appsettings.json")
                                        .Build();
            if (level >= 9)
                return config.GetSection(nameof(AdminAdminJWTSettings)).Get<AdminAdminJWTSettings>();
            else if (level >= 6)
                return config.GetSection(nameof(EmployeeAdminJWTSettings)).Get<EmployeeAdminJWTSettings>();
            else if (level > 4)
                return config.GetSection(nameof(EmployeeJWTSettings)).Get<EmployeeJWTSettings>();
            throw new ApplicationException("Employee level is too low");
        }

        public class LoginObject
        {
            [Required]
            [MinLength(5)]
            public string Key { get; set; }
            [Required]
            [MinLength(3)]
            public string Name { get; set; }
            [Required]
            [MinLength(6)]
            public string Value { get; set; }
        }

        public class StringObject
        {
            [Required(AllowEmptyStrings = false)]
            [MinLength(3)]
            public string Value { get; set; }
        }
    }
}
