using E_BangDomain.EntitiesDto.Role;

namespace E_BangDomain.EntitiesDto.User
{
    public class UserInfoDto
    {
        public string Email { get; set; }
        public string FirstName { get; set; }   
        public string? SecondName { get; set; } 
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }

        public List<RolesDto> Roles { get; set; }
    }
}
