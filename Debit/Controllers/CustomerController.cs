using AutoMapper;
using Debit.DTOs;
using Debit.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Debit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly AppDbContext dbContext;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;

        public CustomerController
        (
            AppDbContext dbContext, 
            IConfiguration configuration,
            IMapper mapper
        )
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
            this.configuration = configuration;
        }

        [HttpPost]
        public async Task<ActionResult> Post(string Name,string PhoneNumber)
        {
            if(!IsValidPhoneNumber(PhoneNumber))
            {
                return BadRequest(new { message = "Số diện thoại không đúng định dạng" });
            }    
            else if (CheckCustomer(Name, PhoneNumber))
            {
                Customer customer = new Customer();
                customer.Id = Guid.NewGuid();
                customer.Name = Name;
                customer.PhoneNumber = PhoneNumber;
                await dbContext.Customers.AddAsync(customer);
                await dbContext.SaveChangesAsync();
                return Ok(customer);
            }
            return BadRequest(new { message = "Tên hoặc số điện thoại đã tồn tại !" });
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private bool CheckCustomer(string Name, string PhoneNumber)
        {
            var Customer = dbContext.Customers.FirstOrDefault(x => x.Name == Name || x.PhoneNumber == PhoneNumber);
            if (Customer == null)
            {
                return true;
            }
            return false;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private bool IsValidPhoneNumber(string phoneNumber)
        {
            return Regex.Match(phoneNumber,
                @"^([\+]?61[-]?|[0])?[1-9][0-9]{8}$").Success;
        }

        [HttpGet]
        [Route("GetAllCustomer")]
        public async Task<ActionResult<List<CustomerDTO>>> GetAllCustomer()
        {
            List<Customer> customer = await dbContext.Customers.ToListAsync();
            var listCustomer = mapper.Map<List<CustomerDTO>>(customer);
            return Ok(listCustomer);
        }

        
    }
}
