namespace E_BangDomain.RequestDtos.User
{
    public class UpdatePasswordDto
    {
       
        public string NewPassword { get; set; } = string.Empty;

        public string ConfirmNewPassword {  get; set; } = string.Empty; 
      
        public string OldPassword { get; set; } = string.Empty;
    }
}
