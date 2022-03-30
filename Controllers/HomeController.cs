using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DataBaseTest.Controllers
{
    /// <summary>
    /// This should all be in the frontend side and not here.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {

        [HttpPost("postaccount")]
        public async Task OpenPartialAccount()
        {
            await Task.CompletedTask;
        }

        public async Task CustomerLoginWithCredentials()
        {
            await Task.CompletedTask;
        }

        //When Logged in as an employee, depending on your clearance level,
        //You should be able to see a button to open an account for a client
        /*
         * This button is the generic account opening button.
         * For a more specific button, one should navigate
         * to the corresponding tab. For example, the Consumer
         * tab will always have a savings and current account
         * as well as a time deposit and a dom account. On the
         * other hand, the company tab will have loan accounts,
         * debenture account, company account and so on...
         * So this method here should be attributed with the
         * [Authorized] attribute to prevent just anyone from
         * accessing it.
         * 
         * As an the ceo or main administrator or the human
         * resources person assigned for this, when an employee
         * is logged in, depending on their authourisation,
         * they should see a button to add a new employee
         */

        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task OpenAccount()
        {
            await Task.CompletedTask;
        }
    }
}
