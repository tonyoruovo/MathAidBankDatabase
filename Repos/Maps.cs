using DataBaseTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static DataBaseTest.Repos.Builders;

namespace DataBaseTest.Repos
{
    public static class Maps
    {
        public static IName Map(NameModel m)
            => new NameMap().Map(m);
        public static NameModel ReverseMap(IName i)
            => new NameMap().ReverseMap(i);
        private class NameMap : IMapper<NameModel, IName>
        {
            public IName Map(NameModel from)
            {
                if (from == null)
                    return null;
                return new NameBuilder()
                    .SetName(from.Name)
                    .Build();
            }

            public NameModel ReverseMap(IName to)
            {
                if (to == null)
                    return null;
                return new()
                {
                    Name = to.Name
                };
            }
        }

        public static IFullName Map(FullNameModel m)
            => new FullNameMap().Map(m);
        public static FullNameModel ReverseMap(IFullName i)
            => new FullNameMap().ReverseMap(i);
        private class FullNameMap : IMapper<FullNameModel, IFullName>
        {
            public IFullName Map(FullNameModel from)
            {
                if (from == null)
                    return null;
                var f = new FullNameBuilder()
                    .SetName(new NameMap().Map(from.Name))
                    .SetTitle(from.Title)
                    .SetMaidenName(new NameMap().Map(from.MaidenName))
                    .SetSurname(new NameMap().Map(from.Surname))
                    .SetNickname(new NameMap().Map(from.Nickname));

                f.AddOthers(from.OtherNames.Select(n => new NameMap().Map(n)));

                return f.Build();
            }

            public FullNameModel ReverseMap(IFullName to)
            {
                if (to == null)
                    return null;
                return new()
                {
                    Name = new NameMap().ReverseMap(to.Name),
                    Surname = new NameMap().ReverseMap(to.Surname),
                    Title = to.Title,
                    Nickname = new NameMap().ReverseMap(to.Nickname),
                    MaidenName = new NameMap().ReverseMap(to.MaidenName),
                    OtherNames = TryForNull(to.OtherNames.Select(n => new NameMap().ReverseMap(n)).ToList)
                };
            }
        }

        public static INationality Map(NationalityModel m)
            => new NationalityMap().Map(m);
        public static NationalityModel ReverseMap(INationality i)
            => new NationalityMap().ReverseMap(i);
        private class NationalityMap : IMapper<NationalityModel, INationality>
        {
            public INationality Map(NationalityModel from)
            {
                if (from == null)
                    return null;
                return new NationalityBuilder()
                    .SetCityTown(new NameMap().Map(from.CityTown))
                    .SetCountryName(new NameMap().Map(from.CountryName))
                    .SetLanguage(from.Language)
                    .SetLGA(from.LGA)
                    .SetState(new NameMap().Map(from.State))
                    .Build();
            }

            public NationalityModel ReverseMap(INationality to)
            {
                if (to == null)
                    return null;
                return new()
                {
                    CityTown = new NameMap().ReverseMap(to.CityTown),
                    CountryName = new NameMap().ReverseMap(to.CountryName),
                    Language = to.Language,
                    LGA = to.LGA,
                    State = new NameMap().ReverseMap(to.State)
                };
            }
        }

        public static IAddress Map(AddressModel m)
            => new AddressMap().Map(m);
        public static AddressModel ReverseMap(IAddress i)
            => new AddressMap().ReverseMap(i);
        private class AddressMap : IMapper<AddressModel, IAddress>
        {
            public IAddress Map(AddressModel from)
            {
                if (from == null)
                    return null;
                return new AddressBuilder()
                    .SetCountryOfResidence(new NationalityMap().Map(from.CountryOfResidence))
                    .SetPMB(from.PMB)
                    .SetStreetAddress(from.StreetAddress)
                    .SetZipCode(from.ZIPCode)
                    .Build();
            }

            public AddressModel ReverseMap(IAddress to)
            {
                if (to == null)
                    return null;
                return new()
                {
                    CountryOfResidence = new NationalityMap().ReverseMap(to.CountryOfResidence),
                    PMB = to.PMB,
                    StreetAddress = to.StreetAddress,
                    ZIPCode = to.ZIPCode
                };
            }
        }

        public static IContact Map(ContactModel m)
            => new ContactMap().Map(m);
        public static ContactModel ReverseMap(IContact i)
            => new ContactMap().ReverseMap(i);
        private class ContactMap : IMapper<ContactModel, IContact>
        {
            public IContact Map(ContactModel from)
            {
                if (from == null)
                    return null;
                var c = new ContactBuilder()
                    .SetAddress(new AddressMap().Map(from.Address))
                    .AddMobiles(from.Mobiles)
                    .AddEmails(from.Emails)
                    .AddSocialMedia(from.SocialMedia)
                    .AddWebsite(from.Websites);

                return c.Build();
            }

            public ContactModel ReverseMap(IContact to)
            {
                if (to == null)
                    return null;
                var c = new ContactModel()
                {
                    Address = new AddressMap().ReverseMap(to.Address)
                };
                c.Mobiles = TryForNull(to.Mobiles.ToList);
                c.Emails = TryForNull(to.Emails.ToList);
                c.Websites = TryForNull(to.Websites.ToList);
                c.SocialMedia = TryForNull(to.SocialMedia.ToList);

                return c;
            }
        }

        public static IEntity Map(EntityModel m)
            => new EntityMap().Map(m);
        public static EntityModel ReverseMap(IEntity i)
            => new EntityMap().ReverseMap(i);
        private class EntityMap : IMapper<EntityModel, IEntity>
        {
            public IEntity Map(EntityModel from)
            {
                if (from == null)
                    return null;
                return new EntityBuilder().SetName(new NameMap().Map(from.Name)).SetContact(new ContactMap().Map(from.Contact)).Build();
            }

            public EntityModel ReverseMap(IEntity to)
            {
                if (to == null)
                    return null;
                return new()
                {
                    Contact = new ContactMap().ReverseMap(to.Contact),
                    Name = new NameMap().ReverseMap(to.Name)
                };
            }
        }

        public static IPerson Map(PersonModel m)
            => new PersonMap().Map(m);
        public static PersonModel ReverseMap(IPerson i)
            => new PersonMap().ReverseMap(i);
        private class PersonMap : IMapper<PersonModel, IPerson>
        {
            public IPerson Map(PersonModel from)
            {
                if (from == null)
                    return null;
                var p = new PersonBuilder();
                p.SetSex(from.IsMale)
                .SetFullName(new FullNameMap().Map(from.FullName))
                    .SetBirthDate(from.BirthDate)
                    .SetMaritalStatus(from.MaritalStatus)
                    .SetPassport(from.Passport)
                    .SetFingerPrint(from.FingerPrint)
                    .SetSignature(from.Signature)
                    .SetUniqueTag(from.UniqueTag)
                    .SetCountryOfOrigin(new NationalityMap().Map(from.CountryOfOrigin))
                    .SetNextOfKin(new PersonMap().Map(from.NextOfKin))
                    .AddIdentifications(from.Identification)
                .SetEntity(new EntityMap().Map(from.Entity))
                .SetOfficialEntity(new EntityMap().Map(from.OfficialEntity))
                .SetJobType(from.JobType);
                return p.Build();
            }

            public PersonModel ReverseMap(IPerson to)
            {
                if (to == null)
                    return null;

                var p = new PersonModel();
                p.IsMale = to.IsMale;
                p.FullName = new FullNameMap().ReverseMap(to.FullName);
                p.Entity = new EntityMap().ReverseMap(to.Entity);
                p.OfficialEntity = new EntityMap().ReverseMap(to.OfficialEntity);
                p.Signature = to.Signature;
                p.Passport = to.Passport;
                p.FingerPrint = to.FingerPrint;
                try
                {
                    p.BirthDate = to.BirthDate;
                    p.MaritalStatus = to.MaritalStatus;
                    p.UniqueTag = to.UniqueTag;
                    p.CountryOfOrigin = new NationalityMap().ReverseMap(to.CountryOfOrigin);
                    p.Identification = LookUp(to.Identification);
                    p.NextOfKin = new PersonMap().ReverseMap(to.NextOfKin);
                    p.JobType = to.JobType;
                }
                catch (NullReferenceException)
                { }
                return p;
            }
        }

        public static ILoginCredentials Map(LoginCredentialModel m)
            => new LoginCredentialsMap().Map(m);
        public static LoginCredentialModel ReverseMap(ILoginCredentials i)
            => new LoginCredentialsMap().ReverseMap(i);
        private class LoginCredentialsMap : IMapper<LoginCredentialModel, ILoginCredentials>
        {
            public ILoginCredentials Map(LoginCredentialModel from)
            {
                if (from == null)
                    return null;
                return new LoginCredentialsBuilder().SetPersonalOnlineKey(from.PersonalOnlineKey).SetUsername(from.Username).SetPassword(from.Password).Build();
            }

            public LoginCredentialModel ReverseMap(ILoginCredentials to)
            {
                if (to == null)
                    return null;
                return new()
                {
                    PersonalOnlineKey = to.PersonalOnlineKey,
                    Username = to.Username,
                    Password = to.Password
                };
            }
        }

        public static IQualification Map(QualificationModel m)
            => new QualificationMap().Map(m);
        public static QualificationModel ReverseMap(IQualification i)
            => new QualificationMap().ReverseMap(i);
        private class QualificationMap : IMapper<QualificationModel, IQualification>
        {
            public IQualification Map(QualificationModel from)
            {
                if (from == null)
                    return null;
                return new QualificationBuilder().SetAcademy(new AddressMap().Map(from.Academy)).SetCertification(from.Certification).AddGrades(from.Grades).Build();
            }

            public QualificationModel ReverseMap(IQualification to)
            {
                if (to == null)
                    return null;
                return new()
                {
                    Academy = new AddressMap().ReverseMap(to.Academy),
                    Certification = to.Certification,
                    Grades = LookUp(to.Grades)
                };
            }
        }

        public static IEducation Map(EducationModel m)
            => new EducationMap().Map(m);
        public static EducationModel ReverseMap(IEducation i)
            => new EducationMap().ReverseMap(i);
        private class EducationMap : IMapper<EducationModel, IEducation>
        {
            public IEducation Map(EducationModel from)
            {
                if (from == null)
                    return null;

                var e = new EducationBuilder()
                      .SetPrimary(new QualificationMap().Map(from.Primary))
                      .SetSecondary(new QualificationMap().Map(from.Secondary))
                      .SetPrimaryTertiary(new QualificationMap().Map(from.PrimaryTertiary));
                e.AddQualifications(from.Others.Select(q => new QualificationMap().Map(q)));

                return e.Build();
            }

            public EducationModel ReverseMap(IEducation to)
            {
                if (to == null)
                    return null;
                return new()
                {
                    Others = to.Others.Select(q => new QualificationMap().ReverseMap(q)).ToList(),
                    Primary = new QualificationMap().ReverseMap(to.Primary),
                    PrimaryTertiary = new QualificationMap().ReverseMap(to.PrimaryTertiary),
                    Secondary = new QualificationMap().ReverseMap(to.Secondary)
                };
            }
        }

        public static ITransaction Map(TransactionModel m)
            => new TransactionMap().Map(m);
        public static TransactionModel ReverseMap(ITransaction i)
            => new TransactionMap().ReverseMap(i);
        private class TransactionMap : IMapper<TransactionModel, ITransaction>
        {
            public ITransaction Map(TransactionModel from)
            {
                if (from == null)
                    return null;
                return new TransactionBuilder()
                    .SetAmount(from.Amount)
                    .SetCreditor(new EntityMap().Map(from.Creditor))
                    .SetCurrency(from.Currency)
                    .SetDate(from.Date)
                    .SetDebitor(new EntityMap()
                    .Map(from.Debitor))
                    .SetDescription(from.Description)
                    .SetEmployeeId(from.EmployeeId == null ? 0 : from.EmployeeId.Value)
                    .SetIsIncoming(from.IsIncoming)
                    .SetTransactionGuid(from.TransactionGuid)
                    .SetTransactionType(from.TransactionType)
                    .Build();
            }

            public TransactionModel ReverseMap(ITransaction to)
            {
                if (to == null)
                    return null;
                return new()
                {
                    Amount = to.Amount,
                    Creditor = new EntityMap().ReverseMap(to.Creditor),
                    Currency = to.Currency,
                    Date = to.Date,
                    Debitor = new EntityMap().ReverseMap(to.Debitor),
                    Description = to.Description,
                    EmployeeId = to.EmployeeId,
                    IsIncoming = to.IsIncoming,
                    TransactionGuid = to.TransactionGuid,
                    TransactionType = to.TransactionType
                };
            }
        }

        public static ICard Map(CardModel c)
            => new CardMap().Map(c);
        public static CardModel ReverseMap(ICard i)
            => new CardMap().ReverseMap(i);
        private class CardMap : IMapper<CardModel, ICard>
        {
            public ICard Map(CardModel from)
            {
                if (from == null)
                    return null;
                return new CardBuilder()
                    .SetBrand(new EntityMap().Map(from.Brand))
                    .SetCountryOfUse(new NationalityMap().Map(from.CountryOfUse))
                    .SetCurrency(from.Currency).SetExpiry(from.ExpiryDate)
                    .SetIssuedCost(from.IssuedCost)
                    .SetIssuedDate(from.IssuedDate)
                    .SetMonthlyRate(from.MonthlyRate)
                    .SetSecret(new LoginCredentialsMap().Map(from.Secret))
                    .SetType(from.Type)
                    .SetWithdrawalLimit(from.WithdrawalLimit)
                    .Build();
            }

            public CardModel ReverseMap(ICard to)
            {
                if (to == null)
                    return null;
                return new()
                {
                    Brand = new EntityMap().ReverseMap(to.Brand),
                    CountryOfUse = new NationalityMap().ReverseMap(to.CountryOfUse),
                    Currency = to.Currency,
                    ExpiryDate = to.ExpiryDate,
                    IssuedCost = to.IssuedCost,
                    IssuedDate = to.IssuedDate,
                    MonthlyRate = to.MonthlyRate,
                    Secret = new LoginCredentialsMap().ReverseMap(to.Secret),
                    Type = to.Type,
                    WithdrawalLimit = to.WithdrawalLimit
                };
            }
        }

        public static IAccount Map(AccountModel m)
            => new AccountMap().Map(m);
        public static AccountModel ReverseMap(IAccount i)
            => new AccountMap().ReverseMap(i);
        private class AccountMap : IMapper<AccountModel, IAccount>
        {
            public IAccount Map(AccountModel from)
            {
                if (from == null)
                    return null;
                var a = new AccountBuilder()
                    .SetBalance(from.Balance)
                    .SetCreditIntervalLimit(from.CreditIntervalLimit)
                    .SetCreditLimit(from.CreditLimit)
                    .SetCurrency(from.Currency)
                    .SetDebitIntervalLimit(from.DebitIntervalLimit)
                    .SetDebitLimit(from.DebitLimit)
                    .SetDebt(from.DebitLimit)
                    .SetEntryDate(from.EntryDate)
                    .SetNumber(from.Number)
                    .SetType(from.Type)
                    .SetPercentageDecrease(from.PercentageDecrease)
                    .SetPercentageIncrease(from.PercentageIncrease)
                    .SetStatuses(from.Statuses)
                    .SetEmailAlertList(from.EmailAlertList)
                    .SetGuarantor(new PersonMap().Map(from.Guarantor))
                    .SetSMSAlertList(from.SMSAlertList)
                    .SetSmallestBalance(from.SmallestBalance)
                    .SetSmallestTransferIn(from.SmallestTransferIn)
                    .SetSmallestTransferOut(from.SmallestTransferOut)
                    .SetLargestBalance(from.LargestBalance)
                    .SetLargestTransferIn(from.LargestTransferIn)
                    .SetLargestTransferOut(from.LargestTransferOut);
                a.SetTransactions(from.Transactions.Select(t => new TransactionMap().Map(t)));
                a.SetCards(from.Cards.Select(c => new CardMap().Map(c)));
                return a.Build();
            }

            public AccountModel ReverseMap(IAccount to)
            {
                if (to == null)
                    return null;
                return new()
                {
                    Balance = to.Balance,
                    CreditIntervalLimit = to.CreditIntervalLimit,
                    CreditLimit = to.CreditLimit,
                    Currency = to.Currency,
                    DebitIntervalLimit = to.DebitIntervalLimit,
                    DebitLimit = to.DebitLimit,
                    Debt = to.Debt,
                    EntryDate = to.EntryDate,
                    Number = to.Number,
                    Type = to.Type,
                    PercentageDecrease = to.PercentageDecrease,
                    PercentageIncrease = to.PercentageIncrease,
                    SmallestBalance = to.SmallestBalance,
                    SmallestTransferIn = to.SmallestTransferIn,
                    SmallestTransferOut = to.SmallestTransferOut,
                    LargestBalance = to.LargestBalance,
                    LargestTransferIn = to.LargestTransferIn,
                    LargestTransferOut = to.LargestTransferOut,
                    Statuses = to.Statuses,
                    EmailAlertList = TryForNull(to.EmailAlertList.ToList),
                    SMSAlertList = TryForNull(to.SMSAlertList.ToList),
                    Guarantor = new PersonMap().ReverseMap(to.Guarantor),
                    Transactions = TryForNull(to.Transactions.Select(t => new TransactionMap().ReverseMap(t)).ToList),
                    Cards = TryForNull(to.Cards.Select(c => new CardMap().ReverseMap(c)).ToList)
                };
            }
        }

        public static ICustomer Map(CustomerModel m)
            => new CustomerMap().Map(m);
        public static CustomerModel ReverseMap(ICustomer i)
            => new CustomerMap().ReverseMap(i);
        private class CustomerMap : IMapper<CustomerModel, ICustomer>
        {
            public ICustomer Map(CustomerModel from)
            {
                if (from == null)
                    return null;
                return new CustomerBuilder()
                    .SetBVN(from.BVN)
                    .SetEntryDate(from.EntryDate)
                    .SetLoginCredentials(new LoginCredentialsMap().Map(from.LoginCredential))
                    .SetPerson(new PersonMap().Map(from.Person))
                    .AddAccounts(from.Accounts.Select(a => new AccountMap().Map(a)))
                    .Build();
            }

            public CustomerModel ReverseMap(ICustomer to)
            {
                if (to == null)
                    return null;
                return new()
                {
                    Accounts = to.Accounts.Select(a => new AccountMap().ReverseMap(a)).ToList(),
                    BVN = to.BVN,
                    EntryDate = to.EntryDate,
                    LoginCredential = new LoginCredentialsMap().ReverseMap(to.LoginCredentials),
                    Person = new PersonMap().ReverseMap(to.Person)
                };
            }
        }

        public static IEmployee Map(EmployeeModel m)
            => new EmployeeMap().Map(m);
        public static EmployeeModel ReverseMap(IEmployee i)
            => new EmployeeMap().ReverseMap(i);
        private class EmployeeMap : IMapper<EmployeeModel, IEmployee>
        {
            public IEmployee Map(EmployeeModel from)
            {
                if (from == null)
                    return null;
                var x = new EmployeeBuilder()
                    .SetHireDate(from.HireDate)
                        .SetLevel(from.Level)
                        .SetLoginCredentials(new LoginCredentialsMap().Map(from.Secret))
                        .SetPerson(new PersonMap().Map(from.Person))
                        .SetPosition(from.Position)
                        .SetSalary(from.Salary)
                        .SetQualification(new EducationMap().Map(from.Qualification))
                        .SetWorkingStatus(from.WorkingStatus)
                        .SetSuperior(new EmployeeMap().Map(from.Superior))
                        .SetGuarantor(new PersonMap().Map(from.Guarantor))
                        .SetSubordinate(new EmployeeMap().Map(from.Subordinate))
                        .SetSupervisor(new EmployeeMap().Map(from.Supervisor))
                        .SetAccount(new AccountMap().Map(from.Account));
                x.AddGroup(from.Group == null ? null : from.Group.Select(e => new EmployeeMap().Map(e)));

                return x.Build();
            }

            public EmployeeModel ReverseMap(IEmployee to)
            {
                if (to == null)
                    return null;
                var x = new EmployeeModel();
                x.HireDate = to.HireDate;
                x.Level = to.Level;
                x.Secret = new LoginCredentialsMap().ReverseMap(to.Secret);
                x.Person = new PersonMap().ReverseMap(to.Person);
                x.Salary = to.Salary;
                x.Account = new AccountMap().ReverseMap(to.Account);
                x.Position = to.Position;
                x.Qualification = new EducationMap().ReverseMap(to.Qualification);
                x.WorkingStatus = to.WorkingStatus;
                x.Supervisor = new EmployeeMap().ReverseMap(to.Supervisor);
                var group = to.Group;
                if(group != null && group.Count() > 0)
                {
                    x.Group = new List<EmployeeModel>();
                    foreach (var employee in group)
                    {
                        if(employee != null)
                            x.Group.Add(new EmployeeMap().ReverseMap(employee));
                    }
                }
                x.Subordinate = new EmployeeMap().ReverseMap(to.Subordinate);
                x.Superior = new EmployeeMap().ReverseMap(to.Superior);
                x.Guarantor = new PersonMap().ReverseMap(to.Guarantor);
                return x;
            }
        }

        private static void TryForNull<TArgument, U>(Func<TArgument, U> f, TArgument arg)
        {
            try
            {
                f.Invoke(arg);
            }
            catch (NullReferenceException) { }
            catch (ArgumentNullException) { }
        }

        private static TResult TryForNull<TResult>(Func<TResult> f)
            where TResult : class
        {
            try
            {
                return f.Invoke();
            }
            catch (NullReferenceException)
            {
            }
            catch (ArgumentNullException) { }
            return null;
        }

        private static Dictionary<string, string> LookUp(IDictionary<string, string> d)
        {
            try
            {
                return new Dictionary<string, string>(d);
            }
            catch (NullReferenceException)
            {
            }
            catch (ArgumentNullException) { }
            return null;
        }
    
        public static object MapKeys(NameModel to, NameModel from)
        {
            if (to == null || from == null)
                return null;
            to.NameModelId = from.NameModelId;
            //to.CityTownId = from.CityTownId;
            //to.CountryNameId = from.CountryNameId;
            //to.EntityNameId = from.EntityNameId;
            //to.NameId = from.NameId;
            //to.NicknameId = from.NicknameId;
            //to.OthernamesId = from.OthernamesId;
            //to.StateId = from.StateId;
            //to.SurnameId = from.SurnameId;
            //to.MaidenNameId = from.MaidenNameId;

            return new
            {
                //to.CityTownId, to.CountryNameId, to.EntityNameId, to.NameId, to.NameModelId,
                //to.NicknameId, to.OthernamesId, to.StateId, to.SurnameId, to.MaidenNameId
                to.NameModelId
            };
        }

        public static object MapKeys(FullNameModel to, FullNameModel from)
        {
            if (to == null || from == null)
                return null;
            to.FullNameModelId = from.FullNameModelId;
            to.NameModelId = from.NameModelId;
            to.SurnameNameModelId = from.SurnameNameModelId;
            to.NicknameNameModelId = from.NicknameNameModelId;
            to.MaidenNameNameModelId = from.MaidenNameNameModelId;

            MapKeys(to.Name, from.Name);
            MapKeys(to.Surname, from.Surname);
            MapKeys(to.Nickname, from.Nickname);
            MapKeys(to.MaidenName, from.MaidenName);
            for (var i = 0; i < to.OtherNames.Count; i++)
            {
                MapKeys(to.OtherNames[i], from.OtherNames[i]);
            }

            return new
            {
                to.FullNameModelId, to.NameModelId, to.SurnameNameModelId, to.NicknameNameModelId, to.MaidenNameNameModelId
            };
        }

        public static object MapKeys(EntityModel to, EntityModel from)
        {
            if (to == null || from == null)
                return null;
            to.EntityModelId = from.EntityModelId;
            to.NameModelIdId = from.NameModelIdId;
            to.ContactModelIdId = from.ContactModelIdId;
            //to.BrandId = from.BrandId;
            //to.CreditorId = from.CreditorId;
            //to.DebitorId = from.DebitorId;
            //to.EntityId = from.EntityId;
            //to.OfficialEntityId = from.OfficialEntityId;

            MapKeys(to.Name, from.Name);
            MapKeys(to.Contact, from.Contact);

            return new
            {
                //to.BrandId,
                //to.CreditorId,
                //to.DebitorId,
                //to.EntityId,
                //to.OfficialEntityId
                to.EntityModelId,
                to.NameModelIdId,
                to.ContactModelIdId
            };
        }

        public static object MapKeys(ContactModel to, ContactModel from)
        {
            if (to == null || from == null)
                return null;
            //to.ContactId = from.ContactId;
            to.ContactModelId = from.ContactModelId;
            to.AddressModelId = from.AddressModelId;

            MapKeys(to.Address, from.Address);

            return new
            {
                to.AddressModelId,
                to.ContactModelId
            };
        }

        public static object MapKeys(AddressModel to, AddressModel from)
        {
            if (to == null || from == null)
                return null;
            //to.AcademyId = from.AcademyId;
            //to.AddressId = from.AddressId;
            to.AddressModelId = from.AddressModelId;
            to.CountryOfResidenceNationalityModelId = from.CountryOfResidenceNationalityModelId;

            MapKeys(to.CountryOfResidence, from.CountryOfResidence);

            return new
            {
                //to.AcademyId, to.AddressId, to.AddressModelId
                to.AddressModelId,
                to.CountryOfResidenceNationalityModelId
            };
        }

        public static object MapKeys(NationalityModel to, NationalityModel from)
        {
            if (to == null || from == null)
                return null;
            //to.CountryOfResidenceId = from.CountryOfResidenceId;
            //to.CountryOfOriginId = from.CountryOfOriginId;
            //to.CountryOfUseId = from.CountryOfUseId;
            to.NationalityModelId = to.NationalityModelId;
            to.CountryNameNameModelId = from.CountryNameNameModelId;
            to.StateNameModelId = from.StateNameModelId;
            to.CityTownNameModelId = from.CityTownNameModelId;

            MapKeys(to.CountryName, from.CountryName);
            MapKeys(to.State, from.State);
            MapKeys(to.CityTown, from.CityTown);

            return new
            {
                //to.CountryOfResidenceId,
                //to.CountryOfOriginId,
                //to.CountryOfUseId,
                to.NationalityModelId,
                to.CountryNameNameModelId,
                to.StateNameModelId,
                to.CityTownNameModelId
            };
        }

        public static object MapKeys(LoginCredentialModel to, LoginCredentialModel from)
        {
            if (to == null || from == null)
                return null;
            //to.CardSecretId = from.CardSecretId;
            //to.EmployeeSecretId = from.EmployeeSecretId;
            //to.LoginCredentialId = from.LoginCredentialId;
            to.LoginCredentialModelId = from.LoginCredentialModelId;

            return new
            {
                //to.CardSecretId,
                //to.EmployeeSecretId,
                //to.LoginCredentialId,
                to.LoginCredentialModelId
            };
        }

        public static object MapKeys(EducationModel to, EducationModel from)
        {
            if (to == null || from == null)
                return null;
            to.EducationModelId = from.EducationModelId;
            //to.QualificationId = from.QualificationId;
            to.PrimaryQualificationModelId = from.PrimaryQualificationModelId;
            to.SecondaryQualificationModelId = from.SecondaryQualificationModelId;
            to.PrimaryTertiaryQualificationModelId = from.PrimaryTertiaryQualificationModelId;

            MapKeys(to.Primary, from.Primary);
            MapKeys(to.Secondary, from.Secondary);
            MapKeys(to.PrimaryTertiary, from.PrimaryTertiary);
            for(int i = 0; i < to.Others.Count; i++)
            {
                MapKeys(to.Others[i], from.Others[i]);
            }

            return new
            {
                to.EducationModelId,
                to.PrimaryQualificationModelId,
                to.SecondaryQualificationModelId,
                to.PrimaryTertiaryQualificationModelId
            };
        }

        public static object MapKeys(QualificationModel to, QualificationModel from)
        {
            if (to == null || from == null)
                return null;
            to.QualificationModelId = from.QualificationModelId;
            to.AcademyAddressModelId = from.AcademyAddressModelId;
            //to.OthersId = from.OthersId;
            //to.PrimaryId = from.PrimaryId;
            //to.PrimaryTertiaryId = from.PrimaryTertiaryId;
            //to.SecondaryId = from.SecondaryId;

            MapKeys(to.Academy, from.Academy);

            return new
            {
                //to.OthersId,
                //to.PrimaryId,
                //to.PrimaryTertiaryId,
                //to.SecondaryId
                to.QualificationModelId,
                to.AcademyAddressModelId
            };
        }

        public static object MapKeys(AccountModel to, AccountModel from)
        {
            if (to == null || from == null)
                return null;
            to.AccountModelId = from.AccountModelId;
            //to.CustomerAccountsId = from.CustomerAccountsId;
            to.GuarantorPersonModelId = from.GuarantorPersonModelId;

            for(int i = 0; i < from.Transactions.Count; i++)
            {
                MapKeys(to.Transactions[i], from.Transactions[i]);
            }
            for (int i = 0; i < from.Cards.Count; i++)
            {
                MapKeys(to.Cards[i], from.Cards[i]);
            }
            MapKeys(to.Guarantor, from.Guarantor);

            return new
            {
                to.AccountModelId,
                to.GuarantorPersonModelId
            };
        }

        public static object MapKeys(CardModel to, CardModel from)
        {
            if (to == null || from == null)
                return null;
            to.CardModelId = from.CardModelId;
            //to.CardsId = from.CardsId;
            to.BrandEntityModelId = from.BrandEntityModelId;
            to.CountryOfUseNationalityModelId = from.CountryOfUseNationalityModelId;
            to.SecretLoginCredentialModelId = from.SecretLoginCredentialModelId;

            MapKeys(to.Brand, from.Brand);
            MapKeys(to.CountryOfUse, from.CountryOfUse);
            MapKeys(to.Secret, from.Secret);

            return new
            {
                to.CardModelId,
                to.BrandEntityModelId,
                to.CountryOfUseNationalityModelId,
                to.SecretLoginCredentialModelId
            };
        }

        public static object MapKeys(TransactionModel to, TransactionModel from)
        {
            if (to == null || from == null)
                return null;
            to.TransactionModelId = from.TransactionModelId;
            //to.TransactionsId = from.TransactionsId;
            to.CreditorEntityModelId = from.CreditorEntityModelId;
            to.DebitorEntityModelId = from.DebitorEntityModelId;

            MapKeys(to.Creditor, from.Creditor);
            MapKeys(to.Debitor, from.Debitor);

            return new
            {
                to.TransactionModelId,
                to.CreditorEntityModelId,
                to.DebitorEntityModelId
            };
        }

        public static object MapKeys(PersonModel to, PersonModel from)
        {
            if (to == null || from == null)
                return null;

            //to.AccountGuarantorId = from.AccountGuarantorId;
            //to.CustomerPersonId = from.CustomerPersonId;
            //to.EmployeeGuarantorId = from.EmployeeGuarantorId;
            //to.EmployeePersonId = from.EmployeePersonId;
            //to.NextOfKinId = from.NextOfKinId;
            to.PersonModelId = from.PersonModelId;
            to.CountryOfOriginNationalityModelId = from.CountryOfOriginNationalityModelId;
            to.EntityModelId = from.EntityModelId;
            to.FullNameModelId = from.FullNameModelId;
            to.OfficialEntityEntityModelId = from.OfficialEntityEntityModelId;

            MapKeys(to.Entity, from.Entity);
            MapKeys(to.OfficialEntity, from.OfficialEntity);
            MapKeys(to.FullName, from.FullName);
            MapKeys(to.CountryOfOrigin, from.CountryOfOrigin);
            MapKeys(to.NextOfKin, from.NextOfKin);

            return new
            {
                //to.AccountGuarantorId,
                //to.CustomerPersonId,
                //to.EmployeeGuarantorId,
                //to.EmployeePersonId,
                //to.NextOfKinId,
                to.PersonModelId,
                to.CountryOfOriginNationalityModelId,
                to.EntityModelId,
                to.FullNameModelId,
                to.OfficialEntityEntityModelId
            };
        }

        public static object MapKeys(EmployeeModel to, EmployeeModel from)
        {
            if (to == null || from == null)
                return null;
            to.EmployeeModelId = from.EmployeeModelId;
            to.GuarantorPersonModelId = from.GuarantorPersonModelId;
            to.PersonModelId = from.PersonModelId;
            to.QualificationEducationModelId = from.QualificationEducationModelId;
            to.SecretLoginCredentialModelId = from.SecretLoginCredentialModelId;

            MapKeys(to.Secret, from.Secret);
            MapKeys(to.Guarantor, from.Guarantor);
            MapKeys(to.Qualification, from.Qualification);

            return new
            {
                to.EmployeeModelId,
                to.GuarantorPersonModelId,
                to.PersonModelId,
                to.QualificationEducationModelId,
                to.SecretLoginCredentialModelId
            };
        }

        public static object MapKeys(CustomerModel to, CustomerModel from)
        {
            if (to == null || from == null)
                return null;
            to.CustomerModelId = from.CustomerModelId;

            MapKeys(to.LoginCredential, from.LoginCredential);
            MapKeys(to.Person, from.Person);

            return new
            {
                to.CustomerModelId
            };
        }
    }
}
