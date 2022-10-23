using AutoMapper;
using Debit.DTOs;
using Debit.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

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
            DebitCustomer GetDebitOnchange;
            DebitDTO debitDTO;
            if (debit.Money >= priceDebit)
            {
                Accumulate accumulate = new Accumulate();
                accumulate.Id = Guid.NewGuid();
                accumulate.DebitId = debitId;
                accumulate.Money = money;
                await dbContext.Accumulates.AddAsync(accumulate);
                if (priceDebit + money >= debit.Money)
                {
                    debit.ProcessMoney = (decimal)(priceDebit + money);
                    debit.Status = true;
                    debit.DateComplete = DateTime.UtcNow;
                    GetDebitOnchange = await dbContext.DebitCustomer.Where(x => x.Id == debit.Id).Include(x => x.Customer).FirstOrDefaultAsync();
                    debitDTO = mapper.Map<DebitDTO>(GetDebitOnchange);
                    dbContext.SaveChanges();
                    return Ok(debitDTO);
                }
            }
            debit.ProcessMoney = (decimal)(priceDebit + money);
            dbContext.SaveChanges();
            GetDebitOnchange = await dbContext.DebitCustomer.Where(x => x.Id == debit.Id).Include(x => x.Customer).FirstOrDefaultAsync();
            debitDTO = mapper.Map<DebitDTO>(GetDebitOnchange);
            return Ok(debitDTO);
        }

        [HttpPost]
        [Route("GetDateNow")]
        public async Task<ActionResult> GetDateNow(DataTableDTO dataTable)
        {
            var column = dataTable.Order.First().Column == 0 ? "Item" :
                          dataTable.Order.First().Column == 1 ? "Money" :
                          dataTable.Order.First().Column == 2 ? "Name"
                          : "CreatedAt";
            var sort = dataTable.Order.First().Dir;
            if (dataTable.Start < 0) dataTable.Start = 0;
            var accumulate = await dbContext.Accumulates
                    .Where(x => x.CreatedAt.Date == DateTime.Now.Date)
                    .Include(x => x.Debit.Customer).ToListAsync();
            if (dataTable.Search.Value != "")
            {
                accumulate = accumulate.Where
                    (x =>
                       x.Debit.Customer.Name.ToLower().Contains(dataTable.Search.Value.ToLower())
                       || x.Debit.Items.ToLower().Contains(dataTable.Search.Value.ToLower())
                    ).ToList();
            }
            accumulate = Orderby(accumulate,column,sort);
            var accumulateDTO = mapper.Map<List<AccumulateDTO>>(accumulate.Skip(dataTable.Start).Take(dataTable.Length));
            var total = await dbContext.Accumulates.CountAsync(x => x.CreatedAt.Date == DateTime.Now.Date);
            DTData data = new DTData() { Data = accumulateDTO , Draw = dataTable.Draw , RecordsTotal = total , RecordsFiltered = total};
            return Ok(data);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public List<Accumulate> Orderby(List<Accumulate> accumulate, string column, string sort)
        {
            
            if (sort == "desc")
            {
                accumulate = column == "Item"? accumulate.OrderByDescending(x => x.Debit.Items).ToList() :
                             column == "Money" ? accumulate.OrderByDescending(x => x.Money).ToList() :
                             column == "Name" ? accumulate.OrderByDescending(x => x.Debit.Customer.Name).ToList(): 
                             accumulate.OrderByDescending(x => x.CreatedAt).ToList();
            }
            else
            {
                accumulate = column == "Item" ? accumulate.OrderBy(x => x.Debit.Items).ToList() :
                             column == "Money" ? accumulate.OrderBy(x => x.Money).ToList() :
                             column == "Name" ? accumulate.OrderBy(x => x.Debit.Customer.Name).ToList() :
                             accumulate.OrderBy(x => x.CreatedAt).ToList();
            }
            return accumulate;
        }
        //[HttpGet]
        //[Route("GetLargerDate")]
        //public async Task<ActionResult> GetLargerDate(DateTime date)
        //{
        //    var accumulate = await dbContext.Accumulates.Where(x => x.CreatedAt.Date > date.Date).ToListAsync();
        //    var accumulateDTO = mapper.Map<List<AccumulateDTO>>(accumulate);
        //    return Ok(GetListAccumulate(accumulateDTO));
        //}

        //[HttpGet]
        //[Route("GetLessDate")]
        //public async Task<ActionResult> GetLessDate(DateTime date)
        //{
        //    var accumulate = await dbContext.Accumulates.Where(x => x.CreatedAt.Date < date.Date).ToListAsync();
        //    var accumulateDTO = mapper.Map<List<AccumulateDTO>>(accumulate);
        //    return Ok(GetListAccumulate(accumulateDTO));
        //}

        //[HttpGet]
        //[Route("GetAll")]
        //public async Task<ActionResult> GetAll()
        //{
        //    var accumulate = await dbContext.Accumulates.Include(x => x.Debit).Include(x => x.Debit.Customer).ToListAsync();
        //    var accumulateDTO = mapper.Map<List<AccumulateDTO>>(accumulate);
        //    return Ok(GetListAccumulate(accumulateDTO));
        //}
        //private List<AccumulateDetailDTO> GetListAccumulate(List<AccumulateDTO> accumulateDTO)
        //{
        //    List<AccumulateDetailDTO> listAccumulateDetails = new List<AccumulateDetailDTO>();
        //    foreach (var item in accumulateDTO)
        //    {
        //        AccumulateDetailDTO temp = new AccumulateDetailDTO();
        //        temp.Id = item.Id;
        //        temp.DebitId = item.DebitId;
        //        temp.Money = item.Money;
        //        temp.CreatedAt = item.CreatedAt;
        //        temp.Items = item.Debit.Items;
        //        temp.CustomerName = item.Debit.Customer.Name;
        //        listAccumulateDetails.Add(temp);
        //    }
        //    return listAccumulateDetails;
        //}


    }
}
