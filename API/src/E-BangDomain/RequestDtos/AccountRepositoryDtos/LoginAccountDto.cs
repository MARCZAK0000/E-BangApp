namespace E_BangDomain.RequestDtos.AccountRepositoryDtos
{
    public class LoginAccountDto : CredentialsAccountDto
    {
      
       public string? TwoFactorCode { get; set; }
    }
}
