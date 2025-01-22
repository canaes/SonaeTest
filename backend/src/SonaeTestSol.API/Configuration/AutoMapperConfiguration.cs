using SonaeTestSol.Domain.Entities;
using AutoMapper;
using SonaeTestSol.Domain.DTO;
using Microsoft.OpenApi.Extensions;

namespace SonaeTestSol.API.Configuration
{
    public class AutoMapperConfiguration: Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<Order, OrderDTO>()
                .ForMember(dest => dest.StatusStr, opt => opt.MapFrom(src => src.Status.GetDisplayName()));
        }
    }
}
