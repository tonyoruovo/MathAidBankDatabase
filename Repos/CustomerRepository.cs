
using DataBaseTest.Data;
using DataBaseTest.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataBaseTest.Repos.Builders;
using static DataBaseTest.Repos.EmployeeRepository;

namespace DataBaseTest.Repos
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly BankDbContext2 _context;
        public CustomerRepository(BankDbContext2 context)
        {
            _context = context;
        }

        ~CustomerRepository()
        {
            //new Action(async () => await _context.DisposeAsync()).Invoke();
            _context.Dispose();
        }

        //GETs
        public async Task<IEnumerable<ICustomer>> GetCustomers()
        {
            var res = await _context.Customers
                .Include(x => x.Person)
                .ThenInclude(x => x.Entity)
                .Include(x => x.Accounts)
                .Select(cus => Maps.Map(cus)).ToListAsync();
            return res;
        }

        public async Task<ICustomer> GetCustomer(int? id)
        {
            var x = await _context.Customers.FindAsync(id);
            if (x == null) return null;
            await _context.Entry(x)
                .Reference(x => x.Person)
                .Query()
                .Include(x => x.CountryOfOrigin)
                .ThenInclude(x => x.CountryName)
                .Include(x => x.Entity)
                .ThenInclude(x => x.Contact)
                .ThenInclude(x => x.Address)
                .ThenInclude(x => x.CountryOfResidence)
                .ThenInclude(x => x.CountryName)
                .Include(x => x.OfficialEntity)
                .ThenInclude(x => x.Contact)
                .ThenInclude(x => x.Address)
                .ThenInclude(x => x.CountryOfResidence)
                .ThenInclude(x => x.CountryName)
                .Include(x => x.FullName)
                .ThenInclude(x => x.Name)
                .LoadAsync();
            await _context.Entry(x)
                .Reference(x => x.Person)
                .Query()
                .Include(x => x.CountryOfOrigin)
                .ThenInclude(x => x.State)
                .Include(x => x.Entity)
                .ThenInclude(x => x.Name)
                .Include(x => x.OfficialEntity)
                .ThenInclude(x => x.Name)
                .Include(x => x.FullName)
                .ThenInclude(x => x.Surname)
                .LoadAsync();
            await _context.Entry(x)
                .Reference(x => x.Person)
                .Query()
                .Include(x => x.CountryOfOrigin)
                .ThenInclude(x => x.CityTown)
                .Include(x => x.Entity)
                .ThenInclude(x => x.Contact)
                .ThenInclude(x => x.Address)
                .ThenInclude(x => x.CountryOfResidence)
                .ThenInclude(x => x.State)
                .Include(x => x.OfficialEntity)
                .ThenInclude(x => x.Contact)
                .ThenInclude(x => x.Address)
                .ThenInclude(x => x.CountryOfResidence)
                .ThenInclude(x => x.State)
                .Include(x => x.FullName)
                .ThenInclude(x => x.MaidenName)
                .LoadAsync();
            await _context.Entry(x)
                .Reference(x => x.Person)
                .Query()
                .Include(x => x.Entity)
                .ThenInclude(x => x.Contact)
                .ThenInclude(x => x.Address)
                .ThenInclude(x => x.CountryOfResidence)
                .ThenInclude(x => x.CityTown)
                .Include(x => x.OfficialEntity)
                .ThenInclude(x => x.Contact)
                .ThenInclude(x => x.Address)
                .ThenInclude(x => x.CountryOfResidence)
                .ThenInclude(x => x.CityTown)
                .Include(x => x.FullName)
                .ThenInclude(x => x.OtherNames)
                .LoadAsync();

            return Maps.Map(x);
        }
        
        public async Task<IEnumerable<IAccount>> GetAccounts(int? id)
        {
            var x = await _context.Customers.FindAsync(id);
            if (x == null) return null;
            await _context.Entry(x)
                .Collection(x => x.Accounts)
                .LoadAsync();

            return Maps.Map(x).Accounts;
        }
        
        public async Task<IAccount> GetAccount(int? id, string accountNumber)
        {
            var x = await _context.Customers.FindAsync(id);
            if (x == null) return null;
            await _context.Entry(x)
                .Collection(x => x.Accounts)
                .Query()
                .Where(a => a.Number.CompareTo(accountNumber) == 0)
                .Include(x => x.Transactions)
                .ThenInclude(x => x.Creditor)
                .ThenInclude(x => x.Name)
                .LoadAsync();
            await _context.Entry(x)
                .Collection(x => x.Accounts)
                .Query()
                .Include(x => x.Transactions)
                .ThenInclude(x => x.Creditor)
                .ThenInclude(x => x.Contact)
                .ThenInclude(x => x.Address)
                .LoadAsync();
            await _context.Entry(x)
                .Collection(x => x.Accounts)
                .Query()
                .Include(x => x.Transactions)
                .ThenInclude(x => x.Debitor)
                .ThenInclude(x => x.Name)
                .LoadAsync();
            await _context.Entry(x)
                .Collection(x => x.Accounts)
                .Query()
                .Include(x => x.Transactions)
                .ThenInclude(x => x.Debitor)
                .ThenInclude(x => x.Contact)
                .ThenInclude(x => x.Address)
                .LoadAsync();

            return Maps.Map(x).Accounts.FirstOrDefault();
        }
        
        public async Task<IEnumerable<ICard>> GetCards(int? id, string accountNumber)
        {
            var x = await _context.Customers.FindAsync(id);
            if (x == null) return null;
            await _context.Entry(x)
                .Collection(x => x.Accounts)
                .Query()
                .Where(x => x.Number.CompareTo(accountNumber) == 0)
                .Include(x => x.Cards)
                .LoadAsync();

            return Maps.Map(x).Accounts.FirstOrDefault().Cards;
        }
        
        public async Task<ICard> GetCard(int? id, string accountNumber, ILoginCredentials cardCredential)
        {
            var x = await _context.Customers.FindAsync(id);
            if (x == null) return null;
            await _context.Entry(x)
                .Collection(x => x.Accounts)
                .Query()
                .Where(x => x.Number.CompareTo(accountNumber) == 0)
                .Include(x => x.Cards.Where(c => c.Secret.PersonalOnlineKey.CompareTo(cardCredential.PersonalOnlineKey) == 0))
                .LoadAsync();
            return Maps.Map(x).Accounts.FirstOrDefault().Cards.FirstOrDefault();
        }

        public async Task<IName> GetName(int? id)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.FullName)
                    .ThenInclude(x => x.Name)
                    .LoadAsync();
                return Maps.Map(x.Person.FullName.Name);
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public async Task<IName> GetSurname(int? id)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.FullName)
                    .ThenInclude(x => x.Surname)
                    .LoadAsync();
                return Maps.Map(x.Person.FullName.Surname);
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public async Task<IName> GetNickname(int? id)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.FullName)
                    .ThenInclude(x => x.Nickname)
                    .LoadAsync();
                return Maps.Map(x.Person.FullName.Nickname);
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public async Task<IName> GetMaidenName(int? id)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.FullName)
                    .ThenInclude(x => x.MaidenName)
                    .LoadAsync();
                return Maps.Map(x.Person.FullName.MaidenName);
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<IEnumerable<IName>> GetOtherNames(int? id)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.FullName)
                    .LoadAsync();
                //foreach (var name in x.Person.FullName.OtherNames)
                //{
                //    await _context.Entry(name)
                //        .Reference(x => x.Name)
                //        .LoadAsync();
                //}
                return x.Person.FullName.OtherNames.Select(x => Maps.Map(x));
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public async Task<IEnumerable<IName>> GetTitles(int? id)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                //await _context.Entry(x)
                //    .Reference(x => x.Person)
                //    .Query()
                //    .Include(x => x.FullName)
                //    .ThenInclude(x => x.OtherNames)
                //    .LoadAsync();
                var titles = x.Person.FullName.Title.Split(",");
                return titles.Select(x => new NameBuilder().SetName(x).Build());
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public async Task<IEnumerable<ITransaction>> GetTransactions(int? id, string accountNumber)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Transactions).Query().Include(x => x.Creditor).ThenInclude(x => x.Name).Include(x => x.Debitor).ThenInclude(x => x.Name)
                    .LoadAsync();
                await _context.Entry(a).Collection(x => x.Transactions).Query()
                    .Include(x => x.Creditor).ThenInclude(x => x.Contact).ThenInclude(x => x.Address).ThenInclude(x => x.CountryOfResidence).ThenInclude(x => x.CountryName)
                    .Include(x => x.Debitor).ThenInclude(x => x.Contact).ThenInclude(x => x.Address).ThenInclude(x => x.CountryOfResidence).ThenInclude(x => x.CountryName)
                    .LoadAsync();
                await _context.Entry(a).Collection(x => x.Transactions).Query()
                    .Include(x => x.Creditor).ThenInclude(x => x.Contact).ThenInclude(x => x.Address).ThenInclude(x => x.CountryOfResidence).ThenInclude(x => x.State)
                    .Include(x => x.Debitor).ThenInclude(x => x.Contact).ThenInclude(x => x.Address).ThenInclude(x => x.CountryOfResidence).ThenInclude(x => x.State)
                    .LoadAsync();
                await _context.Entry(a).Collection(x => x.Transactions).Query()
                    .Include(x => x.Creditor).ThenInclude(x => x.Contact).ThenInclude(x => x.Address).ThenInclude(x => x.CountryOfResidence).ThenInclude(x => x.CityTown)
                    .Include(x => x.Debitor).ThenInclude(x => x.Contact).ThenInclude(x => x.Address).ThenInclude(x => x.CountryOfResidence).ThenInclude(x => x.CityTown)
                    .LoadAsync();

                return a.Transactions.Select(t => Maps.Map(t));
            }
            catch (NullReferenceException)
            {
                return null;
            }
            catch (InvalidOperationException) { return null; }
        }

        public async Task<ITransaction> GetTransaction(int? id, string accountNumber, Guid transactionId)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                if (a == default) return null;
                //await _context.Entry(a).Collection(x => x.Transactions).Query().Include(x => x.Creditor).ThenInclude(x => x.Name).Include(x => x.Debitor).ThenInclude(x => x.Name)
                //    .LoadAsync();
                //await _context.Entry(a).Collection(x => x.Transactions).Query()
                //    .Include(x => x.Creditor).ThenInclude(x => x.Contact).ThenInclude(x => x.Address).ThenInclude(x => x.CountryOfResidence).ThenInclude(x => x.CountryName)
                //    .Include(x => x.Debitor).ThenInclude(x => x.Contact).ThenInclude(x => x.Address).ThenInclude(x => x.CountryOfResidence).ThenInclude(x => x.CountryName)
                //    .LoadAsync();
                //await _context.Entry(a).Collection(x => x.Transactions).Query()
                //    .Include(x => x.Creditor).ThenInclude(x => x.Contact).ThenInclude(x => x.Address).ThenInclude(x => x.CountryOfResidence).ThenInclude(x => x.State)
                //    .Include(x => x.Debitor).ThenInclude(x => x.Contact).ThenInclude(x => x.Address).ThenInclude(x => x.CountryOfResidence).ThenInclude(x => x.State)
                //    .LoadAsync();
                //await _context.Entry(a).Collection(x => x.Transactions).Query()
                //    .Include(x => x.Creditor).ThenInclude(x => x.Contact).ThenInclude(x => x.Address).ThenInclude(x => x.CountryOfResidence).ThenInclude(x => x.CityTown)
                //    .Include(x => x.Debitor).ThenInclude(x => x.Contact).ThenInclude(x => x.Address).ThenInclude(x => x.CountryOfResidence).ThenInclude(x => x.CityTown)
                //    .LoadAsync();
                var t = await _context.Transactions.SingleOrDefaultAsync(tr => tr.TransactionGuid == transactionId);
                await _context.Entry(t).Reference(x => x.Debitor).Query().Include(x => x.Name).LoadAsync();
                await _context.Entry(t).Reference(x => x.Debitor).Query().Include(x => x.Contact).ThenInclude(x => x.Address).ThenInclude(x => x.CountryOfResidence).ThenInclude(x => x.CountryName).LoadAsync();
                await _context.Entry(t).Reference(x => x.Debitor).Query().Include(x => x.Contact).ThenInclude(x => x.Address).ThenInclude(x => x.CountryOfResidence).ThenInclude(x => x.State).LoadAsync();
                await _context.Entry(t).Reference(x => x.Debitor).Query().Include(x => x.Contact).ThenInclude(x => x.Address).ThenInclude(x => x.CountryOfResidence).ThenInclude(x => x.CityTown).LoadAsync();
                await _context.Entry(t).Reference(x => x.Creditor).Query().Include(x => x.Name).LoadAsync();
                await _context.Entry(t).Reference(x => x.Creditor).Query().Include(x => x.Contact).ThenInclude(x => x.Address).ThenInclude(x => x.CountryOfResidence).ThenInclude(x => x.CountryName).LoadAsync();
                await _context.Entry(t).Reference(x => x.Creditor).Query().Include(x => x.Contact).ThenInclude(x => x.Address).ThenInclude(x => x.CountryOfResidence).ThenInclude(x => x.State).LoadAsync();
                await _context.Entry(t).Reference(x => x.Creditor).Query().Include(x => x.Contact).ThenInclude(x => x.Address).ThenInclude(x => x.CountryOfResidence).ThenInclude(x => x.CityTown).LoadAsync();

                return Maps.Map(t);
            }
            catch (NullReferenceException)
            {
                return null;
            }
            catch (InvalidOperationException) { return null; }
        }

        public async Task<IPerson> GetGuarantor(int? id, string accountNumber)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Reference(x => x.Guarantor).Query().Include(x => x.OfficialEntity).ThenInclude(x => x.Name).Include(x => x.Entity).ThenInclude(x => x.Name)
                    .LoadAsync();
                await _context.Entry(a).Reference(x => x.Guarantor).Query()
                    .Include(x => x.OfficialEntity).ThenInclude(x => x.Contact).ThenInclude(x => x.Address).ThenInclude(x => x.CountryOfResidence).ThenInclude(x => x.CountryName)
                    .Include(x => x.Entity).ThenInclude(x => x.Contact).ThenInclude(x => x.Address).ThenInclude(x => x.CountryOfResidence).ThenInclude(x => x.CountryName)
                    .LoadAsync();
                await _context.Entry(a).Reference(x => x.Guarantor).Query()
                    .Include(x => x.OfficialEntity).ThenInclude(x => x.Contact).ThenInclude(x => x.Address).ThenInclude(x => x.CountryOfResidence).ThenInclude(x => x.State)
                    .Include(x => x.Entity).ThenInclude(x => x.Contact).ThenInclude(x => x.Address).ThenInclude(x => x.CountryOfResidence).ThenInclude(x => x.State)
                    .LoadAsync();
                await _context.Entry(a).Reference(x => x.Guarantor).Query()
                    .Include(x => x.OfficialEntity).ThenInclude(x => x.Contact).ThenInclude(x => x.Address).ThenInclude(x => x.CountryOfResidence).ThenInclude(x => x.CityTown)
                    .Include(x => x.Entity).ThenInclude(x => x.Contact).ThenInclude(x => x.Address).ThenInclude(x => x.CountryOfResidence).ThenInclude(x => x.CityTown)
                    .LoadAsync();
                await _context.Entry(a).Reference(x => x.Guarantor).Query().Include(x => x.FullName).ThenInclude(x => x.Name).LoadAsync();
                await _context.Entry(a).Reference(x => x.Guarantor).Query().Include(x => x.FullName).ThenInclude(x => x.Surname).LoadAsync();
                await _context.Entry(a).Reference(x => x.Guarantor).Query().Include(x => x.FullName).ThenInclude(x => x.Nickname).LoadAsync();
                await _context.Entry(a).Reference(x => x.Guarantor).Query().Include(x => x.FullName).ThenInclude(x => x.MaidenName).LoadAsync();

                return Maps.Map(a.Guarantor);
            }
            catch (NullReferenceException)
            {
                return null;
            }
            catch (InvalidOperationException) { return null; }
        }

        public async Task<string> GetPersonalOnlineKey(int? id, string accountNumber, ILoginCredentials password)
        {
            var x = await _context.Customers.FindAsync(id);
            if (x == default) return null;
            try
            {
                var a = await _context.Accounts.SingleOrDefaultAsync(x => x.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret).LoadAsync();
                return Maps.Map(a.Cards.Where(c => Decode(Maps.Map(c.Secret), password).Result).SingleOrDefault()).Secret.PersonalOnlineKey;
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public async Task<string> GetUsername(int? id, string accountNumber, ILoginCredentials password)
        {
            var x = await _context.Customers.FindAsync(id);
            if (x == default) return null;
            try
            {
                var a = await _context.Accounts.SingleOrDefaultAsync(x => x.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret).LoadAsync();
                return Maps.Map(a.Cards.Where(c => Decode(Maps.Map(c.Secret), password).Result).SingleOrDefault()).Secret.Username;
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public async Task<string> GetPassword(int? id, string accountNumber, ILoginCredentials password)
        {
            var x = await _context.Customers.FindAsync(id);
            if (x == default) return null;
            try
            {
                var a = await _context.Accounts.SingleOrDefaultAsync(x => x.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret).LoadAsync();
                var refer = Maps.Map(a.Cards.Where(c => Decode(Maps.Map(c.Secret), password).Result).SingleOrDefault()).Secret.Password.ToCharArray();
                return await Utilities.Decrypt(refer);
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        async Task<bool> Decode(ILoginCredentials p1, ILoginCredentials p2)
        {
            var isSameKey_Name = p1.PersonalOnlineKey.CompareTo(p2.PersonalOnlineKey) == 0 &&
                p1.Username.CompareTo(p2.Username) == 0;
            if (!isSameKey_Name) return false;
            var refer = p1.Password.ToCharArray();
            var decoded = await Utilities.Decrypt(refer);
            return decoded.CompareTo(p2.Password) == 0;
        }

        bool Decode2(ILoginCredentials p1, ILoginCredentials p2)
        {
            var isSameKey_Name = p1.PersonalOnlineKey.CompareTo(p2.PersonalOnlineKey) == 0 &&
                p1.Username.CompareTo(p2.Username) == 0;
            if (!isSameKey_Name) return false;
            var refer = Encoding.UTF8.GetString(Convert.FromBase64String(p1.Password));
            var decoded = Utilities.Decrypt(ref refer);
            return decoded.CompareTo(p2.Password) == 0;
        }

        public async Task<IName> GetBrandName(int? id, string accountNumber, ILoginCredentials password)
        {
            var x = await _context.Customers.FindAsync(id);
            if (x == default) return null;
            try
            {
                var a = await _context.Accounts.SingleOrDefaultAsync(x => x.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .Include(c => c.Brand)
                    .ThenInclude(e => e.Name)
                    .LoadAsync();
                return Maps.Map(a.Cards.Where(c => Decode(Maps.Map(c.Secret), password).Result).SingleOrDefault()).Brand.Name;
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public async Task<string> GetBrandPhone(int? id, string accountNumber, ILoginCredentials password)
        {
            var x = await _context.Customers.FindAsync(id);
            if (x == default) return null;
            try
            {
                var a = await _context.Accounts.SingleOrDefaultAsync(x => x.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .Include(c => c.Brand)
                    .ThenInclude(e => e.Contact)
                    .LoadAsync();
                return Maps.Map(a.Cards.Where(c => Decode(Maps.Map(c.Secret), password).Result).SingleOrDefault()).Brand.Contact.Mobiles.FirstOrDefault();
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public async Task<string> GetBrandEmail(int? id, string accountNumber, ILoginCredentials password)
        {
            var x = await _context.Customers.FindAsync(id);
            if (x == default) return null;
            try
            {
                var a = await _context.Accounts.SingleOrDefaultAsync(x => x.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .Include(c => c.Brand)
                    .ThenInclude(e => e.Contact)
                    .LoadAsync();
                return Maps.Map(a.Cards.Where(c => Decode(Maps.Map(c.Secret), password).Result).SingleOrDefault()).Brand.Contact.Emails.FirstOrDefault();
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public async Task<string> GetBrandSocialMedia(int? id, string accountNumber, ILoginCredentials password)
        {
            var x = await _context.Customers.FindAsync(id);
            if (x == default) return null;
            try
            {
                var a = await _context.Accounts.SingleOrDefaultAsync(x => x.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .Include(c => c.Brand)
                    .ThenInclude(e => e.Contact)
                    .LoadAsync();
                return Maps.Map(a.Cards.Where(c => Decode(Maps.Map(c.Secret), password).Result).SingleOrDefault()).Brand.Contact.SocialMedia.FirstOrDefault();
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public async Task<string> GetBrandWebsite(int? id, string accountNumber, ILoginCredentials password)
        {
            var x = await _context.Customers.FindAsync(id);
            if (x == default) return null;
            try
            {
                var a = await _context.Accounts.SingleOrDefaultAsync(x => x.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .Include(c => c.Brand)
                    .ThenInclude(e => e.Contact)
                    .LoadAsync();
                return Maps.Map(a.Cards.Where(c => Decode(Maps.Map(c.Secret), password).Result).SingleOrDefault()).Brand.Contact.Websites.FirstOrDefault();
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public async Task<string> GetBrandStreetAddress(int? id, string accountNumber, ILoginCredentials password)
        {
            var x = await _context.Customers.FindAsync(id);
            if (x == default) return null;
            try
            {
                var a = await _context.Accounts.SingleOrDefaultAsync(x => x.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .Include(c => c.Brand)
                    .ThenInclude(e => e.Contact)
                    .ThenInclude(e => e.Address)
                    .LoadAsync();
                return Maps.Map(a.Cards.Where(c => Decode(Maps.Map(c.Secret), password).Result).SingleOrDefault()).Brand.Contact.Address.StreetAddress;
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public async Task<string> GetBrandZIPCode(int? id, string accountNumber, ILoginCredentials password)
        {
            var x = await _context.Customers.FindAsync(id);
            if (x == default) return null;
            try
            {
                var a = await _context.Accounts.SingleOrDefaultAsync(x => x.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .Include(c => c.Brand)
                    .ThenInclude(e => e.Contact)
                    .ThenInclude(e => e.Address)
                    .LoadAsync();
                return Maps.Map(a.Cards.Where(c => Decode(Maps.Map(c.Secret), password).Result).SingleOrDefault()).Brand.Contact.Address.ZIPCode;
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public async Task<string> GetBrandPMB(int? id, string accountNumber, ILoginCredentials password)
        {
            var x = await _context.Customers.FindAsync(id);
            if (x == default) return null;
            try
            {
                var a = await _context.Accounts.SingleOrDefaultAsync(x => x.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .Include(c => c.Brand)
                    .ThenInclude(e => e.Contact)
                    .ThenInclude(e => e.Address)
                    .LoadAsync();
                return Maps.Map(a.Cards.Where(c => Decode(Maps.Map(c.Secret), password).Result).SingleOrDefault()).Brand.Contact.Address.PMB;
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public async Task<IName> GetBrandCountryOfResidence(int? id, string accountNumber, ILoginCredentials password)
        {
            var x = await _context.Customers.FindAsync(id);
            if (x == default) return null;
            try
            {
                var a = await _context.Accounts.SingleOrDefaultAsync(x => x.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .Include(c => c.Brand)
                    .ThenInclude(e => e.Contact)
                    .ThenInclude(e => e.Address)
                    .ThenInclude(e => e.CountryOfResidence)
                    .ThenInclude(e => e.CountryName)
                    .LoadAsync();
                return Maps.Map(a.Cards.Where(c => Decode(Maps.Map(c.Secret), password).Result).SingleOrDefault()).Brand.Contact.Address.CountryOfResidence.CountryName;
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public async Task<IName> GetBrandStateOfResidence(int? id, string accountNumber, ILoginCredentials password)
        {
            var x = await _context.Customers.FindAsync(id);
            if (x == default) return null;
            try
            {
                var a = await _context.Accounts.SingleOrDefaultAsync(x => x.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .Include(c => c.Brand)
                    .ThenInclude(e => e.Contact)
                    .ThenInclude(e => e.Address)
                    .ThenInclude(e => e.CountryOfResidence)
                    .ThenInclude(e => e.State)
                    .LoadAsync();
                return Maps.Map(a.Cards.Where(c => Decode(Maps.Map(c.Secret), password).Result).SingleOrDefault()).Brand.Contact.Address.CountryOfResidence.State;
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public async Task<string> GetBrandLGAOfResidence(int? id, string accountNumber, ILoginCredentials password)
        {
            var x = await _context.Customers.FindAsync(id);
            if (x == default) return null;
            try
            {
                var a = await _context.Accounts.SingleOrDefaultAsync(x => x.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .Include(c => c.Brand)
                    .ThenInclude(e => e.Contact)
                    .ThenInclude(e => e.Address)
                    .ThenInclude(e => e.CountryOfResidence)
                    .LoadAsync();
                return Maps.Map(a.Cards.Where(c => Decode(Maps.Map(c.Secret), password).Result).SingleOrDefault()).Brand.Contact.Address.CountryOfResidence.LGA;
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public async Task<string> GetBrandLanguage(int? id, string accountNumber, ILoginCredentials password)
        {
            var x = await _context.Customers.FindAsync(id);
            if (x == default) return null;
            try
            {
                var a = await _context.Accounts.SingleOrDefaultAsync(x => x.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .Include(c => c.Brand)
                    .ThenInclude(e => e.Contact)
                    .ThenInclude(e => e.Address)
                    .ThenInclude(e => e.CountryOfResidence)
                    .LoadAsync();
                return Maps.Map(a.Cards.Where(c => Decode(Maps.Map(c.Secret), password).Result).SingleOrDefault()).Brand.Contact.Address.CountryOfResidence.Language;
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public async Task<IName> GetBrandCityOfResidence(int? id, string accountNumber, ILoginCredentials password)
        {
            var x = await _context.Customers.FindAsync(id);
            if (x == default) return null;
            try
            {
                var a = await _context.Accounts.SingleOrDefaultAsync(x => x.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .Include(c => c.Brand)
                    .ThenInclude(e => e.Contact)
                    .ThenInclude(e => e.Address)
                    .ThenInclude(e => e.CountryOfResidence)
                    .ThenInclude(e => e.CityTown)
                    .LoadAsync();
                return Maps.Map(a.Cards.Where(c => Decode(Maps.Map(c.Secret), password).Result).SingleOrDefault()).Brand.Contact.Address.CountryOfResidence.CityTown;
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public async Task<IName> GetBrandCountryOfUse(int? id, string accountNumber, ILoginCredentials password)
        {
            var x = await _context.Customers.FindAsync(id);
            if (x == default) return null;
            try
            {
                var a = await _context.Accounts.SingleOrDefaultAsync(x => x.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .Include(c => c.CountryOfUse)
                    .ThenInclude(e => e.CountryName)
                    .LoadAsync();
                return Maps.Map(a.Cards.Where(c => Decode(Maps.Map(c.Secret), password).Result).SingleOrDefault()).CountryOfUse.CountryName;
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public async Task<IName> GetBrandStateOfUse(int? id, string accountNumber, ILoginCredentials password)
        {
            var x = await _context.Customers.FindAsync(id);
            if (x == default) return null;
            try
            {
                var a = await _context.Accounts.SingleOrDefaultAsync(x => x.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .Include(c => c.CountryOfUse)
                    .ThenInclude(e => e.State)
                    .LoadAsync();
                return Maps.Map(a.Cards.Where(c => Decode(Maps.Map(c.Secret), password).Result).SingleOrDefault()).CountryOfUse.State;
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public async Task<string> GetBrandLGAOfUse(int? id, string accountNumber, ILoginCredentials password)
        {
            var x = await _context.Customers.FindAsync(id);
            if (x == default) return null;
            try
            {
                var a = await _context.Accounts.SingleOrDefaultAsync(x => x.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .Include(c => c.CountryOfUse)
                    .LoadAsync();
                return Maps.Map(a.Cards.Where(c => Decode(Maps.Map(c.Secret), password).Result).SingleOrDefault()).CountryOfUse.LGA;
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public async Task<string> GetBrandLanguageOfUse(int? id, string accountNumber, ILoginCredentials password)
        {
            var x = await _context.Customers.FindAsync(id);
            if (x == default) return null;
            try
            {
                var a = await _context.Accounts.SingleOrDefaultAsync(x => x.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .Include(c => c.CountryOfUse)
                    .LoadAsync();
                return Maps.Map(a.Cards.Where(c => Decode(Maps.Map(c.Secret), password).Result).SingleOrDefault()).CountryOfUse.Language;
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public async Task<IName> GetBrandCityTownOfUse(int? id, string accountNumber, ILoginCredentials password)
        {
            var x = await _context.Customers.FindAsync(id);
            if (x == default) return null;
            try
            {
                var a = await _context.Accounts.SingleOrDefaultAsync(x => x.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .Include(c => c.CountryOfUse)
                    .ThenInclude(e => e.CityTown)
                    .LoadAsync();
                return Maps.Map(a.Cards.Where(c => Decode(Maps.Map(c.Secret), password).Result).SingleOrDefault()).CountryOfUse.CityTown;
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public async Task<ICurrency> GetCurrencyOfUse(int? id, string accountNumber, ILoginCredentials password)
        {
            var x = await _context.Customers.FindAsync(id);
            if (x == default) return null;
            try
            {
                var a = await _context.Accounts.SingleOrDefaultAsync(x => x.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .LoadAsync();
                return Maps.Map(a.Cards.Where(c => Decode(Maps.Map(c.Secret), password).Result).SingleOrDefault()).Currency;
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public async Task<CardType> GetCardType(int? id, string accountNumber, ILoginCredentials password)
        {
            var x = await _context.Customers.FindAsync(id);
            if (x == default) return default;
            try
            {
                var a = await _context.Accounts.SingleOrDefaultAsync(x => x.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .LoadAsync();
                return Maps.Map(a.Cards.Where(c => Decode(Maps.Map(c.Secret), password).Result).SingleOrDefault()).Type;
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<DateTime> GetIssuedDate(int? id, string accountNumber, ILoginCredentials password)
        {
            var x = await _context.Customers.FindAsync(id);
            if (x == default) return default;
            try
            {
                var a = await _context.Accounts.SingleOrDefaultAsync(x => x.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .LoadAsync();
                return Maps.Map(a.Cards.Where(c => Decode(Maps.Map(c.Secret), password).Result).SingleOrDefault()).IssuedDate;
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<DateTime> GetExpiryDate(int? id, string accountNumber, ILoginCredentials password)
        {
            var x = await _context.Customers.FindAsync(id);
            if (x == default) return default;
            try
            {
                var a = await _context.Accounts.SingleOrDefaultAsync(x => x.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .LoadAsync();
                return Maps.Map(a.Cards.Where(c => Decode(Maps.Map(c.Secret), password).Result).SingleOrDefault()).ExpiryDate;
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<decimal> GetIssuedCost(int? id, string accountNumber, ILoginCredentials password)
        {
            var x = await _context.Customers.FindAsync(id);
            if (x == default) return default;
            try
            {
                var a = await _context.Accounts.SingleOrDefaultAsync(x => x.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .LoadAsync();
                return Maps.Map(a.Cards.Where(c => Decode(Maps.Map(c.Secret), password).Result).SingleOrDefault()).IssuedCost;
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<decimal> GetMonthlyRate(int? id, string accountNumber, ILoginCredentials password)
        {
            var x = await _context.Customers.FindAsync(id);
            if (x == default) return default;
            try
            {
                var a = await _context.Accounts.SingleOrDefaultAsync(x => x.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .LoadAsync();
                return Maps.Map(a.Cards.Where(c => Decode(Maps.Map(c.Secret), password).Result).SingleOrDefault()).MonthlyRate;
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<decimal> GetWithdrawalLimit(int? id, string accountNumber, ILoginCredentials password)
        {
            var x = await _context.Customers.FindAsync(id);
            if (x == default) return default;
            try
            {
                var a = await _context.Accounts.SingleOrDefaultAsync(x => x.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .LoadAsync();
                return Maps.Map(a.Cards.Where(c => Decode(Maps.Map(c.Secret), password).Result).SingleOrDefault()).WithdrawalLimit;
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<string> GetBVN(int? id)
        {
            var x = await _context.Customers.FindAsync(id);
            return x.BVN;
        }

        public async Task<DateTime> GetEntryDate(int? id)
        {
            var x = await _context.Customers.FindAsync(id);
            return x.EntryDate;
        }

        public async Task<string> GetPersonalOnlineKey(int? id, ILoginCredentials password)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                var login = await _context.LoginCredentials.SingleOrDefaultAsync(l => l.LoginCredentialModelId == x.LoginCredentialModelId);
                if (!Decode2(Maps.Map(login), password))
                    return null;
                return login.PersonalOnlineKey;
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public async Task<string> GetUsername(int? id, ILoginCredentials password)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                var login = await _context.LoginCredentials.SingleOrDefaultAsync(l => l.LoginCredentialModelId == x.LoginCredentialModelId);
                if (!Decode2(Maps.Map(login), password))
                    return null;
                return login.Username;
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public async Task<string> GetPassword(int? id, ILoginCredentials password)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                var login = await _context.LoginCredentials.SingleOrDefaultAsync(l => l.LoginCredentialModelId == x.LoginCredentialModelId);
                if (!Decode2(Maps.Map(login), password))
                    return null;
                var refer = login.Password;
                return Utilities.Decrypt(ref refer);
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public async Task<string> GetIdentification(int? id, string type)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .LoadAsync();
                return x.Person.Identification[type];
            }
            catch (NullReferenceException)
            {
                return default;
            }
            catch (KeyNotFoundException ex) { throw new ApplicationArgumentException("The id type provided does not exist", ex); }
        }

        public async Task<byte[]> GetFingerprint(int? id)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .LoadAsync();
                return x.Person.FingerPrint;
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<byte[]> GetSignature(int? id)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .LoadAsync();
                return x.Person.Signature;
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<byte[]> GetPassport(int? id)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .LoadAsync();
                return x.Person.Passport;
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<string> GetJobType(int? id)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .LoadAsync();
                return x.Person.JobType;
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<bool> GetSex(int? id)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .LoadAsync();
                return x.Person.IsMale;
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<Guid> GetUniqueTag(int? id)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .LoadAsync();
                return x.Person.UniqueTag;
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<DateTime> GetBirthDate(int? id)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .LoadAsync();
                return x.Person.BirthDate;
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<IName> GetCountryOfOrigin(int? id)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.CountryOfOrigin)
                    .ThenInclude( x=> x.CountryName)
                    .LoadAsync();
                return Maps.Map(x.Person.CountryOfOrigin.CountryName);
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<IName> GetStateOfOrigin(int? id)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.CountryOfOrigin)
                    .ThenInclude(x => x.State)
                    .LoadAsync();
                return Maps.Map(x.Person.CountryOfOrigin.State);
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<IName> GetTown_CityOfOrigin(int? id)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.CountryOfOrigin)
                    .ThenInclude(x => x.CityTown)
                    .LoadAsync();
                return Maps.Map(x.Person.CountryOfOrigin.CityTown);
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<string> GetLGAOfOrigin(int? id)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.CountryOfOrigin)
                    .LoadAsync();
                return x.Person.CountryOfOrigin.LGA;
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<string> GetLanguage(int? id)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.CountryOfOrigin)
                    .LoadAsync();
                return x.Person.CountryOfOrigin.Language;
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<IPerson> GetNextOfKin(int? id)
        {
            var a = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                //var a = await _context.Accounts
                //    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Reference(x => x.Person).Query().Include(x => x.OfficialEntity).ThenInclude(x => x.Name).Include(x => x.Entity).ThenInclude(x => x.Name)
                    .LoadAsync();
                await _context.Entry(a).Reference(x => x.Person).Query()
                    .Include(x => x.OfficialEntity).ThenInclude(x => x.Contact).ThenInclude(x => x.Address).ThenInclude(x => x.CountryOfResidence).ThenInclude(x => x.CountryName)
                    .Include(x => x.Entity).ThenInclude(x => x.Contact).ThenInclude(x => x.Address).ThenInclude(x => x.CountryOfResidence).ThenInclude(x => x.CountryName)
                    .LoadAsync();
                await _context.Entry(a).Reference(x => x.Person).Query()
                    .Include(x => x.OfficialEntity).ThenInclude(x => x.Contact).ThenInclude(x => x.Address).ThenInclude(x => x.CountryOfResidence).ThenInclude(x => x.State)
                    .Include(x => x.Entity).ThenInclude(x => x.Contact).ThenInclude(x => x.Address).ThenInclude(x => x.CountryOfResidence).ThenInclude(x => x.State)
                    .LoadAsync();
                await _context.Entry(a).Reference(x => x.Person).Query()
                    .Include(x => x.OfficialEntity).ThenInclude(x => x.Contact).ThenInclude(x => x.Address).ThenInclude(x => x.CountryOfResidence).ThenInclude(x => x.CityTown)
                    .Include(x => x.Entity).ThenInclude(x => x.Contact).ThenInclude(x => x.Address).ThenInclude(x => x.CountryOfResidence).ThenInclude(x => x.CityTown)
                    .LoadAsync();
                await _context.Entry(a).Reference(x => x.Person).Query().Include(x => x.FullName).ThenInclude(x => x.Name).LoadAsync();
                await _context.Entry(a).Reference(x => x.Person).Query().Include(x => x.FullName).ThenInclude(x => x.Surname).LoadAsync();
                await _context.Entry(a).Reference(x => x.Person).Query().Include(x => x.FullName).ThenInclude(x => x.Nickname).LoadAsync();
                await _context.Entry(a).Reference(x => x.Person).Query().Include(x => x.FullName).ThenInclude(x => x.MaidenName).LoadAsync();

                return Maps.Map(a.Person);
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public async Task<MaritalStatus> GetMaritalStatus(int? id)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .LoadAsync();
                return x.Person.MaritalStatus;
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<IName> GetBusinessName(int? id)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.OfficialEntity)
                    .ThenInclude(x => x.Name)
                    .LoadAsync();
                return Maps.Map(x.Person.OfficialEntity.Name);
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<IEnumerable<string>> GetBusinessEmails(int? id)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.OfficialEntity)
                    .ThenInclude(x => x.Contact)
                    .LoadAsync();
                return x.Person.OfficialEntity.Contact.Emails;
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<IEnumerable<string>> GetBusinessMobiles(int? id)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.OfficialEntity)
                    .ThenInclude(x => x.Contact)
                    .LoadAsync();
                return x.Person.OfficialEntity.Contact.Mobiles;
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<IEnumerable<string>> GetBusinessSocialMedia(int? id)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.OfficialEntity)
                    .ThenInclude(x => x.Contact)
                    .LoadAsync();
                return x.Person.OfficialEntity.Contact.SocialMedia;
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<IEnumerable<string>> GetBusinessWebsites(int? id)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.OfficialEntity)
                    .ThenInclude(x => x.Contact)
                    .LoadAsync();
                return x.Person.OfficialEntity.Contact.Websites;
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<string> GetBusinessStreetAddress(int? id)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.OfficialEntity)
                    .ThenInclude(x => x.Contact)
                    .ThenInclude(x => x.Address)
                    .LoadAsync();
                return x.Person.OfficialEntity.Contact.Address.StreetAddress;
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<string> GetBusinessZIPCode(int? id)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.OfficialEntity)
                    .ThenInclude(x => x.Contact)
                    .ThenInclude(x => x.Address)
                    .LoadAsync();
                return x.Person.OfficialEntity.Contact.Address.ZIPCode;
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<string> GetBusinessPMB(int? id)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.OfficialEntity)
                    .ThenInclude(x => x.Contact)
                    .ThenInclude(x => x.Address)
                    .LoadAsync();
                return x.Person.OfficialEntity.Contact.Address.PMB;
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<IName> GetBusinessCountryOfResidence(int? id)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.OfficialEntity)
                    .ThenInclude(x => x.Contact)
                    .ThenInclude(x => x.Address)
                    .ThenInclude(x => x.CountryOfResidence)
                    .ThenInclude(x => x.CountryName)
                    .LoadAsync();
                return Maps.Map(x.Person.OfficialEntity.Contact.Address.CountryOfResidence.CountryName);
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<IName> GetBusinessStateOfResidence(int? id)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.OfficialEntity)
                    .ThenInclude(x => x.Contact)
                    .ThenInclude(x => x.Address)
                    .ThenInclude(x => x.CountryOfResidence)
                    .ThenInclude(x => x.State)
                    .LoadAsync();
                return Maps.Map(x.Person.OfficialEntity.Contact.Address.CountryOfResidence.State);
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<IName> GetBusinessTown_CityOfResidence(int? id)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.OfficialEntity)
                    .ThenInclude(x => x.Contact)
                    .ThenInclude(x => x.Address)
                    .ThenInclude(x => x.CountryOfResidence)
                    .ThenInclude(x => x.CityTown)
                    .LoadAsync();
                return Maps.Map(x.Person.OfficialEntity.Contact.Address.CountryOfResidence.CityTown);
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<string> GetBusinessLGAOfResidence(int? id)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.OfficialEntity)
                    .ThenInclude(x => x.Contact)
                    .ThenInclude(x => x.Address)
                    .ThenInclude(x => x.CountryOfResidence)
                    .LoadAsync();
                return x.Person.OfficialEntity.Contact.Address.CountryOfResidence.LGA;
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<IName> GetDisplayName(int? id)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.Entity)
                    .ThenInclude(x => x.Name)
                    .LoadAsync();
                return Maps.Map(x.Person.Entity.Name);
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<IEnumerable<string>> GetEmails(int? id)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.Entity)
                    .ThenInclude(x => x.Contact)
                    .LoadAsync();
                return x.Person.Entity.Contact.Emails;
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<IEnumerable<string>> GetMobiles(int? id)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.Entity)
                    .ThenInclude(x => x.Contact)
                    .LoadAsync();
                return x.Person.Entity.Contact.Mobiles;
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<IEnumerable<string>> GetSocialMedia(int? id)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.Entity)
                    .ThenInclude(x => x.Contact)
                    .LoadAsync();
                return x.Person.Entity.Contact.SocialMedia;
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<IEnumerable<string>> GetWebsites(int? id)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.Entity)
                    .ThenInclude(x => x.Contact)
                    .LoadAsync();
                return x.Person.Entity.Contact.Websites;
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<string> GetStreetAddress(int? id)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.Entity)
                    .ThenInclude(x => x.Contact)
                    .ThenInclude(x => x.Address)
                    .LoadAsync();
                return x.Person.Entity.Contact.Address.StreetAddress;
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<string> GetZIPCode(int? id)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.Entity)
                    .ThenInclude(x => x.Contact)
                    .ThenInclude(x => x.Address)
                    .LoadAsync();
                return x.Person.Entity.Contact.Address.ZIPCode;
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<string> GetPMB(int? id)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.Entity)
                    .ThenInclude(x => x.Contact)
                    .ThenInclude(x => x.Address)
                    .LoadAsync();
                return x.Person.Entity.Contact.Address.PMB;
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<IName> GetCountryOfResidence(int? id)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.Entity)
                    .ThenInclude(x => x.Contact)
                    .ThenInclude(x => x.Address)
                    .ThenInclude(x => x.CountryOfResidence)
                    .ThenInclude(x => x.CountryName)
                    .LoadAsync();
                return Maps.Map(x.Person.Entity.Contact.Address.CountryOfResidence.CountryName);
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<IName> GetStateOfResidence(int? id)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.Entity)
                    .ThenInclude(x => x.Contact)
                    .ThenInclude(x => x.Address)
                    .ThenInclude(x => x.CountryOfResidence)
                    .ThenInclude(x => x.State)
                    .LoadAsync();
                return Maps.Map(x.Person.Entity.Contact.Address.CountryOfResidence.State);
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<IName> GetTown_CityOfResidence(int? id)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.Entity)
                    .ThenInclude(x => x.Contact)
                    .ThenInclude(x => x.Address)
                    .ThenInclude(x => x.CountryOfResidence)
                    .ThenInclude(x => x.CityTown)
                    .LoadAsync();
                return Maps.Map(x.Person.Entity.Contact.Address.CountryOfResidence.CityTown);
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<string> GetLGAOfResidence(int? id)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.Entity)
                    .ThenInclude(x => x.Contact)
                    .ThenInclude(x => x.Address)
                    .ThenInclude(x => x.CountryOfResidence)
                    .LoadAsync();
                return x.Person.Entity.Contact.Address.CountryOfResidence.LGA;
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<PostResult<object>> AddCustomer(ICustomer @new)
        {
            try
            {
                var x = _context.Customers
                            .Where(x => x.Person.UniqueTag == @new.Person.UniqueTag)
                            .FirstOrDefault();
                if (x != null)
                {
                    await _context.Entry(x)
                            .Reference(x => x.Person)
                            .LoadAsync();
                    try
                    {
                        if (x.Person.UniqueTag != default)
                            throw new ApplicationStateException(EntityExists);
                        await _context.Entry(x)
                                .Reference(x => x.Person)
                                .Query()
                                .Include(x => x.FullName)
                                .ThenInclude(x => x.Name)
                                .LoadAsync();
                        await _context.Entry(x)
                                .Reference(x => x.Person)
                                .Query()
                                .Include(x => x.FullName)
                                .ThenInclude(x => x.Surname)
                                .LoadAsync();
                        await _context.Entry(x)
                                .Reference(x => x.Person)
                                .Query()
                                .Include(x => x.FullName)
                                .ThenInclude(x => x.Nickname)
                                .LoadAsync();
                        await _context.Entry(x)
                                .Reference(x => x.Person)
                                .Query()
                                .Include(x => x.FullName)
                                .ThenInclude(x => x.MaidenName)
                                .LoadAsync();
                        await _context.Entry(x)
                                .Reference(x => x.Person)
                                .Query()
                                .Include(x => x.FullName)
                                .ThenInclude(x => x.OtherNames)
                                .LoadAsync();
                        if (x.Person.FullName.Equals(Maps.ReverseMap(@new.Person.FullName)))
                            throw new ApplicationStateException(EntityExists);
                    }
                    catch (NullReferenceException)
                    {
                    }
                }
                var mapped = Maps.ReverseMap(@new);
                try
                {
                    var refer = @new.LoginCredentials.Password;
                    mapped.LoginCredential.Password = Utilities.SecretGen(@new.EntryDate, @new, ref refer);
                }
                catch (NullReferenceException)
                {
                }
                var x1 = await _context.AddAsync(mapped);
                var state = x1.State;
                _ = await _context.SaveChangesAsync();
                return new PostResult<object>
                {
                    Id = x1.Entity.CustomerModelId == null ? 0 : x1.Entity.CustomerModelId.Value,
                    Successful = state == EntityState.Added,
                    Value = x1.Entity
                };
            }
            catch (NullReferenceException ex)
            {
                return new PostResult<object>
                {
                    Id = 0,
                    FailCause = new ApplicationException(ex.Message, ex),
                    Message = ex.Message
                };
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return new PostResult<object>
                {
                    Id = 0,
                    FailCause = new ApplicationException(ex.Message, ex),
                    Message = ex.Message
                };
            }
            catch (DbUpdateException ex)
            {
                return new PostResult<object>
                {
                    Id = 0,
                    FailCause = new ApplicationException(ex.Message, ex),
                    Message = ex.Message
                };
            }
        }

        public async Task<bool> SetFullName(int? id, IFullName @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.FullName)
                    .LoadAsync();
                var current = x.Person.FullName;

                if (current != default) throw new ApplicationStateException(EntityExists);
                var val = Maps.ReverseMap(@new);
                x.Person.FullName = val;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetName(int? id, IName @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.FullName)
                    .ThenInclude(x => x.Name)
                    .LoadAsync();
                var current = x.Person.FullName.Name;

                if (current != default) throw new ApplicationStateException(EntityExists);
                var val = Maps.ReverseMap(@new);
                x.Person.FullName.Name = val;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetSurname(int? id, IName @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.FullName)
                    .ThenInclude(x => x.Surname)
                    .LoadAsync();
                var current = x.Person.FullName.Surname;

                if (current != default) throw new ApplicationStateException(EntityExists);
                var val = Maps.ReverseMap(@new);
                x.Person.FullName.Surname = val;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetNickname(int? id, IName @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.FullName)
                    .ThenInclude(x => x.Nickname)
                    .LoadAsync();
                var current = x.Person.FullName.Nickname;

                if (current != default) throw new ApplicationStateException(EntityExists);
                var val = Maps.ReverseMap(@new);
                x.Person.FullName.Nickname = val;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetMaidenName(int? id, IName @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.FullName)
                    .ThenInclude(x => x.MaidenName)
                    .LoadAsync();
                var current = x.Person.FullName.MaidenName;

                if (current != default) throw new ApplicationStateException(EntityExists);
                if (!x.Person.IsMale) return false;
                var val = Maps.ReverseMap(@new);
                x.Person.FullName.MaidenName = val;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> AddOtherName(int? id, IName @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.FullName)
                    .ThenInclude(x => x.OtherNames)
                    .LoadAsync();
                var current = x.Person.FullName.OtherNames ?? new();

                foreach (var item in current)
                {
                    if (item.Name.CompareTo(@new.Name) == 0) throw new ApplicationStateException(EntityExists);
                }
                var val = Maps.ReverseMap(@new);
                x.Person.FullName.OtherNames.Add(val);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> AddTitle(int? id, string @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.FullName)
                    .LoadAsync();
                var current = x.Person.FullName.Title;

                if (current != default) throw new ApplicationStateException(EntityExists);
                x.Person.FullName.Title = @new;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> AddAccount(int? id, IAccount @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Collection(x => x.Accounts)
                    .LoadAsync();
                var current = x.Accounts;

                if (current.Contains(Maps.ReverseMap(@new))) throw new ApplicationStateException(EntityExists);
                var val = Maps.ReverseMap(@new);
                current.Add(val);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetBalance(int? id, string accountNumber, decimal @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                if(a.Balance != default) throw new ApplicationStateException(EntityExists);
                a.Balance = @new;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetPercentageIncrease(int? id, string accountNumber, decimal @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                if (a.PercentageIncrease != default) throw new ApplicationStateException(EntityExists);
                a.PercentageIncrease = @new;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetPercentageDecrease(int? id, string accountNumber, decimal @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                if (a.PercentageDecrease != default) throw new ApplicationStateException(EntityExists);
                a.PercentageDecrease = @new;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetDebt(int? id, string accountNumber, decimal @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                if (a.Debt != default) throw new ApplicationStateException(EntityExists);
                a.Debt = @new;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetDebitLimit(int? id, string accountNumber, decimal @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                if (a.DebitLimit != default) throw new ApplicationStateException(EntityExists);
                a.DebitLimit = @new;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetCreditLimit(int? id, string accountNumber, decimal @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                if (a.CreditLimit != default) throw new ApplicationStateException(EntityExists);
                a.CreditLimit = @new;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetSmallestBalance(int? id, string accountNumber, decimal @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                if (a.SmallestBalance != default) throw new ApplicationStateException(EntityExists);
                a.SmallestBalance = @new;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetLargestBalance(int? id, string accountNumber, decimal @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                if (a.LargestBalance != default) throw new ApplicationStateException(EntityExists);
                a.LargestBalance = @new;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetSmallestTransferIn(int? id, string accountNumber, decimal @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                if (a.SmallestTransferIn != default) throw new ApplicationStateException(EntityExists);
                a.SmallestTransferIn = @new;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetLargestTransferIn(int? id, string accountNumber, decimal @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                if (a.LargestTransferIn != default) throw new ApplicationStateException(EntityExists);
                a.LargestTransferIn = @new;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetSmallestTransferOut(int? id, string accountNumber, decimal @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                if (a.SmallestTransferOut != default) throw new ApplicationStateException(EntityExists);
                a.SmallestTransferOut = @new;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetLargestTransferOut(int? id, string accountNumber, decimal @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                if (a.LargestTransferOut != default) throw new ApplicationStateException(EntityExists);
                a.LargestTransferOut = @new;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetEntryDate(int? id, string accountNumber, DateTime @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                if (a.EntryDate != default) throw new ApplicationStateException(EntityExists);
                a.EntryDate = @new;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetCurrency(int? id, string accountNumber, ICurrency @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                if (a.Currency != default) throw new ApplicationStateException(EntityExists);
                a.Currency = @new;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> AddMessageNumber(int? id, string accountNumber, [Phone] string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                if (a.SMSAlertList.Contains(@new)) throw new ApplicationStateException(EntityExists);
                a.SMSAlertList.Add(@new);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> AddEmail(int? id, string accountNumber, [EmailAddress] string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                if (a.EmailAlertList.Contains(@new)) throw new ApplicationStateException(EntityExists);
                a.EmailAlertList.Add(@new);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> AddStatus(int? id, string accountNumber, AccountStatusInfo @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                if (a.Statuses.Contains(@new)) throw new ApplicationStateException(EntityExists);
                a.Statuses.Enqueue(@new);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetCreditIntervalLimit(int? id, string accountNumber, TimeSpan @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                if (a.CreditIntervalLimit != default) throw new ApplicationStateException(EntityExists);
                a.CreditIntervalLimit = @new;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetDebitIntervalLimit(int? id, string accountNumber, TimeSpan @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                if (a.DebitIntervalLimit != default) throw new ApplicationStateException(EntityExists);
                a.DebitIntervalLimit = @new;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> AddTransaction(int? id, string accountNumber, ITransaction @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a)
                    .Collection(x => x.Transactions)
                    .LoadAsync();
                var val = Maps.ReverseMap(@new);
                if (a.Transactions.Contains(val)) throw new ApplicationStateException(EntityExists);
                bool isLim = false, isBlocked = false, isInactive = default, isFroz = default;
                foreach (var item in a.Statuses)
                {
                    if (item.Status == AccountStatus.LIMITED)
                        isLim = true;
                    if (item.Status == AccountStatus.BLOCKED)
                        isBlocked = true;
                    if (item.Status == AccountStatus.INACTIVE)
                        isInactive = true;
                    if (item.Status == AccountStatus.FROZEN)
                        isFroz = true;
                }
                if(val.IsIncoming)
                {
                    //First convert to the currency of the Account
                    //decimal amount = a.Currency.Convert(val.Amount, val.Currency);
                    //a.Balance += amount;
                    if (isBlocked || isFroz)
                        throw new ApplicationStateException("This Blocked or Frozen account cannot conduct transactions");
                    a.Balance += val.Amount;
                }
                else
                {
                    //First convert to the currency of the Account
                    //decimal amount = a.Currency.Convert(val.Amount, val.Currency);
                    //a.Balance -= amount;
                    if (isBlocked || isFroz)
                        throw new ApplicationStateException("This Blocked or Frozen account cannot Conduct transactions");
                    if (isInactive || isLim)
                        throw new ApplicationStateException("This Inactive or Limited account cannot Conduct debit transactions");
                    a.Balance -= val.Amount;
                }

                a.Transactions.Add(val);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetType(int? id, string accountNumber, AccountType @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                if (a.Type != default) throw new ApplicationStateException(EntityExists);
                a.Type = @new;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetGuarantor(int? id, string accountNumber, IPerson @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                if (a.Guarantor != default) throw new ApplicationStateException(EntityExists);
                a.Guarantor = Maps.ReverseMap(@new);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> AddCard(int? id, string accountNumber, ICard @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a)
                    .Collection(x => x.Cards)
                    .LoadAsync();
                var val = Maps.ReverseMap(@new);
                if (a.Cards.Contains(val)) throw new ApplicationStateException(EntityExists);
                a.Cards.Add(val);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetLoginCredentials(int? id, string accountNumber, ICard card, ILoginCredentials @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                var val = Maps.ReverseMap(@new);
                var c = await _context.Cards.SingleOrDefaultAsync(c => c.Equals(Maps.ReverseMap(card)));
                var l = await _context.LoginCredentials.SingleOrDefaultAsync(x => x.LoginCredentialModelId == c.SecretLoginCredentialModelId);
                if(l != default) throw new ApplicationStateException(EntityExists);
                val.Password = null;
                l = val;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetPersonalOnlineKey(int? id, string accountNumber, ICard card, ILoginCredentials @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret).LoadAsync();

                var c = a.Cards.SingleOrDefault(c => c.Equals(Maps.ReverseMap(card)));
                if (c.Secret.PersonalOnlineKey != default) throw new ApplicationStateException(EntityExists);
                c.Secret.PersonalOnlineKey = @new.PersonalOnlineKey;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetUsername(int? id, string accountNumber, ICard card, ILoginCredentials @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret).LoadAsync();

                var c = a.Cards.SingleOrDefault(c => c.Equals(Maps.ReverseMap(card)));
                if (c.Secret.PersonalOnlineKey != default) throw new ApplicationStateException(EntityExists);
                c.Secret.Username = @new.Username;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetPassword(int? id, string accountNumber, ICard card, ILoginCredentials @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret).LoadAsync();

                var c = a.Cards.SingleOrDefault(c => c.Equals(Maps.ReverseMap(card)));
                if (c.Secret.Password != default) throw new ApplicationStateException(EntityExists);
                var refer = @new.Password;
                c.Secret.Password = await Utilities.SecretGen(refer);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetBrand(int? id, string accountNumber, ILoginCredentials password, IEntity @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret).Include(c => c.Brand).LoadAsync();

                var c = a.Cards.SingleOrDefault(c => Decode(Maps.Map(c.Secret), password).Result);
                if (c.Brand != default) throw new ApplicationStateException(EntityExists);
                c.Brand = Maps.ReverseMap(@new);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetBrandName(int? id, string accountNumber, ILoginCredentials password, IName @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret).Include(c => c.Brand).ThenInclude(b => b.Name).LoadAsync();

                var c = a.Cards.SingleOrDefault(c => Decode(Maps.Map(c.Secret), password).Result);
                if (c.Brand.Name != default) throw new ApplicationStateException(EntityExists);
                c.Brand.Name = Maps.ReverseMap(@new);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetBrandPhone(int? id, string accountNumber, ILoginCredentials password, [Phone] string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .Include(c => c.Brand)
                    .ThenInclude(b => b.Contact)
                    .LoadAsync();

                var c = a.Cards.SingleOrDefault(c => Decode(Maps.Map(c.Secret), password).Result);
                if (c.Brand.Contact.Mobiles.Contains(@new)) throw new ApplicationStateException(EntityExists);
                c.Brand.Contact.Mobiles.Add(@new);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetBrandEmail(int? id, string accountNumber, ILoginCredentials password, [EmailAddress] string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .Include(c => c.Brand)
                    .ThenInclude(b => b.Contact)
                    .LoadAsync();

                var c = a.Cards.SingleOrDefault(c => Decode(Maps.Map(c.Secret), password).Result);
                if (c.Brand.Contact.Emails.Contains(@new)) throw new ApplicationStateException(EntityExists);
                c.Brand.Contact.Emails.Add(@new);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetBrandSocialMedia(int? id, string accountNumber, ILoginCredentials password, [Url] string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .Include(c => c.Brand)
                    .ThenInclude(b => b.Contact)
                    .LoadAsync();

                var c = a.Cards.SingleOrDefault(c => Decode(Maps.Map(c.Secret), password).Result);
                if (c.Brand.Contact.SocialMedia.Contains(@new)) throw new ApplicationStateException(EntityExists);
                c.Brand.Contact.SocialMedia.Add(@new);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetBrandWebsite(int? id, string accountNumber, ILoginCredentials password, [Url] string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .Include(c => c.Brand)
                    .ThenInclude(b => b.Contact)
                    .LoadAsync();

                var c = a.Cards.SingleOrDefault(c => Decode(Maps.Map(c.Secret), password).Result);
                if (c.Brand.Contact.Websites.Contains(@new)) throw new ApplicationStateException(EntityExists);
                c.Brand.Contact.Websites.Add(@new);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetBrandAddress(int? id, string accountNumber, ILoginCredentials password, IAddress @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .Include(c => c.Brand)
                    .ThenInclude(b => b.Contact)
                    .ThenInclude(b => b.Address)
                    .LoadAsync();

                var c = a.Cards.SingleOrDefault(c => Decode(Maps.Map(c.Secret), password).Result);
                if (c.Brand.Contact.Address != default) throw new ApplicationStateException(EntityExists);
                c.Brand.Contact.Address = Maps.ReverseMap(@new);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetBrandStreetAddress(int? id, string accountNumber, ILoginCredentials password, string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .Include(c => c.Brand)
                    .ThenInclude(b => b.Contact)
                    .ThenInclude(b => b.Address)
                    .LoadAsync();

                var c = a.Cards.SingleOrDefault(c => Decode(Maps.Map(c.Secret), password).Result);
                if (c.Brand.Contact.Address.StreetAddress != default) throw new ApplicationStateException(EntityExists);
                c.Brand.Contact.Address.StreetAddress = @new;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetBrandZIPCode(int? id, string accountNumber, ILoginCredentials password, string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .Include(c => c.Brand)
                    .ThenInclude(b => b.Contact)
                    .ThenInclude(b => b.Address)
                    .LoadAsync();

                var c = a.Cards.SingleOrDefault(c => Decode(Maps.Map(c.Secret), password).Result);
                if (c.Brand.Contact.Address.ZIPCode != default) throw new ApplicationStateException(EntityExists);
                c.Brand.Contact.Address.ZIPCode = @new;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetBrandPMB(int? id, string accountNumber, ILoginCredentials password, string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .Include(c => c.Brand)
                    .ThenInclude(b => b.Contact)
                    .ThenInclude(b => b.Address)
                    .LoadAsync();

                var c = a.Cards.SingleOrDefault(c => Decode(Maps.Map(c.Secret), password).Result);
                if (c.Brand.Contact.Address.PMB != default) throw new ApplicationStateException(EntityExists);
                c.Brand.Contact.Address.PMB = @new;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetBrandCountryOfResidence(int? id, string accountNumber, ILoginCredentials password, INationality @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .Include(c => c.Brand)
                    .ThenInclude(b => b.Contact)
                    .ThenInclude(b => b.Address)
                    .ThenInclude(b => b.CountryOfResidence)
                    .LoadAsync();

                var c = a.Cards.SingleOrDefault(c => Decode(Maps.Map(c.Secret), password).Result);
                if (c.Brand.Contact.Address.CountryOfResidence != default) throw new ApplicationStateException(EntityExists);
                c.Brand.Contact.Address.CountryOfResidence = Maps.ReverseMap(@new);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetBrandCountryOfResidence(int? id, string accountNumber, ILoginCredentials password, IName @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .Include(c => c.Brand)
                    .ThenInclude(b => b.Contact)
                    .ThenInclude(b => b.Address)
                    .ThenInclude(b => b.CountryOfResidence)
                    .ThenInclude(b => b.CountryName)
                    .LoadAsync();

                var c = a.Cards.SingleOrDefault(c => Decode(Maps.Map(c.Secret), password).Result);
                if (c.Brand.Contact.Address.CountryOfResidence.CountryName != default) throw new ApplicationStateException(EntityExists);
                c.Brand.Contact.Address.CountryOfResidence.CountryName = Maps.ReverseMap(@new);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetBrandStateOfResidence(int? id, string accountNumber, ILoginCredentials password, IName @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .Include(c => c.Brand)
                    .ThenInclude(b => b.Contact)
                    .ThenInclude(b => b.Address)
                    .ThenInclude(b => b.CountryOfResidence)
                    .ThenInclude(b => b.State)
                    .LoadAsync();

                var c = a.Cards.SingleOrDefault(c => Decode(Maps.Map(c.Secret), password).Result);
                if (c.Brand.Contact.Address.CountryOfResidence.State != default) throw new ApplicationStateException(EntityExists);
                c.Brand.Contact.Address.CountryOfResidence.State = Maps.ReverseMap(@new);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetBrandLGAOfResidence(int? id, string accountNumber, ILoginCredentials password, string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .Include(c => c.Brand)
                    .ThenInclude(b => b.Contact)
                    .ThenInclude(b => b.Address)
                    .ThenInclude(b => b.CountryOfResidence)
                    .LoadAsync();

                var c = a.Cards.SingleOrDefault(c => Decode(Maps.Map(c.Secret), password).Result);
                if (c.Brand.Contact.Address.CountryOfResidence.LGA != default) throw new ApplicationStateException(EntityExists);
                c.Brand.Contact.Address.CountryOfResidence.LGA = @new;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetBrandLanguage(int? id, string accountNumber, ILoginCredentials password, string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .Include(c => c.Brand)
                    .ThenInclude(b => b.Contact)
                    .ThenInclude(b => b.Address)
                    .ThenInclude(b => b.CountryOfResidence)
                    .LoadAsync();

                var c = a.Cards.SingleOrDefault(c => Decode(Maps.Map(c.Secret), password).Result);
                if (c.Brand.Contact.Address.CountryOfResidence.Language != default) throw new ApplicationStateException(EntityExists);
                c.Brand.Contact.Address.CountryOfResidence.Language = @new;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetBrandCityOfResidence(int? id, string accountNumber, ILoginCredentials password, IName @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .Include(c => c.Brand)
                    .ThenInclude(b => b.Contact)
                    .ThenInclude(b => b.Address)
                    .ThenInclude(b => b.CountryOfResidence)
                    .ThenInclude(b => b.CityTown)
                    .LoadAsync();

                var c = a.Cards.SingleOrDefault(c => Decode(Maps.Map(c.Secret), password).Result);
                if (c.Brand.Contact.Address.CountryOfResidence.CityTown != default) throw new ApplicationStateException(EntityExists);
                c.Brand.Contact.Address.CountryOfResidence.CityTown = Maps.ReverseMap(@new);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetBrandCountryOfUse(int? id, string accountNumber, ILoginCredentials password, INationality @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .Include(c => c.CountryOfUse)
                    .LoadAsync();

                var c = a.Cards.SingleOrDefault(c => Decode(Maps.Map(c.Secret), password).Result);
                if (c.CountryOfUse != default) throw new ApplicationStateException(EntityExists);
                c.CountryOfUse = Maps.ReverseMap(@new);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetBrandCountryOfUse(int? id, string accountNumber, ILoginCredentials password, IName @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .Include(c => c.CountryOfUse)
                    .ThenInclude(c => c.CountryName)
                    .LoadAsync();

                var c = a.Cards.SingleOrDefault(c => Decode(Maps.Map(c.Secret), password).Result);
                if (c.CountryOfUse.CountryName != default) throw new ApplicationStateException(EntityExists);
                c.CountryOfUse.CountryName = Maps.ReverseMap(@new);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetBrandStateOfUse(int? id, string accountNumber, ILoginCredentials password, IName @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .Include(c => c.CountryOfUse)
                    .ThenInclude(c => c.State)
                    .LoadAsync();

                var c = a.Cards.SingleOrDefault(c => Decode(Maps.Map(c.Secret), password).Result);
                if (c.CountryOfUse.State != default) throw new ApplicationStateException(EntityExists);
                c.CountryOfUse.State = Maps.ReverseMap(@new);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetBrandLGAOfUse(int? id, string accountNumber, ILoginCredentials password, string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .Include(c => c.CountryOfUse)
                    .LoadAsync();

                var c = a.Cards.SingleOrDefault(c => Decode(Maps.Map(c.Secret), password).Result);
                if (c.CountryOfUse.LGA != default) throw new ApplicationStateException(EntityExists);
                c.CountryOfUse.LGA = @new;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetBrandLanguageOfUse(int? id, string accountNumber, ILoginCredentials password, string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .Include(c => c.CountryOfUse)
                    .LoadAsync();

                var c = a.Cards.SingleOrDefault(c => Decode(Maps.Map(c.Secret), password).Result);
                if (c.CountryOfUse.Language != default) throw new ApplicationStateException(EntityExists);
                c.CountryOfUse.Language = @new;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetBrandCityTownOfUse(int? id, string accountNumber, ILoginCredentials password, IName @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .Include(c => c.CountryOfUse)
                    .ThenInclude(c => c.CityTown)
                    .LoadAsync();

                var c = a.Cards.SingleOrDefault(c => Decode(Maps.Map(c.Secret), password).Result);
                if (c.CountryOfUse.CityTown != default) throw new ApplicationStateException(EntityExists);
                c.CountryOfUse.CityTown = Maps.ReverseMap(@new);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetCurrencyOfUse(int? id, string accountNumber, ILoginCredentials password, ICurrency @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .LoadAsync();

                var c = a.Cards.SingleOrDefault(c => Decode(Maps.Map(c.Secret), password).Result);
                if (c.Currency != default) throw new ApplicationStateException(EntityExists);
                c.Currency = @new;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetCardType(int? id, string accountNumber, ILoginCredentials password, CardType @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .LoadAsync();

                var c = a.Cards.SingleOrDefault(c => Decode(Maps.Map(c.Secret), password).Result);
                if (c.Type != default) throw new ApplicationStateException(EntityExists);
                c.Type = @new;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetIssuedDate(int? id, string accountNumber, ILoginCredentials password, DateTime @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .LoadAsync();

                var c = a.Cards.SingleOrDefault(c => Decode(Maps.Map(c.Secret), password).Result);
                if (c.IssuedDate != default) throw new ApplicationStateException(EntityExists);
                c.IssuedDate = @new;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetExpiryDate(int? id, string accountNumber, ILoginCredentials password, DateTime @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .LoadAsync();

                var c = a.Cards.SingleOrDefault(c => Decode(Maps.Map(c.Secret), password).Result);
                if (c.ExpiryDate != default) throw new ApplicationStateException(EntityExists);
                c.ExpiryDate = @new;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetIssuedCost(int? id, string accountNumber, ILoginCredentials password, decimal @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .LoadAsync();

                var c = a.Cards.SingleOrDefault(c => Decode(Maps.Map(c.Secret), password).Result);
                if (c.IssuedCost != default) throw new ApplicationStateException(EntityExists);
                c.IssuedCost = @new;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetMonthlyRate(int? id, string accountNumber, ILoginCredentials password, decimal @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .LoadAsync();

                var c = a.Cards.SingleOrDefault(c => Decode(Maps.Map(c.Secret), password).Result);
                if (c.MonthlyRate != default) throw new ApplicationStateException(EntityExists);
                c.MonthlyRate = @new;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetWithdrawalLimit(int? id, string accountNumber, ILoginCredentials password, decimal @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                var a = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(a).Collection(x => x.Cards).Query().Include(c => c.Secret)
                    .LoadAsync();

                var c = a.Cards.SingleOrDefault(c => Decode(Maps.Map(c.Secret), password).Result);
                if (c.WithdrawalLimit != default) throw new ApplicationStateException(EntityExists);
                c.WithdrawalLimit = @new;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetBVN(int? id, string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                if (x.BVN != default) throw new ApplicationStateException(EntityExists);
                x.BVN = @new;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetEntryDate(int? id, DateTime @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                if (x.EntryDate != default) throw new ApplicationStateException(EntityExists);
                x.EntryDate = @new;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetLoginCredential(int? id, ILoginCredentials @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.LoginCredential)
                    .LoadAsync();
                if (x.LoginCredential != default) throw new ApplicationStateException(EntityExists);
                x.LoginCredential = Maps.ReverseMap(@new);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetPersonalOnlineKey(int? id, ILoginCredentials @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.LoginCredential)
                    .LoadAsync();
                if (x.LoginCredential.PersonalOnlineKey != default) throw new ApplicationStateException(EntityExists);
                x.LoginCredential.PersonalOnlineKey = @new.PersonalOnlineKey;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetUsername(int? id, ILoginCredentials @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.LoginCredential)
                    .LoadAsync();
                if (x.LoginCredential.Username != default) throw new ApplicationStateException(EntityExists);
                x.LoginCredential.Username = @new.Username;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetPassword(int? id, ILoginCredentials @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.LoginCredential)
                    .LoadAsync();
                if (x.LoginCredential.Password != default) throw new ApplicationStateException(EntityExists);
                var refer = @new.Password;
                x.LoginCredential.Password = await Utilities.SecretGen(refer);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetPerson(int? id, IPerson @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .LoadAsync();
                if (x.Person != default) throw new ApplicationStateException(EntityExists);
                x.Person = Maps.ReverseMap(@new);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> AddIdentification(int? id, string type, string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .LoadAsync();
                if (x.Person.Identification.ContainsKey(type) && x.Person.Identification[type] != default) throw new ApplicationStateException(EntityExists);
                x.Person.Identification[type] = @new;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> SetFingerprint(int? id, byte[] @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .LoadAsync();
                var current = x.Person.FingerPrint;

                if (current != default) throw new ApplicationStateException(EntityExists);
                x.Person.FingerPrint = @new;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetSignature(int? id, byte[] @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .LoadAsync();
                var current = x.Person.Signature;

                if (current != default) throw new ApplicationStateException(EntityExists);
                x.Person.Signature = @new;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetPassport(int? id, byte[] @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .LoadAsync();
                var current = x.Person.Passport;

                if (current != default) throw new ApplicationStateException(EntityExists);
                x.Person.Passport = @new;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetJobType(int? id, string @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .LoadAsync();
                var current = x.Person.JobType;

                if (current != default) throw new ApplicationStateException(EntityExists);
                x.Person.JobType = @new;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetSex(int? id, bool @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .LoadAsync();

                x.Person.IsMale = @new;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetUniqueTag(int? id, Guid @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .LoadAsync();
                var current = x.Person.UniqueTag;

                if (current != default) throw new ApplicationStateException(EntityExists);
                x.Person.UniqueTag = @new;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetBirthDate(int? id, DateTime @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .LoadAsync();
                var current = x.Person.BirthDate;

                if (current != default) throw new ApplicationStateException(EntityExists);
                x.Person.BirthDate = @new;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetCountryOfOrigin(int? id, INationality @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.CountryOfOrigin)
                    .LoadAsync();
                var current = x.Person.CountryOfOrigin;

                if (current != default) throw new ApplicationStateException(EntityExists);
                x.Person.CountryOfOrigin = Maps.ReverseMap(@new);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetCountryOfOrigin(int? id, IName @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.CountryOfOrigin)
                    .ThenInclude(x => x.CountryName)
                    .LoadAsync();
                var current = x.Person.CountryOfOrigin.CountryName;

                if (current != default) throw new ApplicationStateException(EntityExists);
                x.Person.CountryOfOrigin.CountryName = Maps.ReverseMap(@new);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetStateOfOrigin(int? id, IName @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.CountryOfOrigin)
                    .ThenInclude(x => x.State)
                    .LoadAsync();
                var current = x.Person.CountryOfOrigin.State;

                if (current != default) throw new ApplicationStateException(EntityExists);
                x.Person.CountryOfOrigin.State = Maps.ReverseMap(@new);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetTown_CityOfOrigin(int? id, IName @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.CountryOfOrigin)
                    .ThenInclude(x => x.CityTown)
                    .LoadAsync();
                var current = x.Person.CountryOfOrigin.CityTown;

                if (current != default) throw new ApplicationStateException(EntityExists);
                x.Person.CountryOfOrigin.CityTown = Maps.ReverseMap(@new);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetLGAOfOrigin(int? id, string @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.CountryOfOrigin)
                    .LoadAsync();
                var current = x.Person.CountryOfOrigin.LGA;

                if (current != default) throw new ApplicationStateException(EntityExists);
                x.Person.CountryOfOrigin.LGA = @new;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetLanguage(int? id, string @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.CountryOfOrigin)
                    .LoadAsync();
                var current = x.Person.CountryOfOrigin.Language;

                if (current != default) throw new ApplicationStateException(EntityExists);
                x.Person.CountryOfOrigin.Language = @new;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetNextOfKin(int? id, IPerson @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .LoadAsync();
                var current = x.Person.NextOfKin;

                if (current != default) throw new ApplicationStateException(EntityExists);
                x.Person.NextOfKin = Maps.ReverseMap(@new);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetMaritalStatus(int? id, MaritalStatus @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.CountryOfOrigin)
                    .LoadAsync();
                x.Person.MaritalStatus = @new;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetOfficialEntity(int? id, IEntity @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.OfficialEntity)
                    .LoadAsync();
                var current = x.Person.OfficialEntity;

                if (current != default) throw new ApplicationStateException(EntityExists);
                x.Person.OfficialEntity = Maps.ReverseMap(@new);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetBusinessName(int? id, IName @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.OfficialEntity)
                    .ThenInclude(x => x.Name)
                    .LoadAsync();
                var current = x.Person.OfficialEntity.Name;

                if (current != default) throw new ApplicationStateException(EntityExists);
                x.Person.OfficialEntity.Name = Maps.ReverseMap(@new);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetOfficialContact(int? id, IContact @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.OfficialEntity)
                    .ThenInclude(x => x.Contact)
                    .LoadAsync();
                var current = x.Person.OfficialEntity.Contact;

                if (current != default) throw new ApplicationStateException(EntityExists);
                x.Person.OfficialEntity.Contact = Maps.ReverseMap(@new);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> AddBusinessEmail(int? id, [EmailAddress] string @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.OfficialEntity)
                    .ThenInclude(x => x.Contact)
                    .LoadAsync();
                var current = x.Person.OfficialEntity.Contact.Emails;

                if (current.Contains(@new)) throw new ApplicationStateException(EntityExists);
                x.Person.OfficialEntity.Contact.Emails.Add(@new);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> AddBusinessMobile(int? id, [Phone] string @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.OfficialEntity)
                    .ThenInclude(x => x.Contact)
                    .LoadAsync();
                var current = x.Person.OfficialEntity.Contact.Mobiles;

                if (current.Contains(@new)) throw new ApplicationStateException(EntityExists);
                x.Person.OfficialEntity.Contact.Mobiles.Add(@new);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> AddBusinessSocialMedia(int? id, [Url] string @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.OfficialEntity)
                    .ThenInclude(x => x.Contact)
                    .LoadAsync();
                var current = x.Person.OfficialEntity.Contact.SocialMedia;

                if (current.Contains(@new)) throw new ApplicationStateException(EntityExists);
                x.Person.OfficialEntity.Contact.SocialMedia.Add(@new);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> AddBusinessWebsite(int? id, [Url] string @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.OfficialEntity)
                    .ThenInclude(x => x.Contact)
                    .LoadAsync();
                var current = x.Person.OfficialEntity.Contact.Websites;

                if (current.Contains(@new)) throw new ApplicationStateException(EntityExists);
                x.Person.OfficialEntity.Contact.Websites.Add(@new);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetOfficialAddress(int? id, IAddress @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.OfficialEntity)
                    .ThenInclude(x => x.Contact)
                    .ThenInclude(x => x.Address)
                    .LoadAsync();
                var current = x.Person.OfficialEntity.Contact.Address;

                if (current != default) throw new ApplicationStateException(EntityExists);
                x.Person.OfficialEntity.Contact.Address = Maps.ReverseMap(@new);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetBusinessStreetAddress(int? id, string @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.OfficialEntity)
                    .ThenInclude(x => x.Contact)
                    .ThenInclude(x => x.Address)
                    .LoadAsync();
                var current = x.Person.OfficialEntity.Contact.Address.StreetAddress;

                if (current != default) throw new ApplicationStateException(EntityExists);
                x.Person.OfficialEntity.Contact.Address.StreetAddress = @new;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetBusinessZIPCode(int? id, string @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.OfficialEntity)
                    .ThenInclude(x => x.Contact)
                    .ThenInclude(x => x.Address)
                    .LoadAsync();
                var current = x.Person.OfficialEntity.Contact.Address.ZIPCode;

                if (current != default) throw new ApplicationStateException(EntityExists);
                x.Person.OfficialEntity.Contact.Address.ZIPCode = @new;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetBusinessPMB(int? id, string @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.OfficialEntity)
                    .ThenInclude(x => x.Contact)
                    .ThenInclude(x => x.Address)
                    .LoadAsync();
                var current = x.Person.OfficialEntity.Contact.Address.PMB;

                if (current != default) throw new ApplicationStateException(EntityExists);
                x.Person.OfficialEntity.Contact.Address.PMB = @new;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetOfficalNationality(int? id, INationality @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.OfficialEntity)
                    .ThenInclude(x => x.Contact)
                    .ThenInclude(x => x.Address)
                    .ThenInclude(x => x.CountryOfResidence)
                    .LoadAsync();
                var current = x.Person.OfficialEntity.Contact.Address.CountryOfResidence;

                if (current != default) throw new ApplicationStateException(EntityExists);
                x.Person.OfficialEntity.Contact.Address.CountryOfResidence = Maps.ReverseMap(@new);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetBusinessCountryOfResidence(int? id, IName @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.OfficialEntity)
                    .ThenInclude(x => x.Contact)
                    .ThenInclude(x => x.Address)
                    .ThenInclude(x => x.CountryOfResidence)
                    .ThenInclude(x => x.CountryName)
                    .LoadAsync();
                var current = x.Person.OfficialEntity.Contact.Address.CountryOfResidence.CountryName;

                if (current != default) throw new ApplicationStateException(EntityExists);
                x.Person.OfficialEntity.Contact.Address.CountryOfResidence.CountryName = Maps.ReverseMap(@new);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetBusinessStateOfResidence(int? id, IName @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.OfficialEntity)
                    .ThenInclude(x => x.Contact)
                    .ThenInclude(x => x.Address)
                    .ThenInclude(x => x.CountryOfResidence)
                    .ThenInclude(x => x.State)
                    .LoadAsync();
                var current = x.Person.OfficialEntity.Contact.Address.CountryOfResidence.State;

                if (current != default) throw new ApplicationStateException(EntityExists);
                x.Person.OfficialEntity.Contact.Address.CountryOfResidence.State = Maps.ReverseMap(@new);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetBusinessTown_CityOfResidence(int? id, IName @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.OfficialEntity)
                    .ThenInclude(x => x.Contact)
                    .ThenInclude(x => x.Address)
                    .ThenInclude(x => x.CountryOfResidence)
                    .ThenInclude(x => x.CityTown)
                    .LoadAsync();
                var current = x.Person.OfficialEntity.Contact.Address.CountryOfResidence.CityTown;

                if (current != default) throw new ApplicationStateException(EntityExists);
                x.Person.OfficialEntity.Contact.Address.CountryOfResidence.CityTown = Maps.ReverseMap(@new);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetBusinessLGAOfResidence(int? id, string @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.OfficialEntity)
                    .ThenInclude(x => x.Contact)
                    .ThenInclude(x => x.Address)
                    .ThenInclude(x => x.CountryOfResidence)
                    .ThenInclude(x => x.CountryName)
                    .LoadAsync();
                var current = x.Person.OfficialEntity.Contact.Address.CountryOfResidence.LGA;

                if (current != default) throw new ApplicationStateException(EntityExists);
                x.Person.OfficialEntity.Contact.Address.CountryOfResidence.LGA = @new;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetEntity(int? id, IEntity @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.Entity)
                    .LoadAsync();
                var current = x.Person.Entity;

                if (current != default) throw new ApplicationStateException(EntityExists);
                x.Person.Entity = Maps.ReverseMap(@new);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetDisplayName(int? id, IName @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.Entity)
                    .ThenInclude(x => x.Name)
                    .LoadAsync();
                var current = x.Person.Entity.Name;

                if (current != default) throw new ApplicationStateException(EntityExists);
                x.Person.Entity.Name = Maps.ReverseMap(@new);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetContact(int? id, IContact @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.Entity)
                    .ThenInclude(x => x.Contact)
                    .LoadAsync();
                var current = x.Person.Entity.Contact;

                if (current != default) throw new ApplicationStateException(EntityExists);
                x.Person.Entity.Contact = Maps.ReverseMap(@new);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> AddEmail(int? id, [EmailAddress] string @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.Entity)
                    .ThenInclude(x => x.Contact)
                    .LoadAsync();
                var current = x.Person.Entity.Contact.Emails;

                if (current.Contains(@new)) throw new ApplicationStateException(EntityExists);
                x.Person.Entity.Contact.Emails.Add(@new);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> AddMobile(int? id, [Phone] string @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.Entity)
                    .ThenInclude(x => x.Contact)
                    .LoadAsync();
                var current = x.Person.Entity.Contact.Mobiles;

                if (current.Contains(@new)) throw new ApplicationStateException(EntityExists);
                x.Person.Entity.Contact.Mobiles.Add(@new);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> AddSocialMedia(int? id, [Url] string @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.Entity)
                    .ThenInclude(x => x.Contact)
                    .LoadAsync();
                var current = x.Person.Entity.Contact.SocialMedia;

                if (current.Contains(@new)) throw new ApplicationStateException(EntityExists);
                x.Person.Entity.Contact.SocialMedia.Add(@new);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> AddWebsite(int? id, [Url] string @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.Entity)
                    .ThenInclude(x => x.Contact)
                    .LoadAsync();
                var current = x.Person.Entity.Contact.Websites;

                if (current.Contains(@new)) throw new ApplicationStateException(EntityExists);
                x.Person.Entity.Contact.Websites.Add(@new);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetAddress(int? id, IAddress @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.Entity)
                    .ThenInclude(x => x.Contact)
                    .ThenInclude(x => x.Address)
                    .LoadAsync();
                var current = x.Person.Entity.Contact.Address;

                if (current != default) throw new ApplicationStateException(EntityExists);
                x.Person.Entity.Contact.Address = Maps.ReverseMap(@new);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetStreetAddress(int? id, string @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.Entity)
                    .ThenInclude(x => x.Contact)
                    .ThenInclude(x => x.Address)
                    .LoadAsync();
                var current = x.Person.Entity.Contact.Address.StreetAddress;

                if (current.Contains(@new)) throw new ApplicationStateException(EntityExists);
                x.Person.Entity.Contact.Address.StreetAddress = @new;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetZIPCode(int? id, string @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.Entity)
                    .ThenInclude(x => x.Contact)
                    .ThenInclude(x => x.Address)
                    .LoadAsync();
                var current = x.Person.Entity.Contact.Address.ZIPCode;

                if (current.Contains(@new)) throw new ApplicationStateException(EntityExists);
                x.Person.Entity.Contact.Address.ZIPCode = @new;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetPMB(int? id, string @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.Entity)
                    .ThenInclude(x => x.Contact)
                    .ThenInclude(x => x.Address)
                    .LoadAsync();
                var current = x.Person.Entity.Contact.Address.PMB;

                if (current.Contains(@new)) throw new ApplicationStateException(EntityExists);
                x.Person.Entity.Contact.Address.PMB = @new;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetNationality(int? id, INationality @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.Entity)
                    .ThenInclude(x => x.Contact)
                    .ThenInclude(x => x.Address)
                    .ThenInclude(x => x.CountryOfResidence)
                    .LoadAsync();
                var current = x.Person.Entity.Contact.Address.CountryOfResidence;

                if (current != default) throw new ApplicationStateException(EntityExists);
                x.Person.Entity.Contact.Address.CountryOfResidence = Maps.ReverseMap(@new);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetCountryOfResidence(int? id, IName @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.Entity)
                    .ThenInclude(x => x.Contact)
                    .ThenInclude(x => x.Address)
                    .ThenInclude(x => x.CountryOfResidence)
                    .ThenInclude(x => x.CountryName)
                    .LoadAsync();
                var current = x.Person.Entity.Contact.Address.CountryOfResidence.CountryName;

                if (current != default) throw new ApplicationStateException(EntityExists);
                x.Person.Entity.Contact.Address.CountryOfResidence.CountryName = Maps.ReverseMap(@new);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetStateOfResidence(int? id, IName @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.Entity)
                    .ThenInclude(x => x.Contact)
                    .ThenInclude(x => x.Address)
                    .ThenInclude(x => x.CountryOfResidence)
                    .ThenInclude(x => x.State)
                    .LoadAsync();
                var current = x.Person.Entity.Contact.Address.CountryOfResidence.State;

                if (current != default) throw new ApplicationStateException(EntityExists);
                x.Person.Entity.Contact.Address.CountryOfResidence.State = Maps.ReverseMap(@new);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetTown_CityOfResidence(int? id, IName @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.Entity)
                    .ThenInclude(x => x.Contact)
                    .ThenInclude(x => x.Address)
                    .ThenInclude(x => x.CountryOfResidence)
                    .ThenInclude(x => x.CityTown)
                    .LoadAsync();
                var current = x.Person.Entity.Contact.Address.CountryOfResidence.CityTown;

                if (current != default) throw new ApplicationStateException(EntityExists);
                x.Person.Entity.Contact.Address.CountryOfResidence.CityTown = Maps.ReverseMap(@new);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetLGAOfResidence(int? id, string @new)
        {
            var x = await _context.Customers.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.Entity)
                    .ThenInclude(x => x.Contact)
                    .ThenInclude(x => x.Address)
                    .ThenInclude(x => x.CountryOfResidence)
                    .LoadAsync();
                var current = x.Person.Entity.Contact.Address.CountryOfResidence.LGA;

                if (current != default) throw new ApplicationStateException(EntityExists);
                x.Person.Entity.Contact.Address.CountryOfResidence.LGA = @new;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> AddToken(int? id, string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.LoginCredential)
                    .LoadAsync();
                x.LoginCredential.Tokens.Add(@new);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> UpdateName(int? id, IName old, IName @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.FullName)
                    .LoadAsync();
                int? cId = x.Person.FullName.NameModelId;

                if (cId == default) throw new ApplicationStateException(EntityMissing, 0, null);

                var value = await _context.Names.SingleOrDefaultAsync(val => val.NameModelId == cId);

                if (value.Name.CompareTo(old.Name) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                value.Name = @new.Name;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateSurname(int? id, IName old, IName @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.FullName)
                    .LoadAsync();
                int? cId = x.Person.FullName.SurnameNameModelId;

                if (cId == default) throw new ApplicationStateException(EntityMissing, 0, null);

                var value = await _context.Names.SingleOrDefaultAsync(val => val.NameModelId == cId);

                if (value.Name.CompareTo(old.Name) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                value.Name = @new.Name;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateNickname(int? id, IName old, IName @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.FullName)
                    .LoadAsync();
                int? cId = x.Person.FullName.NicknameNameModelId;

                if (cId == default) throw new ApplicationStateException(EntityMissing, 0, null);

                var value = await _context.Names.SingleOrDefaultAsync(val => val.NameModelId == cId);

                if (value.Name.CompareTo(old.Name) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                value.Name = @new.Name;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateMaidenName(int? id, IName old, IName @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.FullName)
                    .LoadAsync();
                int? cId = x.Person.FullName.MaidenNameNameModelId;

                if (cId == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (!x.Person.IsMale) return false;

                var value = await _context.Names.SingleOrDefaultAsync(val => val.NameModelId == cId);

                if (value.Name.CompareTo(old.Name) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                value.Name = @new.Name;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateOthername(int? id, IName old, IName @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .LoadAsync();
                var val = (await _context.FullNames.SingleOrDefaultAsync(y => y.FullNameModelId == x.Person.FullNameModelId)).OtherNames;

                if (!val.Any(x => x.Name.CompareTo(old.Name) == 0)) throw new ApplicationStateException(EntityMissing, 0, null);
                //if (!val.Any(x => x.Name.CompareTo(old.Name) == 0)) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val.Remove(Maps.ReverseMap(old));
                val.Add(Maps.ReverseMap(@new));

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateTitle(int? id, string old, string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .LoadAsync();
                var val = (await _context.FullNames.SingleOrDefaultAsync(y => y.FullNameModelId == x.Person.FullNameModelId)).Title;

                if (val == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val.CompareTo(old) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val = @new;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateNumber(int? id, string old, string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            try
            {
                await _context.Entry(x)
                    .Collection(x => x.Accounts)
                    .LoadAsync();
                var acct = x.Accounts.Where(x => x.Number.CompareTo(old) == 0).SingleOrDefault();
                //var acct = await _context.Accounts.SingleOrDefaultAsync(y => y.Number.CompareTo(old) == 0);
                //var val = acct.Number;

                if (acct.Number == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (acct.Number.CompareTo(old) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                acct.Number = @new;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> UpdateBalance(int? id, string accountNumber, decimal old, decimal @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            if(x == default) throw NotFound(InvalidId, id == null ? 0 : id.Value);
            try
            {
                var val = (await _context.Accounts.SingleOrDefaultAsync(y => y.Number.CompareTo(accountNumber) == 0)).Balance;

                if (val == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val != old) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val = @new;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdatePercentageIncrease(int? id, string accountNumber, decimal old, decimal @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            if (x == default) throw NotFound(InvalidId, id == null ? 0 : id.Value);
            try
            {
                var val = (await _context.Accounts.SingleOrDefaultAsync(y => y.Number.CompareTo(accountNumber) == 0)).PercentageIncrease;

                if (val == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val != old) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val = @new;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdatePercentageDecrease(int? id, string accountNumber, decimal old, decimal @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            if (x == default) throw NotFound(InvalidId, id == null ? 0 : id.Value);
            try
            {
                var val = (await _context.Accounts.SingleOrDefaultAsync(y => y.Number.CompareTo(accountNumber) == 0)).PercentageDecrease;

                if (val == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val != old) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val = @new;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateDebt(int? id, string accountNumber, decimal old, decimal @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            if (x == default) throw NotFound(InvalidId, id == null ? 0 : id.Value);
            try
            {
                var val = (await _context.Accounts.SingleOrDefaultAsync(y => y.Number.CompareTo(accountNumber) == 0)).Debt;

                if (val == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val != old) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val = @new;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateDebitLimit(int? id, string accountNumber, decimal old, decimal @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            if (x == default) throw NotFound(InvalidId, id == null ? 0 : id.Value);
            try
            {
                var val = (await _context.Accounts.SingleOrDefaultAsync(y => y.Number.CompareTo(accountNumber) == 0)).DebitLimit;

                if (val == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val != old) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val = @new;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateCreditLimit(int? id, string accountNumber, decimal old, decimal @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            if (x == default) throw NotFound(InvalidId, id == null ? 0 : id.Value);
            try
            {
                var val = (await _context.Accounts.SingleOrDefaultAsync(y => y.Number.CompareTo(accountNumber) == 0)).CreditLimit;

                if (val == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val != old) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val = @new;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateSmallestBalance(int? id, string accountNumber, decimal old, decimal @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            if (x == default) throw NotFound(InvalidId, id == null ? 0 : id.Value);
            try
            {
                var val = (await _context.Accounts.SingleOrDefaultAsync(y => y.Number.CompareTo(accountNumber) == 0)).SmallestBalance;

                if (val == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val != old) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val = @new;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateLargestBalance(int? id, string accountNumber, decimal old, decimal @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            if (x == default) throw NotFound(InvalidId, id == null ? 0 : id.Value);
            try
            {
                var val = (await _context.Accounts.SingleOrDefaultAsync(y => y.Number.CompareTo(accountNumber) == 0)).LargestBalance;

                if (val == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val != old) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val = @new;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateSmallestTransferIn(int? id, string accountNumber, decimal old, decimal @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            if (x == default) throw NotFound(InvalidId, id == null ? 0 : id.Value);
            try
            {
                var val = (await _context.Accounts.SingleOrDefaultAsync(y => y.Number.CompareTo(accountNumber) == 0)).SmallestTransferIn;

                if (val == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val != old) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val = @new;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateLargestTransferIn(int? id, string accountNumber, decimal old, decimal @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            if (x == default) throw NotFound(InvalidId, id == null ? 0 : id.Value);
            try
            {
                var val = (await _context.Accounts.SingleOrDefaultAsync(y => y.Number.CompareTo(accountNumber) == 0)).LargestTransferIn;

                if (val == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val != old) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val = @new;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateSmallestTransferOut(int? id, string accountNumber, decimal old, decimal @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            if (x == default) throw NotFound(InvalidId, id == null ? 0 : id.Value);
            try
            {
                var val = (await _context.Accounts.SingleOrDefaultAsync(y => y.Number.CompareTo(accountNumber) == 0)).SmallestTransferOut;

                if (val == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val != old) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val = @new;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateLargestTransferOut(int? id, string accountNumber, decimal old, decimal @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            if (x == default) throw NotFound(InvalidId, id == null ? 0 : id.Value);
            try
            {
                var val = (await _context.Accounts.SingleOrDefaultAsync(y => y.Number.CompareTo(accountNumber) == 0)).LargestTransferOut;

                if (val == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val != old) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val = @new;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateEntryDate(int? id, string accountNumber, DateTime old, DateTime @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            if (x == default) throw NotFound(InvalidId, id == null ? 0 : id.Value);
            try
            {
                var val = (await _context.Accounts.SingleOrDefaultAsync(y => y.Number.CompareTo(accountNumber) == 0)).EntryDate;

                if (val == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val != old) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val = @new;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateCurrency(int? id, string accountNumber, ICurrency old, ICurrency @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            if (x == default) throw NotFound(InvalidId, id == null ? 0 : id.Value);
            try
            {
                var val = (await _context.Accounts.SingleOrDefaultAsync(y => y.Number.CompareTo(accountNumber) == 0)).Currency;

                if (val == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val.Equals(old)) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val = @new;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateMessageNumber(int? id, string accountNumber, [Phone] string old, [Phone] string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            if (x == default) throw NotFound(InvalidId, id == null ? 0 : id.Value);
            try
            {
                var val = (await _context.Accounts.SingleOrDefaultAsync(y => y.Number.CompareTo(accountNumber) == 0)).SMSAlertList;

                //if (val == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (!val.Contains(old)) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val.Remove(old);
                val.Add(@new);
                //val = @new;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateEmail(int? id, string accountNumber, [EmailAddress] string old, [EmailAddress] string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            if (x == default) throw NotFound(InvalidId, id == null ? 0 : id.Value);
            try
            {
                var val = (await _context.Accounts.SingleOrDefaultAsync(y => y.Number.CompareTo(accountNumber) == 0)).EmailAlertList;

                //if (val == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (!val.Contains(old)) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val.Remove(old);
                val.Add(@new);
                //val = @new;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        //public async Task<bool> UpdateStatus(int? id, string accountNumber, AccountStatusInfo old, AccountStatusInfo @new)
        //{
        //    var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
        //    if (x == default) throw NotFound(InvalidId, id == null ? 0 : id.Value);
        //    try
        //    {
        //        var val = (await _context.Accounts.SingleOrDefaultAsync(y => y.Number.CompareTo(accountNumber) == 0)).Statuses;

        //        //if (val == default) throw new ApplicationStateException(EntityMissing, 0, null);
        //        if (!val.Contains(old)) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
        //        val.Dequeue(old);
        //        val.Enqueue(@new);
        //        //val = @new;

        //        await _context.SaveChangesAsync();

        //        return true;
        //    }
        //    catch (NullReferenceException)
        //    {
        //        throw NotFound(InvalidId, id == null ? 0 : id.Value);
        //    }
        //    catch (DbUpdateConcurrencyException) { return false; }
        //    catch (DbUpdateException) { return false; }
        //}

        public async Task<bool> UpdateCreditIntervalLimit(int? id, string accountNumber, TimeSpan old, TimeSpan @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            if (x == default) throw NotFound(InvalidId, id == null ? 0 : id.Value);
            try
            {
                var val = (await _context.Accounts.SingleOrDefaultAsync(y => y.Number.CompareTo(accountNumber) == 0)).CreditIntervalLimit;

                if (val == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val != old) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val = @new;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateDebitIntervalLimit(int? id, string accountNumber, TimeSpan old, TimeSpan @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            if (x == default) throw NotFound(InvalidId, id == null ? 0 : id.Value);
            try
            {
                var val = (await _context.Accounts.SingleOrDefaultAsync(y => y.Number.CompareTo(accountNumber) == 0)).DebitIntervalLimit;

                if (val == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val != old) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val = @new;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateType(int? id, string accountNumber, AccountType old, AccountType @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            if (x == default) throw NotFound(InvalidId, id == null ? 0 : id.Value);
            try
            {
                var val = (await _context.Accounts.SingleOrDefaultAsync(y => y.Number.CompareTo(accountNumber) == 0)).Type;

                if (val == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val != old) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val = @new;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateGuarantor(int? id, string accountNumber, IPerson old, IPerson @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            if (x == default) throw NotFound(InvalidId, id == null ? 0 : id.Value);
            try
            {
                var val = (await _context.Accounts.SingleOrDefaultAsync(y => y.Number.CompareTo(accountNumber) == 0)).Guarantor;

                if (val == default) throw new ApplicationStateException(EntityMissing, 0, null);
                var old0 = Maps.ReverseMap(@new);
                Maps.MapKeys(old0, val);
                if (val.Equals(old0)) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val = old0;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdatePersonalOnlineKey(int? id, string accountNumber, ILoginCredentials password, ILoginCredentials @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            if (x == default) throw NotFound(InvalidId, id == null ? 0 : id.Value);
            try
            {
                var account = await _context.Accounts.SingleOrDefaultAsync(y => y.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(account)
                    .Collection(c => c.Cards)
                    .Query()
                    .Include(c => c.Secret)
                    .LoadAsync();
                var card = account.Cards.Where(x => Decode(Maps.Map(x.Secret), password).Result).SingleOrDefault();
                var val = await _context.LoginCredentials.SingleOrDefaultAsync(x => x.LoginCredentialModelId == card.SecretLoginCredentialModelId);
                if (val.PersonalOnlineKey == default) throw new ApplicationStateException(EntityMissing, 0, null);
                val.PersonalOnlineKey = @new.PersonalOnlineKey;

                //_context.LoginCredentials.Update(val);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateUsername(int? id, string accountNumber, ILoginCredentials password, ILoginCredentials @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            if (x == default) throw NotFound(InvalidId, id == null ? 0 : id.Value);
            try
            {
                var account = await _context.Accounts.SingleOrDefaultAsync(y => y.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(account)
                    .Collection(c => c.Cards)
                    .Query()
                    .Include(c => c.Secret)
                    .LoadAsync();
                var card = account.Cards.Where(x => Decode(Maps.Map(x.Secret), password).Result).SingleOrDefault();
                var val = await _context.LoginCredentials.SingleOrDefaultAsync(x => x.LoginCredentialModelId == card.SecretLoginCredentialModelId);
                if (val.Username == default) throw new ApplicationStateException(EntityMissing, 0, null);
                val.Username = @new.Username;

                //_context.LoginCredentials.Update(val);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdatePassword(int? id, string accountNumber, ILoginCredentials password, ILoginCredentials @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            if (x == default) throw NotFound(InvalidId, id == null ? 0 : id.Value);
            try
            {
                var account = await _context.Accounts.SingleOrDefaultAsync(y => y.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(account)
                    .Collection(c => c.Cards)
                    .Query()
                    .Include(c => c.Secret)
                    .LoadAsync();
                var card = account.Cards.Where(x => Decode(Maps.Map(x.Secret), password).Result).SingleOrDefault();
                var val = await _context.LoginCredentials.SingleOrDefaultAsync(x => x.LoginCredentialModelId == card.SecretLoginCredentialModelId);
                if (val.Password == default) throw new ApplicationStateException(EntityMissing, 0, null);
                var refer = password.Password;
                refer = @new.Password;
                val.Password = await Utilities.SecretGen(refer);

                //_context.LoginCredentials.Update(val);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateBrandName(int? id, string accountNumber, ILoginCredentials password, IName old, IName @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            if (x == default) throw NotFound(InvalidId, id == null ? 0 : id.Value);
            try
            {
                var account = await _context.Accounts.SingleOrDefaultAsync(y => y.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(account)
                    .Collection(c => c.Cards)
                    .Query()
                    .Include(c => c.Secret)
                    .Include(c => c.Brand)
                    .LoadAsync();
                var card = account.Cards.Where(x => Decode(Maps.Map(x.Secret), password).Result).SingleOrDefault();
                var entity = await _context.Entities.SingleOrDefaultAsync(x => x.EntityModelId == card.BrandEntityModelId);
                var val = await _context.Names.SingleOrDefaultAsync(x => x.NameModelId == entity.NameModelIdId);
                if (val.Name == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val.Name.CompareTo(old.Name) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val.Name = @new.Name;

                _context.Names.Update(val);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateBrandPhone(int? id, string accountNumber, ILoginCredentials password, [Phone] string old, [Phone] string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            if (x == default) throw NotFound(InvalidId, id == null ? 0 : id.Value);
            try
            {
                var account = await _context.Accounts.SingleOrDefaultAsync(y => y.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(account)
                    .Collection(c => c.Cards)
                    .Query()
                    .Include(c => c.Secret)
                    .Include(c => c.Brand)
                    .LoadAsync();
                var card = account.Cards.Where(x => Decode(Maps.Map(x.Secret), password).Result).SingleOrDefault();
                var entity = await _context.Entities.SingleOrDefaultAsync(x => x.EntityModelId == card.BrandEntityModelId);
                var val = await _context.Contacts.SingleOrDefaultAsync(x => x.ContactModelId == entity.ContactModelIdId);
                //if (val.Mobiles == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (!val.Mobiles.Contains(old)) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val.Mobiles.Remove(old);
                val.Mobiles.Add(@new);
                //val.Name = @new.Name;

                //_context.Contacts.Update(val);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateBrandEmail(int? id, string accountNumber, ILoginCredentials password, [EmailAddress] string old, [EmailAddress] string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            if (x == default) throw NotFound(InvalidId, id == null ? 0 : id.Value);
            try
            {
                var account = await _context.Accounts.SingleOrDefaultAsync(y => y.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(account)
                    .Collection(c => c.Cards)
                    .Query()
                    .Include(c => c.Secret)
                    .Include(c => c.Brand)
                    .LoadAsync();
                var card = account.Cards.Where(x => Decode(Maps.Map(x.Secret), password).Result).SingleOrDefault();
                var entity = await _context.Entities.SingleOrDefaultAsync(x => x.EntityModelId == card.BrandEntityModelId);
                var val = await _context.Contacts.SingleOrDefaultAsync(x => x.ContactModelId == entity.ContactModelIdId);
                //if (val.Mobiles == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (!val.Emails.Contains(old)) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val.Emails.Remove(old);
                val.Emails.Add(@new);
                //val.Name = @new.Name;

                //_context.Contacts.Update(val);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateBrandSocialMedia(int? id, string accountNumber, ILoginCredentials password, [Url] string old, [Url] string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            if (x == default) throw NotFound(InvalidId, id == null ? 0 : id.Value);
            try
            {
                var account = await _context.Accounts.SingleOrDefaultAsync(y => y.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(account)
                    .Collection(c => c.Cards)
                    .Query()
                    .Include(c => c.Secret)
                    .Include(c => c.Brand)
                    .LoadAsync();
                var card = account.Cards.Where(x => Decode(Maps.Map(x.Secret), password).Result).SingleOrDefault();
                var entity = await _context.Entities.SingleOrDefaultAsync(x => x.EntityModelId == card.BrandEntityModelId);
                var val = await _context.Contacts.SingleOrDefaultAsync(x => x.ContactModelId == entity.ContactModelIdId);
                //if (val.Mobiles == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (!val.SocialMedia.Contains(old)) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val.SocialMedia.Remove(old);
                val.SocialMedia.Add(@new);
                //val.Name = @new.Name;

                //_context.Contacts.Update(val);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateBrandWebsite(int? id, string accountNumber, ILoginCredentials password, [Url] string old, [Url] string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            if (x == default) throw NotFound(InvalidId, id == null ? 0 : id.Value);
            try
            {
                var account = await _context.Accounts.SingleOrDefaultAsync(y => y.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(account)
                    .Collection(c => c.Cards)
                    .Query()
                    .Include(c => c.Secret)
                    .Include(c => c.Brand)
                    .LoadAsync();
                var card = account.Cards.Where(x => Decode(Maps.Map(x.Secret), password).Result).SingleOrDefault();
                var entity = await _context.Entities.SingleOrDefaultAsync(x => x.EntityModelId == card.BrandEntityModelId);
                var val = await _context.Contacts.SingleOrDefaultAsync(x => x.ContactModelId == entity.ContactModelIdId);
                //if (val.Mobiles == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (!val.Websites.Contains(old)) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val.Websites.Remove(old);
                val.Websites.Add(@new);
                //val.Name = @new.Name;

                //_context.Contacts.Update(val);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateBrandStreetAddress(int? id, string accountNumber, ILoginCredentials password, string old, string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            if (x == default) throw NotFound(InvalidId, id == null ? 0 : id.Value);
            try
            {
                var account = await _context.Accounts.SingleOrDefaultAsync(y => y.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(account)
                    .Collection(c => c.Cards)
                    .Query()
                    .Include(c => c.Secret)
                    .Include(c => c.Brand)
                    .LoadAsync();
                var card = account.Cards.Where(x => Decode(Maps.Map(x.Secret), password).Result).SingleOrDefault();
                var entity = await _context.Entities.SingleOrDefaultAsync(x => x.EntityModelId == card.BrandEntityModelId);
                var contact = await _context.Contacts.SingleOrDefaultAsync(x => x.ContactModelId == entity.ContactModelIdId);
                var val = await _context.Addresses.SingleOrDefaultAsync(x => x.AddressModelId == contact.AddressModelId);
                if (val.StreetAddress == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val.StreetAddress.CompareTo(old) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val.StreetAddress = @new;

                //_context.Addresses.Update(val);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateBrandZIPCode(int? id, string accountNumber, ILoginCredentials password, string old, string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            if (x == default) throw NotFound(InvalidId, id == null ? 0 : id.Value);
            try
            {
                var account = await _context.Accounts.SingleOrDefaultAsync(y => y.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(account)
                    .Collection(c => c.Cards)
                    .Query()
                    .Include(c => c.Secret)
                    .Include(c => c.Brand)
                    .LoadAsync();
                var card = account.Cards.Where(x => Decode(Maps.Map(x.Secret), password).Result).SingleOrDefault();
                var entity = await _context.Entities.SingleOrDefaultAsync(x => x.EntityModelId == card.BrandEntityModelId);
                var contact = await _context.Contacts.SingleOrDefaultAsync(x => x.ContactModelId == entity.ContactModelIdId);
                var val = await _context.Addresses.SingleOrDefaultAsync(x => x.AddressModelId == contact.AddressModelId);
                if (val.ZIPCode == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val.ZIPCode.CompareTo(old) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val.ZIPCode = @new;

                //_context.Addresses.Update(val);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateBrandPMB(int? id, string accountNumber, ILoginCredentials password, string old, string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            if (x == default) throw NotFound(InvalidId, id == null ? 0 : id.Value);
            try
            {
                var account = await _context.Accounts.SingleOrDefaultAsync(y => y.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(account)
                    .Collection(c => c.Cards)
                    .Query()
                    .Include(c => c.Secret)
                    .Include(c => c.Brand)
                    .LoadAsync();
                var card = account.Cards.Where(x => Decode(Maps.Map(x.Secret), password).Result).SingleOrDefault();
                var entity = await _context.Entities.SingleOrDefaultAsync(x => x.EntityModelId == card.BrandEntityModelId);
                var contact = await _context.Contacts.SingleOrDefaultAsync(x => x.ContactModelId == entity.ContactModelIdId);
                var val = await _context.Addresses.SingleOrDefaultAsync(x => x.AddressModelId == contact.AddressModelId);
                if (val.PMB == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val.PMB.CompareTo(old) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val.PMB = @new;

                //_context.Addresses.Update(val);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateBrandCountryOfResidence(int? id, string accountNumber, ILoginCredentials password, IName old, IName @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            if (x == default) throw NotFound(InvalidId, id == null ? 0 : id.Value);
            try
            {
                var account = await _context.Accounts.SingleOrDefaultAsync(y => y.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(account)
                    .Collection(c => c.Cards)
                    .Query()
                    .Include(c => c.Secret)
                    .Include(c => c.Brand)
                    .LoadAsync();
                var card = account.Cards.Where(x => Decode(Maps.Map(x.Secret), password).Result).SingleOrDefault();
                var entity = await _context.Entities.SingleOrDefaultAsync(x => x.EntityModelId == card.BrandEntityModelId);
                var contact = await _context.Contacts.SingleOrDefaultAsync(x => x.ContactModelId == entity.ContactModelIdId);
                var address = await _context.Addresses.SingleOrDefaultAsync(x => x.AddressModelId == contact.AddressModelId);
                var nation = await _context.Nations.SingleOrDefaultAsync(x => x.NationalityModelId == address.CountryOfResidenceNationalityModelId);
                var val = await _context.Names.SingleOrDefaultAsync(x => x.NameModelId == nation.CountryNameNameModelId);

                if (val == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val.Name.CompareTo(old.Name) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val.Name  = @new.Name;

                _context.Names.Update(val);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateBrandStateOfResidence(int? id, string accountNumber, ILoginCredentials password, IName old, IName @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            if (x == default) throw NotFound(InvalidId, id == null ? 0 : id.Value);
            try
            {
                var account = await _context.Accounts.SingleOrDefaultAsync(y => y.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(account)
                    .Collection(c => c.Cards)
                    .Query()
                    .Include(c => c.Secret)
                    .Include(c => c.Brand)
                    .LoadAsync();
                var card = account.Cards.Where(x => Decode(Maps.Map(x.Secret), password).Result).SingleOrDefault();
                var entity = await _context.Entities.SingleOrDefaultAsync(x => x.EntityModelId == card.BrandEntityModelId);
                var contact = await _context.Contacts.SingleOrDefaultAsync(x => x.ContactModelId == entity.ContactModelIdId);
                var address = await _context.Addresses.SingleOrDefaultAsync(x => x.AddressModelId == contact.AddressModelId);
                var nation = await _context.Nations.SingleOrDefaultAsync(x => x.NationalityModelId == address.CountryOfResidenceNationalityModelId);
                var val = await _context.Names.SingleOrDefaultAsync(x => x.NameModelId == nation.StateNameModelId);

                if (val == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val.Name.CompareTo(old.Name) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val.Name = @new.Name;

                _context.Names.Update(val);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateBrandLGAOfResidence(int? id, string accountNumber, ILoginCredentials password, string old, string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            if (x == default) throw NotFound(InvalidId, id == null ? 0 : id.Value);
            try
            {
                var account = await _context.Accounts.SingleOrDefaultAsync(y => y.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(account)
                    .Collection(c => c.Cards)
                    .Query()
                    .Include(c => c.Secret)
                    .Include(c => c.Brand)
                    .LoadAsync();
                var card = account.Cards.Where(x => Decode(Maps.Map(x.Secret), password).Result).SingleOrDefault();
                var entity = await _context.Entities.SingleOrDefaultAsync(x => x.EntityModelId == card.BrandEntityModelId);
                var contact = await _context.Contacts.SingleOrDefaultAsync(x => x.ContactModelId == entity.ContactModelIdId);
                var address = await _context.Addresses.SingleOrDefaultAsync(x => x.AddressModelId == contact.AddressModelId);
                var val = await _context.Nations.SingleOrDefaultAsync(x => x.NationalityModelId == address.CountryOfResidenceNationalityModelId);

                if (val.LGA == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val.LGA.CompareTo(old) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val.LGA = @new;

                //_context.Nations.Update(val);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateBrandLanguage(int? id, string accountNumber, ILoginCredentials password, string old, string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            if (x == default) throw NotFound(InvalidId, id == null ? 0 : id.Value);
            try
            {
                var account = await _context.Accounts.SingleOrDefaultAsync(y => y.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(account)
                    .Collection(c => c.Cards)
                    .Query()
                    .Include(c => c.Secret)
                    .Include(c => c.Brand)
                    .LoadAsync();
                var card = account.Cards.Where(x => Decode(Maps.Map(x.Secret), password).Result).SingleOrDefault();
                var entity = await _context.Entities.SingleOrDefaultAsync(x => x.EntityModelId == card.BrandEntityModelId);
                var contact = await _context.Contacts.SingleOrDefaultAsync(x => x.ContactModelId == entity.ContactModelIdId);
                var address = await _context.Addresses.SingleOrDefaultAsync(x => x.AddressModelId == contact.AddressModelId);
                var val = await _context.Nations.SingleOrDefaultAsync(x => x.NationalityModelId == address.CountryOfResidenceNationalityModelId);

                if (val.LGA == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val.LGA.CompareTo(old) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val.Language = @new;

                //_context.Nations.Update(val);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateBrandCityOfResidence(int? id, string accountNumber, ILoginCredentials password, IName old, IName @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            if (x == default) throw NotFound(InvalidId, id == null ? 0 : id.Value);
            try
            {
                var account = await _context.Accounts.SingleOrDefaultAsync(y => y.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(account)
                    .Collection(c => c.Cards)
                    .Query()
                    .Include(c => c.Secret)
                    .LoadAsync();
                var card = account.Cards.Where(x => Decode(Maps.Map(x.Secret), password).Result).SingleOrDefault();
                var entity = await _context.Entities.SingleOrDefaultAsync(x => x.EntityModelId == card.BrandEntityModelId);
                var contact = await _context.Contacts.SingleOrDefaultAsync(x => x.ContactModelId == entity.ContactModelIdId);
                var address = await _context.Addresses.SingleOrDefaultAsync(x => x.AddressModelId == contact.AddressModelId);
                var nation = await _context.Nations.SingleOrDefaultAsync(x => x.NationalityModelId == address.CountryOfResidenceNationalityModelId);
                var val = await _context.Names.SingleOrDefaultAsync(x => x.NameModelId == nation.CityTownNameModelId);

                if (val == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val.Name.CompareTo(old.Name) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val.Name = @new.Name;

                _context.Names.Update(val);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateBrandCountryOfUse(int? id, string accountNumber, ILoginCredentials password, IName old, IName @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            if (x == default) throw NotFound(InvalidId, id == null ? 0 : id.Value);
            try
            {
                var account = await _context.Accounts.SingleOrDefaultAsync(y => y.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(account)
                    .Collection(c => c.Cards)
                    .Query()
                    .Include(c => c.Secret)
                    .LoadAsync();
                var card = account.Cards.Where(x => Decode(Maps.Map(x.Secret), password).Result).SingleOrDefault();
                var nation = await _context.Nations.SingleOrDefaultAsync(x => x.NationalityModelId == card.CountryOfUseNationalityModelId);
                var val = await _context.Names.SingleOrDefaultAsync(x => x.NameModelId == nation.CountryNameNameModelId);

                if (val == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val.Name.CompareTo(old.Name) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val.Name = @new.Name;

                _context.Names.Update(val);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateBrandStateOfUse(int? id, string accountNumber, ILoginCredentials password, IName old, IName @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            if (x == default) throw NotFound(InvalidId, id == null ? 0 : id.Value);
            try
            {
                var account = await _context.Accounts.SingleOrDefaultAsync(y => y.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(account)
                    .Collection(c => c.Cards)
                    .Query()
                    .Include(c => c.Secret)
                    .LoadAsync();
                var card = account.Cards.Where(x => Decode(Maps.Map(x.Secret), password).Result).SingleOrDefault();
                var nation = await _context.Nations.SingleOrDefaultAsync(x => x.NationalityModelId == card.CountryOfUseNationalityModelId);
                var val = await _context.Names.SingleOrDefaultAsync(x => x.NameModelId == nation.StateNameModelId);

                if (val == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val.Name.CompareTo(old.Name) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val.Name = @new.Name;

                _context.Names.Update(val);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateBrandLGAOfUse(int? id, string accountNumber, ILoginCredentials password, string old, string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            if (x == default) throw NotFound(InvalidId, id == null ? 0 : id.Value);
            try
            {
                var account = await _context.Accounts.SingleOrDefaultAsync(y => y.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(account)
                    .Collection(c => c.Cards)
                    .Query()
                    .Include(c => c.Secret)
                    .LoadAsync();
                var card = account.Cards.Where(x => Decode(Maps.Map(x.Secret), password).Result).SingleOrDefault();
                var val = await _context.Nations.SingleOrDefaultAsync(x => x.NationalityModelId == card.CountryOfUseNationalityModelId);

                if (val.LGA == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val.LGA.CompareTo(old) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val.LGA = @new;

                //_context.Nations.Update(val);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateBrandLanguageOfUse(int? id, string accountNumber, ILoginCredentials password, string old, string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            if (x == default) throw NotFound(InvalidId, id == null ? 0 : id.Value);
            try
            {
                var account = await _context.Accounts.SingleOrDefaultAsync(y => y.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(account)
                    .Collection(c => c.Cards)
                    .Query()
                    .Include(c => c.Secret)
                    .LoadAsync();
                var card = account.Cards.Where(x => Decode(Maps.Map(x.Secret), password).Result).SingleOrDefault();
                var val = await _context.Nations.SingleOrDefaultAsync(x => x.NationalityModelId == card.CountryOfUseNationalityModelId);

                if (val.Language == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val.Language.CompareTo(old) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val.Language = @new;

                //_context.Nations.Update(val);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateBrandCityTownOfUse(int? id, string accountNumber, ILoginCredentials password, IName old, IName @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            if (x == default) throw NotFound(InvalidId, id == null ? 0 : id.Value);
            try
            {
                var account = await _context.Accounts.SingleOrDefaultAsync(y => y.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(account)
                    .Collection(c => c.Cards)
                    .Query()
                    .Include(c => c.Secret)
                    .LoadAsync();
                var card = account.Cards.Where(x => Decode(Maps.Map(x.Secret), password).Result).SingleOrDefault();
                var nation = await _context.Nations.SingleOrDefaultAsync(x => x.NationalityModelId == card.CountryOfUseNationalityModelId);
                var val = await _context.Names.SingleOrDefaultAsync(x => x.NameModelId == nation.CityTownNameModelId);

                if (val == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val.Name.CompareTo(old.Name) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val.Name = @new.Name;

                _context.Names.Update(val);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateCurrencyOfUse(int? id, string accountNumber, ILoginCredentials password, ICurrency old, ICurrency @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            if (x == default) throw NotFound(InvalidId, id == null ? 0 : id.Value);
            try
            {
                var account = await _context.Accounts.SingleOrDefaultAsync(y => y.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(account)
                    .Collection(c => c.Cards)
                    .Query()
                    .Include(c => c.Secret)
                    .LoadAsync();
                var val = account.Cards.Where(x => Decode(Maps.Map(x.Secret), password).Result).SingleOrDefault();

                if (val.Currency == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val.Currency.CompareTo(old) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val.Currency = @new;

                //_context.Cards.Update(val);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateCardType(int? id, string accountNumber, ILoginCredentials password, CardType old, CardType @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            if (x == default) throw NotFound(InvalidId, id == null ? 0 : id.Value);
            try
            {
                var account = await _context.Accounts.SingleOrDefaultAsync(y => y.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(account)
                    .Collection(c => c.Cards)
                    .Query()
                    .Include(c => c.Secret)
                    .LoadAsync();
                var val = account.Cards.Where(x => Decode(Maps.Map(x.Secret), password).Result).SingleOrDefault();

                if (val.Type == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val.Type != old) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val.Type = @new;

                //_context.Cards.Update(val);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateIssuedDate(int? id, string accountNumber, ILoginCredentials password, DateTime old, DateTime @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            if (x == default) throw NotFound(InvalidId, id == null ? 0 : id.Value);
            try
            {
                var account = await _context.Accounts.SingleOrDefaultAsync(y => y.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(account)
                    .Collection(c => c.Cards)
                    .Query()
                    .Include(c => c.Secret)
                    .LoadAsync();
                var val = account.Cards.Where(x => Decode(Maps.Map(x.Secret), password).Result).SingleOrDefault();

                if (val.IssuedDate == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val.IssuedDate != old) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val.IssuedDate = @new;

                //_context.Cards.Update(val);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateExpiryDate(int? id, string accountNumber, ILoginCredentials password, DateTime old, DateTime @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            if (x == default) throw NotFound(InvalidId, id == null ? 0 : id.Value);
            try
            {
                var account = await _context.Accounts.SingleOrDefaultAsync(y => y.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(account)
                    .Collection(c => c.Cards)
                    .Query()
                    .Include(c => c.Secret)
                    .LoadAsync();
                var val = account.Cards.Where(x => Decode(Maps.Map(x.Secret), password).Result).SingleOrDefault();

                if (val.ExpiryDate == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val.ExpiryDate != old) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val.ExpiryDate = @new;

                //_context.Cards.Update(val);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateIssuedCost(int? id, string accountNumber, ILoginCredentials password, decimal old, decimal @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            if (x == default) throw NotFound(InvalidId, id == null ? 0 : id.Value);
            try
            {
                var account = await _context.Accounts.SingleOrDefaultAsync(y => y.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(account)
                    .Collection(c => c.Cards)
                    .Query()
                    .Include(c => c.Secret)
                    .LoadAsync();
                var val = account.Cards.Where(x => Decode(Maps.Map(x.Secret), password).Result).SingleOrDefault();

                if (val.IssuedCost == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val.IssuedCost != old) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val.IssuedCost = @new;

                //_context.Cards.Update(val);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateMonthlyRate(int? id, string accountNumber, ILoginCredentials password, decimal old, decimal @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            if (x == default) throw NotFound(InvalidId, id == null ? 0 : id.Value);
            try
            {
                var account = await _context.Accounts.SingleOrDefaultAsync(y => y.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(account)
                    .Collection(c => c.Cards)
                    .Query()
                    .Include(c => c.Secret)
                    .LoadAsync();
                var val = account.Cards.Where(x => Decode(Maps.Map(x.Secret), password).Result).SingleOrDefault();

                if (val.MonthlyRate == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val.MonthlyRate != old) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val.MonthlyRate = @new;

                //_context.Cards.Update(val);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateWithdrawalLimit(int? id, string accountNumber, ILoginCredentials password, decimal old, decimal @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            if (x == default) throw NotFound(InvalidId, id == null ? 0 : id.Value);
            try
            {
                var account = await _context.Accounts.SingleOrDefaultAsync(y => y.Number.CompareTo(accountNumber) == 0);
                await _context.Entry(account)
                    .Collection(c => c.Cards)
                    .Query()
                    .Include(c => c.Secret)
                    .LoadAsync();
                var val = account.Cards.Where(x => Decode(Maps.Map(x.Secret), password).Result).SingleOrDefault();

                if (val.WithdrawalLimit == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val.WithdrawalLimit != old) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val.WithdrawalLimit = @new;

                //_context.Cards.Update(val);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateBVN(int? id, string old, string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            try
            {
                if (x.BVN == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (x.BVN != old) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                x.BVN = @new;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateEntryDate(int? id, DateTime old, DateTime @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            try
            {
                if (x.EntryDate == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (x.EntryDate != old) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                x.EntryDate = @new;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdatePersonalOnlineKey(int? id, ILoginCredentials password, ILoginCredentials @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            try
            {
                var val = await _context.LoginCredentials.SingleOrDefaultAsync(l => l.LoginCredentialModelId == x.LoginCredentialModelId);
                if (val.PersonalOnlineKey == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val.PersonalOnlineKey.CompareTo(password.PersonalOnlineKey) != 0 || !Decode2(Maps.Map(val), password)) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val.PersonalOnlineKey = @new.PersonalOnlineKey;

                //_context.LoginCredentials.Update(val);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateUsername(int? id, ILoginCredentials password, ILoginCredentials @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            try
            {
                var val = await _context.LoginCredentials.SingleOrDefaultAsync(l => l.LoginCredentialModelId == x.LoginCredentialModelId);
                if (val.Username == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val.Username.CompareTo(password.Username) != 0 || !Decode2(Maps.Map(val), password)) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val.Username = @new.Username;

                //_context.LoginCredentials.Update(val);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdatePassword(int? id, ILoginCredentials old, ILoginCredentials @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            try
            {
                var val = await _context.LoginCredentials.SingleOrDefaultAsync(l => l.LoginCredentialModelId == x.LoginCredentialModelId);
                if (val.Password == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (!Decode2(Maps.Map(val), old)) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                var refer = @new.Password;
                val.Password = Utilities.SecretGen(x.EntryDate, Guid.NewGuid(), ref refer);

                //_context.LoginCredentials.Update(val);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateIdentification(int? id, string idType, string old, string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(x => x.CustomerModelId == id);
            try
            {
                var v = await _context.Persons.SingleOrDefaultAsync(p => p.PersonModelId == x.PersonModelId);
                if (!v.Identification.ContainsKey(idType))
                    throw new ApplicationArgumentException($"Argument \"idType\" did not match any found", id);
                try
                {
                    if (v.Identification[idType].CompareTo(old) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                    v.Identification[idType] = @new;

                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (KeyNotFoundException ex)
                {
                    throw new ApplicationArgumentException("Something went wrong when setting the grade in the dictionary", x.CustomerModelId == null ? 0 : x.CustomerModelId.Value, ex);
                }
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateFingerprint(int? id, byte[] old, byte[] @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            try
            {
                var person = await _context.Persons.SingleOrDefaultAsync(p => p.PersonModelId == x.PersonModelId);
                var val = person.FingerPrint;

                if (val == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (!Array.Equals(val, old)) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                person.FingerPrint = @new;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateSignature(int? id, byte[] old, byte[] @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            try
            {
                var person = await _context.Persons.SingleOrDefaultAsync(p => p.PersonModelId == x.PersonModelId);
                var val = person.Signature;

                if (val == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (!Array.Equals(val, old)) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                person.Signature = @new;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdatePassport(int? id, byte[] old, byte[] @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            try
            {
                var person = await _context.Persons.SingleOrDefaultAsync(p => p.PersonModelId == x.PersonModelId);
                var val = person.Passport;

                if (val == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (!Array.Equals(val, old)) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                person.Passport = @new;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateJobType(int? id, string old, string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            try
            {
                var person = await _context.Persons.SingleOrDefaultAsync(p => p.PersonModelId == x.PersonModelId);
                var val = person.JobType;

                if (val == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val.CompareTo(old) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                person.JobType = @new;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateSex(int? id, bool old, bool @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            try
            {
                var person = await _context.Persons.SingleOrDefaultAsync(p => p.PersonModelId == x.PersonModelId);
                var val = person.IsMale;

                //if (val == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val != old) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                person.IsMale = @new;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateUniqueTag(int? id, Guid old, Guid @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            try
            {
                var person = await _context.Persons.SingleOrDefaultAsync(p => p.PersonModelId == x.PersonModelId);
                var val = person.UniqueTag;

                if (val == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val != old) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                person.UniqueTag = @new;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateBirthDate(int? id, DateTime old, DateTime @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            try
            {
                var person = await _context.Persons.SingleOrDefaultAsync(p => p.PersonModelId == x.PersonModelId);
                var val = person.BirthDate;

                if (val == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val.CompareTo(old) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                person.BirthDate = @new;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateCountryOfOrigin(int? id, IName old, IName @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            try
            {
                var person = await _context.Persons.SingleOrDefaultAsync(p => p.PersonModelId == x.PersonModelId);
                var country = await _context.Nations.SingleOrDefaultAsync(c => c.NationalityModelId == person.CountryOfOriginNationalityModelId);
                var name = await _context.Names.SingleOrDefaultAsync(n => n.NameModelId == country.CountryNameNameModelId);

                if (name == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (name.Name.CompareTo(old.Name) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                var n = Maps.ReverseMap(@new);
                Maps.MapKeys(n, name);

                _context.Names.Update(n);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateStateOfOrigin(int? id, IName old, IName @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            try
            {
                var person = await _context.Persons.SingleOrDefaultAsync(p => p.PersonModelId == x.PersonModelId);
                var country = await _context.Nations.SingleOrDefaultAsync(c => c.NationalityModelId == person.CountryOfOriginNationalityModelId);
                var name = await _context.Names.SingleOrDefaultAsync(n => n.NameModelId == country.StateNameModelId);

                if (name == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (name.Name.CompareTo(old.Name) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                var n = Maps.ReverseMap(@new);
                Maps.MapKeys(n, name);

                _context.Names.Update(n);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateTown_CityOfOrigin(int? id, IName old, IName @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            try
            {
                var person = await _context.Persons.SingleOrDefaultAsync(p => p.PersonModelId == x.PersonModelId);
                var country = await _context.Nations.SingleOrDefaultAsync(c => c.NationalityModelId == person.CountryOfOriginNationalityModelId);
                var name = await _context.Names.SingleOrDefaultAsync(n => n.NameModelId == country.CityTownNameModelId);

                if (name == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (name.Name.CompareTo(old.Name) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                var n = Maps.ReverseMap(@new);
                Maps.MapKeys(n, name);

                _context.Names.Update(n);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateLGAOfOrigin(int? id, string old, string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            try
            {
                var person = await _context.Persons.SingleOrDefaultAsync(p => p.PersonModelId == x.PersonModelId);
                var country = await _context.Nations.SingleOrDefaultAsync(c => c.NationalityModelId == person.CountryOfOriginNationalityModelId);
                var val = country.LGA;

                if (val == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val.CompareTo(old) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                country.LGA = @new;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateLanguage(int? id, string old, string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            try
            {
                var person = await _context.Persons.SingleOrDefaultAsync(p => p.PersonModelId == x.PersonModelId);
                var country = await _context.Nations.SingleOrDefaultAsync(c => c.NationalityModelId == person.CountryOfOriginNationalityModelId);
                var val = country.Language;

                if (val == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val.CompareTo(old) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                country.LGA = @new;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateNextOfKin(int? id, IPerson old, IPerson @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            try
            {
                var person = await _context.Persons.SingleOrDefaultAsync(p => p.PersonModelId == x.PersonModelId);
                var val = person.NextOfKin;

                if (val == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (!Maps.Map(val).Equals(old)) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                person.NextOfKin = Maps.ReverseMap(@new);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateMaritalStatus(int? id, MaritalStatus old, MaritalStatus @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            try
            {
                var person = await _context.Persons.SingleOrDefaultAsync(p => p.PersonModelId == x.PersonModelId);
                var val = person.MaritalStatus;

                //if (val == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val != old) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                person.MaritalStatus = @new;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateBusinessName(int? id, IName old, IName @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            try
            {
                var person = await _context.Persons.SingleOrDefaultAsync(p => p.PersonModelId == x.PersonModelId);
                var entity = await _context.Entities.SingleOrDefaultAsync(e => e.EntityModelId == person.OfficialEntityEntityModelId);
                var name = await _context.Names.SingleOrDefaultAsync(n => n.NameModelId == entity.NameModelIdId);

                if (name == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (!Maps.Map(name).Equals(old)) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                var name2 = Maps.ReverseMap(@new);
                Maps.MapKeys(name2, name);

                _context.Names.Update(name2);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateBusinessEmail(int? id, [EmailAddress] string old, [EmailAddress] string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            try
            {
                var person = await _context.Persons.SingleOrDefaultAsync(p => p.PersonModelId == x.PersonModelId);
                var entity = await _context.Entities.SingleOrDefaultAsync(e => e.EntityModelId == person.OfficialEntityEntityModelId);
                var contact = await _context.Contacts.SingleOrDefaultAsync(c => c.ContactModelId == entity.ContactModelIdId);

                if (!contact.Emails.Contains(old)) throw new ApplicationStateException(EntityMissing, 0, null);
                //if (!Maps.Map(name).Equals(old)) throw new ApplicationArgumentException(MismatchedEntities, 0, null);

                contact.Emails.Remove(old);
                contact.Emails.Add(@new);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateBusinessMobile(int? id, [Phone] string old, [Phone] string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            try
            {
                var person = await _context.Persons.SingleOrDefaultAsync(p => p.PersonModelId == x.PersonModelId);
                var entity = await _context.Entities.SingleOrDefaultAsync(e => e.EntityModelId == person.OfficialEntityEntityModelId);
                var contact = await _context.Contacts.SingleOrDefaultAsync(c => c.ContactModelId == entity.ContactModelIdId);

                if (!contact.Mobiles.Contains(old)) throw new ApplicationStateException(EntityMissing, 0, null);
                //if (!Maps.Map(name).Equals(old)) throw new ApplicationArgumentException(MismatchedEntities, 0, null);

                contact.Mobiles.Remove(old);
                contact.Mobiles.Add(@new);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateBusinessSocialMedia(int? id, [Url] string old, [Url] string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            try
            {
                var person = await _context.Persons.SingleOrDefaultAsync(p => p.PersonModelId == x.PersonModelId);
                var entity = await _context.Entities.SingleOrDefaultAsync(e => e.EntityModelId == person.OfficialEntityEntityModelId);
                var contact = await _context.Contacts.SingleOrDefaultAsync(c => c.ContactModelId == entity.ContactModelIdId);

                if (!contact.SocialMedia.Contains(old)) throw new ApplicationStateException(EntityMissing, 0, null);
                //if (!Maps.Map(name).Equals(old)) throw new ApplicationArgumentException(MismatchedEntities, 0, null);

                contact.SocialMedia.Remove(old);
                contact.SocialMedia.Add(@new);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateBusinessWebsite(int? id, [Url] string old, [Url] string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            try
            {
                var person = await _context.Persons.SingleOrDefaultAsync(p => p.PersonModelId == x.PersonModelId);
                var entity = await _context.Entities.SingleOrDefaultAsync(e => e.EntityModelId == person.OfficialEntityEntityModelId);
                var contact = await _context.Contacts.SingleOrDefaultAsync(c => c.ContactModelId == entity.ContactModelIdId);

                if (!contact.Websites.Contains(old)) throw new ApplicationStateException(EntityMissing, 0, null);
                //if (!Maps.Map(name).Equals(old)) throw new ApplicationArgumentException(MismatchedEntities, 0, null);

                contact.Websites.Remove(old);
                contact.Websites.Add(@new);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateBusinessStreetAddress(int? id, string old, string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            try
            {
                var person = await _context.Persons.SingleOrDefaultAsync(p => p.PersonModelId == x.PersonModelId);
                var entity = await _context.Entities.SingleOrDefaultAsync(e => e.EntityModelId == person.OfficialEntityEntityModelId);
                var contact = await _context.Contacts.SingleOrDefaultAsync(c => c.ContactModelId == entity.ContactModelIdId);
                var address = await _context.Addresses.SingleOrDefaultAsync(a => a.AddressModelId == contact.AddressModelId);

                if (address.StreetAddress == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (address.StreetAddress.CompareTo(old) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);

                address.StreetAddress = @new;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateBusinessZIPCode(int? id, string old, string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            try
            {
                var person = await _context.Persons.SingleOrDefaultAsync(p => p.PersonModelId == x.PersonModelId);
                var entity = await _context.Entities.SingleOrDefaultAsync(e => e.EntityModelId == person.OfficialEntityEntityModelId);
                var contact = await _context.Contacts.SingleOrDefaultAsync(c => c.ContactModelId == entity.ContactModelIdId);
                var address = await _context.Addresses.SingleOrDefaultAsync(a => a.AddressModelId == contact.AddressModelId);

                if (address.ZIPCode == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (address.ZIPCode.CompareTo(old) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);

                address.ZIPCode = @new;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateBusinessPMB(int? id, string old, string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            try
            {
                var person = await _context.Persons.SingleOrDefaultAsync(p => p.PersonModelId == x.PersonModelId);
                var entity = await _context.Entities.SingleOrDefaultAsync(e => e.EntityModelId == person.OfficialEntityEntityModelId);
                var contact = await _context.Contacts.SingleOrDefaultAsync(c => c.ContactModelId == entity.ContactModelIdId);
                var address = await _context.Addresses.SingleOrDefaultAsync(a => a.AddressModelId == contact.AddressModelId);

                if (address.PMB == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (address.PMB.CompareTo(old) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);

                address.PMB = @new;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateBusinessCountryOfResidence(int? id, IName old, IName @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            try
            {
                var person = await _context.Persons.SingleOrDefaultAsync(p => p.PersonModelId == x.PersonModelId);
                var entity = await _context.Entities.SingleOrDefaultAsync(e => e.EntityModelId == person.OfficialEntityEntityModelId);
                var contact = await _context.Contacts.SingleOrDefaultAsync(c => c.ContactModelId == entity.ContactModelIdId);
                var address = await _context.Addresses.SingleOrDefaultAsync(a => a.AddressModelId == contact.AddressModelId);
                var country = await _context.Nations.SingleOrDefaultAsync(c => c.NationalityModelId == address.CountryOfResidenceNationalityModelId);
                var name = await _context.Names.SingleOrDefaultAsync(n => n.NameModelId == country.CountryNameNameModelId);

                if (name == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (name.Name.CompareTo(old.Name) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);

                var val = Maps.ReverseMap(@new);
                Maps.MapKeys(val, name);
                name = val;

                _context.Names.Update(val);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateBusinessStateOfResidence(int? id, IName old, IName @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            try
            {
                var person = await _context.Persons.SingleOrDefaultAsync(p => p.PersonModelId == x.PersonModelId);
                var entity = await _context.Entities.SingleOrDefaultAsync(e => e.EntityModelId == person.OfficialEntityEntityModelId);
                var contact = await _context.Contacts.SingleOrDefaultAsync(c => c.ContactModelId == entity.ContactModelIdId);
                var address = await _context.Addresses.SingleOrDefaultAsync(a => a.AddressModelId == contact.AddressModelId);
                var country = await _context.Nations.SingleOrDefaultAsync(c => c.NationalityModelId == address.CountryOfResidenceNationalityModelId);
                var name = await _context.Names.SingleOrDefaultAsync(n => n.NameModelId == country.StateNameModelId);

                if (name == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (name.Name.CompareTo(old.Name) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);

                var val = Maps.ReverseMap(@new);
                Maps.MapKeys(val, name);
                name = val;

                _context.Names.Update(val);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateBusinessTown_CityOfResidence(int? id, IName old, IName @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            try
            {
                var person = await _context.Persons.SingleOrDefaultAsync(p => p.PersonModelId == x.PersonModelId);
                var entity = await _context.Entities.SingleOrDefaultAsync(e => e.EntityModelId == person.OfficialEntityEntityModelId);
                var contact = await _context.Contacts.SingleOrDefaultAsync(c => c.ContactModelId == entity.ContactModelIdId);
                var address = await _context.Addresses.SingleOrDefaultAsync(a => a.AddressModelId == contact.AddressModelId);
                var country = await _context.Nations.SingleOrDefaultAsync(c => c.NationalityModelId == address.CountryOfResidenceNationalityModelId);
                var name = await _context.Names.SingleOrDefaultAsync(n => n.NameModelId == country.CityTownNameModelId);

                if (name == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (name.Name.CompareTo(old.Name) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);

                var val = Maps.ReverseMap(@new);
                Maps.MapKeys(val, name);
                name = val;

                _context.Names.Update(val);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateBusinessLGAOfResidence(int? id, string old, string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            try
            {
                var person = await _context.Persons.SingleOrDefaultAsync(p => p.PersonModelId == x.PersonModelId);
                var entity = await _context.Entities.SingleOrDefaultAsync(e => e.EntityModelId == person.OfficialEntityEntityModelId);
                var contact = await _context.Contacts.SingleOrDefaultAsync(c => c.ContactModelId == entity.ContactModelIdId);
                var address = await _context.Addresses.SingleOrDefaultAsync(a => a.AddressModelId == contact.AddressModelId);
                var country = await _context.Nations.SingleOrDefaultAsync(c => c.NationalityModelId == address.CountryOfResidenceNationalityModelId);

                if (country.LGA == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (country.LGA.CompareTo(old) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);

                country.LGA = @new;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateDisplayName(int? id, IName old, IName @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            try
            {
                var person = await _context.Persons.SingleOrDefaultAsync(p => p.PersonModelId == x.PersonModelId);
                var entity = await _context.Entities.SingleOrDefaultAsync(e => e.EntityModelId == person.EntityModelId);
                var name = await _context.Names.SingleOrDefaultAsync(n => n.NameModelId == entity.NameModelIdId);

                if (name == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (!Maps.Map(name).Equals(old)) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                var name2 = Maps.ReverseMap(@new);
                Maps.MapKeys(name2, name);

                _context.Names.Update(name2);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateEmail(int? id, [EmailAddress] string old, [EmailAddress] string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            try
            {
                var person = await _context.Persons.SingleOrDefaultAsync(p => p.PersonModelId == x.PersonModelId);
                var entity = await _context.Entities.SingleOrDefaultAsync(e => e.EntityModelId == person.EntityModelId);
                var contact = await _context.Contacts.SingleOrDefaultAsync(c => c.ContactModelId == entity.ContactModelIdId);

                if (!contact.Emails.Contains(old)) throw new ApplicationStateException(EntityMissing, 0, null);
                //if (!Maps.Map(name).Equals(old)) throw new ApplicationArgumentException(MismatchedEntities, 0, null);

                contact.Emails.Remove(old);
                contact.Emails.Add(@new);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateMobile(int? id, [Phone] string old, [Phone] string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            try
            {
                var person = await _context.Persons.SingleOrDefaultAsync(p => p.PersonModelId == x.PersonModelId);
                var entity = await _context.Entities.SingleOrDefaultAsync(e => e.EntityModelId == person.EntityModelId);
                var contact = await _context.Contacts.SingleOrDefaultAsync(c => c.ContactModelId == entity.ContactModelIdId);

                if (!contact.Mobiles.Contains(old)) throw new ApplicationStateException(EntityMissing, 0, null);
                //if (!Maps.Map(name).Equals(old)) throw new ApplicationArgumentException(MismatchedEntities, 0, null);

                contact.Mobiles.Remove(old);
                contact.Mobiles.Add(@new);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateSocialMedia(int? id, [Url] string old, [Url] string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            try
            {
                var person = await _context.Persons.SingleOrDefaultAsync(p => p.PersonModelId == x.PersonModelId);
                var entity = await _context.Entities.SingleOrDefaultAsync(e => e.EntityModelId == person.EntityModelId);
                var contact = await _context.Contacts.SingleOrDefaultAsync(c => c.ContactModelId == entity.ContactModelIdId);

                if (!contact.SocialMedia.Contains(old)) throw new ApplicationStateException(EntityMissing, 0, null);
                //if (!Maps.Map(name).Equals(old)) throw new ApplicationArgumentException(MismatchedEntities, 0, null);

                contact.SocialMedia.Remove(old);
                contact.SocialMedia.Add(@new);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateWebsite(int? id, [Url] string old, [Url] string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            try
            {
                var person = await _context.Persons.SingleOrDefaultAsync(p => p.PersonModelId == x.PersonModelId);
                var entity = await _context.Entities.SingleOrDefaultAsync(e => e.EntityModelId == person.EntityModelId);
                var contact = await _context.Contacts.SingleOrDefaultAsync(c => c.ContactModelId == entity.ContactModelIdId);

                if (!contact.Websites.Contains(old)) throw new ApplicationStateException(EntityMissing, 0, null);
                //if (!Maps.Map(name).Equals(old)) throw new ApplicationArgumentException(MismatchedEntities, 0, null);

                contact.Websites.Remove(old);
                contact.Websites.Add(@new);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateStreetAddress(int? id, string old, string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            try
            {
                var person = await _context.Persons.SingleOrDefaultAsync(p => p.PersonModelId == x.PersonModelId);
                var entity = await _context.Entities.SingleOrDefaultAsync(e => e.EntityModelId == person.EntityModelId);
                var contact = await _context.Contacts.SingleOrDefaultAsync(c => c.ContactModelId == entity.ContactModelIdId);
                var address = await _context.Addresses.SingleOrDefaultAsync(a => a.AddressModelId == contact.AddressModelId);

                if (address.StreetAddress == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (address.StreetAddress.CompareTo(old) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);

                address.StreetAddress = @new;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateZIPCode(int? id, string old, string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            try
            {
                var person = await _context.Persons.SingleOrDefaultAsync(p => p.PersonModelId == x.PersonModelId);
                var entity = await _context.Entities.SingleOrDefaultAsync(e => e.EntityModelId == person.EntityModelId);
                var contact = await _context.Contacts.SingleOrDefaultAsync(c => c.ContactModelId == entity.ContactModelIdId);
                var address = await _context.Addresses.SingleOrDefaultAsync(a => a.AddressModelId == contact.AddressModelId);

                if (address.ZIPCode == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (address.ZIPCode.CompareTo(old) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);

                address.ZIPCode = @new;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdatePMB(int? id, string old, string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            try
            {
                var person = await _context.Persons.SingleOrDefaultAsync(p => p.PersonModelId == x.PersonModelId);
                var entity = await _context.Entities.SingleOrDefaultAsync(e => e.EntityModelId == person.EntityModelId);
                var contact = await _context.Contacts.SingleOrDefaultAsync(c => c.ContactModelId == entity.ContactModelIdId);
                var address = await _context.Addresses.SingleOrDefaultAsync(a => a.AddressModelId == contact.AddressModelId);

                if (address.PMB == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (address.PMB.CompareTo(old) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);

                address.PMB = @new;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateCountryOfResidence(int? id, IName old, IName @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            try
            {
                var person = await _context.Persons.SingleOrDefaultAsync(p => p.PersonModelId == x.PersonModelId);
                var entity = await _context.Entities.SingleOrDefaultAsync(e => e.EntityModelId == person.EntityModelId);
                var contact = await _context.Contacts.SingleOrDefaultAsync(c => c.ContactModelId == entity.ContactModelIdId);
                var address = await _context.Addresses.SingleOrDefaultAsync(a => a.AddressModelId == contact.AddressModelId);
                var country = await _context.Nations.SingleOrDefaultAsync(c => c.NationalityModelId == address.CountryOfResidenceNationalityModelId);
                var name = await _context.Names.SingleOrDefaultAsync(n => n.NameModelId == country.CountryNameNameModelId);

                if (name == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (name.Name.CompareTo(old.Name) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);

                var val = Maps.ReverseMap(@new);
                Maps.MapKeys(val, name);
                name = val;

                _context.Names.Update(val);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateStateOfResidence(int? id, IName old, IName @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            try
            {
                var person = await _context.Persons.SingleOrDefaultAsync(p => p.PersonModelId == x.PersonModelId);
                var entity = await _context.Entities.SingleOrDefaultAsync(e => e.EntityModelId == person.EntityModelId);
                var contact = await _context.Contacts.SingleOrDefaultAsync(c => c.ContactModelId == entity.ContactModelIdId);
                var address = await _context.Addresses.SingleOrDefaultAsync(a => a.AddressModelId == contact.AddressModelId);
                var country = await _context.Nations.SingleOrDefaultAsync(c => c.NationalityModelId == address.CountryOfResidenceNationalityModelId);
                var name = await _context.Names.SingleOrDefaultAsync(n => n.NameModelId == country.StateNameModelId);

                if (name == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (name.Name.CompareTo(old.Name) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);

                var val = Maps.ReverseMap(@new);
                Maps.MapKeys(val, name);
                name = val;

                _context.Names.Update(val);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateTown_CityOfResidence(int? id, IName old, IName @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            try
            {
                var person = await _context.Persons.SingleOrDefaultAsync(p => p.PersonModelId == x.PersonModelId);
                var entity = await _context.Entities.SingleOrDefaultAsync(e => e.EntityModelId == person.EntityModelId);
                var contact = await _context.Contacts.SingleOrDefaultAsync(c => c.ContactModelId == entity.ContactModelIdId);
                var address = await _context.Addresses.SingleOrDefaultAsync(a => a.AddressModelId == contact.AddressModelId);
                var country = await _context.Nations.SingleOrDefaultAsync(c => c.NationalityModelId == address.CountryOfResidenceNationalityModelId);
                var name = await _context.Names.SingleOrDefaultAsync(n => n.NameModelId == country.CityTownNameModelId);

                if (name == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (name.Name.CompareTo(old.Name) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);

                var val = Maps.ReverseMap(@new);
                Maps.MapKeys(val, name);
                name = val;

                _context.Names.Update(val);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateLGAOfResidence(int? id, string old, string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(val => val.CustomerModelId == id);
            try
            {
                var person = await _context.Persons.SingleOrDefaultAsync(p => p.PersonModelId == x.PersonModelId);
                var entity = await _context.Entities.SingleOrDefaultAsync(e => e.EntityModelId == person.EntityModelId);
                var contact = await _context.Contacts.SingleOrDefaultAsync(c => c.ContactModelId == entity.ContactModelIdId);
                var address = await _context.Addresses.SingleOrDefaultAsync(a => a.AddressModelId == contact.AddressModelId);
                var country = await _context.Nations.SingleOrDefaultAsync(c => c.NationalityModelId == address.CountryOfResidenceNationalityModelId);

                if (country.LGA == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (country.LGA.CompareTo(old) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);

                country.LGA = @new;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> InvalidateToken(int? id, string @new)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.LoginCredential)
                    .LoadAsync();
                var res = x.LoginCredential.Tokens.Remove(@new);

                await _context.SaveChangesAsync();
                return res;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> InvalidateTokens(int? id)
        {
            var x = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerModelId == id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.LoginCredential)
                    .LoadAsync();
                await x.LoginCredential.InvalidateTokens();

                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id == null ? 0 : id.Value);
            }
            catch (InvalidOperationException) { throw; }
            catch (DbUpdateConcurrencyException) { return default; }
            catch (DbUpdateException) { return default; }
        }

        public async Task<bool> RemoveAccount(int? id, IAccount acc)
        {
            var x = await _context.Customers.FindAsync(id);
            await _context.Entry(x)
                .Collection(x => x.Accounts)
                .LoadAsync();
            try
            {
                //data.Person.OfficialEntity = Maps.ReverseMap(official);
                int i = 0;
                foreach (var n in x.Accounts)
                {
                    if (n.Number.CompareTo(acc.Number) == 0)
                    { 
                        x.Accounts.RemoveAt(i);
                        //_context.Update(n);
                        break;
                    }
                    i++;
                }
                //_context.Update<CustomerModel>(x);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (NullReferenceException)
            {
                return false;
            }
        }

        public async Task<bool> ValidatePassword(int? id, ILoginCredentials value)
        {
            try
            {
                //var x = await _context.FindAsync<EmployeeModel>(Int32.Parse(e.DatabaseId));
                var x = await _context.Customers.FindAsync(id);
                await _context.Entry(x)
                    .Reference(x => x.LoginCredential)
                    .LoadAsync();
                var secret = x.LoginCredential.Password;
                //var refer = Utilities.ToString(Convert.FromBase64String(secret));
                var refer = Encoding.UTF8.GetString(Convert.FromBase64String(secret));
                var decoded = DataBaseTest.Utilities.Decrypt(ref refer);
                return decoded.CompareTo(value.Password) == 0;
            }
            catch (NullReferenceException) { return false; }
            finally { value = null; }
        }

        public async Task<bool> ValidateCardPassword(int? id, string num, string cardId, ILoginCredentials val)
        {
            try
            {
                //var x = await _context.FindAsync<EmployeeModel>(Int32.Parse(e.DatabaseId));
                var x = await _context.Customers.FindAsync(id);
                await _context.Entry(x)
                    .Collection(x => x.Accounts)
                    .Query()
                    .Where(x => x.Number.CompareTo(num) == 0)
                    .Include(x => x.Cards)
                    .ThenInclude(x => x.Secret)
                    .LoadAsync();
                var secret = x.Accounts.FirstOrDefault().Cards.Where(x=>x.Secret.PersonalOnlineKey.CompareTo(cardId) == 0).FirstOrDefault().Secret.Password;
                //var refer = Utilities.ToString(Convert.FromBase64String(secret));
                var refer = secret.ToCharArray(); //Encoding.UTF8.GetString(Convert.FromBase64String(secret));
                var decoded = await Utilities.Decrypt(refer);
                return decoded.CompareTo(val.Password) == 0;
            }
            catch (NullReferenceException) { return false; }
            finally { val = null; }
        }

        public async Task<bool> ValidateAndChangePassword(int? id, ILoginCredentials oldValue, ILoginCredentials newValue)
        {
            if (newValue == null)
                throw new NullReferenceException("newValue cannot be null");
            try
            {
                //var x = await _context.FindAsync<EmployeeModel>(Int32.Parse(e.DatabaseId));
                var x = await _context.Customers.FindAsync(id);
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.CountryOfOrigin)
                    .ThenInclude(x => x.CountryName)
                    .Include(x => x.Entity)
                    .ThenInclude(x => x.Contact)
                    .ThenInclude(x => x.Address)
                    .ThenInclude(x => x.CountryOfResidence)
                    .ThenInclude(x => x.CountryName)
                    .Include(x => x.OfficialEntity)
                    .ThenInclude(x => x.Contact)
                    .ThenInclude(x => x.Address)
                    .ThenInclude(x => x.CountryOfResidence)
                    .ThenInclude(x => x.CountryName)
                    .Include(x => x.FullName)
                    .ThenInclude(x => x.Name)
                    .LoadAsync();
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.CountryOfOrigin)
                    .ThenInclude(x => x.State)
                    .Include(x => x.Entity)
                    .ThenInclude(x => x.Name)
                    .Include(x => x.OfficialEntity)
                    .ThenInclude(x => x.Name)
                    .Include(x => x.FullName)
                    .ThenInclude(x => x.Surname)
                    .LoadAsync();
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.CountryOfOrigin)
                    .ThenInclude(x => x.CityTown)
                    .Include(x => x.Entity)
                    .ThenInclude(x => x.Contact)
                    .ThenInclude(x => x.Address)
                    .ThenInclude(x => x.CountryOfResidence)
                    .ThenInclude(x => x.State)
                    .Include(x => x.OfficialEntity)
                    .ThenInclude(x => x.Contact)
                    .ThenInclude(x => x.Address)
                    .ThenInclude(x => x.CountryOfResidence)
                    .ThenInclude(x => x.State)
                    .Include(x => x.FullName)
                    .ThenInclude(x => x.MaidenName)
                    .LoadAsync();
                await _context.Entry(x)
                    .Reference(x => x.LoginCredential)
                    .LoadAsync();
                if (x.LoginCredential.Password != null && oldValue == null)
                    throw new ApplicationException("Password already exists");
                else if (oldValue == null)
                {
                    /*This should be done at the creation of the ilogincredentials object*/
                    //val.Secret.Password = Convert.ToBase64String(Encoding.UTF8.GetBytes(newValue.Password));
                    var reference = newValue.Password;
                    x.LoginCredential.Password = Utilities.SecretGen(x.EntryDate, Maps.Map(x.Person), ref reference);
                    //_context.Entry(val.Secret).State = EntityState.Modified;
                    //_context.Update(x);
                    _context.Update(x.LoginCredential);
                    await _context.SaveChangesAsync();
                    return true;
                }
                if (await ValidatePassword(id, oldValue))
                {
                    /*This should be done at the creation of the ilogincredentials object*/
                    //val.Secret.Password = Convert.ToBase64String(Encoding.UTF8.GetBytes(newValue.Password));
                    var reference = newValue.Password;
                    x.LoginCredential.Password = Utilities.SecretGen(x.EntryDate, Maps.Map(x.Person), ref reference);
                    //_context.Entry(val.Secret).State = EntityState.Modified;
                    //_context.Update(x);
                    _context.Update(x.LoginCredential);
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            catch (NullReferenceException)
            {
                return false;
            }
            finally { oldValue = null; newValue = null; }

            return false;
        }

        public async Task<bool> ValidateAndChangeCardPassword(int? id, string num, string cardId, ILoginCredentials old, ILoginCredentials nw)
        {
            if (nw == null)
                return false;
            try
            {
                //var x = await _context.FindAsync<EmployeeModel>(Int32.Parse(e.DatabaseId));
                var y = await _context.Customers.FindAsync(id);
                await _context.Entry(y)
                    .Collection(x => x.Accounts)
                    .Query()
                    .Where(x => x.Number.CompareTo(num) == 0)
                    .Include(x => x.Cards)
                    .ThenInclude(x => x.Secret)
                    .LoadAsync();
                var x = y.Accounts.FirstOrDefault().Cards.Where(x => x.Secret.PersonalOnlineKey.CompareTo(cardId) == 0).FirstOrDefault();
                if (old == null)
                {
                    if (x.Secret.Password != null)
                        if (x.Secret.Password.Length > 0)
                            throw new ApplicationException("Password already exists");
                    //if (x.Secret.Password != null && oldValue == null)
                }
                else if (old == null)
                {
                    /*This should be done at the creation of the ilogincredentials object*/
                    //val.Secret.Password = Convert.ToBase64String(Encoding.UTF8.GetBytes(newValue.Password));
                    var reference = nw.Password;
                    x.Secret.Password = await Utilities.SecretGen(reference);
                    //_context.Entry(val.Secret).State = EntityState.Modified;
                    //_context.Update(x);
                    _context.Update(x.Secret);
                    await _context.SaveChangesAsync();
                    return true;
                }
                if (await ValidateCardPassword(id, num, cardId, old))
                {
                    /*This should be done at the creation of the ilogincredentials object*/
                    //val.Secret.Password = Convert.ToBase64String(Encoding.UTF8.GetBytes(newValue.Password));
                    var reference = nw.Password;
                    x.Secret.Password = await Utilities.SecretGen(reference);
                    //_context.Entry(val.Secret).State = EntityState.Modified;
                    //_context.Update(x);
                    _context.Update(x.Secret);
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            catch (NullReferenceException)
            {
                return false;
            }
            finally { old = null; nw = null; }

            return false;
        }
    
        public async Task<ICustomer> Verify(int? id, ILoginCredentials pass)
        {
            var emp = await _context.Customers.FindAsync(id);
            if (emp == null)
                return default;
            await _context.Entry(emp).Reference(x => x.LoginCredential).LoadAsync();
            var val = pass.PersonalOnlineKey.CompareTo(emp.LoginCredential.PersonalOnlineKey) == 0
                && pass.Username.CompareTo(emp.LoginCredential.Username) == 0
                && Verify(Maps.Map(emp.LoginCredential), pass);
            if (val) 
            {
                await _context.Entry(emp).Collection(c => c.Accounts).LoadAsync();
                return Maps.Map(emp);
            } 
            return null;
        }

        private static bool Verify(ILoginCredentials x, ILoginCredentials y)
        {
            var refer = Encoding.UTF8.GetString(Convert.FromBase64String(x.Password));
            var decoded = Utilities.Decrypt(ref refer);
            return decoded.CompareTo(y.Password) == 0;
        }
    }
}
