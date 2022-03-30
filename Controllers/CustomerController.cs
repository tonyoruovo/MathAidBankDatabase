using DataBaseTest.Repos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static DataBaseTest.Repos.Builders;

namespace DataBaseTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customer;

        public ICustomer Value { get; private set; }
        public int Id { get; private set; }
        public string Token { get; private set; }

        public CustomerController(ICustomerRepository customer)
        {
            _customer = customer;
        }

        public async Task<IActionResult> SignIn()
        {
            return Ok(Task.CompletedTask);
        }

        public new async Task<IActionResult> SignOut()
        {
            Id = 0;
            Value = null;
            Token = null;
            //var user = ;
            return Ok(Task.CompletedTask);
        }

        public async Task<IActionResult> PostAccount([FromBody] object @new)
        {
            var ac = @new as IAccount;
            var val = await _customer.AddAccount(Id, ac);
            if (val)
                return NoContent();
            else return BadRequest();
        }

        //public async Task<IActionResult> PutAccount([FromBody] object val)
        //{
        //    var ac = val as IAccount;
        //    var v = await _customer.UpdateAccount(Id, ac);
        //    if (v)
        //        return Ok(v);
        //    else return BadRequest();
        //}

        public async Task<IActionResult> GetAccount([FromQuery]string num)
        {
            var val = await _customer.GetAccount(Id, num);
            if (val != default)
            {
                Value = new CustomerBuilder()
                    .ReBuild(Value)
                    .AddAccount(val)
                    .Build();
                return Ok(val);
            }
            else return BadRequest();
        }

    }
}
