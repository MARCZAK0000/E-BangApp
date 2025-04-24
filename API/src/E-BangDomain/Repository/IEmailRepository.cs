namespace E_BangDomain.Repository
{
    public interface IEmailRepository
    {
        Task SendRegistrationConfirmAccountEmailAsync(string token, string email, CancellationToken cancellationToken);
        Task SendEmailConfirmAccountAsync(string token, string email, CancellationToken cancellationToken);
    }
}
