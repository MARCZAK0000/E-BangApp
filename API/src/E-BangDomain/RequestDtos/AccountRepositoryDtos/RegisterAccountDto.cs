namespace E_BangDomain.RequestDtos.AccountRepositoryDtos
{
    public class RegisterAccountDto : CredentialsAccountDto
    {
        public string ConfirmPassword { get; set; }

        public bool TwoFactorEnable { get; set; }
    }
}
