using AutoMapper;
using Debit.Models;

namespace Debit.DTOs
{
    public class MyProfile : Profile
    {
        public MyProfile()
        {
            CreateMap<Customer, CustomerDTO>();
            //.ForMember(s => s.Debits, m => m.Ignore());

            CreateMap<DebitCustomer, DebitDTO>(); 

            CreateMap<Accumulate, AccumulateDTO>();
        }
    }
}
