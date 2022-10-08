using AutoMapper;
using Debit.DTOs;
using Debit.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Debit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DebitController : ControllerBase
    {
        private readonly AppDbContext dbContext;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;
        public DebitController
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
        [Route("CreateDebit")]
        public async Task<ActionResult> CreateDebit([FromForm] DebitDTO debit)
        {
            DebitCustomer debitCustomer = new DebitCustomer();
            debitCustomer.Id = Guid.NewGuid();
            debitCustomer.CustomerId = debit.CustomerId;
            debitCustomer.Items = debit.Items;
            debitCustomer.Money = debit.Money;
            await dbContext.DebitCustomer.AddAsync(debitCustomer);
            await dbContext.SaveChangesAsync();
            return Ok(debitCustomer);
        }

        [HttpGet]
        [Route("GetDebitProcess")]
        public async Task<ActionResult> GetDebitProcess()
        {
            var debit = await dbContext.DebitCustomer.Where(x => x.Status == false).ToListAsync();
            var debitDTO = mapper.Map<List<DebitDTO>>(debit);
            return Ok(debitDTO);
        }

        [HttpGet]
        [Route("GetDebitSuccess")]
        public async Task<ActionResult> GetDebitSuccess()
        {
            var debit = await dbContext.DebitCustomer.Where(x => x.Status == true).ToListAsync();
            var debitDTO = mapper.Map<List<DebitDTO>>(debit);
            return Ok(debitDTO);
        }

        [HttpGet]
        [Route("GetDateNow")]
        public async Task<ActionResult> GetDateNow(DateTime date, bool statusProcess)
        {
            var debit = new List<DebitCustomer>();
            debit = statusProcess == true ?
                await dbContext.DebitCustomer.Where(x => x.DateComplete.Date == date.Date && x.Status == statusProcess).ToListAsync() :
                await dbContext.DebitCustomer.Where(x => x.DateComplete.Date == date.Date).ToListAsync();
            var debitDTO = mapper.Map<List<DebitDTO>>(debit);
            return Ok(debitDTO);
        }
        [HttpGet]
        [Route("GetLargerDate")]
        public async Task<ActionResult> GetLargerDate(DateTime date, bool statusProcess)
        {
            var debit = new List<DebitCustomer>();
            debit = statusProcess == true ?
                await dbContext.DebitCustomer.Where(x => x.DateComplete.Date > date.Date && x.Status == statusProcess).ToListAsync() :
                await dbContext.DebitCustomer.Where(x => x.DateComplete.Date > date.Date).ToListAsync();
            var debitDTO = mapper.Map<List<DebitDTO>>(debit);
            return Ok(debitDTO);
        }

        [HttpGet]
        [Route("GetLessDate")]
        public async Task<ActionResult> GetLessDate(DateTime date, bool statusProcess)
        {
            var debit = new List<DebitCustomer>();
            debit = statusProcess == true ? 
                await dbContext.DebitCustomer.Where(x => x.DateComplete.Date < date.Date && x.Status == statusProcess).ToListAsync() :
                await dbContext.DebitCustomer.Where(x => x.DateComplete.Date < date.Date).ToListAsync();
            var debitDTO = mapper.Map<List<DebitDTO>>(debit);
            return Ok(debitDTO);
        }

    }
}
