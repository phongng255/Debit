using AutoMapper;
using Debit.Models;
using System.Globalization;

namespace Debit.DTOs
{
    public class MyProfile : Profile
    {
        public MyProfile()
        {

            CreateMap<Customer, CustomerDTO>();

            CreateMap<DebitCustomer, DebitDTO>(); 
                      
            CreateMap<User, UserDTO>();

            CreateMap<Customer, CustomerDeBitDTO>();

            CreateMap<Accumulate, AccumulateDTO>()
                .ForMember(x => x.CreatedAt,
                 opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(x => x.Money,
                 opt => opt.MapFrom(src => src.Money))
                .ForMember(x => x.Item,
                 opt => opt.MapFrom(src => src.Debit.Items))
                .ForMember(x => x.Name,
                 opt => opt.MapFrom(src => src.Debit.Customer.Name));
        }
    }
}
