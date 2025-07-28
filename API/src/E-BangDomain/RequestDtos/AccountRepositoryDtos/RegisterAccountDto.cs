namespace E_BangDomain.RequestDtos.AccountRepositoryDtos
{
    public class RegisterAccountDto : CredentialsAccountDto
    {

        public bool TwoFactorEnable { get; set; }
    }
}
