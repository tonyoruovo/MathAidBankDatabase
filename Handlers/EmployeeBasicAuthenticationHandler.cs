using DataBaseTest.Repos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using static DataBaseTest.Repos.Builders;

namespace DataBaseTest.Handlers
{
    public class EmployeeBasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IEmployeeRepository _employees;

        public EmployeeBasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IEmployeeRepository employees)
            : base(options, logger, encoder, clock)
        {
            _employees = employees;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var key = "Authorization";
            if(!Request.Headers.ContainsKey(key))
                return AuthenticateResult.Fail("Unauthorised request");

            try
            {
                var splitChar = ':';
                var authenticationHeaderValue = AuthenticationHeaderValue.Parse(Request.Headers[key]);
                var bytes = Convert.FromBase64String(authenticationHeaderValue.Parameter);
                var credentials = Encoding.UTF8.GetString(bytes).Split(splitChar);
                var id = Int32.Parse(credentials[1]);
                var pass = credentials[2];

                var login = new LoginCredentialsBuilder().SetPassword(pass);
                var isValid = await _employees.ValidatePassword(id, login.Build());

                if (!isValid)
                    return AuthenticateResult.Fail("Id and password does not match");

                var claims = new[] { 
                    new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                    new Claim(ClaimTypes.Name, credentials[0]) // The display/user name
                };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }
            catch (Exception e)
            {
                return AuthenticateResult.Fail(e);
            }
        }
    }
}
