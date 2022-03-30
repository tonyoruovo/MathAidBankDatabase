using DataBaseTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static DataBaseTest.Utilities;

namespace DataBaseTest.Repos
{
    public static class Builders
    {
        private static bool IsCompletelyNull(double[] dValues, bool[] bValues, params Object[] obj)
        {
            foreach(var o in obj)
            {
                if (o != null) return false;
            }
            foreach (var d in dValues)
            {
                if (d != 0.0) return false;
            }
            foreach (var b in bValues)
            {
                if (b) return false;
            }
            return true;
        }

        public class EmployeeBuilder : IBuilder<IEmployee, EmployeeBuilder>
        {
            private class EmployeeImpl : IEmployee
            {
                public EmployeeImpl()
                {
                    Group = new List<IEmployee>();
                }
                public IPerson Person { get; set; }

                public string Level { get; set; }

                public string Position { get; set; }

                public decimal Salary { get; set; }

                public IEnumerable<IEmployee> Group { get; set; }

                public IEmployee Supervisor { get; set; }

                public IEmployee Superior { get; set; }

                public IEmployee Subordinate { get; set; }

                public DateTime HireDate { get; set; }

                public WorkingStatus WorkingStatus { get; set; }

                public IEducation Qualification { get; set; }

                public IPerson Guarantor { get; set; }

                public ILoginCredentials Secret { get; set; }

                public IAccount Account { get; set; }

                public override int GetHashCode()
                {
                    var hashCode = -1 & HireDate.GetHashCode() & WorkingStatus.GetHashCode() & Person.GetHashCode(); //for null properties
                    hashCode &= (Level == null ? 0 : Level.GetHashCode());
                    return hashCode;
                }

                public override bool Equals(object obj)
                {
                    if(obj is IEmployee e)
                    {
                        var isEquals = HireDate == e.HireDate && WorkingStatus == e.WorkingStatus;
                        isEquals = isEquals && (Person == null ? e.Person == null : Person.Equals(e.Person));
                        return isEquals;
                    }
                    return false;
                }
            }

            public EmployeeBuilder()
            {
                _e = new EmployeeImpl();
            }

            public EmployeeBuilder SetPerson(IPerson person)
            {
                _e.Person = person;
                return this;
            }

            public EmployeeBuilder SetLevel(string level)
            {
                _e.Level = level;
                return this;
            }

            public EmployeeBuilder SetPosition(string position)
            {
                _e.Position = position;
                return this;
            }

            public EmployeeBuilder SetSalary(decimal sal)
            {
                _e.Salary = sal;
                return this;
            }

            public EmployeeBuilder SetLoginCredentials(ILoginCredentials secret)
            {
                _e.Secret = secret;
                return this;
            }

            public EmployeeBuilder SetAccount(IAccount account)
            {
                _e.Account = account;
                return this;
            }

            public EmployeeBuilder SetSupervisor(IEmployee supervisor)
            {
                _e.Supervisor = supervisor;
                return this;
            }

            public EmployeeBuilder SetSuperior(IEmployee superior)
            {
                _e.Superior = superior;
                return this;
            }

            public EmployeeBuilder SetSubordinate(IEmployee subordinate)
            {
                _e.Subordinate = subordinate;
                return this;
            }

            public EmployeeBuilder SetHireDate(DateTime date)
            {
                _e.HireDate = date;
                return this;
            }

            public EmployeeBuilder SetWorkingStatus(WorkingStatus workingStatus)
            {
                _e.WorkingStatus = workingStatus;
                return this;
            }

            public EmployeeBuilder SetQualification(IEducation qualification)
            {
                _e.Qualification = qualification;
                return this;
            }

            public EmployeeBuilder SetGuarantor(IPerson Guarantor)
            {
                _e.Guarantor = Guarantor;
                return this;
            }

            public EmployeeBuilder AddEmployee(IEmployee e)
            {
                ((List<IEmployee>)_e.Group).Add(e);
                return this;
            }

            public EmployeeBuilder AddGroup(IEnumerable<IEmployee> e)
            {
                _e.Group = e;
                return this;
            }

            public IEmployee Build()
            {
                double[] nums = { (double)_e.Salary };
                if (IsCompletelyNull(nums, Array.Empty<bool>(), _e.Account, _e.Group, _e.Guarantor, _e.HireDate,
                    _e.Level, _e.Person, _e.Position, _e.Qualification, _e.Secret, _e.Subordinate,
                    _e.Superior, _e.Supervisor, _e.WorkingStatus))
                    return null;
                return _e;
            }

            public void Clear()
            {
                _e = new EmployeeImpl();
            }

            public EmployeeBuilder ReBuild(IEmployee model)
            {
                _e.Account = model.Account;
                _e.Group = model.Group;
                _e.Guarantor = model.Guarantor;
                _e.HireDate = model.HireDate;
                _e.Level = model.Level;
                _e.Person = model.Person;
                _e.Position = model.Position;
                _e.Qualification = model.Qualification;
                _e.Salary = model.Salary;
                _e.Secret = model.Secret;
                _e.Subordinate = model.Subordinate;
                _e.Superior = model.Superior;
                _e.Supervisor = model.Supervisor;
                _e.WorkingStatus = model.WorkingStatus;

                return this;
            }

            private EmployeeImpl _e;
        }

        public class CustomerBuilder : IBuilder<ICustomer, CustomerBuilder>
        {
            private class CustomerImpl : ICustomer
            {
                public CustomerImpl()
                {
                    Accounts = new List<IAccount>();
                }
                public IPerson Person { get; set; }

                public string BVN { get; set; }

                public DateTime EntryDate { get; set; }

                public ILoginCredentials LoginCredentials { get; set; }

                public IEnumerable<IAccount> Accounts { get; set; }

                public override int GetHashCode()
                {
                    var hash = -1;
                    hash &= (Person == null ? 0 : Person.GetHashCode()) & (BVN ?? "0").GetHashCode() &
                        EntryDate.GetHashCode();
                    return hash;
                }

                public override bool Equals(object obj)
                {
                    if (obj is ICustomer c)
                    {
                        var val = EntryDate == c.EntryDate;
                        val = val && (BVN == null ? c.BVN == null : BVN.CompareTo(c.BVN) == 0) && (Person == null ? c.Person == null : Person.Equals(c.Person));
                        return val;
                    }
                    return false;
                }
            }

            public CustomerBuilder()
            {
                _c = new CustomerImpl();
            }

            public CustomerBuilder SetPerson(IPerson p)
            {
                _c.Person = p;
                return this;
            }

            public CustomerBuilder SetBVN(string bvn)
            {
                _c.BVN = bvn;
                return this;
            }

            public CustomerBuilder SetEntryDate(DateTime date)
            {
                _c.EntryDate = date;
                return this;
            }

            public CustomerBuilder SetLoginCredentials(ILoginCredentials x)
            {
                _c.LoginCredentials = x;
                return this;
            }

            public CustomerBuilder AddAccount(IAccount account)
            {if (_c.Accounts == null) _c.Accounts = new List<IAccount>();
                ((List<IAccount>)_c.Accounts).Add(account);
                return this;
            }

            public CustomerBuilder AddAccounts(IEnumerable<IAccount> accounts)
            {
                _c.Accounts = accounts;
                return this;
            }

            public ICustomer Build()
            {
                if (IsCompletelyNull(Array.Empty<double>(), Array.Empty<bool>(), _c.Accounts, _c.BVN,
                    _c.EntryDate, _c.LoginCredentials, _c.Person))
                    return null;
                return _c;
            }

            public void Clear()
            {
                _c = new CustomerImpl();
            }

            public CustomerBuilder ReBuild(ICustomer model)
            {
                _c.Accounts = model.Accounts;
                _c.BVN = model.BVN;
                _c.EntryDate = model.EntryDate;
                _c.LoginCredentials = model.LoginCredentials;
                _c.Person = model.Person;

                return this;
            }

            private CustomerImpl _c;
        }

        public class PersonBuilder : IBuilder<IPerson, PersonBuilder>
        {
            private class PersonImpl : IPerson
            {
                public PersonImpl()
                {
                    Identification = new Dictionary<string, string>();
                }

                public IEntity Entity { get; set; }

                public IEntity OfficialEntity { get; set; }

                public IFullName FullName { get; set; }

                public Guid UniqueTag { get; set; }

                public DateTime BirthDate { get; set; }

                public MaritalStatus MaritalStatus { get; set; }

                public INationality CountryOfOrigin { get; set; }

                public IPerson NextOfKin { get; set; }

                public bool IsMale { get; set; }

                public IDictionary<string, string> Identification { get; set; }

                public string JobType { get; set; }

                public byte[] Passport { get; set; }

                public byte[] Signature { get; set; }

                public byte[] FingerPrint { get; set; }

                public byte Age()
                {
                    return (byte)(DateTime.Now.Year - BirthDate.Year);
                }

                public override int GetHashCode()
                {
                    var hash = -1 & UniqueTag.GetHashCode() & BirthDate.GetHashCode() & MaritalStatus.GetHashCode();
                    hash &= (Entity == null ? 0 : Entity.GetHashCode()) &
                        (FullName == null ? 0 : FullName.GetHashCode()) &
                        (CountryOfOrigin == null ? 0 : CountryOfOrigin.GetHashCode()) &
                        Hash(OfficialEntity) & Hash(Passport) & Hash(Signature) & Hash(FingerPrint);
                    return hash;
                }

                public override bool Equals(object obj)
                {
                    if (obj is IPerson p)
                    {
                        var equals = UniqueTag == p.UniqueTag && BirthDate == p.BirthDate && MaritalStatus == p.MaritalStatus;
                        equals = equals && (Entity == null ? p.Entity == null : Entity.Equals(p.Entity)) &&
                            (FullName == null ? p.FullName == null : FullName.Equals(p.FullName)) &&
                            (CountryOfOrigin == null ? p.CountryOfOrigin == null : CountryOfOrigin.Equals(p.CountryOfOrigin))
                            && IsEqual(OfficialEntity, p.OfficialEntity)
                            && IsEqual(Passport, p.Passport) && IsEqual(Signature, p.Signature)
                            && IsEqual(FingerPrint, p.FingerPrint);
                        return equals;
                    }
                    return false;
                }
            }

            public PersonBuilder()
            {
                p = new PersonImpl();
            }

            public PersonBuilder SetEntity(IEntity entity)
            {
                p.Entity = entity;
                return this;
            }

            public PersonBuilder SetOfficialEntity(IEntity entity)
            {
                p.OfficialEntity = entity;
                return this;
            }

            public PersonBuilder SetFullName(IFullName name)
            {
                p.FullName = name;
                return this;
            }

            public PersonBuilder SetUniqueTag(Guid tag)
            {
                p.UniqueTag = tag;
                return this;
            }

            public PersonBuilder SetBirthDate(DateTime date)
            {
                p.BirthDate = date;
                return this;
            }

            public PersonBuilder SetMaritalStatus(MaritalStatus ms)
            {
                p.MaritalStatus = ms;
                return this;
            }

            public PersonBuilder SetCountryOfOrigin(INationality coo)
            {
                p.CountryOfOrigin = coo;
                return this;
            }

            public PersonBuilder SetNextOfKin(IPerson nok)
            {
                p.NextOfKin = nok;
                return this;
            }

            public PersonBuilder SetSex(bool isMale)
            {
                p.IsMale = isMale;
                return this;
            }

            public PersonBuilder SetJobType(string job)
            {
                p.JobType = job;
                return this;
            }

            public PersonBuilder SetPassport(byte[] passport)
            {
                p.Passport = passport;
                return this;
            }

            public PersonBuilder SetSignature(byte[] signature)
            {
                p.Signature = signature;
                return this;
            }

            public PersonBuilder SetFingerPrint(byte[] fingerPrint)
            {
                p.FingerPrint = fingerPrint;
                return this;
            }

            public PersonBuilder AddIdentification(string idCardName, string cardNumber)
            {
                p.Identification.Add(idCardName, cardNumber);
                return this;
            }

            public PersonBuilder AddIdentifications(IDictionary<string, string> ids)
            {
                p.Identification = ids != null ? ids.Any() ? ids : null : null ;
                return this;
            }

            public IPerson Build()
            {
                if (IsCompletelyNull(Array.Empty<double>(), new bool[] { p.IsMale }, p.BirthDate, p.CountryOfOrigin, p.Entity, p.FingerPrint, p.FullName,
                    p.Identification, p.JobType, p.MaritalStatus, p.NextOfKin, p.OfficialEntity, p.Passport,
                    p.Signature, p.UniqueTag))
                    return null;
                return p;
            }

            public void Clear()
            {
                p = new PersonImpl();
            }

            public PersonBuilder ReBuild(IPerson model)
            {
                p.BirthDate = model.BirthDate;
                p.CountryOfOrigin = model.CountryOfOrigin;
                p.Entity = model.Entity;
                p.FingerPrint = model.FingerPrint;
                p.FullName = model.FullName;
                p.Identification = model.Identification;
                p.IsMale = model.IsMale;
                p.JobType = model.JobType;
                p.MaritalStatus = model.MaritalStatus;
                p.NextOfKin = model.NextOfKin;
                p.OfficialEntity = model.OfficialEntity;
                p.Passport = model.Passport;
                p.Signature = model.Signature;
                p.UniqueTag = model.UniqueTag;

                return this;
            }

            private PersonImpl p;
        }

        public class EntityBuilder : IBuilder<IEntity, EntityBuilder>
        {
            private class EntityImpl : IEntity
            {
                public IName Name { get; set; }
                public IContact Contact { get; set; }

                public override int GetHashCode()
                {
                    var hash = -1;
                    hash &= (Name == null ? 0 : Name.GetHashCode()) & (Contact == null ? 0 : Contact.GetHashCode());
                    return hash;
                }

                public override bool Equals(object obj)
                {
                    if (obj is IEntity e)
                    {
                        var isEquals = (Name == null ? e.Name == null : Name.Equals(e.Name))
                            && (Contact == null ? e.Contact == null : Contact.Equals(e.Contact));
                        return isEquals;
                    }
                    return false; 
                }
            }

            public EntityBuilder SetName(IName name)
            {
                e.Name = name;
                return this;
            }

            public EntityBuilder SetContact(IContact con)
            {
                e.Contact = con;
                return this;
            }

            public EntityBuilder()
            {
                e = new EntityImpl();
            }
            public IEntity Build()
            {
                if (IsCompletelyNull(Array.Empty<double>(), Array.Empty<bool>(), e.Contact, e.Name))
                    return null;
                return e;
            }

            public void Clear()
            {
                e = new EntityImpl();
            }

            public EntityBuilder ReBuild(IEntity model)
            {
                e.Contact = model.Contact;
                e.Name = model.Name;

                return this;
            }

            private EntityImpl e;
        }

        public class ContactBuilder : IBuilder<IContact, ContactBuilder>
        {
            private class ContactImpl : IContact
            {
                public ContactImpl()
                {
                    Emails = new List<string>();
                    SocialMedia = new List<string>();
                    Mobiles = new List<string>();
                    Websites = new List<string>();
                }
                public IEnumerable<string> Emails { get; set; }

                public IEnumerable<string> SocialMedia { get; set; }

                public IEnumerable<string> Mobiles { get; set; }

                public IEnumerable<string> Websites { get; set; }

                public IAddress Address { get; set; }

                public override int GetHashCode()
                {
                    return -1 & (Address == null ? 0 : Address.GetHashCode()) &
                        (Emails == null ? 0 : Emails.GetHashCode()) & (SocialMedia == null ? 0 : SocialMedia.GetHashCode()) &
                        (Mobiles == null ? 0 : Mobiles.GetHashCode()) & (Websites == null ? 0 : Websites.GetHashCode());
                }

                public override bool Equals(object obj)
                {
                    if(obj is IContact c)
                    {
                        var isEqual = IsEqual(Emails, c.Emails) && IsEqual(SocialMedia, c.SocialMedia)
                            && IsEqual(Mobiles, c.Mobiles) && IsEqual(Websites, c.Websites);
                        isEqual = isEqual && (Address == null ? c.Address == null : Address.Equals(c.Address));
                        return isEqual;
                    }
                    return false;
                }
            }
            public ContactBuilder()
            {
                c = new ContactImpl();
            }
            public ContactBuilder SetAddress(IAddress address)
            {
                c.Address = address;
                return this;
            }

            public ContactBuilder AddEmail(string email)
            {
                ((List<string>)c.Emails).Add(email);
                return this;
            }

            public ContactBuilder AddSocialMedia(string social)
            {
                ((List<string>)c.SocialMedia).Add(social);
                return this;
            }

            public ContactBuilder AddMobile(string mobile)
            {
                if(!(String.IsNullOrEmpty(mobile) || String.IsNullOrWhiteSpace(mobile)))
                    ((List<string>)c.Mobiles).Add(mobile);
                return this;
            }

            public ContactBuilder AddWebsite(string web)
            {
                ((List<string>)c.Websites).Add(web);
                return this;
            }

            public ContactBuilder AddEmails(IEnumerable<string> emails)
            {
                c.Emails = emails != null ? emails.Any() ? emails : null : null ;
                return this;
            }

            public ContactBuilder AddSocialMedia(IEnumerable<string> socials)
            {
                c.SocialMedia = socials != null ? socials.Any() ? socials : null : null;
                return this;
            }

            public ContactBuilder AddMobiles(IEnumerable<string> mobiles)
            {
                c.Mobiles = mobiles != null ? mobiles.Any() ? mobiles : null : null;
                return this;
            }

            public ContactBuilder AddWebsite(IEnumerable<string> websites)
            {
                c.Websites = websites != null ? websites.Any() ? websites : null : null;
                return this;
            }

            public IContact Build()
            {
                if (IsCompletelyNull(Array.Empty<double>(), Array.Empty<bool>(), c.Address, c.Emails,
                    c.Mobiles, c.SocialMedia, c.Websites))
                    return null;
                return c;
            }

            public void Clear()
            {
                c = new ContactImpl();
            }

            public ContactBuilder ReBuild(IContact model)
            {
                c.Address = model.Address;
                c.Emails = model.Emails;
                c.Mobiles = model.Mobiles;
                c.SocialMedia = model.SocialMedia;
                c.Websites = model.Websites;

                return this;
            }

            private ContactImpl c;
        }

        public class NameBuilder : IBuilder<IName, NameBuilder>
        {
            private class NameImpl : IName
            {
                public string Name
                {
                    get => _name;

                    set
                    {
                        _name = Utilities.ToProper(value);
                    }
                }

                public override int GetHashCode()
                {
                    return -1 & (Name == null ? 0 : Name.GetHashCode());
                }

                public override bool Equals(object obj)
                {
                    if (obj is IName n)
                        return Name == null ? n.Name == null : Name.CompareTo(n.Name) == 0;
                    return false;
                }

                private string _name;
            }
            public NameBuilder()
            {
                n = new NameImpl();
            }

            public NameBuilder SetName(string name)
            {
                n.Name = String.IsNullOrEmpty(name) || String.IsNullOrWhiteSpace(name)
                ? null : name.Trim();
                return this;
            }

            public IName Build()
            {
                return n.Name == null ? null : n;
            }

            public void Clear()
            {
                n = new NameImpl();
            }

            public NameBuilder ReBuild(IName model)
            {
                n.Name = model.Name;

                return this;
            }

            private NameImpl n;
        }

        public class AddressBuilder : IBuilder<IAddress, AddressBuilder>
        {
            private class AddressImpl : IAddress
            {
                public INationality CountryOfResidence { get; set; }

                public string StreetAddress { get; set; }
                public string ZIPCode { get; set; }
                public string PMB { get; set; }

                public override int GetHashCode()
                {
                    var hash = -1;
                    hash &= (CountryOfResidence == null ? 0 : CountryOfResidence.GetHashCode()) &
                        (StreetAddress == null ? 0 : StreetAddress.GetHashCode()) &
                        (ZIPCode == null ? 0 : ZIPCode.GetHashCode()) &
                        (PMB == null ? 0 : PMB.GetHashCode());
                    return hash;
                }

                public override bool Equals(object obj)
                {
                    if (obj is IAddress a)
                    {
                        var isEqual = (CountryOfResidence == null ? a.CountryOfResidence == null : CountryOfResidence.Equals(a.CountryOfResidence))
                            && (StreetAddress == null ? a.StreetAddress == null : StreetAddress.CompareTo(a.StreetAddress) == 0)
                            && (ZIPCode == null ? a.ZIPCode == null : ZIPCode.CompareTo(a.ZIPCode) == 0)
                            && (PMB == null ? a.PMB == null : PMB.CompareTo(a.PMB) == 0);
                        return isEqual;
                    }
                    return false;
                }
            }
            public AddressBuilder()
            {
                a = new AddressImpl();
            }

            public AddressBuilder SetCountryOfResidence(INationality cor)
            {
                a.CountryOfResidence = cor;
                return this;
            }

            public AddressBuilder SetStreetAddress(string streetAddress)
            {
                a.StreetAddress = String.IsNullOrEmpty(streetAddress) || String.IsNullOrWhiteSpace(streetAddress)
                ? null : streetAddress;
                return this;
            }

            public AddressBuilder SetZipCode(string zip)
            {
                a.ZIPCode = String.IsNullOrEmpty(zip) || String.IsNullOrWhiteSpace(zip)
                ? null : zip;
                return this;
            }

            public AddressBuilder SetPMB(string pmb)
            {
                a.PMB = String.IsNullOrEmpty(pmb) || String.IsNullOrWhiteSpace(pmb)
                ? null : pmb;
                return this;
            }

            public IAddress Build()
            {
                if (IsCompletelyNull(Array.Empty<double>(), Array.Empty<bool>(), a.CountryOfResidence, a.PMB,
                    a.StreetAddress, a.ZIPCode))
                    return null;
                return a;
            }

            public void Clear()
            {
                a = new AddressImpl();
            }

            public AddressBuilder ReBuild(IAddress model)
            {
                a.CountryOfResidence = model.CountryOfResidence;
                a.PMB = model.PMB;
                a.StreetAddress = model.StreetAddress;
                a.ZIPCode = model.ZIPCode;

                return this;
            }

            private AddressImpl a;
        }

        public class NationalityBuilder : IBuilder<INationality, NationalityBuilder>
        {
            private class NationalityImpl : INationality
            {
                public IName CountryName { get; set; }
                public string Language { get; set; }
                public IName State { get; set; }
                public string LGA { get; set; }
                public IName CityTown { get; set; }

                public override int GetHashCode()
                {
                    return -1 & (CountryName == null ? 0 : CountryName.GetHashCode()) &
                        (Language == null ? 0 : Language.GetHashCode()) &
                        (State == null ? 0 : State.GetHashCode()) &
                        (LGA == null ? 0 : LGA.GetHashCode()) &
                        (CityTown == null ? 0 : CityTown.GetHashCode());
                }

                public override bool Equals(object obj)
                {
                    if (obj is INationality n)
                    {
                        return (CountryName == null ? n.CountryName == null : CountryName.Equals(n.CountryName))
                            && (Language == null ? n.Language == null : Language.CompareTo(n.Language) == 0)
                            && (State == null ? n.State == null : State.Equals(n.State))
                            && (LGA == null ? n.LGA == null : LGA.CompareTo(n.LGA) == 0)
                            && (CityTown == null ? n.CityTown == null : CityTown.Equals(n.CityTown));
                    }
                    return false;
                }
            }

            public NationalityBuilder()
            {
                n = new NationalityImpl();
            }

            public NationalityBuilder SetCountryName(IName cName)
            {
                n.CountryName = cName;
                return this;
            }

            public NationalityBuilder SetLanguage(string l)
            {
                n.Language = String.IsNullOrEmpty(l) || String.IsNullOrWhiteSpace(l)
                ? null : l;
                return this;
            }

            public NationalityBuilder SetState(IName sName)
            {
                n.State = sName;
                return this;
            }

            public NationalityBuilder SetLGA(string lga)
            {
                n.LGA = String.IsNullOrEmpty(lga) || String.IsNullOrWhiteSpace(lga)
                ? null : lga;
                return this;
            }

            public NationalityBuilder SetCityTown(IName ct)
            {
                n.CityTown = ct;
                return this;
            }

            public INationality Build()
            {
                if (IsCompletelyNull(Array.Empty<double>(), Array.Empty<bool>(), n.CountryName, n.Language,
                    n.State, n.LGA, n.CityTown))
                    return null;
                return n;
            }

            public void Clear()
            {
                n = new NationalityImpl();
            }

            public NationalityBuilder ReBuild(INationality model)
            {
                n.CityTown = model.CityTown;
                n.CountryName = model.CountryName;
                n.Language = model.Language;
                n.LGA = model.LGA;
                n.State = model.State;
                return this;
            }

            private NationalityImpl n;
        }

        public class FullNameBuilder : IBuilder<IFullName, FullNameBuilder>
        {
            private class FullNameImpl : IFullName
            {
                public FullNameImpl()
                {
                    OtherNames = new List<IName>();
                }
                public IName Name { get; set; }

                public IName Surname { get; set; }

                public string Title { get; set; }

                public IName Nickname { get; set; }

                public IName MaidenName { get; set; }

                public IEnumerable<IName> OtherNames { get; set; }

                public override int GetHashCode()
                {
                    return -1 & (Name == null ? 0 : Name.GetHashCode()) &
                        (Surname == null ? 0 : Surname.GetHashCode()) &
                        (Title == null ? 0 : Title.GetHashCode()) &
                        (Nickname == null ? 0 : Nickname.GetHashCode()) &
                        (Hash(MaidenName)) &
                        CombineHash(OtherNames);
                }

                public override bool Equals(object obj)
                {
                    if(obj is IFullName f)
                    {
                        return (Name == null ? f.Name == null : Name.Equals(f.Name))
                            && (Surname == null ? f.Surname == null : Surname.Equals(f.Surname))
                            && (Title == null ? f.Title == null : Title.CompareTo(f.Title) == 0)
                            && (Nickname == null ? f.Nickname == null : Nickname.Equals(f.Nickname))
                            && IsEqual(OtherNames, f.OtherNames);
                    }
                    return false;
                }
            }

            public FullNameBuilder()
            {
                f = new FullNameImpl();
            }

            public FullNameBuilder SetName(IName firstName)
            {
                f.Name = firstName;
                return this;
            }

            public FullNameBuilder SetSurname(IName surname)
            {
                f.Surname = surname;
                return this;
            }

            public FullNameBuilder SetTitle(string title)
            {
                f.Title = String.IsNullOrEmpty(title) || String.IsNullOrWhiteSpace(title)
                ? null : title;
                return this;
            }

            public FullNameBuilder SetNickname(IName nickname)
            {
                f.Nickname = nickname;
                return this;
            }

            public FullNameBuilder SetMaidenName(IName maidenName)
            {
                f.MaidenName = maidenName;
                return this;
            }

            public FullNameBuilder AddName(IName name)
            {
                ((List<IName>)f.OtherNames).Add(name);
                return this;
            }

            public FullNameBuilder AddOthers(IEnumerable<IName>others)
            {
                f.OtherNames = others != null ? others.Any() ? others : null : null;
                return this;
            }

            public IFullName Build()
            {
                if (IsCompletelyNull(Array.Empty<double>(), Array.Empty<bool>(), f.MaidenName, f.Name,
                    f.Nickname, f.OtherNames, f.Surname, f.Title))
                    return null;
                return f;
            }

            public void Clear()
            {
                f = new FullNameImpl();
            }

            public FullNameBuilder ReBuild(IFullName model)
            {
                f.MaidenName = model.MaidenName;
                f.Name = model.Name;
                f.Nickname = model.Nickname;
                f.OtherNames = model.OtherNames;
                f.Surname = model.Surname;
                f.Title = model.Title;

                return this;
            }

            private FullNameImpl f;
        }

        public class LoginCredentialsBuilder : IBuilder<ILoginCredentials, LoginCredentialsBuilder>
        {
            private class LoginCredentialsImpl : ILoginCredentials
            {
                public string PersonalOnlineKey { get; set; }

                public string Username { get; set; }

                public string Password { get; set; }

                public override int GetHashCode()
                {
                    return -1 & (PersonalOnlineKey == null ? 0 : PersonalOnlineKey.GetHashCode()) &
                        (Username == null ? 0 : Username.GetHashCode()) &
                        (Password == null ? 0 : Password.GetHashCode());
                }

                public override bool Equals(object obj)
                {
                    if (obj is ILoginCredentials l)
                        return PersonalOnlineKey.CompareTo(l.PersonalOnlineKey) == 0
                            && Username.CompareTo(l.Username) == 0
                            && Password.CompareTo(l.Password) == 0;
                    return false;
                }
            }

            public LoginCredentialsBuilder()
            {
                l = new LoginCredentialsImpl();
            }

            public LoginCredentialsBuilder SetPersonalOnlineKey(string pok)
            {
                l.PersonalOnlineKey = pok;
                return this;
            }

            public LoginCredentialsBuilder SetUsername(string username)
            {
                l.Username = username;
                return this;
            }

            public LoginCredentialsBuilder SetPassword(string password)
            {
                l.Password = password;
                return this;
            }

            public ILoginCredentials Build()
            {
                if (IsCompletelyNull(Array.Empty<double>(), Array.Empty<bool>(), l.Password, l.PersonalOnlineKey, l.Username))
                    return null;
                return l;
            }

            public void Clear()
            {
                l = new LoginCredentialsImpl();
            }

            public LoginCredentialsBuilder ReBuild(ILoginCredentials model)
            {
                l.Password = model.Password;
                l.PersonalOnlineKey = model.PersonalOnlineKey;
                l.Username = model.Username;

                return this;
            }

            private LoginCredentialsImpl l;
        }

        public class AccountBuilder : IBuilder<IAccount, AccountBuilder>
        {
            private class AccountImpl : IAccount
            {
                public AccountImpl()
                {
                    SMSAlertList = new List<string>();
                    EmailAlertList = new List<string>();
                    Statuses = new Queue<AccountStatusInfo>();
                    Transactions = new List<ITransaction>();
                    Cards = new List<ICard>();
                }
                public string Number { get; set; }

                public decimal Balance { get; set; }

                public decimal PercentageIncrease { get; set; }

                public decimal PercentageDecrease { get; set; }

                public decimal Debt { get; set; }

                public decimal DebitLimit { get; set; }

                public decimal CreditLimit { get; set; }

                public DateTime EntryDate { get; set; }

                public ICurrency Currency { get; set; }

                public IEnumerable<string> SMSAlertList { get; set; }

                public IEnumerable<string> EmailAlertList { get; set; }

                public Queue<AccountStatusInfo> Statuses { get; set; }

                public TimeSpan CreditIntervalLimit { get; set; }

                public TimeSpan DebitIntervalLimit { get; set; }

                public IEnumerable<ITransaction> Transactions { get; set; }

                public IEnumerable<ICard> Cards { get; set; }

                public AccountType Type { get; set; }

                public IPerson Guarantor { get; set; }

                public decimal SmallestBalance { get; set; }

                public decimal SmallestTransferIn { get; set; }

                public decimal SmallestTransferOut { get; set; }

                public decimal LargestBalance { get; set; }

                public decimal LargestTransferIn { get; set; }

                public decimal LargestTransferOut { get; set; }

                public bool Credit(string jsonDetails)
                {
                    throw new NotImplementedException();
                }

                public bool Debit(string jsonDetails)
                {
                    throw new NotImplementedException();
                }

                public override int GetHashCode()
                {
                    var hash = -1 & Balance.GetHashCode() & EntryDate.GetHashCode() & Type.GetHashCode();
                    hash &= (Number == null ? 0 : Number.GetHashCode()) &
                        (Currency == null ? 0 : Currency.GetHashCode()) &
                        (Cards == null ? 0 : CombineHash(Cards));
                    return hash;
                }

                public override bool Equals(object obj)
                {
                    if(obj is IAccount a)
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
            }

            public AccountBuilder()
            {
                a = new AccountImpl();
            }

            public AccountBuilder SetNumber(string n)
            {
                a.Number = String.IsNullOrEmpty(n) || String.IsNullOrWhiteSpace(n) ? null : n.Trim();
                return this;
            }

            public AccountBuilder SetBalance(decimal bal)
            {
                a.Balance = bal;
                return this;
            }

            public AccountBuilder SetPercentageIncrease(decimal pi)
            {
                a.PercentageIncrease = pi;
                return this;
            }

            public AccountBuilder SetPercentageDecrease(decimal pd)
            {
                a.PercentageDecrease = pd;
                return this;
            }

            public AccountBuilder SetDebt(decimal d)
            {
                a.Debt = d;
                return this;
            }

            public AccountBuilder SetDebitLimit(decimal d)
            {
                a.DebitLimit = d;
                return this;
            }

            public AccountBuilder SetCreditLimit(decimal c)
            {
                a.CreditLimit = c;
                return this;
            }

            public AccountBuilder SetEntryDate(DateTime date)
            {
                a.EntryDate = date;
                return this;
            }

            public AccountBuilder SetCurrency(ICurrency c)
            {
                a.Currency = c;
                return this;
            }

            public AccountBuilder SetCreditIntervalLimit(TimeSpan ts)
            {
                a.CreditIntervalLimit = ts;
                return this;
            }

            public AccountBuilder SetDebitIntervalLimit(TimeSpan ts)
            {
                a.DebitIntervalLimit = ts;
                return this;
            }

            public AccountBuilder SetType(AccountType type)
            {
                a.Type = type;
                return this;
            }

            public AccountBuilder SetGuarantor(IPerson guarantor)
            {
                a.Guarantor = guarantor;
                return this;
            }

            public AccountBuilder SetSmallestBalance(decimal bal)
            {
                a.SmallestBalance = bal;
                return this;
            }

            public AccountBuilder SetSmallestTransferIn(decimal tr)
            {
                a.SmallestTransferIn = tr;
                return this;
            }

            public AccountBuilder SetSmallestTransferOut(decimal tr)
            {
                a.SmallestTransferOut = tr;
                return this;
            }

            public AccountBuilder SetLargestBalance(decimal bal)
            {
                a.LargestBalance = bal;
                return this;
            }

            public AccountBuilder SetLargestTransferIn(decimal tr)
            {
                a.LargestTransferIn = tr;
                return this;
            }

            public AccountBuilder SetLargestTransferOut(decimal tr)
            {
                a.LargestTransferOut = tr;
                return this;
            }

            public AccountBuilder AddSMSAlertNumber(string mobile)
            {
                ((List<string>)a.SMSAlertList).Add(mobile);
                return this;
            }

            public AccountBuilder AddEmailAlert(string email)
            {
                ((List<string>)a.EmailAlertList).Add(email);
                return this;
            }

            public AccountBuilder AddTransaction(ITransaction transaction)
            {
                ((List<ITransaction>)a.Transactions).Add(transaction);
                return this;
            }

            public AccountBuilder AddStatus(AccountStatusInfo status)
            {
                ((Queue<AccountStatusInfo>)a.Statuses).Enqueue(status);
                return this;
            }

            public AccountBuilder SetSMSAlertList(IEnumerable<string> mobiles)
            {
                a.SMSAlertList = mobiles;
                return this;
            }

            public AccountBuilder SetEmailAlertList(IEnumerable<string> emails)
            {
                a.EmailAlertList = emails;
                return this;
            }

            public AccountBuilder SetTransactions(IEnumerable<ITransaction> transactions)
            {
                a.Transactions = transactions;
                return this;
            }

            public AccountBuilder SetStatuses(Queue<AccountStatusInfo> statuses)
            {
                a.Statuses = statuses;
                return this;
            }

            public AccountBuilder AddCard(ICard card)
            {
                ((List<ICard>)a.Cards).Add(card);
                return this;
            }

            public AccountBuilder SetCards(IEnumerable<ICard> cards)
            {
                a.Cards = cards;
                return this;
            }

            public IAccount Build()
            {
                double[] doubles = { (double)a.Balance, (double)a.CreditLimit, (double)a.DebitLimit, (double)a.Debt, (double)a.LargestBalance, (double)a.LargestTransferIn,
                (double)a.LargestTransferOut, (double)a.PercentageDecrease, (double)a.PercentageIncrease, (double)a.SmallestBalance, (double)a.SmallestTransferIn, (double)a.SmallestTransferOut};
                if (IsCompletelyNull(doubles, Array.Empty<bool>(), a.Cards, a.CreditIntervalLimit, a.Currency, a.DebitIntervalLimit, a.EmailAlertList, a.EntryDate, a.Guarantor, a.Number,
                    a.SMSAlertList, a.Statuses, a.Transactions, a.Type))
                    return null;
                return a;
            }

            public void Clear()
            {
                a = new AccountImpl();
            }

            public AccountBuilder ReBuild(IAccount model)
            {
                a.Balance = model.Balance;
                a.Cards = model.Cards;
                a.CreditIntervalLimit = model.CreditIntervalLimit;
                a.CreditLimit = model.CreditLimit;
                a.Currency = model.Currency;
                a.DebitIntervalLimit = model.DebitIntervalLimit;
                a.DebitLimit = model.DebitLimit;
                a.Debt = model.Debt;
                a.EmailAlertList = model.EmailAlertList;
                a.EntryDate = model.EntryDate;
                a.Guarantor = model.Guarantor;
                a.LargestBalance = model.LargestBalance;
                a.LargestTransferIn = model.LargestTransferIn;
                a.LargestTransferOut = model.LargestTransferOut;
                a.Number = model.Number;
                a.PercentageDecrease = model.PercentageDecrease;
                a.PercentageIncrease = model.PercentageIncrease;
                a.SmallestBalance = model.SmallestBalance;
                a.SmallestTransferIn = model.SmallestTransferIn;
                a.SmallestTransferOut = model.SmallestTransferOut;
                a.SMSAlertList = model.SMSAlertList;
                a.Statuses = model.Statuses;
                a.Transactions = model.Transactions;
                a.Type = model.Type;

                return this;
            }

            private AccountImpl a;
        }

        public class TransactionBuilder : IBuilder<ITransaction, TransactionBuilder>
        {
            private class TransactionImpl : ITransaction
            {
                public decimal Amount { get; set; }
                public bool IsIncoming { get; set; }
                public string Description { get; set; }
                public DateTime Date { get; set; }
                public IEntity Creditor { get; set; }
                public IEntity Debitor { get; set; }
                public TransactionType TransactionType { get; set; }
                public Guid TransactionGuid { get; set; }
                public int EmployeeId { get; set; }
                public ICurrency Currency{ get; set; }

                public override int GetHashCode()
                {
                    return -1 & Hash(Amount) &
                        (IsIncoming ? 1 : 0) & Date.GetHashCode() &
                        Hash(Creditor) & Hash(Debitor) & TransactionType.GetHashCode() & TransactionGuid.GetHashCode();
                }

                public override bool Equals(object obj)
                {
                    if(obj is ITransaction t)
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
            }

            public TransactionBuilder()
            {
                t = new TransactionImpl();
            }

            public TransactionBuilder SetAmount(decimal amt)
            {
                t.Amount = amt;
                return this;
            }

            public TransactionBuilder SetIsIncoming(bool isIncoming)
            {
                t.IsIncoming = isIncoming;
                return this;
            }

            public TransactionBuilder SetDescription(string desc)
            {
                t.Description = String.IsNullOrWhiteSpace(desc) || String.IsNullOrEmpty(desc) ? null : desc.Trim();
                return this;
            }

            public TransactionBuilder SetDate(DateTime date)
            {
                t.Date = date;
                return this;
            }

            public TransactionBuilder SetCreditor(IEntity creditor)
            {
                t.Creditor = creditor;
                return this;
            }

            public TransactionBuilder SetDebitor(IEntity debitor)
            {
                t.Debitor = debitor;
                return this;
            }

            public TransactionBuilder SetTransactionType(TransactionType type)
            {
                t.TransactionType = type;
                return this;
            }

            public TransactionBuilder SetTransactionGuid(Guid guid)
            {
                t.TransactionGuid = guid;
                return this;
            }

            public TransactionBuilder SetEmployeeId(int id)
            {
                t.EmployeeId = id;
                return this;
            }

            public TransactionBuilder SetCurrency(ICurrency c)
            {
                t.Currency = c;
                return this;
            }

            public ITransaction Build()
            {
                if (IsCompletelyNull(new double[] { (double)t.Amount }, new bool[] { t.IsIncoming }, t.Creditor, t.Date, t.Debitor, t.Description, t.TransactionType))
                    return null;
                return t;
            }

            public void Clear()
            {
                t = new TransactionImpl();
            }

            public TransactionBuilder ReBuild(ITransaction model)
            {
                t.Amount = model.Amount;
                t.Creditor = model.Creditor;
                t.Date = model.Date;
                t.Debitor = model.Debitor;
                t.Description = model.Description;
                t.IsIncoming = model.IsIncoming;
                t.TransactionType = model.TransactionType;

                return this;
            }

            private TransactionImpl t;
        }

        public class EducationBuilder : IBuilder<IEducation, EducationBuilder>
        {
            private class EducationImpl : IEducation
            {
                public EducationImpl()
                {
                    Others = new List<IQualification>();
                }
                public IQualification Primary { get; set; }

                public IQualification Secondary { get; set; }

                public IQualification PrimaryTertiary { get; set; }

                public IEnumerable<IQualification> Others { get; set; }

                public override int GetHashCode()
                {
                    return -1 & Hash(Primary) & Hash(Secondary) & Hash(PrimaryTertiary) & CombineHash(Others);
                }

                public override bool Equals(object obj)
                {
                    if(obj is IEducation e)
                    {
                        return IsEqual(Primary, e.Primary)
                            && IsEqual(Secondary, e.Secondary)
                            && IsEqual(PrimaryTertiary, e.PrimaryTertiary)
                            && IsEqual(Others, e.Others);
                    }
                    return false;
                }
            }

            public EducationBuilder()
            {
                e = new EducationImpl();
            }

            public EducationBuilder SetPrimary(IQualification pri)
            {
                e.Primary = pri;
                return this;
            }

            public EducationBuilder SetSecondary(IQualification sec)
            {
                e.Secondary = sec;
                return this;
            }

            public EducationBuilder SetPrimaryTertiary(IQualification uni)
            {
                e.PrimaryTertiary = uni;
                return this;
            }

            public EducationBuilder AddQualification(IQualification credential)
            {
                ((List<IQualification>)e.Others).Add(credential);
                return this;
            }

            public EducationBuilder AddQualifications(IEnumerable<IQualification> credentials)
            {
                e.Others = credentials;
                return this;
            }

            public IEducation Build()
            {
                if (IsCompletelyNull(Array.Empty<double>(), Array.Empty<bool>(), e.Others, e.Primary, e.PrimaryTertiary, e.Secondary))
                    return null;
                return e;
            }

            public void Clear()
            {
                e = new EducationImpl();
            }

            public EducationBuilder ReBuild(IEducation model)
            {
                e.Others = model.Others;
                e.Primary = model.Primary;
                e.PrimaryTertiary = model.PrimaryTertiary;
                e.Secondary = model.Secondary;

                return this;
            }

            private EducationImpl e;
        }

        public class QualificationBuilder : IBuilder<IQualification, QualificationBuilder>
        {
            private class QualificationImpl : IQualification
            {
                public QualificationImpl()
                {
                    Grades = new Dictionary<string, string>();
                }
                public IDictionary<string, string> Grades { get; set; }

                public string Certification { get; set; }

                public IAddress Academy { get; set; }

                public override int GetHashCode()
                {
                    return -1 & Hash(Academy) & Hash(Certification) & CombineHash(Grades);
                }

                public override bool Equals(object obj)
                {
                    if(obj is IQualification q)
                    {
                        foreach(var grade in Grades)
                        {
                            if (!q.Grades.Contains(grade)) return false;
                        }

                        return (Certification == null ? q.Certification == null : Certification.CompareTo(q.Certification) == 0)
                            && IsEqual(Academy, q.Academy);
                    }
                    return false;
                }
            }

            public QualificationBuilder()
            {
                q = new QualificationImpl();
            }

            public QualificationBuilder SetCertification(string cert)
            {
                q.Certification = String.IsNullOrEmpty(cert) || String.IsNullOrWhiteSpace(cert) ? null : cert.Trim();
                return this;
            }

            public QualificationBuilder SetAcademy(IAddress school)
            {
                q.Academy = school;
                return this;
            }

            public QualificationBuilder AddGrade(string subject, string grading)
            {
                q.Grades.Add(subject, grading);
                return this;
            }

            public QualificationBuilder AddGrades(IDictionary<string, string> grades)
            {
                q.Grades = grades;
                return this;
            }

            public IQualification Build()
            {
                if (IsCompletelyNull(Array.Empty<double>(), Array.Empty<bool>(), q.Academy, q.Certification, q.Grades))
                    return null;
                return q;
            }

            public void Clear()
            {
                q = new QualificationImpl();
            }

            public QualificationBuilder ReBuild(IQualification model)
            {
                q.Academy = model.Academy;
                q.Certification = model.Certification;
                q.Grades = model.Grades;

                return this;
            }

            private QualificationImpl q;
        }
    
        public class CardBuilder : IBuilder<ICard, CardBuilder>
        {
            private class CardImpl : ICard
            {
                public ILoginCredentials Secret { get; set; }

                public IEntity Brand { get; set; }

                public INationality CountryOfUse { get; set; }

                public ICurrency Currency { get; set; }

                public CardType Type { get; set; }

                public DateTime IssuedDate { get; set; }

                public DateTime ExpiryDate { get; set; }

                public decimal IssuedCost { get; set; }

                public decimal MonthlyRate { get; set; }

                public decimal WithdrawalLimit { get; set; }

                public override int GetHashCode()
                {
                    return -1 & Hash(Secret) & Hash(Brand) & Hash(CountryOfUse) & Hash(Currency) & Hash(Type) &
                        Hash(IssuedDate) & Hash(ExpiryDate);
                }

                public override bool Equals(object obj)
                {
                    if (obj is ICard c)
                        return IsEqual(Secret, c.Secret) && IsEqual(Brand, c.Brand) && IsEqual(CountryOfUse, c.CountryOfUse)
                            && IsEqual(Currency, c.Currency) && IsEqual(Type, c.Type) && IsEqual(IssuedDate, c.IssuedDate)
                            && IsEqual(ExpiryDate, c.ExpiryDate);
                    return false;
                }
            }

            public CardBuilder()
            {
                c = new CardImpl();
            }

            public CardBuilder SetSecret(ILoginCredentials secret)
            {
                c.Secret = secret;
                return this;
            }

            public CardBuilder SetBrand(IEntity vendor)
            {
                c.Brand = vendor;
                return this;
            }

            public CardBuilder SetCountryOfUse(INationality cou)
            {
                c.CountryOfUse = cou;
                return this;
            }

            public CardBuilder SetCurrency(ICurrency currency)
            {
                c.Currency = currency;
                return this;
            }

            public CardBuilder SetType(CardType ct)
            {
                c.Type = ct;
                return this;
            }

            public CardBuilder SetIssuedDate(DateTime d)
            {
                c.IssuedDate = d;
                return this;
            }

            public CardBuilder SetExpiry(DateTime d)
            {
                c.ExpiryDate = d;
                return this;
            }

            public CardBuilder SetIssuedCost(decimal ic)
            {
                c.IssuedCost = ic;
                return this;
            }

            public CardBuilder SetMonthlyRate(decimal rate)
            {
                c.MonthlyRate = rate;
                return this;
            }

            public CardBuilder SetWithdrawalLimit(decimal limit)
            {
                c.WithdrawalLimit = limit;
                return this;
            }

            public ICard Build()
            {
                double[] doubles = { (double)c.IssuedCost, (double)c.MonthlyRate, (double)c.WithdrawalLimit };
                if (IsCompletelyNull(doubles, Array.Empty<bool>(), c.Brand, c.CountryOfUse, c.Currency,
                    c.ExpiryDate, c.IssuedDate, c.Secret, c.Type))
                    return null;
                return c;
            }

            public void Clear()
            {
                c = new CardImpl();
            }

            public CardBuilder ReBuild(ICard model)
            {
                c.Brand = model.Brand;
                c.CountryOfUse = model.CountryOfUse;
                c.Currency = model.Currency;
                c.ExpiryDate = model.ExpiryDate;
                c.IssuedCost = model.IssuedCost;
                c.IssuedDate = model.IssuedDate;
                c.MonthlyRate = model.MonthlyRate;
                c.Secret = model.Secret;
                c.Type = model.Type;
                c.WithdrawalLimit = model.WithdrawalLimit;

                return this;
            }

            private CardImpl c;
        }
    }
}
