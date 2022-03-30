using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json;
using DataBaseTest.Repos;

namespace DataBaseTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeLoginController : ControllerBase
    {
        /*
         * Note that 'id' here should be encrypted
         */
        [HttpGet("{id:int}")]
        public IActionResult RequestLogin(int id)
        {
            //string raw = Request.Path.Value;//should be encypted at by now!
            //var x = Convert.ToBase64String(Encoding.UTF8.GetBytes(raw));
            //var bytes = Convert.FromBase64String(txtId);
            //var id = Int32.Parse(Encoding.UTF8.GetString(bytes));
            string actualUrl;
            var dict = JsonSerializer
                .Deserialize<Dictionary<uint, string>>(
                System.Net.WebUtility.UrlDecode(Utilities.EmployeeIdDictionary)
                );
            var hasVal = dict.TryGetValue((uint)id, out string value);
            if (!hasVal)
                return BadRequest("Unknown");
            /*
                * The id is encrypted.
                * This page should have a form comprising of a
                * username field and a password, hence the employee object. It should have
                * "hello" first name (or nickname) as it,s page
                * heading
                */
            var encryptedId = System.Net.WebUtility.UrlEncode(id.ToString());
            actualUrl = $"/employees/login?id={encryptedId}&name={System.Net.WebUtility.UrlEncode(value)}";
            return RedirectToPage(actualUrl);
        }

        public IActionResult Login([FromQuery]int id, [FromQuery] string username)
        {
            //var builder = new Builders.LoginCredentialsBuilder();
            //builder.SetPersonalOnlineKey(id.ToString());
            //builder.SetUsername(username);
            //builder.SetPassword(HttpContext.User.)
            //var credentials = n

            return Ok();
        }

        //[HttpPost("[action]")]
        //public async Task<IActionResult> LoginAsync([FromForm] int id, [FromForm] string username, [FromForm] string password)
        //{
        //    //IConfigurationBuilder bd = new ConfigurationBuilder();
        //    IConfigurationBuilder cb = new ConfigurationBuilder();
        //    cb.AddJsonFile("appSettings.json");
        //    IConfiguration cf = cb.Build();
        //    using (var c = new BankDbContext(cf))
        //    {
        //        var emp = c.Employees.Where(e =>
        //        e.Username.Equals(username)
        //            && Encoding.UTF8.GetString(Convert.FromBase64String(e.Password)).Equals(password)
        //            && e.EmployeeModelId == id
        //        ).FirstOrDefault();
        //        if (emp != null)
        //            emp.Password = null;
        //    }

        //    return Ok("<p style=\"color:red\">Truth be told<p>");
        //}
    }

}
