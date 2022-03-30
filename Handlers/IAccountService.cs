using DataBaseTest.Repos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseTest.Handlers
{
    public interface IAccountService
    {
        //void SignUp(string username, string password);
        JsonWebToken SignIn(int id, ILoginCredentials credential, IJWTSettings settings);
        JsonWebToken RefreshAccessToken(string token, IJWTSettings s);
        void RevokeRefreshToken(string token);
        Task<object> Signout(ILoginCredentials credentialToCompare);
        bool HasValidToken();
    }

    public class JsonWebToken
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expires { get; set; }
    }

    public class RefreshToken
    {
        public ILoginCredentials User { get; protected internal set; }
        public int Id { get; protected internal set; }
        public string Token { get; protected internal set; }
        public bool Revoked { get; protected internal set; }
    }

    public interface ITokenManager
    {
        Task<bool> IsCurrentActiveToken();
        Task DeactivateCurrentAsync();
        Task<bool> IsActiveAsync(string token);
        Task DeactivateAsync(string token);
    }

    public class AccountService : IAccountService
    {
        public AccountService(): this(5)
        {
        }

        public AccountService(/*HttpContext context, */int minutes)
        {
            _refreshTokens = new List<RefreshToken>();
            _jwtHandler = new JWTHandler(minutes);
            //_accessor = context;
        }

        ~AccountService()
        {
            RevokeAll();
        }

        public JsonWebToken SignIn(int id, ILoginCredentials credential, IJWTSettings settings)
        {
            var s = $"{credential.PersonalOnlineKey}:{id}";
            var jwt = _jwtHandler.Create(id, credential.Username, settings);
            var refreshToken = Utilities.SecretGen(DateTime.UtcNow, Guid.NewGuid(), ref s);
            jwt.RefreshToken = refreshToken;
            _refreshTokens.Add(new RefreshToken { User = credential, Token = refreshToken });
            return jwt;
        }

        public JsonWebToken RefreshAccessToken(string token, IJWTSettings settings)
        {
            var refreshToken = GetRefreshToken(token);
            if (refreshToken == null)
                throw new ApplicationException("Refresh token was not found.");
            if (refreshToken.Revoked)
                throw new ApplicationException("Refresh token was revoked");
            var jwt = _jwtHandler.Create(refreshToken.Id, refreshToken.User.Username, settings);
            jwt.RefreshToken = refreshToken.Token;

            return jwt;
        }

        public void RevokeRefreshToken(string token)
        {
            var refreshToken = GetRefreshToken(token);
            if (refreshToken == null)
                throw new ApplicationException("Refresh token was not found.");
            if (refreshToken.Revoked)
                throw new ApplicationException("Refresh token was already revoked.");
            refreshToken.Revoked = true;
        }

        public bool HasValidToken()
        {
            foreach (var item in _refreshTokens)
            {
                if (!item.Revoked)
                    return true;
            }
            return false;
        }

        void RevokeAll()
        {
            foreach (var item in _refreshTokens)
            {
                try
                {
                    RevokeRefreshToken(item.Token);
                }
                catch (ApplicationException)
                {
                }
            }
        }

        public async Task<object> Signout(ILoginCredentials credentialToCompare)
        {
            return await Task.Run(() =>
            {
                foreach (var item in _refreshTokens)
                {
                    var user = item.User;
                    if (user.Username.CompareTo(credentialToCompare.Username) == 0
                        && user.PersonalOnlineKey.CompareTo(credentialToCompare.PersonalOnlineKey) == 0)
                    {
                        var refer = Encoding.UTF8.GetString(Convert.FromBase64String(user.Password));
                        refer = Utilities.Decrypt(ref refer);
                        if (refer.CompareTo(credentialToCompare.Password) == 0)
                        {
                            RevokeAll();
                            var val = new { Credential = user, Tokens = _refreshTokens };
                            _refreshTokens.Clear();
                            return val;
                        }
                    }
                }
                return null;
            });
        }

        private RefreshToken GetRefreshToken(string token)
        {
            foreach (var item in _refreshTokens)
            {
                var refer = Encoding.UTF8.GetString(Convert.FromBase64String(item.Token));
                var t = Utilities.Decrypt(ref refer);
                if (t.CompareTo(token) == 0)
                    return item;
            }
            return null;
        }

        private readonly List<RefreshToken> _refreshTokens;
        private readonly JWTHandler _jwtHandler;
        //private readonly HttpContext _accessor;
    }

    public class JWTHandler
    {
        public JWTHandler(int minutes)
        {
            Minutes = minutes;
        }

        public IJWTSettings JWTSettings { get; private set; }
        public int Minutes { get; }

        public JsonWebToken Create(int id, string username, IJWTSettings jwtSettings)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, jwtSettings.Role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(Minutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = jwtSettings.ValidAudience,
                Issuer = jwtSettings.ValidIssuer,
                IssuedAt = DateTime.UtcNow
                //Claims = Not set because it is the same as Subject
            };
            //var s = username.ToString() + "*" + id;
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = new JsonWebToken
            {
                AccessToken = tokenHandler.WriteToken(token),
                //RefreshToken = Utilities.SecretGen(DateTime.UtcNow, Guid.NewGuid(), ref s),
                Expires = (DateTime) tokenDescriptor.Expires
                
            };
            JWTSettings = jwtSettings;

            return jwt;
        }
    }

    public class TokenManager : ITokenManager
    {
        private readonly IDistributedCache _cache;
        private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly IOptions<JwtOptions> _jwtOptions;

        public TokenManager(IDistributedCache cache,
                IHttpContextAccessor httpContextAccessor)
        {
            _cache = cache;
            _httpContextAccessor = httpContextAccessor;
            //_jwtOptions = jwtOptions;
        }

        public async Task<bool> IsCurrentActiveToken()
            => await IsActiveAsync(GetCurrentAsync());

        public async Task DeactivateCurrentAsync()
            => await DeactivateAsync(GetCurrentAsync());

        public async Task<bool> IsActiveAsync(string token)
            => await _cache.GetStringAsync(GetKey(token)) == null;

        public async Task DeactivateAsync(string token)
            => await _cache.SetStringAsync(GetKey(token),
                " ", new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = null
                    //TimeSpan.FromMinutes(_jwtOptions.Value.ExpiryMinutes)
                });

        private string GetCurrentAsync()
        {
            var authorizationHeader = _httpContextAccessor
                .HttpContext.Request.Headers["authorization"];

            return authorizationHeader == StringValues.Empty
                ? string.Empty
                : authorizationHeader.Single().Split(" ").Last();
        }

        private static string GetKey(string token)
            => $"tokens:{token}:deactivated";
    }
}
