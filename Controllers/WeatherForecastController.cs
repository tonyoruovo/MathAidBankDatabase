using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DataBaseTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IConfiguration _config;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        [HttpGet]
        public /*IEnumerable<WeatherForecast>*/string Get()
        {
            /*var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            //.ToArray();*/
            return _config.GetConnectionString("employees");
            //var fn = new FullNameModel {
            //FullNameModelId= new Random().Next(),
            //    Name = new NameModel { Name = "Anthony", NameModelId = new Random().Next() },
            //    Nickname = new NameModel { Name = "tine", NameModelId = new Random().Next() },
            //    OtherNames = null,
            //    Surname = new NameModel { Name = "Oruovo", NameModelId = new Random().Next() },
            //    Title = "Mr"
            //};
            //var cur = new NationalityModel{ 
            //CityTown = new NameModel{Name="Ikoyi", NameModelId=new Random().Next() },
            //    CountryName = new NameModel { Name = "Nigeria", NameModelId = new Random().Next() },
            //    Language = "English",
            //    LGA = "IKoyi",
            //    NationalityModelId = new Random().Next(),
            //    State = new NameModel { Name = "Lagos", NameModelId = new Random().Next() }
            //};
            //var ad = new AddressModel {
            //ZIPCode = "203982",
            //StreetAddress = "No 14 Lake Island",
            //PMB = "kjsnh3287",
            //AddressModelId = new Random().Next(),
            //CountryOfResidence = cur
            //};
            //var sc = new List<string>();
            //sc.Add("twitter.com/tonyOruovo");
            //sc.Add("youtube.com/tonyOruovo");
            //sc.Add("facebook.com/tonyOruovo");
            //var websites = new List<string>();
            //websites.Add("www.Estag.Com");
            //var mobiles = new List<string>();
            //mobiles.Add("080762363284623");
            //mobiles.Add("080398365489465");
            //var emails = new List<string>();
            //emails.Add("tonyOruovo@gmail.com");
            //emails.Add("anthonyOruovo1@gmail.com");
            //emails.Add("Oruovoanthony1@gmail.com");
            //var cont = new ContactModel
            //{
            //    ContactModelId = new Random().Next(),
            //    Emails = emails,
            //    Mobiles = mobiles,
            //    Websites = websites,
            //    SocialMedia = sc,
            //    Address = ad
            //};
            //var ent = new EntityModel
            //{
            //    Name = new NameModel { Name = "central executive officer", NameModelId = new Random().Next()},
            //    EntityModelId = new Random().Next(),
            //    Contact = cont
            //};
            //var cor = new NationalityModel
            //{
            //    CityTown = new NameModel { Name = "VGC", NameModelId = new Random().Next() },
            //    CountryName = new NameModel { Name = "Nigeria", NameModelId = new Random().Next() },
            //    Language = "English",
            //    LGA = "Ikoyi",
            //    NationalityModelId = new Random().Next(),
            //    State = new NameModel { Name = "Lagos", NameModelId = new Random().Next() },
            //};
            //var coo = new NationalityModel {
            //CityTown = new NameModel { Name="Erhuvwaren",NameModelId=new Random().Next()},
            //CountryName = new NameModel { Name = "Nigeria", NameModelId = new Random().Next() },
            //Language = "English",
            //LGA = "Ugelli South",
            //NationalityModelId = new Random().Next(),
            //State = new NameModel { Name = "Delta", NameModelId = new Random().Next() },
            //};
            //var iden = new Dictionary<string, string>();
            //iden.Add(Constants.INTERNATIONAL, "03947239y6et8edtg283e872");
            //iden.Add(Constants.DRIVER, "eiuceiugi348893");
            //var per = new PersonModel
            //{
            //    UniqueTag = Guid.NewGuid(),
            //    NextOfKin = null,
            //    PersonModelId = new Random().Next(),
            //    MaritalStatus = MaritalStatus.DIVORCED,
            //    BirthDate = new DateTime(1992, 10, 17),
            //    IsMale = true,
            //    Identification = iden,
            //    CountryOfOrigin = coo,
            //    countryOfResidence = cor,
            //    Entity = ent,
            //    FullName = fn
            //};
            //var terCOR = new NationalityModel
            //{
            //    CountryName = new NameModel { NameModelId = new Random().Next(), Name = "Nigeria" },
            //    CityTown = new NameModel { NameModelId = new Random().Next(), Name = "Abraka" },
            //    State = new NameModel { NameModelId = new Random().Next(), Name = "Delta" },
            //    Language = "English",
            //    LGA = "Ethiope East",
            //    NationalityModelId = new Random().Next()
            //};
            //var terAddress = new AddressModel
            //{
            //    PMB = new Random().Next().ToString(),
            //    ZIPCode = "AAS230G",
            //    AddressModelId = new Random().Next(),
            //    CountryOfResidence = terCOR,
            //    StreetAddress = "Delta State University, Abraka",
            //};
            //Dictionary<string, string> terGrades = new Dictionary<string, string>();
            //terGrades.Add("Mathematics", "2");
            //terGrades.Add("English", "1");
            //terGrades.Add("Commerce", "3");
            //terGrades.Add("Economics", "3");
            //terGrades.Add("Government", "2");
            //terGrades.Add("History", "1");
            //terGrades.Add("Biology", "6");
            //terGrades.Add("Literature-in-English", "9");
            //var ter = new QualificationModel
            //{
            //    Ceritfication = "Bachelor of Science",
            //    Academy = terAddress,
            //    Grades = terGrades,
            //    QualificationModelId = new Random().Next()
            //};
            //var secCOR = new NationalityModel
            //{
            //    CountryName = new NameModel { NameModelId = new Random().Next(), Name = "Nigeria" },
            //    CityTown = new NameModel { NameModelId = new Random().Next(), Name = "Agbarho" },
            //    State = new NameModel { NameModelId = new Random().Next(), Name = "Delta" },
            //    Language = "English",
            //    LGA = "Ughelli North",
            //    NationalityModelId = new Random().Next()
            //};
            //var secAddress = new AddressModel
            //{
            //    PMB = "405",
            //    ZIPCode = "AAS230G",
            //    AddressModelId = new Random().Next(),
            //    CountryOfResidence = secCOR,
            //    StreetAddress = "Agbarho Grammar School",
            //};
            //Dictionary<string, string> secGrades = new Dictionary<string, string>();
            //secGrades.Add("Mathematics", "2");
            //secGrades.Add("English", "1");
            //secGrades.Add("Commerce", "3");
            //secGrades.Add("Economics", "3");
            //secGrades.Add("Government", "2");
            //secGrades.Add("History", "1");
            //secGrades.Add("Biology", "6");
            //secGrades.Add("Literature-in-English", "9");
            //var sec = new QualificationModel
            //{
            //    Ceritfication = "Senior School Certificate Examination",
            //    Academy = secAddress,
            //    Grades = secGrades,
            //    QualificationModelId = new Random().Next()
            //};
            //var priCOR = new NationalityModel {
            //    CountryName = new NameModel { NameModelId = new Random().Next(), Name = "Nigeria" },
            //    CityTown = new NameModel { NameModelId = new Random().Next(), Name = "Warri" },
            //    State = new NameModel { NameModelId = new Random().Next(), Name = "Delta" },
            //    Language = "English",
            //    LGA = "Warri South",
            //    NationalityModelId = new Random().Next()
            //};
            //var priAddress = new AddressModel
            //{
            //    PMB = "405",
            //    ZIPCode = "AAS230G",
            //    AddressModelId = new Random().Next(),
            //    CountryOfResidence = priCOR,
            //    StreetAddress = "Pessu Primary School Essi-Layout Warri",
            //};
            //Dictionary<string, string> priGrades = new Dictionary<string, string>();
            //priGrades.Add("Mathematics", "2");
            //priGrades.Add("English", "1");
            //priGrades.Add("Elementary Science", "0");
            //priGrades.Add("Social Studies", "3");
            //priGrades.Add("CRS", "1");
            //var pri = new QualificationModel
            //{
            //    Ceritfication = "School Leaving Certificate",
            //    Academy = priAddress,
            //    Grades = priGrades,
            //    QualificationModelId = new Random().Next()
            //};
            //var res = new EducationModel
            //{
            //    Primary = pri,
            //    Secondary = sec,
            //    PrimaryTertiary = ter,
            //    EducationModelId = new Random().Next(),
            //    Others = null
            //};
            //var emp = new EmployeeModel
            //{
            //    EmployeeModelId = new Random().Next(),
            //    WorkingStatus = WorkingStatus.ACTIVE,
            //    HireDate = DateTime.Now,
            //    Subordinate = null,
            //    Superior = null,
            //    Supervisor = null,
            //    Group = null,
            //    Password = "23456789090876543",
            //    Username = "Employee451",
            //    Position = "CEO",
            //    Level = "0",
            //    Qualification = res,
            //    Person = per
            //};
            //return JsonSerializer.Serialize(emp);
        }
    }
}
