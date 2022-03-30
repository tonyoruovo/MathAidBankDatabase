using DataBaseTest.Data;
using DataBaseTest.Handlers;
using DataBaseTest.Repos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace DataBaseTest
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            //services.AddMvc(options => options.EnableEndpointRouting = false)
            //    .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
            //    .AddNewtonsoftJson()
            //services.AddAuthentication("BasicAuthentication")
            //    .AddScheme<AuthenticationSchemeOptions, EmployeeBasicAuthenticationHandler>("BasicAuthentication", null);
            //services.AddAuthentication("BasicAuthentication")
            //    .AddScheme<AuthenticationSchemeOptions, CustomerBasicAuthenticationHandler>("BasicAuthentication", null);

            services.AddDbContext<BankDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("employees")));
            services.AddDbContext<BankDbContext2>(options => options.UseSqlServer(Configuration.GetConnectionString("customers")));

            //services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<BankDbContext>()
            //    .AddDefaultTokenProviders();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            //services.AddTransient<ITokenManager, TokenManager>();
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddScoped<ILoginAccountRepository, CustomerRepository>();

            services.AddCors(opt =>
            {
                opt.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyHeader().AllowAnyMethod();
                });
            });

            var empJwtSection = Configuration.GetSection(nameof(EmployeeJWTSettings));
            var empAdminJwtSection = Configuration.GetSection(nameof(EmployeeAdminJWTSettings));
            var adminAdminJwtSection = Configuration.GetSection(nameof(AdminAdminJWTSettings));
            var jwtSection = Configuration.GetSection(nameof(JWTSettings));
            var customerJwtSection = Configuration.GetSection(nameof(CustomerJWTSettings));
            var vipCustomerJwtSection = Configuration.GetSection(nameof(VIPCustomerJWTSettings));
            services.Configure<EmployeeJWTSettings>(empJwtSection);
            services.Configure<EmployeeAdminJWTSettings>(empAdminJwtSection);
            services.Configure<AdminAdminJWTSettings>(adminAdminJwtSection);
            services.Configure<JWTSettings>(jwtSection);
            services.Configure<CustomerJWTSettings>(customerJwtSection);
            services.Configure<VIPCustomerJWTSettings>(vipCustomerJwtSection);

            var appSettings = empJwtSection.Get<EmployeeJWTSettings>();
            var key = Encoding.UTF8.GetBytes(appSettings.SecretKey);
            var appSettings2 = empAdminJwtSection.Get<EmployeeAdminJWTSettings>();
            var key2 = Encoding.UTF8.GetBytes(appSettings2.SecretKey);
            var appSettings3 = adminAdminJwtSection.Get<AdminAdminJWTSettings>();
            var key3 = Encoding.UTF8.GetBytes(appSettings3.SecretKey);
            var appSettings4 = jwtSection.Get<JWTSettings>();
            var key4 = Encoding.UTF8.GetBytes(appSettings4.SecretKey);
            var appSettings5 = empAdminJwtSection.Get<CustomerJWTSettings>();
            var key5 = Encoding.UTF8.GetBytes(appSettings5.SecretKey);
            var appSettings6 = adminAdminJwtSection.Get<VIPCustomerJWTSettings>();
            var key6 = Encoding.UTF8.GetBytes(appSettings6.SecretKey);

            /*
             * Using this snippet causes the authentication middleware to run every request through
             * the specified schemes.
             */
            //services.AddAuthentication(x =>
            //{
            //    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            services.AddAuthentication()
                .AddJwtBearer(Utilities.Auth_1, x =>
                {
                    x.RequireHttpsMetadata = true;
                    x.SaveToken = true;//Remember to find out what this stands for
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        /*Already set @ login in the employees controller */
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidAudience = appSettings.ValidAudience,
                        ValidIssuer = appSettings.ValidIssuer
                    };
                })
                .AddJwtBearer(Utilities.Auth_2, x =>
                {
                    x.RequireHttpsMetadata = true;
                    x.SaveToken = true;//Remember to find out what this stands for
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key2),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = appSettings2.ValidAudience,
                        ValidIssuer = appSettings2.ValidIssuer
                    };
                })
                .AddJwtBearer(Utilities.Auth_3, x =>
                {
                    x.RequireHttpsMetadata = true;
                    x.SaveToken = true;//Remember to find out what this stands for
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key3),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = appSettings3.ValidAudience,
                        ValidIssuer = appSettings3.ValidIssuer
                    };
                })
                .AddJwtBearer(Utilities.Auth_4, x =>
                {
                    x.RequireHttpsMetadata = true;
                    x.SaveToken = true;//Remember to find out what this stands for
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key4),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = appSettings4.ValidAudience,
                        ValidIssuer = appSettings4.ValidIssuer
                    };
                })
                .AddJwtBearer(Utilities.Auth_5, x =>
                {
                    x.RequireHttpsMetadata = true;
                    x.SaveToken = true;//Remember to find out what this stands for
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key5),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = appSettings5.ValidAudience,
                        ValidIssuer = appSettings5.ValidIssuer
                    };
                })
                .AddJwtBearer(Utilities.Auth_6, x =>
                {
                    x.RequireHttpsMetadata = true;
                    x.SaveToken = true;//Remember to find out what this stands for
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key6),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = appSettings6.ValidAudience,
                        ValidIssuer = appSettings6.ValidIssuer
                    };
                });

            services
                .AddAuthorization(options =>
                {
                    //When you use the [Authorize] attribute on a controller, without any parameter(s), this policy gets used for 
                    //Authorization
                    options.DefaultPolicy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .AddAuthenticationSchemes(Utilities.Auth_1, Utilities.Auth_2, Utilities.Auth_3, Utilities.Auth_4, Utilities.Auth_5, Utilities.Auth_6)
                        .RequireClaim(ClaimTypes.Role, appSettings.Role, appSettings2.Role, appSettings3.Role, appSettings4.Role, appSettings5.Role, appSettings6.Role)
                        .Build();

                    options.AddPolicy(Utilities.Policy5, new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .AddAuthenticationSchemes(Utilities.Auth_1, Utilities.Auth_2, Utilities.Auth_3, Utilities.Auth_4, Utilities.Auth_5)
                        .RequireClaim(ClaimTypes.Role, appSettings.Role, appSettings2.Role, appSettings3.Role, appSettings4.Role, appSettings5.Role)
                        .Build());

                    options.AddPolicy(Utilities.Policy4, new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .AddAuthenticationSchemes(Utilities.Auth_1, Utilities.Auth_2, Utilities.Auth_3, Utilities.Auth_4)
                        .RequireClaim(ClaimTypes.Role, appSettings4.Role, appSettings.Role, appSettings2.Role, appSettings3.Role)
                        .Build());

                    options.AddPolicy(Utilities.Policy3, new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .AddAuthenticationSchemes(Utilities.Auth_1, Utilities.Auth_2, Utilities.Auth_3)
                        .RequireClaim(ClaimTypes.Role, appSettings.Role, appSettings2.Role, appSettings3.Role)
                        .Build());

                    options.AddPolicy(Utilities.Policy2, new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .AddAuthenticationSchemes(Utilities.Auth_2, Utilities.Auth_3)
                        .RequireClaim(ClaimTypes.Role, appSettings2.Role, appSettings3.Role)
                        .Build());

                    options.AddPolicy(Utilities.Policy1, new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .AddAuthenticationSchemes(Utilities.Auth_3)
                        .RequireClaim(ClaimTypes.Role, appSettings3.Role)
                        .Build());

                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors();

            app.UseAuthentication();
            //app.UseMiddleware<TokenManagerMiddleware>();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
