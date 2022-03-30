using DataBaseTest.Repos;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static DataBaseTest.Repos.Builders;
using static DataBaseTest.Controllers.FormTitles;
using static DataBaseTest.Utilities;
using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using static DataBaseTest.Controllers.EmployeesController;
using System.Text.Json;
using DataBaseTest.Handlers;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace DataBaseTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomersController: ControllerBase
    {
        private readonly ICustomerRepository _customers;
        private static string _unallocated;
        private static IEnumerator<uint> numGen = Numbers((uint)Guid.NewGuid().GetHashCode());
        private static readonly List<uint> signatures = new();
        private readonly IAccountService _accountService;
        //private readonly 

        public CustomersController(ICustomerRepository customers)
        {
            _customers = customers;
            _accountService = new AccountService(15);
        }

        [AllowAnonymous]
        internal class LoginModel : ILoginCredentials
        {
            public string PersonalOnlineKey { get; set; }

            public string Username { get; set; }

            public string Password { get; set; }
        }

        internal class TokenRef
        {
            public TokenRef(ILoginCredentials cref, List<RefreshToken> tref)
            {
                Credential = cref;
                Tokens = new(tref);
            }
            public ILoginCredentials Credential { get; }
            public List<RefreshToken> Tokens { get; }
        }

        [HttpPost("{id}/login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromRoute] int id, [FromBody] object x)
        {
            ILoginCredentials credential;
            try
            {
                var jsonObject = JsonSerializer.Serialize(x);
                credential = JsonSerializer.Deserialize<LoginModel>(jsonObject);
                if (credential == null)
                    return BadRequest(new BadHttpRequestException("The input type is not the same as expected"));
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex);
            }
            catch(JsonException ex)
            {
                return BadRequest(ex);
            }
            catch(NotSupportedException ex)
            {
                return BadRequest(ex);
            }
            SignedCustomer cus = new(await _customers.Verify(id, credential), id);

            var jwt = _accountService.SignIn(id, credential, GetJWTSettings(cus));
            await _customers.AddToken(id, $"{jwt.RefreshToken}:{jwt.AccessToken}");
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
                await _customers.InvalidateTokens(id);
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

        [HttpGet]
        [Authorize(Policy = Policy3)]
        public async Task<IActionResult> Get()
        {
            var x = await _customers.GetCustomers();
            if (!x.Any())
                return NotFound();
            return Ok(x);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = Policy3)]
        public async Task<IActionResult> Get([FromRoute]int id)
        {
            var x = await _customers.GetCustomer(id);
            if (x != null)
                return Ok(x);
            return NotFound();
        }

        [HttpGet("{id}/accounts")]
        public async Task<IActionResult> GetAccounts([FromRoute] int id)
        {
            var x = await _customers.GetAccounts(id);
            if (x != null)
                return Ok(x);

            return NotFound();
        }

        //[HttpGet("{int:id}/accounts?accountNumber={num}")]
        [HttpGet("{id}/accounts")]
        public async Task<IActionResult> GetAccount([FromRoute] int id, [FromQuery][StringLength(10, ErrorMessage = "Invalid number")] string num)
        {
            var x = await _customers.GetAccount(id, num);
            if (x != null)
                return Ok(x);

            return NotFound();
        }

        //[HttpGet("{int:id}/accounts?accountNumber={num}/cards")]
        [HttpGet("{id}/accounts/cards")]
        public async Task<IActionResult> GetCards([FromRoute] int id, [FromQuery] string num)
        {
            var x = await _customers.GetCards(id, num);
            if (x != null)
                return Ok(x);

            return NotFound();
        }

        //[HttpGet("{int:id}/accounts?accountNumber={num}/cards")]
        [HttpGet("{id}/accounts/cards")]
        public async Task<IActionResult> GetCard([FromRoute] int id, [FromQuery] string num, [FromBody] LoginObject login)
        {
            var credential = new LoginCredentialsBuilder()
                .SetPersonalOnlineKey(login.Key).SetUsername(login.Name).SetPassword(login.Value).Build();
            var x = await _customers.GetCard(id, num, credential);
            if (x != null)
                return Ok(x);

            return NotFound();
        }

        [HttpGet("{id}/maidenname")]
        public async Task<IActionResult> GetMaidenName([FromRoute] int id)
        {
            try
            {
                var val = await _customers.GetMaidenName(id);
                if (val == default) return StatusCode(502, "Security exception. Try again later!");
                return Ok(val);
            }
            catch (ApplicationNullReferenceException ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Authorize(Policy = Policy3)]
        public async Task<IActionResult> Post()
        {
            var form = HttpContext.Request.Form;
            var formFile = form.Files;

            var customerBuilder = new CustomerBuilder();

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
            customerBuilder.SetPerson(person);

            bDate = GetDate(form[EntryYear].ToString().Trim(), form[EntryMonth].ToString().Trim(), form[EntryDay].ToString().Trim());
            customerBuilder.SetEntryDate(bDate);
            customerBuilder.SetBVN(form[BVN].ToString().Trim());
            int id = 0;

            try
            {
                var x = await _customers.AddCustomer(customerBuilder.Build());
                if (x.Successful)
                {
                    UpdateCustomerLookup((uint)x.Id, $"name={person.Entity.Name.Name},dbid={x.Id}");
                    id = x.Id;
                }
                else return Conflict();
            }
            finally
            {
                form = null;
            }
            return CreatedAtAction(nameof(Get), new {id, controller = "customers" }, customerBuilder.Build());
        }

        [HttpPost("quicksignup")]
        [AllowAnonymous]
        public async Task<IActionResult> QuickSignUp([FromBody]QuickSignUpCustomer model)
        {
            if (model.Phone == null && model.Email == null)
                return BadRequest("Phone and email cannot be mutually skipped");
            var name = new FullNameBuilder()
                        .SetName(new NameBuilder().SetName(model.FirstName).Build())
                        .SetSurname(new NameBuilder().SetName(model.Surname).Build())
                        .Build();
            IContact contact = null;
            var cb = new ContactBuilder();
            if (model.Email != null)
                cb.AddEmail(model.Email);
            if (model.Phone != null)
                cb.AddMobile(model.Phone);
            contact = cb.Build();

            var tag = Guid.NewGuid();
            _GenerateAccountNumber(tag);
            var limited = new AccountStatusInfo(AccountStatus.LIMITED, "Registration not completed");

            var customer = new CustomerBuilder()
                                .SetEntryDate(DateTime.UtcNow)
                                .SetPerson(
                                    new PersonBuilder()
                                        .SetUniqueTag(tag)
                                        .SetFullName(name)
                                        .SetEntity(
                                            new EntityBuilder()
                                                .SetName(new NameBuilder()
                                                    .SetName($"{model.FirstName} {model.Surname}")
                                                    .Build())
                                                .SetContact(contact)
                                                .Build())
                                        .SetSex(model.IsMale)
                                        .Build())
                                .AddAccount(
                                    new AccountBuilder()
                                    .SetNumber(_unallocated)
                                    .SetType(model.IsSavingsAccount ? Models.AccountType.Savings : Models.AccountType.Current)
                                    .AddStatus(limited)
                                    .SetEntryDate(DateTime.UtcNow)
                                    .SetCurrency(Models.Currencies.TryParse(566))
                                    .Build()).Build();

            var res = await _customers.AddCustomer(customer);

            if(res.Successful)
                return CreatedAtAction(nameof(Get), new { res, controller = "customers" }, customer);

            return StatusCode(422, model);
        }

        [HttpPost("{id}/accounts")]
        public async Task<IActionResult> PostAccount([FromRoute]int id)
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
            try
            {
                _GenerateAccountNumber((await _customers.GetCustomer(id)).Person.UniqueTag);
            }
            catch (Exception)
            {
                return NotFound();
            }

            var accountBuilder = new AccountBuilder();
            accountBuilder.SetNumber(_unallocated);
            var n = form[AccountType].ToString().Trim();
            accountBuilder.SetType(Enum.Parse<Models.AccountType>(n));
            n = form[Currency].ToString().Trim();
            accountBuilder.SetCurrency(Models.Currencies.TryParse(Int32.Parse(n)));

            var list = Utilities.GetMultipleValues(form[PersonalMobile_], form);
            accountBuilder.SetSMSAlertList(list);
            list = Utilities.GetMultipleValues(form[PersonalEmail_], form);
            accountBuilder.SetEmailAlertList(list);

            
            //Guarantor
            var guarantorBuilder = new PersonBuilder();
            var contactBuilder = new ContactBuilder();
            var addressBuilder = new AddressBuilder();
            var nationBuilder = new NationalityBuilder();
            var entityBuilder = new EntityBuilder();
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

            //This for the guarantor's IPerson property "Entity"
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
            var fnb = new FullNameBuilder();
            n = form[GuarantorFirstName].ToString().Trim();
            fnb.SetName(new NameBuilder().SetName(n).Build());
            n = form[GuarantorSurname].ToString().Trim();
            fnb.SetSurname(new NameBuilder().SetName(n).Build());
            n = form[GuarantorTitle].ToString().Trim();
            fnb.SetTitle(n);
            n = form[GuarantorNickname].ToString().Trim();
            fnb.SetNickname(new NameBuilder().SetName(n).Build());
            var na = form[GuarantorOtherNames].ToString().Split(",",
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
            accountBuilder.SetGuarantor(guarantorBuilder.Build());

            var result = await _customers.AddAccount(id, accountBuilder.Build());

            if (result)
            {
                return CreatedAtAction(nameof(GetAccount), new { controller = "customers" }, accountBuilder.Build());
            }

            return Conflict();
        }

        [HttpPut("{id}/accounts/{num}/cards")]
        [Authorize(Policy = Policy3)]
        public async Task<IActionResult> PostCard([FromRoute] int id, [FromRoute] string num)
        {
            var form = HttpContext.Request.Form;
            IAccount account = null;
            try
            {
                account = (await _customers.GetAccount(id, num));
                if (account == null) return BadRequest();
            }
            catch (NullReferenceException)
            {
                return BadRequest();
            }
            var accountBuilder = new AccountBuilder().ReBuild(account);
            var cardBuilder = new CardBuilder();
            var loginBuilder = new LoginCredentialsBuilder();
            var brandBuilder = new EntityBuilder();
            var countryBuilder = new NationalityBuilder();
            var contactBuilder = new ContactBuilder();
            var addressBuilder = new AddressBuilder();

            //login
            loginBuilder.SetPersonalOnlineKey(Guid.NewGuid().ToString());
            //loginBuilder.SetPassword(form[CardPassword]);
            loginBuilder.SetUsername(_GenerateCardSignature(Int32.Parse(num)).ToString());
            cardBuilder.SetSecret(loginBuilder.Build());//Note Password may not be saved here

            //Brand
            brandBuilder.SetName(new NameBuilder().SetName(form[CardIssuerName].ToString().Trim()).Build());
            //Brand contact
            contactBuilder.AddEmail(form[CardIssuerEmail].ToString().Trim());
            contactBuilder.AddMobiles(Utilities.GetMultipleValues(form[CardIssuerMobile_].ToString().Trim(), form));
            contactBuilder.AddWebsite(form[CardIssuerWebsite].ToString().Trim());
            //Brand Address
            addressBuilder.SetPMB(form[CardIssuerPMB].ToString().Trim());
            addressBuilder.SetZipCode(form[CardIssuerZIP].ToString().Trim());
            addressBuilder.SetStreetAddress(form[CardIssuerStreetAddress].ToString().Trim());
            //Brand Country of residence
            countryBuilder.SetCountryName(new NameBuilder().SetName(form[CardIssuerCountryOfResidence].ToString().Trim()).Build());
            countryBuilder.SetState(new NameBuilder().SetName(form[CardIssuerStateOfResidence].ToString().Trim()).Build());
            countryBuilder.SetLGA(form[CardIssuerLocalGovernmentOfResidence].ToString().Trim());
            countryBuilder.SetLanguage(form[CardIssuerLanguage].ToString().Trim());
            countryBuilder.SetCityTown(new NameBuilder().SetName(form[CardIssuerCityTown].ToString().Trim()).Build());
            addressBuilder.SetCountryOfResidence(countryBuilder.Build());
            contactBuilder.SetAddress(addressBuilder.Build());
            brandBuilder.SetContact(contactBuilder.Build());

            cardBuilder.SetBrand(brandBuilder.Build());

            countryBuilder.Clear();
            //Country of use
            countryBuilder.SetCountryName(new NameBuilder().SetName(form[CardCountryOfUse].ToString().Trim()).Build());
            countryBuilder.SetState(new NameBuilder().SetName(form[CardStateOfUse].ToString().Trim()).Build());
            countryBuilder.SetLGA(form[CardLocalGovernmentOfUse].ToString().Trim());
            countryBuilder.SetLanguage(form[CardLanguageOfUse].ToString().Trim());
            countryBuilder.SetCityTown(new NameBuilder().SetName(form[CardCityTownOfUse].ToString().Trim()).Build());
            cardBuilder.SetCountryOfUse(countryBuilder.Build());

            cardBuilder.SetCurrency(Models.Currencies.TryParse(Int32.Parse(form[CardCurrency].ToString().Trim())));
            var isParsed = Int32.TryParse(form[CardType].ToString().Trim(), out int res);
            if (!isParsed)
                return BadRequest();
            cardBuilder.SetType((Models.CardType)res);
            var isd = DateTime.Now;
            cardBuilder.SetIssuedDate(isd);
            cardBuilder.SetExpiry(isd.AddYears(Int32.Parse(form[CardLifeSpan].ToString().Trim())));
            cardBuilder.SetIssuedCost(Decimal.Parse(form[CardIssuedCost].ToString().Trim()));
            cardBuilder.SetMonthlyRate(Decimal.Parse(form[CardMonthlyRate].ToString().Trim()));
            cardBuilder.SetWithdrawalLimit(Decimal.Parse(form[CardWithDrawalLimit].ToString().Trim()));

            accountBuilder.AddCard(cardBuilder.Build());

            var result = await _customers.AddCard(id, accountBuilder.Build().Number, cardBuilder.Build());

            if (result)
            {
                await _customers.ValidateAndChangeCardPassword(id, num,
                    loginBuilder.Build().PersonalOnlineKey, null, loginBuilder.SetPassword(form[CardPassword]).Build());
                return CreatedAtAction(nameof(GetAccount), new { id, controller = "customers" }, cardBuilder.Build());
            }

            return Conflict();
        }

        /*
                    [HttpPut("{id}/accounts/{num}/cards/{cardId}")]
                    public async Task<IActionResult> UpdateCard([FromRoute] int id, [FromRoute] string num, [FromRoute] string cardId)
                    {
                        var form = HttpContext.Request.Form;
                        IAccount account = null;
                        ICard card = null;
                        try
                        {
                            account = await _customers.GetAccount(id, num);
                            card = await _customers.GetCard(id, num, new LoginCredentialsBuilder().SetPersonalOnlineKey(cardId).Build());
                            //card = account.Cards.AsQueryable().Where(x => x.Secret.PersonalOnlineKey.CompareTo(cardId) == 0).FirstOrDefault();
                            if (card == null || account == null) return BadRequest();
                        }
                        catch (NullReferenceException)
                        {
                            return BadRequest();
                        }
                        var cardBuilder = new CardBuilder().ReBuild(card);
                        var loginBuilder = new LoginCredentialsBuilder();
                        var brandBuilder = new EntityBuilder();
                        var countryBuilder = new NationalityBuilder();
                        var contactBuilder = new ContactBuilder();
                        var addressBuilder = new AddressBuilder();

                        //login
                        loginBuilder.SetPersonalOnlineKey(Guid.NewGuid().ToString());
                        loginBuilder.SetPassword(form[CardPassword]);
                        loginBuilder.SetUsername(_GenerateCardSignature(Int32.Parse(num)).ToString());
                        cardBuilder.SetSecret(loginBuilder.Build());//Note Password may not be saved here

                        //Brand
                        brandBuilder.SetName(new NameBuilder().SetName(form[CardIssuerName].ToString().Trim()).Build());
                        //Brand contact
                        contactBuilder.AddEmail(form[CardIssuerEmail].ToString().Trim());
                        contactBuilder.AddMobiles(Utilities.GetMultipleValues(form[CardIssuerMobile_].ToString().Trim(), form));
                        contactBuilder.AddWebsite(form[CardIssuerWebsite].ToString().Trim());
                        //Brand Address
                        addressBuilder.SetPMB(form[CardIssuerPMB].ToString().Trim());
                        addressBuilder.SetZipCode(form[CardIssuerZIP].ToString().Trim());
                        addressBuilder.SetStreetAddress(form[CardIssuerStreetAddress].ToString().Trim());
                        //Brand Country of residence
                        countryBuilder.SetCountryName(new NameBuilder().SetName(form[CardIssuerCountryOfResidence].ToString().Trim()).Build());
                        countryBuilder.SetState(new NameBuilder().SetName(form[CardIssuerStateOfResidence].ToString().Trim()).Build());
                        countryBuilder.SetLGA(form[CardIssuerLocalGovernmentOfResidence].ToString().Trim());
                        countryBuilder.SetLanguage(form[CardIssuerLanguage].ToString().Trim());
                        countryBuilder.SetCityTown(new NameBuilder().SetName(form[CardIssuerCityTown].ToString().Trim()).Build());
                        addressBuilder.SetCountryOfResidence(countryBuilder.Build());
                        contactBuilder.SetAddress(addressBuilder.Build());
                        brandBuilder.SetContact(contactBuilder.Build());

                        cardBuilder.SetBrand(brandBuilder.Build());

                        countryBuilder.Clear();
                        //Country of use
                        countryBuilder.SetCountryName(new NameBuilder().SetName(form[CardCountryOfUse].ToString().Trim()).Build());
                        countryBuilder.SetState(new NameBuilder().SetName(form[CardStateOfUse].ToString().Trim()).Build());
                        countryBuilder.SetLGA(form[CardLocalGovernmentOfUse].ToString().Trim());
                        countryBuilder.SetLanguage(form[CardLanguageOfUse].ToString().Trim());
                        countryBuilder.SetCityTown(new NameBuilder().SetName(form[CardCityTownOfUse].ToString().Trim()).Build());
                        cardBuilder.SetCountryOfUse(countryBuilder.Build());

                        cardBuilder.SetCurrency(Models.Currencies.TryParse(Int32.Parse(form[CardCurrency].ToString().Trim())));
                        var isParsed = Int32.TryParse(form[CardType].ToString().Trim(), out int res);
                        if (!isParsed)
                            return BadRequest();
                        cardBuilder.SetType((Models.CardType)res);
                        var isd = DateTime.Now;
                        cardBuilder.SetIssuedDate(isd);
                        cardBuilder.SetExpiry(isd.AddYears(Int32.Parse(form[CardLifeSpan].ToString().Trim())));
                        cardBuilder.SetIssuedCost(Decimal.Parse(form[CardIssuedCost].ToString().Trim()));
                        cardBuilder.SetMonthlyRate(Decimal.Parse(form[CardMonthlyRate].ToString().Trim()));
                        cardBuilder.SetWithdrawalLimit(Decimal.Parse(form[CardWithDrawalLimit].ToString().Trim()));

                        var result = await _customers.UpdateCard(id, account, cardBuilder.Build());

                        if (result)
                            return CreatedAtAction(nameof(GetAccount), new { id, controller = "customers" }, cardBuilder.Build());

                        return Conflict();

                    }*/

        [HttpPost("{id}/maidenname")]
        public async Task<IActionResult> PostMaidenName([FromRoute]int id, [FromBody] string x)
        {
            try
            {
                var val = await _customers.SetMaidenName(id, new NameBuilder().SetName(x).Build());
                if (!val) return StatusCode(502, "Security exception. Try again later!");
                return CreatedAtAction(nameof(GetMaidenName), new { id, controller = "customers" }, val);
            }
            catch (ApplicationStateException ex)
            {
                return BadRequest(ex);
            }

        }

        [HttpPost("{id}/balance")]
        public async Task<IActionResult> PostBalance([FromRoute] int id, [FromQuery]string num, [FromBody] decimal bal)
        {
            var val = await _customers.SetBalance(id, num, bal);
            if (val != default) return Ok(val);
            return BadRequest();
        }

        public class TransactionObject
        {
            public string Amount { get; set; }
            public string IsIncoming { get; set; }

            public string CreditorName { get; set; }
            public string CreditorPhone { get; set; }
            public string CreditorEmail { get; set; }
            public string CreditorWebsite { get; set; }
            public string CreditorAddress { get; set; }
            public string CreditorZIP { get; set; }
            public string CreditorCountry { get; set; }
            public string CreditorState { get; set; }
            public string CreditorLGA { get; set; }
            public string CreditorCity { get; set; }
            public string CreditorLanguage { get; set; }
            public string DebitorName { get; set; }
            public string DebitorPhone { get; set; }
            public string DebitorEmail { get; set; }
            public string DebitorWebsite { get; set; }
            public string DebitorAddress { get; set; }
            public string DebitorZIP { get; set; }
            public string DebitorCountry { get; set; }
            public string DebitorState { get; set; }
            public string DebitorLGA { get; set; }
            public string DebitorCity { get; set; }
            public string DebitorLanguage { get; set; }

            //0 - 10
            public string TType{ get; set; }
            public int employeeId{ get; set; }
            public string AccountNumber{ get; set; }
            public string AccountName{ get; set; }
            public string CurrencyCode{ get; set; }
            public string Id { get; set; } = Guid.NewGuid().ToString();
        }

        [HttpPost("{id}/{num}/transfers")]
        public async Task<IActionResult> PostTransaction([FromRoute]int id, [FromRoute]string num, [FromBody]TransactionObject transaction)
        { 
            ITransaction tra;
            try
            {
                tra = new TransactionBuilder()
                        .SetAmount(Decimal.Parse(transaction.Amount))
                        .SetCreditor(new EntityBuilder()
                            .SetName(new NameBuilder().SetName(transaction.CreditorName).Build())
                            .SetContact(new ContactBuilder()
                                .AddEmail(transaction.CreditorEmail)
                                .AddMobile(transaction.CreditorPhone)
                                .AddWebsite(transaction.CreditorWebsite)
                                .SetAddress(new AddressBuilder()
                                    .SetZipCode(transaction.CreditorZIP)
                                    .SetStreetAddress(transaction.CreditorAddress)
                                    .SetCountryOfResidence(new NationalityBuilder()
                                        .SetLanguage(transaction.CreditorLanguage)
                                        .SetLGA(transaction.CreditorLGA)
                                        .SetCityTown(new NameBuilder().SetName(transaction.CreditorCity).Build())
                                        .SetState(new NameBuilder().SetName(transaction.CreditorState).Build())
                                        .SetCountryName(new NameBuilder().SetName(transaction.CreditorCountry).Build())
                                        .Build())
                                    .Build())
                                .Build())
                            .Build())
                        .SetDebitor(new EntityBuilder()
                            .SetName(new NameBuilder().SetName(transaction.DebitorName).Build())
                            .SetContact(new ContactBuilder()
                                .AddEmail(transaction.DebitorEmail)
                                .AddMobile(transaction.DebitorPhone)
                                .AddWebsite(transaction.DebitorWebsite)
                                .SetAddress(new AddressBuilder()
                                    .SetZipCode(transaction.DebitorZIP)
                                    .SetStreetAddress(transaction.DebitorAddress)
                                    .SetCountryOfResidence(new NationalityBuilder()
                                        .SetLanguage(transaction.DebitorLanguage)
                                        .SetLGA(transaction.DebitorLGA)
                                        .SetCityTown(new NameBuilder().SetName(transaction.DebitorCity).Build())
                                        .SetState(new NameBuilder().SetName(transaction.DebitorState).Build())
                                        .SetCountryName(new NameBuilder().SetName(transaction.DebitorCountry).Build())
                                        .Build())
                                    .Build())
                                .Build())
                            .Build())
                        .SetDate(DateTime.UtcNow)
                        .SetIsIncoming(Boolean.Parse(transaction.IsIncoming))
                        .SetEmployeeId(transaction.employeeId)//Will be set in a separate method inside the EmployeesController class
                        .SetTransactionGuid(Guid.Parse(transaction.Id))
                        .SetTransactionType(Enum.Parse<TransactionType>(transaction.TType, true))
                        .SetDescription(GetDescription(transaction))
                        .SetCurrency(Models.Currencies.TryParse(Int32.Parse(transaction.CurrencyCode)))
                        .Build();

                if(!await _customers.AddTransaction(id, num, tra))
                    return Forbid();
            }
            catch (ArgumentException error)
            {
                return BadRequest(error);
            }
            catch(ApplicationStateException ex)
            {
                return BadRequest(ex);
            }

            return Ok(tra.Description);
        }

        private static uint _GenerateCardSignature(int seed)
        {
            Random r = new(seed);
            uint val = 0U;
            do
            {
                val = (uint)r.Next(9_999, 9_999_999);
            } while (signatures.Contains(val));

            signatures.Add(val);
            if (signatures.Count >= 10_000)
                signatures.RemoveRange(4_999, 4_999);

            return val;
        }

        private static void _GenerateAccountNumber(Guid uniqueTag = default)
        {
            if (uniqueTag != default) numGen = Numbers((uint)uniqueTag.GetHashCode());
            numGen.MoveNext();
            string s = numGen.Current.ToString();
            //var s = new Random(uniqueTag.GetHashCode()).Next(1_000_000, Int32.MaxValue).ToString();
            try
            {
                s = Substring(s, 0, 10);
            }
            catch (ArgumentOutOfRangeException)
            {
                s = s.PadLeft(10, '0');
            }
            _unallocated = s;
            numGen.MoveNext();
        }

        private static IEnumerator<uint> Numbers(uint seed)
        {
            //Console.WriteLine("seed: " + seed);
            uint t = seed += 0x6D2B79F5;
            //Console.WriteLine("t 0 line: " + t);
            while (true)
            {
                t = (uint)Math.BigMul((int)(t ^ t >> 15), (int)(t | 1));
                //Console.WriteLine("t 1st line: " + t);
                t ^= t + (uint)Math.BigMul((int)(t ^ t >> 7), (int)(t | 61));
                //Console.WriteLine("t 2nd line: " + t);
                yield return t;
                //yield return (uint)(((t ^ t >> 14) >> 0) / 4294967296);
            }
        }

        /*[HttpPut("{id}/accounts/{num}/update")]
        public async Task<IActionResult> UpdateAccount([FromRoute] int id, [FromRoute] string num)
        {
            var form = HttpContext.Request.Form;
            IAccount account = null;
            try
            {
                account = (await _customers.GetAccount(id, num));
                if (account == null) return BadRequest();
            }
            catch (NullReferenceException)
            {
                return BadRequest();
            }
            var accountBuilder = new AccountBuilder().ReBuild(account);
            var n = form[AccountType].ToString().Trim();
            accountBuilder.SetType(Enum.Parse<Models.AccountType>(n));
            n = form[Currency].ToString().Trim();
            accountBuilder.SetCurrency(Models.Currencies.TryParse(Int32.Parse(n)));

            var list = Utilities.GetMultipleValues(form[PersonalMobile_], form);
            accountBuilder.SetSMSAlertList(list);
            list = Utilities.GetMultipleValues(form[PersonalEmail_], form);
            accountBuilder.SetEmailAlertList(list);


            //Guarantor
            var guarantorBuilder = new PersonBuilder();
            var contactBuilder = new ContactBuilder();
            var addressBuilder = new AddressBuilder();
            var nationBuilder = new NationalityBuilder();
            var entityBuilder = new EntityBuilder();
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

            //This for the guarantor's IPerson property "Entity"
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
            var fnb = new FullNameBuilder();
            n = form[GuarantorFirstName].ToString().Trim();
            fnb.SetName(new NameBuilder().SetName(n).Build());
            n = form[GuarantorSurname].ToString().Trim();
            fnb.SetSurname(new NameBuilder().SetName(n).Build());
            n = form[GuarantorTitle].ToString().Trim();
            fnb.SetTitle(n);
            n = form[GuarantorNickname].ToString().Trim();
            fnb.SetNickname(new NameBuilder().SetName(n).Build());
            var na = form[GuarantorOtherNames].ToString().Split(",",
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
            accountBuilder.SetGuarantor(guarantorBuilder.Build());

            var result = await _customers.UpdateAccount(id, accountBuilder.Build().Number);

            if (result)
            {
                return CreatedAtAction(nameof(GetAccount), new { id, controller = "customers" }, accountBuilder.Build());
            }

            return Conflict();
        }*/

        [HttpPut("{id}/number")]
        public async Task<IActionResult> PutNumber([FromRoute] int id, [FromQuery] string num)
        {
            _GenerateAccountNumber();
            var val = await _customers.UpdateNumber(id, num, _unallocated);
            if(val != default) return Ok();
            return BadRequest();
        }

        [HttpPut("{id}/maidenname")]
        public async Task<IActionResult> PutMaidenName([FromRoute] int id,[FromQuery] string old, [FromBody] string x)
        {
            try
            {
                var val = await _customers.UpdateMaidenName(id, new NameBuilder().SetName(old).Build(), new NameBuilder().SetName(x).Build());
                if (!val) return StatusCode(502, "Security exception. Try again later!");
                return NoContent();
            }
            catch (ApplicationStateException ex)
            {
                return BadRequest(ex);
            }
            catch(ApplicationArgumentException ex)
            {
                return BadRequest(ex);
            }
            catch(ApplicationNullReferenceException ex)
            {
                return BadRequest(ex);
            }

        }
        
        IJWTSettings GetJWTSettings(ICustomer cus)
        {
            IConfiguration config = new ConfigurationBuilder()
                                        .SetBasePath(Directory.GetCurrentDirectory())
                                        .AddJsonFile("appsettings.json")
                                        .Build();

            Models.AccountType t = default;
            foreach (var item in cus.Accounts)
            {
                t = (int)item.Type > (int)t ? item.Type : t;
            }
            var val = (int)t;
            switch (val)
            {
                case 1:
                    return config.GetSection(nameof(CustomerJWTSettings)).Get<CustomerJWTSettings>();
                default:
                    return config.GetSection(nameof(CustomerJWTSettings)).Get<CustomerJWTSettings>();
            }
        }

        static string GetDescription(TransactionObject t)
        {
            var c = Models.Currencies.TryParse(Int32.Parse(t.CurrencyCode));
            string message = Boolean.Parse(t.IsIncoming)
                ? $"Your account {t.AccountNumber} has been credited with {c.Symbol} {t.Amount}. Reference code is {t.Id}"
                : $"Your transaction to {t.AccountName} : {t.AccountNumber}, with id: {t.Id}, was successful. Your account has been debited of {c.Symbol} {t.Amount}";

            return message;
        }

        private class Id
        {
            [Required]
            [MinLength(4)]
            internal string Type { get; set; }
            [Required]
            [MinLength(10)]
            internal string Value { get; set; }
        }

        public class QuickSignUpCustomer
        {
            [Required]
            [MinLength(3)]
            public string FirstName { get; set; }

            [Required]
            [MinLength(3)]
            public string Surname { get; set; }

            [Phone]
            [StringLength(maximumLength: 11, MinimumLength = 11)]
            public string Phone { get; set; }

            [EmailAddress]
            [MinLength(9)]
            public string Email { get; set; }

            [Required]
            public bool IsMale { get; set; }

            [Required]
            public bool IsSavingsAccount { get; set; }
        }

    }
}
