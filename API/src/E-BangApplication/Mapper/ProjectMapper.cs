using AutoMapper;
using E_BangDomain.Entities;
using E_BangDomain.EntitiesDto.Role;
using E_BangDomain.EntitiesDto.User;

namespace E_BangApplication.Mapper
{
    public class ProjectMapper:Profile
    {
        public ProjectMapper()
        {
            CreateMap<Users, UserInfoDto>();//From Users to UserInfoDtoS
            CreateMap<Roles, RolesDto>(); //From Roles to RolesDto 
        }
    }
}
