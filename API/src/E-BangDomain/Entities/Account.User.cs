using System.ComponentModel.DataAnnotations.Schema;

namespace E_BangDomain.Entities
{
    [Table(name: "User", Schema = "Account")]
    public partial class Users
    {
        public string UserID { get; set; }  
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }   
        public string PhoneNumber { get; set; }
        public string RoleID { get; set; }  
        public DateTime LastUpdateTime { get; set; } = DateTime.UtcNow;
    }

    public partial class Users
    {
        public Account Account { get; set; }
        public UserAddress Address { get; set; }
        public Roles Role { get; set; }
    }
}
