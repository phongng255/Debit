using AutoMapper;
using Debit.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Debit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccumulateController : ControllerBase
    {
        private readonly AppDbContext dbContext;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;

        public AccumulateController
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
        [Route("CreateAccumulate")]
        public async Task<ActionResult> CreateAccumulate(Guid debitId, decimal money)
        {
            var debit = dbContext.DebitCustomer.Find(debitId);
            decimal? priceDebit = dbContext.Accumulates.Where(x => x.DebitId == debitId).ToList().Sum(x => x.Money);
            if (debit.Money <= priceDebit)
            {
                return Ok(new { message = "Thanh toán hoàn tất" });
            }
            else
            {
                Accumulate accumulate = new Accumulate();
                accumulate.Id = Guid.NewGuid();
                accumulate.DebitId = debitId;
                accumulate.Money = money;
                await dbContext.Accumulates.AddAsync(accumulate);
                if (priceDebit + money >= debit.Money)
                {
                    debit.Status = true;
                    debit.DateComplete = DateTime.UtcNow;
                    dbContext.SaveChanges();
                    return Ok(new { message = "Thanh toán hoàn tất" });
                }
            }
            debit.ProcessMoney = (decimal)(priceDebit + money);
            dbContext.SaveChanges();
            return Ok(new { message = "Cộng Tiền Thành Công" });
        }

        [HttpGet]
        [Route("GetDateNow")]
        public async Task<ActionResult> GetDateNow(DateTime date)
        {
            var accumulate = await dbContext.Accumulates.Where(x => x.CreatedAt.Date == date.Date).Include(x=>x.Debit).Include(x=>x.Debit.Customer).ToListAsync();
            var accumulateDTO = mapper.Map<List<AccumulateDTO>>(accumulate);
            return Ok(accumulateDTO);
        }
        [HttpGet]
        [Route("GetLargerDate")]
        public async Task<ActionResult> GetLargerDate(DateTime date)
        {
            var accumulate = await dbContext.Accumulates.Where(x => x.CreatedAt.Date > date.Date).ToListAsync();
            var accumulateDTO = mapper.Map<List<AccumulateDTO>>(accumulate);
            return Ok(accumulateDTO);
        }

        [HttpGet]
        [Route("GetLessDate")]
        public async Task<ActionResult> GetLessDate(DateTime date)
        {
            var accumulate = await dbContext.Accumulates.Where(x => x.CreatedAt.Date < date.Date).ToListAsync();
            var accumulateDTO = mapper.Map<List<AccumulateDTO>>(accumulate);
            return Ok(accumulateDTO);
        }
    }
}
