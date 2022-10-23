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
        public async Task<ActionResult> Post([FromBody] CustomerDeBitDTO CustomerDeBitDTO)
        {
            if(!IsValidPhoneNumber(CustomerDeBitDTO.PhoneNumber))
            {
                return BadRequest(new { message = "Số diện thoại không đúng định dạng" });
            }    
            else if (CheckCustomer(CustomerDeBitDTO.Name, CustomerDeBitDTO.PhoneNumber))
            {
                Customer customer = new Customer();
                customer.Id = Guid.NewGuid();
                customer.Name = CustomerDeBitDTO.Name;
                customer.PhoneNumber = CustomerDeBitDTO.PhoneNumber;
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
        public async Task<ActionResult<List<CustomerDeBitDTO>>> GetAllCustomer()
        {
            List<Customer> customer = await dbContext.Customers.ToListAsync();
            var listCustomer = mapper.Map<List<CustomerDeBitDTO>>(customer);
            return Ok(listCustomer);
        }
        [HttpPost]
        [Route("GetAllCustomerDataTable")]
        public async Task<ActionResult<List<CustomerDeBitDTO>>> GetAllCustomerDataTable(DataTableDTO dataTable)
        {
            var column = dataTable.Order.First().Column == 0 ? "name"
                         : "phoneNumber";
            var sort = dataTable.Order.First().Dir;
            List<Customer> customer = await dbContext.Customers.ToListAsync();
            if (dataTable.Search.Value != "")
            {
                customer = customer.Where
                    (x =>
                       x.Name.ToLower().Contains(dataTable.Search.Value.ToLower())
                       || x.PhoneNumber.Contains(dataTable.Search.Value)
                    ).ToList();
            }
            customer = Orderby(customer,column,sort);
            var listCustomer = mapper.Map<List<CustomerDeBitDTO>>(customer.Skip(dataTable.Start).Take(dataTable.Length));
            var total = await dbContext.Customers.CountAsync();
            DTData data = new DTData() { Data = listCustomer, Draw = dataTable.Draw, RecordsTotal = total, RecordsFiltered = total };
            return Ok(data);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public List<Customer> Orderby(List<Customer> customers, string column, string sort)
        {
            if (sort == "desc")
            {
                customers = column == "name" ? customers.OrderByDescending(x => x.Name).ToList() :
                             customers.OrderByDescending(x => x.PhoneNumber).ToList();
            }
            else
            {
                customers = column == "name" ? customers.OrderBy(x => x.Name).ToList() :
                             customers.OrderBy(x => x.PhoneNumber).ToList();
            }
            return customers;
        }


    }
}
