namespace E_BangDomain.RequestDtos.AccountRepositoryDtos
{
    public class LoginAccountDto : CredentialsAccountDto
    {
       public string ConfirmPassword { get; set; }
       public string? TwoFactorCode { get; set; }
    }
}
