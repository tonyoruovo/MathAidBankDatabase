using DataBaseTest.Data;
using DataBaseTest.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static DataBaseTest.Repos.Builders;

namespace DataBaseTest.Repos
{
    /// <summary>
    /// SaveChangesAsync Should not be called automatically, rather have each PUT operation (and POST ops)
    /// only cause a call to Update(), then have savechangesasync() called separately on a different method
    /// </summary>
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly BankDbContext _context;
        //private readonly UserManager<ApplicationUser> _user;
        public const string InvalidId = "id was invalid";
        //private const string EntrySet = "Entry already set";
        public const string EntityExists = "This entity already exists";
        public const string EntityMissing= "Wrong method. No value to be updated";
        public const string MismatchedEntities = "The argument \"old\" could not be found";

        public EmployeeRepository(BankDbContext context/*, UserManager<ApplicationUser> usermanager*/)
        {
            _context = context;
            //_user = usermanager;
        }

        ~EmployeeRepository()
        {
            _context.Dispose();
        }

        public static ApplicationNullReferenceException NotFound(string msg, int? id)
        {
            return new(msg, id == null ? 0 : id.Value);
        }

        public async Task<IEnumerable<IEmployee>> GetEmployees()
        {
            var x = await _context.Employees.Include( x => x.Person)
                .ThenInclude(x => x.FullName).ToListAsync();
            return x.Select(x => Maps.Map(x)).ToList();
        }

        public async Task<IEmployee> GetEmployee(int? id)
        {
            var x = await _context.Employees.FindAsync(id);
            try
            {
                return Maps.Map(x);
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public async Task<IName> GetName(int? id)
        {
            var x = await _context.Employees.FindAsync(id);
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
            var x = await _context.Employees.FindAsync(id);
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
            var x = await _context.Employees.FindAsync(id);
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
            var x = await _context.Employees.FindAsync(id);
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
                return null;
            }
        }

        public async Task<IEnumerable<IName>> GetOtherNames(int? id)
        {
            var x = await _context.Employees.FindAsync(id);
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
            var x = await _context.Employees.FindAsync(id);
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

        public async Task<string> GetAccountNumber(int? id)
        {
            var x = await _context.Employees.FindAsync(id);
            try
            {
                //await _context.Entry(x)
                //    .Reference(x => x.Person)
                //    .Query()
                //    .Include(x => x.FullName)
                //    .ThenInclude(x => x.OtherNames)
                //    .LoadAsync();
                return x.Account.Number;
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public async Task<string> GetPersonalOnlineKey(int? id)
        {
            var x = await _context.Employees.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Secret)
                    .LoadAsync();
                return x.Secret.PersonalOnlineKey;
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public async Task<string> GetUsername(int? id)
        {
            var x = await _context.Employees.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Secret)
                    .LoadAsync();
                return x.Secret.Username;
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public async Task<string> GetPassword(int? id)
        {
            var x = await _context.Employees.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Secret)
                    .LoadAsync();
                var refer = Encoding.UTF8.GetString(Convert.FromBase64String(x.Secret.Password));
                return Utilities.Decrypt(ref refer);
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public async Task<DateTime> GetHireDate(int? id)
        {
            var x = await _context.Employees.FindAsync(id);
            try
            {
                //await _context.Entry(x)
                //    .Reference(x => x.Person)
                //    .Query()
                //    .Include(x => x.FullName)
                //    .ThenInclude(x => x.OtherNames)
                //    .LoadAsync();
                return x.HireDate;
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<WorkingStatus> GetWorkingStatus(int? id)
        {
            var x = await _context.Employees.FindAsync(id);
            try
            {
                //await _context.Entry(x)
                //    .Reference(x => x.Person)
                //    .Query()
                //    .Include(x => x.FullName)
                //    .ThenInclude(x => x.OtherNames)
                //    .LoadAsync();
                return x.WorkingStatus;
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<decimal> GetSalary(int? id)
        {
            var x = await _context.Employees.FindAsync(id);
            try
            {
                //await _context.Entry(x)
                //    .Reference(x => x.Person)
                //    .Query()
                //    .Include(x => x.FullName)
                //    .ThenInclude(x => x.OtherNames)
                //    .LoadAsync();
                return x.Salary;
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<string> GetLevel(int? id)
        {
            var x = await _context.Employees.FindAsync(id);
            try
            {
                //await _context.Entry(x)
                //    .Reference(x => x.Person)
                //    .Query()
                //    .Include(x => x.FullName)
                //    .ThenInclude(x => x.OtherNames)
                //    .LoadAsync();
                return x.Level;
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<string> GetPosition(int? id)
        {
            var x = await _context.Employees.FindAsync(id);
            try
            {
                //await _context.Entry(x)
                //    .Reference(x => x.Person)
                //    .Query()
                //    .Include(x => x.FullName)
                //    .ThenInclude(x => x.OtherNames)
                //    .LoadAsync();
                return x.Position;
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<IEnumerable<IEmployee>> GetGroup(int? id)
        {
            var x = await _context.Employees.FindAsync(id);
            try
            {
                //await _context.Entry(x)
                //    .Reference(x => x.Person)
                //    .Query()
                //    .Include(x => x.FullName)
                //    .ThenInclude(x => x.OtherNames)
                //    .LoadAsync();
                return x.Group.Select(x => Maps.Map(x));
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<IEmployee> GetSupervisor(int? id)
        {
            var x = await _context.Employees.FindAsync(id);
            try
            {
                //await _context.Entry(x)
                //    .Reference(x => x.Person)
                //    .Query()
                //    .Include(x => x.FullName)
                //    .ThenInclude(x => x.OtherNames)
                //    .LoadAsync();
                return Maps.Map(x.Supervisor);
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<IEmployee> GetSuperior(int? id)
        {
            var x = await _context.Employees.FindAsync(id);
            try
            {
                //await _context.Entry(x)
                //    .Reference(x => x.Person)
                //    .Query()
                //    .Include(x => x.FullName)
                //    .ThenInclude(x => x.OtherNames)
                //    .LoadAsync();
                return Maps.Map(x.Superior);
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<IEmployee> GetSubordinate(int? id)
        {
            var x = await _context.Employees.FindAsync(id);
            try
            {
                return Maps.Map(x.Subordinate);
            }
            catch (NullReferenceException) { return default; }
        }

        public async Task<IReadOnlyDictionary<string, string>> GetGrades(int? id, string level, string certification)
        {
            var x = await _context.Employees.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Qualification)
                    .Query()
                    .Include(x => x.Primary)
                    .LoadAsync();
                await _context.Entry(x)
                    .Reference(x => x.Qualification)
                    .Query()
                    .Include(x => x.Secondary)
                    .LoadAsync();
                await _context.Entry(x)
                    .Reference(x => x.Qualification)
                    .Query()
                    .Include(x => x.PrimaryTertiary)
                    .LoadAsync();
                await _context.Entry(x)
                    .Reference(x => x.Qualification)
                    .Query()
                    .Include(x => x.Others)
                    .LoadAsync();
               return GradeLevelHelper(level, certification, x);
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id);
            }
        }

        private static ApplicationArgumentException CertNotFound(int? id, string cert, object o)
        {
            return new ApplicationArgumentException($"The certificate \"{cert}\" did not match any internal record", id == null ? 0 : id.Value, o);
        }

        private static IReadOnlyDictionary<string, string> GradeLevelHelper(string val, string certification, EmployeeModel x)
        {
            if (val.Equals("Primary", StringComparison.InvariantCultureIgnoreCase))
            {
                var cert = x.Qualification.Primary.Certification;
                if (cert.CompareTo(certification) == 0)
                    return new ReadOnlyDictionary<string, string>(x.Qualification.Primary.Grades);
                else throw CertNotFound(x.EmployeeModelId.Value, cert, x);
            }
            if (val.Equals("Secondary", StringComparison.InvariantCultureIgnoreCase))
            {
                var cert = x.Qualification.Secondary.Certification;
                if (cert.CompareTo(certification) == 0)
                    return new ReadOnlyDictionary<string, string>(x.Qualification.Secondary.Grades);
                else throw CertNotFound(x.EmployeeModelId.Value, cert, x);
            }
            if (val.Equals("PrimaryTertiary", StringComparison.InvariantCultureIgnoreCase))
            {
                var cert = x.Qualification.PrimaryTertiary.Certification;
                if (cert.CompareTo(certification) == 0)
                    return new ReadOnlyDictionary<string, string>(x.Qualification.PrimaryTertiary.Grades);
                else throw CertNotFound(x.EmployeeModelId.Value, cert, x);
            }
            if (val.Equals("Others", StringComparison.InvariantCultureIgnoreCase))
            {
                foreach (var item in x.Qualification.Others)
                {
                    var cert = item.Certification;
                    if (cert.CompareTo(certification) == 0)
                        return new ReadOnlyDictionary<string, string>(item.Grades);
                }
                throw CertNotFound(x.EmployeeModelId.Value, certification, x);
            }
            throw new ApplicationArgumentException($"The level \"{val}\" is invalid", x.EmployeeModelId.Value, x);
        }

        public async Task<IAddress> GetCertificateIssuer(int? id, string level, string certification)
        {
            var x = await _context.Employees.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Qualification)
                    .Query()
                    .Include(x => x.Primary)
                    .ThenInclude(x => x.Academy)
                    .ThenInclude(x => x.CountryOfResidence)
                    .ThenInclude(x => x.CountryName)
                    .LoadAsync();
                await _context.Entry(x)
                    .Reference(x => x.Qualification)
                    .Query()
                    .Include(x => x.Primary)
                    .ThenInclude(x => x.Academy)
                    .ThenInclude(x => x.CountryOfResidence)
                    .ThenInclude(x => x.State)
                    .LoadAsync();
                await _context.Entry(x)
                    .Reference(x => x.Qualification)
                    .Query()
                    .Include(x => x.Primary)
                    .ThenInclude(x => x.Academy)
                    .ThenInclude(x => x.CountryOfResidence)
                    .ThenInclude(x => x.CityTown)
                    .LoadAsync();//
                await _context.Entry(x)
                    .Reference(x => x.Qualification)
                    .Query()
                    .Include(x => x.Secondary)
                    .ThenInclude(x => x.Academy)
                    .ThenInclude(x => x.CountryOfResidence)
                    .ThenInclude(x => x.CountryName)
                    .LoadAsync();
                await _context.Entry(x)
                    .Reference(x => x.Qualification)
                    .Query()
                    .Include(x => x.Secondary)
                    .ThenInclude(x => x.Academy)
                    .ThenInclude(x => x.CountryOfResidence)
                    .ThenInclude(x => x.State)
                    .LoadAsync();
                await _context.Entry(x)
                    .Reference(x => x.Qualification)
                    .Query()
                    .Include(x => x.Secondary)
                    .ThenInclude(x => x.Academy)
                    .ThenInclude(x => x.CountryOfResidence)
                    .ThenInclude(x => x.CityTown)
                    .LoadAsync();//
                await _context.Entry(x)
                    .Reference(x => x.Qualification)
                    .Query()
                    .Include(x => x.PrimaryTertiary)
                    .ThenInclude(x => x.Academy)
                    .ThenInclude(x => x.CountryOfResidence)
                    .ThenInclude(x => x.CountryName)
                    .LoadAsync();
                await _context.Entry(x)
                    .Reference(x => x.Qualification)
                    .Query()
                    .Include(x => x.PrimaryTertiary)
                    .ThenInclude(x => x.Academy)
                    .ThenInclude(x => x.CountryOfResidence)
                    .ThenInclude(x => x.State)
                    .LoadAsync();
                await _context.Entry(x)
                    .Reference(x => x.Qualification)
                    .Query()
                    .Include(x => x.PrimaryTertiary)
                    .ThenInclude(x => x.Academy)
                    .ThenInclude(x => x.CountryOfResidence)
                    .ThenInclude(x => x.CityTown)
                    .LoadAsync();//
                await _context.Entry(x)
                    .Reference(x => x.Qualification)
                    .Query()
                    .Include(x => x.Others)
                    .LoadAsync();
                foreach (var item in x.Qualification.Others)
                {
                    await _context.Entry(item)
                        .Reference(x => x.Academy)
                        .Query()
                        .Include(x => x.CountryOfResidence)
                        .ThenInclude(x => x.CountryName)
                        .LoadAsync();
                }
                foreach (var item in x.Qualification.Others)
                {
                    await _context.Entry(item)
                        .Reference(x => x.Academy)
                        .Query()
                        .Include(x => x.CountryOfResidence)
                        .ThenInclude(x => x.State)
                        .LoadAsync();
                }
                foreach (var item in x.Qualification.Others)
                {
                    await _context.Entry(item)
                        .Reference(x => x.Academy)
                        .Query()
                        .Include(x => x.CountryOfResidence)
                        .ThenInclude(x => x.CityTown)
                        .LoadAsync();
                }
                return GradeCertIssuerHelper(level, certification, x);
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id);
            }
        }

        private static IAddress GradeCertIssuerHelper(string val, string certification, EmployeeModel x)
        {
            if (val.Equals("Primary", StringComparison.InvariantCultureIgnoreCase))
            {
                var cert = x.Qualification.Primary.Certification;
                if (cert.CompareTo(certification) == 0)
                    //return Maps.Map(x.Qualification.Primary.Academy);
                    return Maps.Map(x.Qualification.Primary.Academy);
                else throw CertNotFound(x.EmployeeModelId.Value, cert, x);
            }
            if (val.Equals("Secondary", StringComparison.InvariantCultureIgnoreCase))
            {
                var cert = x.Qualification.Secondary.Certification;
                if (cert.CompareTo(certification) == 0)
                    return Maps.Map(x.Qualification.Secondary.Academy);
                else throw CertNotFound(x.EmployeeModelId.Value, cert, x);
            }
            if (val.Equals("PrimaryTertiary", StringComparison.InvariantCultureIgnoreCase))
            {
                var cert = x.Qualification.PrimaryTertiary.Certification;
                if (cert.CompareTo(certification) == 0)
                    return Maps.Map(x.Qualification.PrimaryTertiary.Academy);
                else throw CertNotFound(x.EmployeeModelId.Value, cert, x);
            }
            if (val.Equals("Others", StringComparison.InvariantCultureIgnoreCase))
            {
                foreach (var item in x.Qualification.Others)
                {
                    var cert = item.Certification;
                    if (cert.CompareTo(certification) == 0)
                    {
                        return Maps.Map(item.Academy);
                    }
                }
                throw CertNotFound(x.EmployeeModelId.Value, certification, x);
            }
            throw new ApplicationArgumentException($"The level \"{val}\" is invalid", x.EmployeeModelId.Value, x);
        }

        public async Task<string> GetGrade(int? id, string level, string certification, string course)
        {
            var x = await _context.Employees.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Qualification)
                    .Query()
                    .Include(x => x.Primary)
                    .LoadAsync();
                await _context.Entry(x)
                    .Reference(x => x.Qualification)
                    .Query()
                    .Include(x => x.Secondary)
                    .LoadAsync();
                await _context.Entry(x)
                    .Reference(x => x.Qualification)
                    .Query()
                    .Include(x => x.PrimaryTertiary)
                    .LoadAsync();
                await _context.Entry(x)
                    .Reference(x => x.Qualification)
                    .Query()
                    .Include(x => x.Others)
                    .LoadAsync(); 
                return GradeHelper(level, certification, course, x);
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id);
            }
        }

        private static string GradeHelper(string val, string certification, string course, EmployeeModel x)
        {
            if (val.Equals("Primary", StringComparison.InvariantCultureIgnoreCase))
            {
                var cert = x.Qualification.Primary.Certification;
                if (cert.CompareTo(certification) == 0)
                    try
                    {
                        return x.Qualification.Primary.Grades[course];
                    }
                    catch (KeyNotFoundException)
                    {
                        throw new ApplicationArgumentException($"The course \"{course}\" did not match any internal record", x.EmployeeModelId.Value, x);
                    }
                else throw CertNotFound(x.EmployeeModelId.Value, cert, x);
            }
            if (val.Equals("Secondary", StringComparison.InvariantCultureIgnoreCase))
            {
                var cert = x.Qualification.Secondary.Certification;
                if (cert.CompareTo(certification) == 0)
                    try
                    {
                        return x.Qualification.Secondary.Grades[course];
                    }
                    catch (KeyNotFoundException)
                    {
                        throw new ApplicationArgumentException($"The course \"{course}\" did not match any internal record", x.EmployeeModelId.Value, x);
                    }
                else throw CertNotFound(x.EmployeeModelId.Value, cert, x);
            }
            if (val.Equals("PrimaryTertiary", StringComparison.InvariantCultureIgnoreCase))
            {
                var cert = x.Qualification.PrimaryTertiary.Certification;
                if (cert.CompareTo(certification) == 0)
                    try
                    {
                        return x.Qualification.PrimaryTertiary.Grades[course];
                    }
                    catch (KeyNotFoundException)
                    {
                        throw new ApplicationArgumentException($"The course \"{course}\" did not match any internal record", x.EmployeeModelId.Value, x);
                    }
                else throw CertNotFound(x.EmployeeModelId.Value, cert, x);
            }
            if (val.Equals("Others", StringComparison.InvariantCultureIgnoreCase))
            {
                foreach (var item in x.Qualification.Others)
                {
                    var cert = item.Certification;
                    if (cert.CompareTo(certification) == 0)
                    {
                        try
                        {
                            return item.Grades[course];
                        }
                        catch (KeyNotFoundException)
                        {
                            throw new ApplicationArgumentException($"The course \"{course}\" did not match any internal record", x.EmployeeModelId.Value, x);
                        }
                    }
                }
                throw CertNotFound(x.EmployeeModelId.Value, certification, x);
            }
            throw NotFound($"The level \"{val}\" is invalid", x.EmployeeModelId.Value);
        }

        public async Task<IPerson> GetGuarantor(int? id)
        {
            var x = await _context.Employees.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Guarantor)
                    .Query()
                    .Include(x => x.FullName)
                    .ThenInclude(x => x.Name)
                    .LoadAsync();
                await _context.Entry(x)
                    .Reference(x => x.Guarantor)
                    .Query()
                    .Include(x => x.FullName)
                    .ThenInclude(x => x.Surname)
                    .LoadAsync();
                return Maps.Map(x.Guarantor);
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<byte[]> GetFingerprint(int? id)
        {
            var x = await _context.Employees.FindAsync(id);
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
            var x = await _context.Employees.FindAsync(id);
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
            var x = await _context.Employees.FindAsync(id);
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
            var x = await _context.Employees.FindAsync(id);
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

        public async Task<string> GetIdNumberFor(int? id, string idType)
        {
            var x = await _context.Employees.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .LoadAsync();
                string v = null;
                try
                {
                    v = x.Person.Identification[idType];
                }
                catch (KeyNotFoundException ex)
                {
                    throw new ApplicationArgumentException($"argument {idType} does not match any found", id == null ? 0 : id.Value, ex);
                }
                return v ?? throw NotFound($"Id type argument {idType} invalid", id);
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id);
            }
        }

        public async Task<bool> GetSex(int? id)
        {
            var x = await _context.Employees.FindAsync(id);
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
            var x = await _context.Employees.FindAsync(id);
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
            var x = await _context.Employees.FindAsync(id);
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
            var x = await _context.Employees.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.CountryOfOrigin)
                    .ThenInclude(x => x.CountryName)
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
            var x = await _context.Employees.FindAsync(id);
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
            var x = await _context.Employees.FindAsync(id);
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
            var x = await _context.Employees.FindAsync(id);
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
            var x = await _context.Employees.FindAsync(id);
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
            var x = await _context.Employees.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .LoadAsync();
                return Maps.Map(x.Person.NextOfKin);
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<MaritalStatus> GetMaritalStatus(int? id)
        {
            var x = await _context.Employees.FindAsync(id);
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
            var x = await _context.Employees.FindAsync(id);
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
            var x = await _context.Employees.FindAsync(id);
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
            var x = await _context.Employees.FindAsync(id);
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
            var x = await _context.Employees.FindAsync(id);
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
            var x = await _context.Employees.FindAsync(id);
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
            var x = await _context.Employees.FindAsync(id);
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
            var x = await _context.Employees.FindAsync(id);
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
            var x = await _context.Employees.FindAsync(id);
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
            var x = await _context.Employees.FindAsync(id);
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
            var x = await _context.Employees.FindAsync(id);
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
            var x = await _context.Employees.FindAsync(id);
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
            var x = await _context.Employees.FindAsync(id);
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
            var x = await _context.Employees.FindAsync(id);
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
            var x = await _context.Employees.FindAsync(id);
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
            var x = await _context.Employees.FindAsync(id);
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
            var x = await _context.Employees.FindAsync(id);
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
            var x = await _context.Employees.FindAsync(id);
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
            var x = await _context.Employees.FindAsync(id);
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
            var x = await _context.Employees.FindAsync(id);
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
            var x = await _context.Employees.FindAsync(id);
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
            var x = await _context.Employees.FindAsync(id);
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
            var x = await _context.Employees.FindAsync(id);
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
            var x = await _context.Employees.FindAsync(id);
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
            var x = await _context.Employees.FindAsync(id);
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

        public async Task<List<string>> GetTokens(int? id)
        {
            var x = await _context.Employees.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Secret)
                    .LoadAsync();
                return x.Secret.Tokens;
            }
            catch (NullReferenceException)
            {
                return default;
            }
        }

        public async Task<PostResult<object>> AddEmployee(IEmployee e)
        {
            try
            {
                var x = _context.Employees
                            .Where(x => x.Person.UniqueTag == e.Person.UniqueTag)
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
                        if(x.Person.FullName.Equals(Maps.ReverseMap(e.Person.FullName)))
                            throw new ApplicationStateException(EntityExists);
                    }
                    catch (NullReferenceException)
                    {
                    }
                }
                var mapped = Maps.ReverseMap(e);
                try
                {
                    var refer = e.Secret.Password;
                    mapped.Secret.Password = Utilities.SecretGen(e.HireDate, e, ref refer);
                }
                catch (NullReferenceException)
                {
                }
                var x1 = await _context.AddAsync(mapped);
                var state = x1.State;
                await _context.SaveChangesAsync();
                return new PostResult<object>
                {
                    Id = x1.Entity.EmployeeModelId == null ? 0 : x1.Entity.EmployeeModelId.Value,
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

        public async Task<bool> SetName(int? id, IName @new)
        {
            var x = await _context.Employees.FindAsync(id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetSurname(int? id, IName @new)
        {
            var x = await _context.Employees.FindAsync(id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetNickname(int? id, IName @new)
        {
            var x = await _context.Employees.FindAsync(id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetMaidenName(int? id, IName @new)
        {
            var x = await _context.Employees.FindAsync(id);
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
                var val = Maps.ReverseMap(@new);
                x.Person.FullName.MaidenName = val;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> AddOtherName(int? id, IName @new)
        {
            var x = await _context.Employees.FindAsync(id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> AddTitle(int? id, string @new)
        {
            var x = await _context.Employees.FindAsync(id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetAccountNumber(int? id, string @new)
        {
            var x = await _context.Employees.FindAsync(id);
            try
            {
                //await _context.Entry(x)
                //    .Reference(x => x.Person)
                //    .Query()
                //    .Include(x => x.FullName)
                //    .LoadAsync();
                var current = x.Account;

                if (current != default) throw new ApplicationStateException(EntityExists);
                var ac = new AccountBuilder().SetNumber(@new).Build();
                x.Account = Maps.ReverseMap(ac);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetLoginCredentials(int? id, ILoginCredentials @new)
        {
            var x = await _context.Employees.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Secret)
                    .LoadAsync();
                var current = x.Secret;

                if (current != default) throw new ApplicationStateException(EntityExists);
                var l = Maps.ReverseMap(@new);
                l.Password = null;
                x.Secret = l;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetPersonalOnlineKey(int? id, ILoginCredentials @new)
        {
            var x = await _context.Employees.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Secret)
                    .LoadAsync();
                var current = x.Secret.PersonalOnlineKey;

                if (current != default) throw new ApplicationStateException(EntityExists);
                x.Secret.PersonalOnlineKey = @new.PersonalOnlineKey;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetUsername(int? id, ILoginCredentials @new)
        {
            var x = await _context.Employees.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Secret)
                    .LoadAsync();
                var current = x.Secret.Username;

                if (current != default) throw new ApplicationStateException(EntityExists);
                x.Secret.Username = @new.Username;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetPassword(int? id, ILoginCredentials @new)
        {
            var x = await _context.Employees.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Secret)
                    .LoadAsync();
                var current = x.Secret.Password;

                if (current != default) throw new ApplicationStateException(EntityExists);
                var refer = @new.Password;
                x.Secret.Password = Utilities.SecretGen(x.HireDate, Guid.NewGuid(), ref refer);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> AddToken(int? id, string @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(c => c.EmployeeModelId == id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Secret)
                    .LoadAsync();
                if (x.Secret.Tokens == null)
                    x.Secret.Tokens = new();
                x.Secret.Tokens.Add(@new);

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

        public async Task<bool> SetHireDate(int? id, DateTime @new)
        {
            var x = await _context.Employees.FindAsync(id);
            try
            {
                var current = x.HireDate;

                if (current != default) throw new ApplicationStateException(EntityExists);
                x.HireDate = @new;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetWorkingStatus(int? id, WorkingStatus @new)
        {
            var x = await _context.Employees.FindAsync(id);
            try
            {
                var current = x.WorkingStatus;

                if (current != default) throw new ApplicationStateException(EntityExists);
                x.WorkingStatus = @new;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetSalary(int? id, decimal @new)
        {
            var x = await _context.Employees.FindAsync(id);
            try
            {
                var current = x.Salary;

                if (current != default) throw new ApplicationStateException(EntityExists);
                x.Salary = @new;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetLevel(int? id, string @new)
        {
            var x = await _context.Employees.FindAsync(id);
            try
            {
                var current = x.Level;

                if (current != default) throw new ApplicationStateException(EntityExists);
                x.Level = @new;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetPosition(int? id, string @new)
        {
            var x = await _context.Employees.FindAsync(id);
            try
            {
                var current = x.Position;

                if (current != default) throw new ApplicationStateException(EntityExists);
                x.Position = @new;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetGroup(int? id, IEnumerable<IEmployee> @new)
        {
            var x = await _context.Employees.FindAsync(id);
            try
            {
                var current = x.Group;

                if (current != default) throw new ApplicationStateException(EntityExists);
                x.Group = @new.Select(x => Maps.ReverseMap(x)).ToList();
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetSupervisor(int? id, IEmployee @new)
        {
            var x = await _context.Employees.FindAsync(id);
            try
            {
                var current = x.Supervisor;

                if (current != default) throw new ApplicationStateException(EntityExists);
                x.Supervisor = Maps.ReverseMap(@new);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetSuperior(int? id, IEmployee @new)
        {
            var x = await _context.Employees.FindAsync(id);
            try
            {
                var current = x.Superior;

                if (current != default) throw new ApplicationStateException(EntityExists);
                x.Superior = Maps.ReverseMap(@new);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetSubordinate(int? id, IEmployee @new)
        {
            var x = await _context.Employees.FindAsync(id);
            try
            {
                var current = x.Subordinate;

                if (current != default) throw new ApplicationStateException(EntityExists);
                x.Subordinate = Maps.ReverseMap(@new);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetGrades(int? id, string level, string certification, IDictionary<string, string> grades)
        {
            var x = await _context.Employees.FindAsync(id);
            try
            {
                await SetGradeLevelHelper(level, certification, x, grades);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        async Task SetGradeLevelHelper(string val, string certification, EmployeeModel x, IDictionary<string, string> grades)
        {
            if (val.Equals("Primary", StringComparison.InvariantCultureIgnoreCase))
            {
                await _context.Entry(x)
                    .Reference(x => x.Qualification)
                    .Query()
                    .Include(x => x.Primary)
                    .LoadAsync();
                var cert = x.Qualification.Primary.Certification;
                if (cert.CompareTo(certification) == 0)
                    x.Qualification.Primary.Grades = x.Qualification.Primary.Grades != null ? throw new ApplicationStateException(EntityExists) : new Dictionary<string, string>(grades);
                else throw CertNotFound(x.EmployeeModelId.Value, cert, x);
                return;
            }
            if (val.Equals("Secondary", StringComparison.InvariantCultureIgnoreCase))
            {
                await _context.Entry(x)
                    .Reference(x => x.Qualification)
                    .Query()
                    .Include(x => x.Secondary)
                    .LoadAsync();
                var cert = x.Qualification.Secondary.Certification;
                if (cert.CompareTo(certification) == 0)
                    x.Qualification.Secondary.Grades = x.Qualification.Secondary.Grades != null ? throw new ApplicationStateException(EntityExists) : new Dictionary<string, string>(grades);
                else throw CertNotFound(x.EmployeeModelId.Value, cert, x);
                return;
            }
            if (val.Equals("PrimaryTertiary", StringComparison.InvariantCultureIgnoreCase))
            {
                await _context.Entry(x)
                    .Reference(x => x.Qualification)
                    .Query()
                    .Include(x => x.PrimaryTertiary)
                    .LoadAsync();
                var cert = x.Qualification.PrimaryTertiary.Certification;
                if (cert.CompareTo(certification) == 0)
                    x.Qualification.PrimaryTertiary.Grades = x.Qualification.PrimaryTertiary.Grades != null ? throw new ApplicationStateException(EntityExists) : new Dictionary<string, string>(grades);
                else throw CertNotFound(x.EmployeeModelId.Value, cert, x);
                return;
            }
            if (val.Equals("Others", StringComparison.InvariantCultureIgnoreCase))
            {
                await _context.Entry(x)
                    .Reference(x => x.Qualification)
                    .Query()
                    .Include(x => x.Others)
                    .LoadAsync();
                foreach (var item in x.Qualification.Others)
                {
                    var cert = item.Certification;
                    if (cert.CompareTo(certification) == 0)
                    {
                        item.Grades = item.Grades != null ? throw new ApplicationStateException(EntityExists) : new Dictionary<string, string>(grades);
                        return;
                    }
                }
                throw CertNotFound(x.EmployeeModelId.Value, certification, x);
            }
            throw new ApplicationArgumentException($"The level \"{val}\" is invalid", x.EmployeeModelId.Value, x);
        }

        public async Task<bool> SetCertificateIssuer(int? id, string level, string certification, IAddress @new)
        {
            var x = await _context.Employees.FindAsync(id);
            try
            {
                await SetCertIssuerHelper(level, certification, x, @new);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        async Task SetCertIssuerHelper(string val, string certification, EmployeeModel x, IAddress v)
        {
            if (val.Equals("Primary", StringComparison.InvariantCultureIgnoreCase))
            {
                await _context.Entry(x)
                    .Reference(x => x.Qualification)
                    .Query()
                    .Include(x => x.Primary)
                    .LoadAsync();
                var cert = x.Qualification.Primary.Certification;
                if (cert.CompareTo(certification) == 0)
                {
                    await _context.Entry(x)
                        .Reference(x => x.Qualification)
                        .Query()
                        .Include(x => x.Primary)
                        .ThenInclude(x => x.Academy)
                        .LoadAsync();

                    x.Qualification.Primary.Academy = x.Qualification.Primary.Academy == null ? throw new ApplicationStateException(EntityExists) : Maps.ReverseMap(v);
                }
                else throw CertNotFound(x.EmployeeModelId.Value, cert, x);
                return;
            }
            if (val.Equals("Secondary", StringComparison.InvariantCultureIgnoreCase))
            {
                await _context.Entry(x)
                    .Reference(x => x.Qualification)
                    .Query()
                    .Include(x => x.Secondary)
                    .LoadAsync();
                var cert = x.Qualification.Secondary.Certification;
                if (cert.CompareTo(certification) == 0)
                {
                    await _context.Entry(x)
                        .Reference(x => x.Qualification)
                        .Query()
                        .Include(x => x.Secondary)
                        .ThenInclude(x => x.Academy)
                        .LoadAsync();

                    x.Qualification.Secondary.Academy = x.Qualification.Secondary.Grades != null ? throw new ApplicationStateException(EntityExists) : Maps.ReverseMap(v);
                }
                else throw CertNotFound(x.EmployeeModelId.Value, cert, x);
                return;
            }
            if (val.Equals("PrimaryTertiary", StringComparison.InvariantCultureIgnoreCase))
            {
                await _context.Entry(x)
                    .Reference(x => x.Qualification)
                    .Query()
                    .Include(x => x.PrimaryTertiary)
                    .LoadAsync();
                var cert = x.Qualification.PrimaryTertiary.Certification;
                if (cert.CompareTo(certification) == 0)
                {
                    await _context.Entry(x)
                        .Reference(x => x.Qualification)
                        .Query()
                        .Include(x => x.PrimaryTertiary)
                        .ThenInclude(x => x.Academy)
                        .LoadAsync();

                    x.Qualification.PrimaryTertiary.Academy = x.Qualification.PrimaryTertiary.Academy == null ? throw new ApplicationStateException(EntityExists) : Maps.ReverseMap(v);
                }
                else throw CertNotFound(x.EmployeeModelId.Value, cert, x);
                return;
            }
            if (val.Equals("Others", StringComparison.InvariantCultureIgnoreCase))
            {
                await _context.Entry(x)
                    .Reference(x => x.Qualification)
                    .Query()
                    .Include(x => x.Others)
                    .LoadAsync();
                foreach (var item in x.Qualification.Others)
                {
                    var cert = item.Certification;
                    if (cert.CompareTo(certification) == 0)
                    {
                        await _context.Entry(item)
                            .Reference(x => x.Academy)
                            .LoadAsync();
                        item.Academy = item.Academy == null ? throw new ApplicationStateException(EntityExists) : Maps.ReverseMap(v);
                        return;
                    }
                }
                throw CertNotFound(x.EmployeeModelId.Value, certification, x);
            }
            throw new ApplicationArgumentException($"The level \"{val}\" is invalid", x.EmployeeModelId.Value, x);
        }

        public async Task<bool> AddGrade(int? id, string level, string certification, string course, string grade)
        {
            var x = await _context.Employees.FindAsync(id);
            try
            {
                await AddGradeHelper(level, certification, course, x, grade);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        async Task AddGradeHelper(string val, string certification, string course, EmployeeModel x, string grade)
        {
            if (val.Equals("Primary", StringComparison.InvariantCultureIgnoreCase))
            {
                await _context.Entry(x)
                    .Reference(x => x.Qualification)
                    .Query()
                    .Include(x => x.Primary)
                    .LoadAsync();
                var cert = x.Qualification.Primary.Certification;
                if (cert.CompareTo(certification) == 0)
                {
                    if (x.Qualification.Primary.Grades.ContainsKey(course))
                        throw new ApplicationArgumentException(EntityExists);
                    try
                    {
                        x.Qualification.Primary.Grades[course] = grade;
                    }
                    catch (KeyNotFoundException ex)
                    {
                        throw new ApplicationArgumentException($"Something went wrong when setting the grade in the dictionary", x.EmployeeModelId.Value, ex);
                    }
                }
                else throw CertNotFound(x.EmployeeModelId.Value, cert, x);
                return;
            }
            if (val.Equals("Secondary", StringComparison.InvariantCultureIgnoreCase))
            {
                await _context.Entry(x)
                    .Reference(x => x.Qualification)
                    .Query()
                    .Include(x => x.Secondary)
                    .LoadAsync();
                var cert = x.Qualification.Secondary.Certification;
                if (cert.CompareTo(certification) == 0)
                {
                    if (x.Qualification.Secondary.Grades.ContainsKey(course))
                        throw new ApplicationArgumentException(EntityExists);
                    try
                    {
                        x.Qualification.Secondary.Grades[course] = grade;
                    }
                    catch (KeyNotFoundException ex)
                    {
                        throw new ApplicationArgumentException($"Something went wrong when setting the grade in the diction", x.EmployeeModelId.Value, ex);
                    }
                }
                else throw CertNotFound(x.EmployeeModelId.Value, cert, x);
                return;
            }
            if (val.Equals("PrimaryTertiary", StringComparison.InvariantCultureIgnoreCase))
            {
                await _context.Entry(x)
                    .Reference(x => x.Qualification)
                    .Query()
                    .Include(x => x.PrimaryTertiary)
                    .LoadAsync();
                var cert = x.Qualification.PrimaryTertiary.Certification;
                if (cert.CompareTo(certification) == 0)
                {
                    if (x.Qualification.PrimaryTertiary.Grades.ContainsKey(course))
                        throw new ApplicationArgumentException(EntityExists);
                    try
                    {
                        x.Qualification.PrimaryTertiary.Grades[course] = grade;
                    }
                    catch (KeyNotFoundException ex)
                    {
                        throw new ApplicationArgumentException($"Something went wrong when setting the grade in the diction", x.EmployeeModelId.Value, ex);
                    }
                }
                else throw CertNotFound(x.EmployeeModelId.Value, cert, x);
                return;
            }
            if (val.Equals("Others", StringComparison.InvariantCultureIgnoreCase))
            {
                await _context.Entry(x)
                    .Reference(x => x.Qualification)
                    .Query()
                    .Include(x => x.Others)
                    .LoadAsync();
                foreach (var item in x.Qualification.Others)
                {
                    var cert = item.Certification;
                    if (cert.CompareTo(certification) == 0)
                    {
                        if (item.Grades.ContainsKey(course))
                            throw new ApplicationArgumentException(EntityExists);
                        try
                        {
                            item.Grades[course] = grade;
                        }
                        catch (KeyNotFoundException ex)
                        {
                            throw new ApplicationArgumentException($"Something went wrong when setting the grade in the diction", x.EmployeeModelId.Value, ex);
                        }
                        return;
                    }
                }
                throw CertNotFound(x.EmployeeModelId.Value, certification, x);
            }
            throw new ApplicationArgumentException($"The level \"{val}\" is invalid", x.EmployeeModelId.Value, x);
        }

        public async Task<bool> SetGuarantor(int? id, IPerson @new)
        {
            var x = await _context.Employees.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Guarantor)
                    .LoadAsync();
                var current = x.Guarantor;

                if (current != default) throw new ApplicationStateException(EntityExists);
                var val = Maps.ReverseMap(@new);
                x.Guarantor = val;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetFingerprint(int? id, byte[] @new)
        {
            var x = await _context.Employees.FindAsync(id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetSignature(int? id, byte[] @new)
        {
            var x = await _context.Employees.FindAsync(id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetPassport(int? id, byte[] @new)
        {
            var x = await _context.Employees.FindAsync(id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetJobType(int? id, string @new)
        {
            var x = await _context.Employees.FindAsync(id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> AddIdNumberFor(int? id, string idType, string number)
        {
            var x = await _context.Employees.FindAsync(id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .LoadAsync();
                if (x.Person.Identification.ContainsKey(idType))
                    throw new ApplicationArgumentException(EntityExists);
                try
                {
                    x.Person.Identification[idType] = number;

                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (KeyNotFoundException ex)
                {
                    throw new ApplicationArgumentException($"Argument \"idType\" did not match any found", id == null ? 0 : id.Value, ex);
                }
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetSex(int? id, bool @new)
        {
            var x = await _context.Employees.FindAsync(id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetUniqueTag(int? id, Guid @new)
        {
            var x = await _context.Employees.FindAsync(id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetBirthDate(int? id, DateTime @new)
        {
            var x = await _context.Employees.FindAsync(id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetCountryOfOrigin(int? id, IName @new)
        {
            var x = await _context.Employees.FindAsync(id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetStateOfOrigin(int? id, IName @new)
        {
            var x = await _context.Employees.FindAsync(id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetTown_CityOfOrigin(int? id, IName @new)
        {
            var x = await _context.Employees.FindAsync(id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetLGAOfOrigin(int? id, string @new)
        {
            var x = await _context.Employees.FindAsync(id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetLanguage(int? id, string @new)
        {
            var x = await _context.Employees.FindAsync(id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetNextOfKin(int? id, IPerson @new)
        {
            var x = await _context.Employees.FindAsync(id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetMaritalStatus(int? id, MaritalStatus @new)
        {
            var x = await _context.Employees.FindAsync(id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetBusinessName(int? id, IName @new)
        {
            var x = await _context.Employees.FindAsync(id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> AddBusinessEmail(int? id, [EmailAddress] string @new)
        {
            var x = await _context.Employees.FindAsync(id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> AddBusinessMobile(int? id, [Phone] string @new)
        {
            var x = await _context.Employees.FindAsync(id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> AddBusinessSocialMedia(int? id, [Url] string @new)
        {
            var x = await _context.Employees.FindAsync(id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> AddBusinessWebsite(int? id, [Url] string @new)
        {
            var x = await _context.Employees.FindAsync(id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetBusinessStreetAddress(int? id, string @new)
        {
            var x = await _context.Employees.FindAsync(id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetBusinessZIPCode(int? id, string @new)
        {
            var x = await _context.Employees.FindAsync(id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetBusinessPMB(int? id, string @new)
        {
            var x = await _context.Employees.FindAsync(id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetBusinessCountryOfResidence(int? id, IName @new)
        {
            var x = await _context.Employees.FindAsync(id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetBusinessStateOfResidence(int? id, IName @new)
        {
            var x = await _context.Employees.FindAsync(id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetBusinessTown_CityOfResidence(int? id, IName @new)
        {
            var x = await _context.Employees.FindAsync(id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetBusinessLGAOfResidence(int? id, string @new)
        {
            var x = await _context.Employees.FindAsync(id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetDisplayName(int? id, IName @new)
        {
            var x = await _context.Employees.FindAsync(id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> AddEmail(int? id, [EmailAddress] string @new)
        {
            var x = await _context.Employees.FindAsync(id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> AddMobile(int? id, [Phone] string @new)
        {
            var x = await _context.Employees.FindAsync(id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> AddSocialMedia(int? id, [Url] string @new)
        {
            var x = await _context.Employees.FindAsync(id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> AddWebsite(int? id, [Url] string @new)
        {
            var x = await _context.Employees.FindAsync(id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetStreetAddress(int? id, string @new)
        {
            var x = await _context.Employees.FindAsync(id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetZIPCode(int? id, string @new)
        {
            var x = await _context.Employees.FindAsync(id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetPMB(int? id, string @new)
        {
            var x = await _context.Employees.FindAsync(id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetCountryOfResidence(int? id, IName @new)
        {
            var x = await _context.Employees.FindAsync(id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetStateOfResidence(int? id, IName @new)
        {
            var x = await _context.Employees.FindAsync(id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetTown_CityOfResidence(int? id, IName @new)
        {
            var x = await _context.Employees.FindAsync(id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> SetLGAOfResidence(int? id, string @new)
        {
            var x = await _context.Employees.FindAsync(id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateName(int? id, IName old, IName @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId == id);
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

                await _context.SaveChangesAsync2(value);

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateSurname(int? id, IName old, IName @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId == id);
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

                await _context.SaveChangesAsync2(value);

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateNickname(int? id, IName old, IName @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId == id);
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

                await _context.SaveChangesAsync2(value);

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateMaidenName(int? id, IName old, IName @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId == id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .Query()
                    .Include(x => x.FullName)
                    .LoadAsync();
                int? cId = x.Person.FullName.MaidenNameNameModelId;

                if (cId == default) throw new ApplicationStateException(EntityMissing, 0, null);

                var value = await _context.Names.SingleOrDefaultAsync(val => val.NameModelId == cId);

                if (value.Name.CompareTo(old.Name) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                value.Name = @new.Name;

                await _context.SaveChangesAsync2(value);

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateOthername(int? id, IName old, IName @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId == id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .LoadAsync();
                var value = await _context.FullNames.SingleOrDefaultAsync(y => y.FullNameModelId == x.Person.FullNameModelId);
                var val = value.OtherNames;

                if (!val.Any(x => x.Name.CompareTo(old.Name) == 0)) throw new ApplicationStateException(EntityMissing, 0, null);
                //if (!val.Any(x => x.Name.CompareTo(old.Name) == 0)) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val.Remove(Maps.ReverseMap(old));
                val.Add(Maps.ReverseMap(@new));

                await _context.SaveChangesAsync2(value);

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateTitle(int? id, string old, string @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId == id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Person)
                    .LoadAsync();
                var value = await _context.FullNames.SingleOrDefaultAsync(y => y.FullNameModelId == x.Person.FullNameModelId);
                var val = value.Title;

                if (val == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val.CompareTo(old) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val = @new;

                await _context.SaveChangesAsync2(value);

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateAccountNumber(int? id, string old, string @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId == id);
            try
            {
                var val = x.Account.Number;

                if (val == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val.CompareTo(old) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val = @new;

                await _context.SaveChangesAsync2(x);

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdatePersonalOnlineKey(int? id, ILoginCredentials old, ILoginCredentials @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId == id);
            try
            {
                var val = await _context.LoginCredentials.SingleOrDefaultAsync(y => y.LoginCredentialModelId == x.SecretLoginCredentialModelId);

                if (val.PersonalOnlineKey == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val.PersonalOnlineKey.CompareTo(old.PersonalOnlineKey) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val.PersonalOnlineKey = @new.PersonalOnlineKey;

                await _context.SaveChangesAsync2(val);

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateUsername(int? id, ILoginCredentials old, ILoginCredentials @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId == id);
            try
            {
                var val = await _context.LoginCredentials.SingleOrDefaultAsync(y => y.LoginCredentialModelId == x.SecretLoginCredentialModelId);

                if (val.Username == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val.Username.CompareTo(old.Username) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                val.Username = @new.Username;

                await _context.SaveChangesAsync2(val);

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdatePassword(int? id, ILoginCredentials old, ILoginCredentials @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId == id);
            try
            {
                var val = await _context.LoginCredentials.SingleOrDefaultAsync(y => y.LoginCredentialModelId == x.SecretLoginCredentialModelId);

                if (val.Password == default) throw new ApplicationStateException(EntityMissing, 0, null);
                var refer = Encoding.UTF8.GetString(Convert.FromBase64String(val.Password));
                var decrypted = Utilities.Decrypt(ref refer);
                if (decrypted.CompareTo(old.Password) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                refer = @new.Password;
                val.Password = Utilities.SecretGen(x.HireDate, Guid.NewGuid(), ref refer);

                await _context.SaveChangesAsync2(val);

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateHireDate(int? id, DateTime old, DateTime @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId == id);
            try
            {
                var val = x.HireDate;

                if (val == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val != old) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                x.HireDate = @new;

                await _context.SaveChangesAsync2(x);

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateWorkingStatus(int? id, WorkingStatus old, WorkingStatus @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId == id);
            try
            {
                var val = x.WorkingStatus;

                //if (val == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val != old) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                x.WorkingStatus = @new;

                await _context.SaveChangesAsync2(x);

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateSalary(int? id, decimal old, decimal @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId == id);
            try
            {
                var val = x.Salary;

                if (val == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val != old) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                x.Salary = @new;

                await _context.SaveChangesAsync2(x);

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateLevel(int? id, string old, string @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId == id);
            try
            {
                var val = x.Level;

                if (val == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val.CompareTo(old) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                x.Level = @new;

                await _context.SaveChangesAsync2(x);

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdatePosition(int? id, string old, string @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId == id);
            try
            {
                var val = x.Position;

                if (val == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val.CompareTo(old) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                x.Position = @new;

                await _context.SaveChangesAsync2(x);

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateGroup(int? id, IEnumerable<IEmployee> old, IEnumerable<IEmployee> @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId == id);
            try
            {
                var val = x.Group;

                if (val == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (!Equals(val, old)) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                x.Group = @new.Select(x => Maps.ReverseMap(x)).ToList();

                await _context.SaveChangesAsync2(x);

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        private static bool Equals(IEnumerable<EmployeeModel> lhs, IEnumerable<IEmployee> rhs)
        {
            if (lhs.Count() != rhs.Count())
                return false;
            for (int i = 0; i < lhs.Count(); i++)
            {
                if (lhs.ElementAt(i).Person.UniqueTag != rhs.ElementAt(i).Person.UniqueTag)
                    return false;
            }
            return true;
        }

        public async Task<bool> UpdateSupervisor(int? id, IEmployee old, IEmployee @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId == id);
            try
            {
                var val = x.Supervisor;

                if (val == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val.Person.UniqueTag != old.Person.UniqueTag) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                x.Supervisor = Maps.ReverseMap(@new);

                await _context.SaveChangesAsync2(x);

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateSuperior(int? id, IEmployee old, IEmployee @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId == id);
            try
            {
                var val = x.Superior;

                if (val == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val.Person.UniqueTag != old.Person.UniqueTag) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                x.Superior = Maps.ReverseMap(@new);

                await _context.SaveChangesAsync2(x);

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateSubordinate(int? id, IEmployee old, IEmployee @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId == id);
            try
            {
                var val = x.Subordinate;

                if (val == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (val.Person.UniqueTag != old.Person.UniqueTag) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                x.Subordinate = Maps.ReverseMap(@new);

                await _context.SaveChangesAsync2(x);

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateCertificateIssuer(int? id, string level, string certification, IAddress old, IAddress @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId == id);
            try
            {
                await UpdateCertIssuerHelper(level, certification, x, old, @new);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        async Task UpdateCertIssuerHelper(string val, string certification, EmployeeModel x, IAddress old, IAddress @new)
        {
            if (val.Equals("Primary", StringComparison.InvariantCultureIgnoreCase))
            {
                await _context.Entry(x)
                    .Reference(x => x.Qualification)
                    .LoadAsync();
                var qualif = await _context.Qualifications.SingleOrDefaultAsync(q => q.QualificationModelId == x.Qualification.PrimaryQualificationModelId);
                var cert = qualif.Certification;
                if (cert.CompareTo(certification) == 0)
                {
                    var address = await _context.Addresses.SingleOrDefaultAsync(a => a.AddressModelId == qualif.AcademyAddressModelId);

                    if(address == default)
                        throw new ApplicationStateException(EntityMissing, 0, null);
                    if (!Maps.Map(address).Equals(old))
                        throw new ApplicationArgumentException(MismatchedEntities, 0, null);

                    var nation = await _context.Nations.SingleOrDefaultAsync(n => n.NationalityModelId == address.CountryOfResidenceNationalityModelId);
                    nation.LGA = @new.CountryOfResidence.LGA;
                    nation.Language = @new.CountryOfResidence.Language;

                    var cname = await _context.Names.SingleOrDefaultAsync(n => n.NameModelId == nation.CountryNameNameModelId);
                    cname.Name = @new.CountryOfResidence.CountryName.Name;
                    var sname = await _context.Names.SingleOrDefaultAsync(n => n.NameModelId == nation.StateNameModelId);
                    sname.Name = @new.CountryOfResidence.State.Name;
                    var tname = await _context.Names.SingleOrDefaultAsync(n => n.NameModelId == nation.CityTownNameModelId);
                    tname.Name = @new.CountryOfResidence.CityTown.Name;

                    address.StreetAddress = @new.StreetAddress;
                    address.PMB = @new.PMB;
                    address.ZIPCode = @new.ZIPCode;

                    //var address2 = Maps.ReverseMap(@new);
                    //Maps.MapKeys(address2, address);
                    //_context.Addresses.Update(address2);
                }
                else throw CertNotFound(x.EmployeeModelId.Value, cert, x);
                return;
            }
            if (val.Equals("Secondary", StringComparison.InvariantCultureIgnoreCase))
            {
                await _context.Entry(x)
                    .Reference(x => x.Qualification)
                    .LoadAsync();
                var qualif = await _context.Qualifications.SingleOrDefaultAsync(q => q.QualificationModelId == x.Qualification.SecondaryQualificationModelId);
                var cert = qualif.Certification;
                if (cert.CompareTo(certification) == 0)
                {
                    var address = await _context.Addresses.SingleOrDefaultAsync(a => a.AddressModelId == qualif.AcademyAddressModelId);

                    if (address == default)
                        throw new ApplicationStateException(EntityMissing, 0, null);
                    if (!Maps.Map(address).Equals(old))
                        throw new ApplicationArgumentException(MismatchedEntities, 0, null);

                    var nation = await _context.Nations.SingleOrDefaultAsync(n => n.NationalityModelId == address.CountryOfResidenceNationalityModelId);
                    nation.LGA = @new.CountryOfResidence.LGA;
                    nation.Language = @new.CountryOfResidence.Language;

                    var cname = await _context.Names.SingleOrDefaultAsync(n => n.NameModelId == nation.CountryNameNameModelId);
                    cname.Name = @new.CountryOfResidence.CountryName.Name;
                    var sname = await _context.Names.SingleOrDefaultAsync(n => n.NameModelId == nation.StateNameModelId);
                    sname.Name = @new.CountryOfResidence.State.Name;
                    var tname = await _context.Names.SingleOrDefaultAsync(n => n.NameModelId == nation.CityTownNameModelId);
                    tname.Name = @new.CountryOfResidence.CityTown.Name;

                    address.StreetAddress = @new.StreetAddress;
                    address.PMB = @new.PMB;
                    address.ZIPCode = @new.ZIPCode;

                    //var address2 = Maps.ReverseMap(@new);
                    //Maps.MapKeys(address2, address);
                    //_context.Addresses.Update(address2);
                }
                else throw CertNotFound(x.EmployeeModelId.Value, cert, x);
                return;
            }
            if (val.Equals("PrimaryTertiary", StringComparison.InvariantCultureIgnoreCase))
            {
                await _context.Entry(x)
                    .Reference(x => x.Qualification)
                    .LoadAsync();
                var qualif = await _context.Qualifications.SingleOrDefaultAsync(q => q.QualificationModelId == x.Qualification.PrimaryTertiaryQualificationModelId);
                var cert = qualif.Certification;
                if (cert.CompareTo(certification) == 0)
                {
                    var address = await _context.Addresses.SingleOrDefaultAsync(a => a.AddressModelId == qualif.AcademyAddressModelId);

                    if (address == default)
                        throw new ApplicationStateException(EntityMissing, 0, null);
                    if (!Maps.Map(address).Equals(old))
                        throw new ApplicationArgumentException(MismatchedEntities, 0, null);

                    var nation = await _context.Nations.SingleOrDefaultAsync(n => n.NationalityModelId == address.CountryOfResidenceNationalityModelId);
                    nation.LGA = @new.CountryOfResidence.LGA;
                    nation.Language = @new.CountryOfResidence.Language;

                    var cname = await _context.Names.SingleOrDefaultAsync(n => n.NameModelId == nation.CountryNameNameModelId);
                    cname.Name = @new.CountryOfResidence.CountryName.Name;
                    var sname = await _context.Names.SingleOrDefaultAsync(n => n.NameModelId == nation.StateNameModelId);
                    sname.Name = @new.CountryOfResidence.State.Name;
                    var tname = await _context.Names.SingleOrDefaultAsync(n => n.NameModelId == nation.CityTownNameModelId);
                    tname.Name = @new.CountryOfResidence.CityTown.Name;

                    address.StreetAddress = @new.StreetAddress;
                    address.PMB = @new.PMB;
                    address.ZIPCode = @new.ZIPCode;

                    //var address2 = Maps.ReverseMap(@new);
                    //Maps.MapKeys(address2, address);
                    //_context.Addresses.Update(address2);
                }
                else throw CertNotFound(x.EmployeeModelId.Value, cert, x);
                return;
            }
            if (val.Equals("Others", StringComparison.InvariantCultureIgnoreCase))
            {
                await _context.Entry(x)
                    .Reference(x => x.Qualification)
                    .Query()
                    .Include(x => x.Others)
                    .LoadAsync();
                foreach (var item in x.Qualification.Others)
                {
                    var cert = item.Certification;
                    if (cert.CompareTo(certification) == 0)
                    {
                        var address = await _context.Addresses.SingleOrDefaultAsync(a => a.AddressModelId == item.AcademyAddressModelId);

                        if(address == default)
                            throw new ApplicationStateException(EntityMissing, 0, null);
                        if (!Maps.Map(address).Equals(old))
                            throw new ApplicationArgumentException(MismatchedEntities, 0, null);

                        var nation = await _context.Nations.SingleOrDefaultAsync(n => n.NationalityModelId == address.CountryOfResidenceNationalityModelId);
                        nation.LGA = @new.CountryOfResidence.LGA;
                        nation.Language = @new.CountryOfResidence.Language;

                        var cname = await _context.Names.SingleOrDefaultAsync(n => n.NameModelId == nation.CountryNameNameModelId);
                        cname.Name = @new.CountryOfResidence.CountryName.Name;
                        var sname = await _context.Names.SingleOrDefaultAsync(n => n.NameModelId == nation.StateNameModelId);
                        sname.Name = @new.CountryOfResidence.State.Name;
                        var tname = await _context.Names.SingleOrDefaultAsync(n => n.NameModelId == nation.CityTownNameModelId);
                        tname.Name = @new.CountryOfResidence.CityTown.Name;

                        address.StreetAddress = @new.StreetAddress;
                        address.PMB = @new.PMB;
                        address.ZIPCode = @new.ZIPCode;

                        //var address2 = Maps.ReverseMap(@new);
                        //Maps.MapKeys(address2, address);
                        //_context.Addresses.Update(address2);
                    }
                    else throw CertNotFound(x.EmployeeModelId.Value, cert, x);
                    return;
                }
                throw CertNotFound(x.EmployeeModelId.Value, certification, x);
            }
            throw new ApplicationArgumentException($"The level \"{val}\" is invalid", x.EmployeeModelId.Value, x);
        }

        public async Task<bool> UpdateGrades(int? id, string level, string certification, IDictionary<string, string> old, IDictionary<string, string> @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId.Value == id);
            try
            {
                await UpdateGradesHelper(level, certification, x, old, @new);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        async Task UpdateGradesHelper(string val, string certification, EmployeeModel x, IDictionary<string, string> old, IDictionary<string, string> @new)
        {
            if (val.Equals("Primary", StringComparison.InvariantCultureIgnoreCase))
            {
                await _context.Entry(x)
                    .Reference(x => x.Qualification)
                    .LoadAsync();
                var qualif = await _context.Qualifications.SingleOrDefaultAsync(q => q.QualificationModelId == x.Qualification.PrimaryQualificationModelId);
                var cert = qualif.Certification;
                if (cert.CompareTo(certification) == 0)
                {
                    var grades = qualif.Grades;

                    if (grades == default)
                        throw new ApplicationStateException(EntityMissing, 0, null);
                    if (!Equals(grades, old))
                        throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                    qualif.Grades = new Dictionary<string, string>(@new);
                }
                else throw CertNotFound(x.EmployeeModelId.Value, cert, x);
                return;
            }
            if (val.Equals("Secondary", StringComparison.InvariantCultureIgnoreCase))
            {
                await _context.Entry(x)
                    .Reference(x => x.Qualification)
                    .LoadAsync();
                var qualif = await _context.Qualifications.SingleOrDefaultAsync(q => q.QualificationModelId == x.Qualification.SecondaryQualificationModelId);
                var cert = qualif.Certification;
                if (cert.CompareTo(certification) == 0)
                {
                    var grades = qualif.Grades;

                    if (grades == default)
                        throw new ApplicationStateException(EntityMissing, 0, null);
                    if (!Equals(grades, old))
                        throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                    qualif.Grades = new Dictionary<string, string>(@new);
                }
                else throw CertNotFound(x.EmployeeModelId.Value, cert, x);
                return;
            }
            if (val.Equals("PrimaryTertiary", StringComparison.InvariantCultureIgnoreCase))
            {
                await _context.Entry(x)
                    .Reference(x => x.Qualification)
                    .LoadAsync();
                var qualif = await _context.Qualifications.SingleOrDefaultAsync(q => q.QualificationModelId == x.Qualification.PrimaryTertiaryQualificationModelId);
                var cert = qualif.Certification;
                if (cert.CompareTo(certification) == 0)
                {
                    var grades = qualif.Grades;

                    if (grades == default)
                        throw new ApplicationStateException(EntityMissing, 0, null);
                    if (!Equals(grades, old))
                        throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                    qualif.Grades = new Dictionary<string, string>(@new);
                }
                else throw CertNotFound(x.EmployeeModelId.Value, cert, x);
                return;
            }
            if (val.Equals("Others", StringComparison.InvariantCultureIgnoreCase))
            {
                await _context.Entry(x)
                    .Reference(x => x.Qualification)
                    .Query()
                    .Include(x => x.Others)
                    .LoadAsync();
                foreach (var item in x.Qualification.Others)
                {
                    var cert = item.Certification;
                    if (cert.CompareTo(certification) == 0)
                    {
                        var grades = item.Grades;

                        if (grades == default)
                            throw new ApplicationStateException(EntityMissing, 0, null);
                        if (!Equals(grades, old))
                            throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                        item.Grades = new Dictionary<string, string>(@new);
                    }
                    else throw CertNotFound(x.EmployeeModelId.Value, cert, x);
                    return;
                }
                throw CertNotFound(x.EmployeeModelId.Value, certification, x);
            }
            throw new ApplicationArgumentException($"The level \"{val}\" is invalid", x.EmployeeModelId.Value, x);
        }

        static bool Equals(IDictionary<string, string> lhs, IDictionary<string, string> rhs)
        {
            var keys = lhs.Keys;
            if (rhs.Keys.Count != keys.Count) return false;
            for (int i = 0; i < keys.Count; i++)
            {
                bool hasKey = rhs.ContainsKey(keys.ElementAt(i));
                if (hasKey && rhs.TryGetValue(keys.ElementAt(i), out string val))
                {
                    if (val.CompareTo(lhs[keys.ElementAt(i)]) == 0)
                        continue;
                    return false;
                }
                else
                    return false;
            }
            return true;
        }

        public async Task<bool> UpdateGrade(int? id, string level, string certification, string course, string old, string @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId.Value == id);
            try
            {
                await UpdateGradeHelper(level, certification, course, x, old, @new);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        async Task UpdateGradeHelper(string val, string certification, string course, EmployeeModel x, string old, string @new)
        {
            if (val.Equals("Primary", StringComparison.InvariantCultureIgnoreCase))
            {
                await _context.Entry(x)
                    .Reference(x => x.Qualification)
                    .LoadAsync();
                var qualif = await _context.Qualifications.SingleOrDefaultAsync(q => q.QualificationModelId == x.Qualification.PrimaryQualificationModelId);
                var cert = qualif.Certification;
                if (cert.CompareTo(certification) == 0)
                {
                    try
                    {
                        var grades = qualif.Grades;
                        if (!grades.ContainsKey(course))
                            throw new ApplicationArgumentException($"The argument \"{nameof(course)}\" was invalid");
                        if (grades[course] == default)
                            throw new ApplicationStateException(EntityMissing, 0, null);
                        if (grades[course].CompareTo(old) != 0)
                            throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                        qualif.Grades[course] = @new;
                    }
                    catch (KeyNotFoundException ex)
                    {
                        throw new ApplicationArgumentException("Something went wrong when setting the grade in the dictionary", x.EmployeeModelId.Value, ex);
                    }
                }
                else throw CertNotFound(x.EmployeeModelId.Value, cert, x);
                return;
            }
            if (val.Equals("Secondary", StringComparison.InvariantCultureIgnoreCase))
            {
                await _context.Entry(x)
                    .Reference(x => x.Qualification)
                    .LoadAsync();
                var qualif = await _context.Qualifications.SingleOrDefaultAsync(q => q.QualificationModelId == x.Qualification.SecondaryQualificationModelId);
                var cert = qualif.Certification;
                if (cert.CompareTo(certification) == 0)
                {
                    try
                    {
                        var grades = qualif.Grades;
                        if (!grades.ContainsKey(course))
                            throw new ApplicationArgumentException($"The argument \"{nameof(course)}\" was invalid");
                        if (grades[course] == default)
                            throw new ApplicationStateException(EntityMissing, 0, null);
                        if (grades[course].CompareTo(old) != 0)
                            throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                        qualif.Grades[course] = @new;
                    }
                    catch (KeyNotFoundException ex)
                    {
                        throw new ApplicationArgumentException("Something went wrong when setting the grade in the dictionary", x.EmployeeModelId.Value, ex);
                    }
                }
                else throw CertNotFound(x.EmployeeModelId.Value, cert, x);
                return;
            }
            if (val.Equals("PrimaryTertiary", StringComparison.InvariantCultureIgnoreCase))
            {
                await _context.Entry(x)
                    .Reference(x => x.Qualification)
                    .LoadAsync();
                var qualif = await _context.Qualifications.SingleOrDefaultAsync(q => q.QualificationModelId == x.Qualification.PrimaryTertiaryQualificationModelId);
                var cert = qualif.Certification;
                if (cert.CompareTo(certification) == 0)
                {
                    try
                    {
                        var grades = qualif.Grades;
                        if (!grades.ContainsKey(course))
                            throw new ApplicationArgumentException($"The argument \"{nameof(course)}\" was invalid");
                        if (grades[course] == default)
                            throw new ApplicationStateException(EntityMissing, 0, null);
                        if (grades[course].CompareTo(old) != 0)
                            throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                        qualif.Grades[course] = @new;
                    }
                    catch (KeyNotFoundException ex)
                    {
                        throw new ApplicationArgumentException("Something went wrong when setting the grade in the dictionary", x.EmployeeModelId.Value, ex);
                    }
                }
                else throw CertNotFound(x.EmployeeModelId.Value, cert, x);
                return;
            }
            if (val.Equals("Others", StringComparison.InvariantCultureIgnoreCase))
            {
                await _context.Entry(x)
                    .Reference(x => x.Qualification)
                    .Query()
                    .Include(x => x.Others)
                    .LoadAsync();
                foreach (var item in x.Qualification.Others)
                {
                    var cert = item.Certification;
                    if (cert.CompareTo(certification) == 0)
                    {
                        try
                        {
                            var grades = item.Grades;
                            if (!grades.ContainsKey(course))
                                throw new ApplicationArgumentException($"The argument \"{nameof(course)}\" was invalid");
                            if (grades[course] == default)
                                throw new ApplicationStateException(EntityMissing, 0, null);
                            if (grades[course].CompareTo(old) != 0)
                                throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                            item.Grades[course] = @new;
                        }
                        catch (KeyNotFoundException ex)
                        {
                            throw new ApplicationArgumentException("Something went wrong when setting the grade in the dictionary", x.EmployeeModelId.Value, ex);
                        }
                    }
                    else throw CertNotFound(x.EmployeeModelId.Value, cert, x);
                    return;
                }
                throw CertNotFound(x.EmployeeModelId.Value, certification, x);
            }
            throw new ApplicationArgumentException($"The level \"{val}\" is invalid", x.EmployeeModelId.Value, x);
        }

        //Needs fixing
        public async Task<bool> UpdateGuarantor(int? id, IPerson old, IPerson @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId.Value == id);
            try
            {
                PersonModel g = await _context.Persons.SingleOrDefaultAsync(p => p.PersonModelId == x.PersonModelId);

                if (g == default) throw new ApplicationStateException(EntityMissing, 0, null);
                if (g.UniqueTag != old.UniqueTag) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                var val = Maps.ReverseMap(@new);
                Maps.MapKeys(val, g);
                g = val;

                _context.Persons.Update(g);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateFingerprint(int? id, byte[] old, byte[] @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId.Value == id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateSignature(int? id, byte[] old, byte[] @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId.Value == id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdatePassport(int? id, byte[] old, byte[] @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId.Value == id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateJobType(int? id, string old, string @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId.Value == id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateIdNumberFor(int? id, string idType, string old, string @new)
        {
            var x = await _context.Employees.FindAsync(id);
            try
            {
                var v = await _context.Persons.SingleOrDefaultAsync(p => p.PersonModelId == x.PersonModelId);
                if(!v.Identification.ContainsKey(idType))
                    throw new ApplicationArgumentException($"Argument \"idType\" did not match any found", id);
                try
                {
                    if(v.Identification[idType].CompareTo(old) != 0) throw new ApplicationArgumentException(MismatchedEntities, 0, null);
                    v.Identification[idType] = @new;

                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (KeyNotFoundException ex)
                {
                    throw new ApplicationArgumentException("Something went wrong when setting the grade in the dictionary", x.EmployeeModelId.Value, ex);
                }
            }
            catch (NullReferenceException)
            {
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateSex(int? id, bool old, bool @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId.Value == id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateUniqueTag(int? id, Guid old, Guid @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId.Value == id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateBirthDate(int? id, DateTime old, DateTime @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId.Value == id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateCountryOfOrigin(int? id, IName old, IName @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId.Value == id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateStateOfOrigin(int? id, IName old, IName @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId.Value == id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateTown_CityOfOrigin(int? id, IName old, IName @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId.Value == id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateLGAOfOrigin(int? id, string old, string @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId.Value == id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateLanguage(int? id, string old, string @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId.Value == id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateNextOfKin(int? id, IPerson old, IPerson @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId.Value == id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateMaritalStatus(int? id, MaritalStatus old, MaritalStatus @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId.Value == id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateBusinessName(int? id, IName old, IName @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId.Value == id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateBusinessEmail(int? id, [EmailAddress] string old, [EmailAddress] string @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId.Value == id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateBusinessMobile(int? id, [Phone] string old, [Phone] string @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId.Value == id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateBusinessSocialMedia(int? id, [Url] string old, [Url] string @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId.Value == id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateBusinessWebsite(int? id, [Url] string old, [Url] string @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId.Value == id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateBusinessStreetAddress(int? id, string old, string @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId.Value == id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateBusinessZIPCode(int? id, string old, string @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId.Value == id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateBusinessPMB(int? id, string old, string @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId.Value == id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateBusinessCountryOfResidence(int? id, IName old, IName @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId.Value == id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateBusinessStateOfResidence(int? id, IName old, IName @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId.Value == id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateBusinessTown_CityOfResidence(int? id, IName old, IName @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId.Value == id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateBusinessLGAOfResidence(int? id, string old, string @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId.Value == id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateDisplayName(int? id, IName old, IName @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId.Value == id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateEmail(int? id, [EmailAddress] string old, [EmailAddress] string @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId.Value == id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateMobile(int? id, [Phone] string old, [Phone] string @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId.Value == id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateSocialMedia(int? id, [Url] string old, [Url] string @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId.Value == id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateWebsite(int? id, [Url] string old, [Url] string @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId.Value == id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateStreetAddress(int? id, string old, string @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId.Value == id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateZIPCode(int? id, string old, string @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId.Value == id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdatePMB(int? id, string old, string @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId.Value == id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateCountryOfResidence(int? id, IName old, IName @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId.Value == id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateStateOfResidence(int? id, IName old, IName @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId.Value == id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateTown_CityOfResidence(int? id, IName old, IName @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId.Value == id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public async Task<bool> UpdateLGAOfResidence(int? id, string old, string @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(val => val.EmployeeModelId.Value == id);
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
                throw NotFound(InvalidId, id);
            }
            catch (DbUpdateConcurrencyException) { return false; }
            catch (DbUpdateException) { return false; }
        }

        public Task<IName> RemoveOtherName(int? id, IName val)
        {
            throw new NotImplementedException();
        }

        public Task<string> RemoveTitle(int? id, string val)
        {
            throw new NotImplementedException();
        }

        public Task<string> RemoveId(int? id, string val)
        {
            throw new NotImplementedException();
        }

        public Task<string> RemoveGrade(int? id, string certification, string course)
        {
            throw new NotImplementedException();
        }

        public Task<string> RemoveBusinessEmail(int? id, [EmailAddress] string val)
        {
            throw new NotImplementedException();
        }

        public Task<string> RemoveBusinessMobile(int? id, [Phone] string val)
        {
            throw new NotImplementedException();
        }

        public Task<string> RemoveBusinessSocialMedia(int? id, [Url] string val)
        {
            throw new NotImplementedException();
        }

        public Task<string> RemoveBusinessWebsites(int? id, [Url] string val)
        {
            throw new NotImplementedException();
        }

        public Task<string> RemoveEmail(int? id, [EmailAddress] string val)
        {
            throw new NotImplementedException();
        }

        public Task<string> RemoveMobile(int? id, [Phone] string val)
        {
            throw new NotImplementedException();
        }

        public Task<string> RemoveSocialMedia(int? id, [Url] string val)
        {
            throw new NotImplementedException();
        }

        public Task<string> RemoveWebsite(int? id, [Url] string val)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> InvalidateToken(int? id, string @new)
        {
            var x = await _context.Employees.SingleOrDefaultAsync(c => c.EmployeeModelId == id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Secret)
                    .LoadAsync();
                var res = x.Secret.Tokens.Remove(@new);

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
            var x = await _context.Employees.SingleOrDefaultAsync(c => c.EmployeeModelId  == id);
            try
            {
                await _context.Entry(x)
                    .Reference(x => x.Secret)
                    .LoadAsync();
                await x.Secret.InvalidateTokens();

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

        public async Task<bool> ValidatePassword(int? id, ILoginCredentials value)
        {
            try
            {
                //var x = await _context.FindAsync<EmployeeModel>(Int32.Parse(e.DatabaseId));
                var x = await _context.Employees.FindAsync(id);
                await _context.Entry(x)
                    .Reference(x => x.Secret)
                    .LoadAsync();
                var secret = x.Secret.Password;
                //var refer = Utilities.ToString(Convert.FromBase64String(secret));
                var refer = Encoding.UTF8.GetString(Convert.FromBase64String(secret));
                var decoded = Utilities.Decrypt(ref refer);
                return decoded.CompareTo(value.Password) == 0;
            }
            catch (NullReferenceException) { return false; }
            finally { value = null; }
        }

        public async Task<bool> ValidateAndChangePassword(int? id, ILoginCredentials oldValue, ILoginCredentials newValue, object hashSrc)
        {
            if (newValue == null)
                throw new NullReferenceException("newValue cannot be null");
            try
            {
                //var x = await _context.FindAsync<EmployeeModel>(Int32.Parse(e.DatabaseId));
                var x = await _context.Employees.FindAsync(id);
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
                    .Reference(x => x.Secret)
                    .LoadAsync();
                if(oldValue == null)
                {
                    if(x.Secret.Password != null)
                        if(x.Secret.Password.Length > 0)
                            throw new ApplicationException("Password already exists");
                //if (x.Secret.Password != null && oldValue == null)
                }
                else if (oldValue == null)
                {
                    /*This should be done at the creation of the ilogincredentials object*/
                    //val.Secret.Password = Convert.ToBase64String(Encoding.UTF8.GetBytes(newValue.Password));
                    var reference = newValue.Password;
                    x.Secret.Password = Utilities.SecretGen(x.HireDate, hashSrc, ref reference);
                    //_context.Entry(val.Secret).State = EntityState.Modified;
                    _context.Entry(x).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return true;
                }
                if (await ValidatePassword(id, oldValue))
                {
                    /*This should be done at the creation of the ilogincredentials object*/
                    //val.Secret.Password = Convert.ToBase64String(Encoding.UTF8.GetBytes(newValue.Password));
                    var reference = newValue.Password;
                    x.Secret.Password = Utilities.SecretGen(x.HireDate, Maps.Map(x.Person), ref reference);
                    //_context.Entry(val.Secret).State = EntityState.Modified;
                    _context.Entry(x).State = EntityState.Added;
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            finally { oldValue = null; newValue = null; }

            return false;
        }

        public async Task<IEmployee> Verify(int? id, ILoginCredentials loginCredential)
        {
            var emp = await _context.Employees.FindAsync(id);
            if (emp == default)
                return default;
            await _context.Entry(emp).Reference(x => x.Secret).LoadAsync();
            var val = loginCredential.PersonalOnlineKey.CompareTo(emp.Secret.PersonalOnlineKey) == 0
                && loginCredential.Username.CompareTo(emp.Secret.Username) == 0
                && Verify(Maps.Map(emp.Secret), loginCredential);
            if (val != default)
                return Maps.Map(emp);
            return default;
        }

        private static bool Verify(ILoginCredentials x, ILoginCredentials y)
        {
            var refer = Encoding.UTF8.GetString(Convert.FromBase64String(x.Password));
            var decoded = Utilities.Decrypt(ref refer);
            return decoded.CompareTo(y.Password) == 0;
        }
    
        public async Task<IEmployee> Remove(int? id)
        {
            var x = await _context.Employees.FindAsync(id);
            if (x == null)
                return null;
            var val = _context.Employees.Remove(x);
            await _context.SaveChangesAsync();
            return Maps.Map(val.Entity);
        }
    }
}
