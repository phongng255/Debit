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
        public async Task<ActionResult> CreateDebit([FromBody] CreateDebit debit)
        {
            DebitCustomer debitCustomer = new DebitCustomer();
            debitCustomer.Id = Guid.NewGuid();
            debitCustomer.CustomerId = debit.CustomerId;
            debitCustomer.Items = debit.Items;
            debitCustomer.Money = debit.Money;
            await dbContext.DebitCustomer.AddAsync(debitCustomer);
            await dbContext.SaveChangesAsync();
            //Get Debit add 
            var GetdebitAdd = await dbContext.DebitCustomer.Where(x => x.Status == false && x.Id == debitCustomer.Id).Include(x => x.Customer).FirstOrDefaultAsync();
            var debitDTO = mapper.Map<DebitDTO>(GetdebitAdd);

            return Ok(debitDTO);
        }

        [HttpPost]
        [Route("GetDebitProcessDataTable")]
        public async Task<ActionResult> GetDebitProcessDataTable(DataTableDTO dataTable)
        {
            var debit = await dbContext.DebitCustomer.Where(x => x.Status == false).Include(x => x.Customer).ToListAsync();
            var debitDTO = mapper.Map<List<DebitDTO>>(debit.Skip(dataTable.Start).Take(dataTable.Length));
            var total = debit.Count();
            DTData data = new DTData() { Data = debitDTO, Draw = dataTable.Draw, RecordsTotal = total, RecordsFiltered = total };
            return Ok(data);
        }
        [HttpGet]
        [Route("GetDebitProcess")]
        public async Task<ActionResult> GetDebitProcess()
        {
            var debit = await dbContext.DebitCustomer.Where(x => x.Status == false).Include(x => x.Customer).ToListAsync();
            var debitDTO = mapper.Map<List<DebitDTO>>(debit);
            return Ok(debitDTO);
        }
        [HttpPost]
        [Route("GetDebitSuccess")]
        public async Task<ActionResult> GetDebitSuccess(DataTableDTO dataTable)
        {
            var debit = await dbContext.DebitCustomer.Where(x => x.Status == true).Include(x => x.Customer).ToListAsync();
            var debitDTO = mapper.Map<List<DebitDTO>>(debit.Skip(dataTable.Start).Take(dataTable.Length));
            var total = debit.Count();
            DTData data = new DTData() { Data = debitDTO, Draw = dataTable.Draw, RecordsTotal = total, RecordsFiltered = total };
            return Ok(data);
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
        [HttpPut]
        [Route("EditDebit")]
        public async Task<ActionResult> EditDebit(Guid id, [FromBody] EditDebit editDebit)
        {
            var debit = dbContext.DebitCustomer.Find(id);
            if (debit != null && debit.Status != true)
            {
                debit.Items = editDebit.Items == null ? debit.Items : editDebit.Items;
                debit.CustomerId = editDebit.CustomerId == null || editDebit.CustomerId == Guid.Empty ? debit.CustomerId : editDebit.CustomerId;
                debit.Money = editDebit.Money == 0 ?  debit.Money : editDebit.Money ;
                dbContext.SaveChanges();
            }
            else
            {
                return BadRequest(new { Messenger = "Đã hoàn tất thanh toán không được chỉnh sửa " }); 
            }
            return Ok(new { Messenger = "Cập nhật thông tin thành công " }); ;
        }
        [HttpPost]
        [Route("FindDebitProcess")]
        public async Task<ActionResult> FindDebitProcess(string value)
        {
            var debit = await dbContext.DebitCustomer.Where(x => x.Status == false).Include(x => x.Customer).ToListAsync();
            debit = debit.Where(x => x.Items.ToLower().Contains(value)||
                                x.Customer.Name.ToLower().Contains(value) ||
                                x.Customer.PhoneNumber.ToLower().Contains(value)).ToList();
            var debitDTO = mapper.Map<List<DebitDTO>>(debit);
            return Ok(debitDTO);
        }

    }
}
