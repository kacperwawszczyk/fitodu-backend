using AutoMapper;
using Fitodu.Model.Entities;
using Fitodu.Service.Models;

namespace Fitodu.Service.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserOutput>()
                .ForMember(x => x.Plan, o => o.MapFrom(x => x.Company.Plan))
                .ForMember(x => x.PlanExpiredOn, o => o.MapFrom(x => x.Company.PlanExpiredOn));
            CreateMap<User, UserListItemOutput>();
            CreateMap<Company, CompanyOutput>();
        }
    }
}
