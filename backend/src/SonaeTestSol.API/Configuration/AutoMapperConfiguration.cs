using SonaeTestSol.Domain.Entities;
using AutoMapper;
using SonaeTestSol.Domain.DTO;

namespace SonaeTestSol.API.Configuration
{
    public class AutoMapperConfiguration: Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<Order, OrderDTO>().ReverseMap();
        }
    }
}
