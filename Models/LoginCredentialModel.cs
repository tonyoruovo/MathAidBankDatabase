using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Threading.Tasks;
using static DataBaseTest.Utilities;
#nullable enable
namespace DataBaseTest.Models
{
    public class LoginCredentialModel
    {
        public override int GetHashCode()
        {
            return -1 & (PersonalOnlineKey == null ? 0 : PersonalOnlineKey.GetHashCode()) &
                (Username == null ? 0 : Username.GetHashCode()) &
                (Password == null ? 0 : Password.GetHashCode());
        }

        public override bool Equals(object? obj)
        {
            if (obj is LoginCredentialModel l)
                return PersonalOnlineKey?.CompareTo(l.PersonalOnlineKey) == 0
                    && Username?.CompareTo(l.Username) == 0
                    && Password?.CompareTo(l.Password) == 0;
            return false;
        }

        public LoginCredentialModel()
        {
            Tokens = new();
        }

        [Key]
        public int? LoginCredentialModelId { get; set; }

        //[ForeignKey(nameof(EmployeeModel.Secret))]
        //public int EmployeeSecretId { get; set; }
        //[ForeignKey(nameof(CardModel.Secret))]
        //public int CardSecretId { get; set; }
        //[ForeignKey(nameof(CustomerModel.LoginCredential))]
        //public int LoginCredentialId { get; set; }

        public string? PersonalOnlineKey { get; set; }//OTTP
        public string? Username { get; set; }
        public string? Password { get; set; }

        public List<string>? Tokens
        {
            get;
            set;
        }

        public async Task InvalidateTokens()
        {
            await Task.Run(() =>
            {
                Tokens?.Clear();
            });
        }
    }
}