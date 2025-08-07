using AutoMapper;
using E_BangDomain.Entities;
using E_BangDomain.EntitiesDto.Role;
using E_BangDomain.EntitiesDto.Shop;
using E_BangDomain.EntitiesDto.User;

namespace E_BangApplication.Mapper
{
    public class ProjectMapper : Profile
    {
        public ProjectMapper()
        {
            CreateMap<Users, UserInfoDto>() //From Users to UserInfoDtoS
                .ForMember(dest => dest.City, src => src.MapFrom(pr => pr.Address.City))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Address.Country))
                .ForMember(dest => dest.StreetName, opt => opt.MapFrom(src => src.Address.StreetName))
                .ForMember(dest => dest.StreetNumber, opt => opt.MapFrom(src => src.Address.StreetNumber))
                .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.Address.PostalCode));


            CreateMap<Roles, RolesDto>(); //From Roles to RolesDto 
            CreateMap<Shop, ShopDto>(); //rom Shop to ShopDto
        }
    }
}
