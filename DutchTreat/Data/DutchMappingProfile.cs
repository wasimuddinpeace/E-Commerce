using AutoMapper;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public class DutchMappingProfile : Profile {

        public DutchMappingProfile()
        {                                                             //Map From is the source i.e Order
            CreateMap<Order, OrderViewModel>().ForMember(o => o.OrderId, ex => ex.MapFrom(o => o.Id)).ReverseMap();

            CreateMap<OrderItem, OrderItemViewModel>().ReverseMap();
        }
    
    }
    
}
