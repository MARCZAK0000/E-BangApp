namespace E_BangDomain.HelperRepository
{
    public interface IEmailRepository
    {
        Task SendRegistrationConfirmAccountEmailAsync(string token, string email, CancellationToken cancellationToken);
        Task SendEmailConfirmAccountAsync(string token, string email, CancellationToken cancellationToken);
        Task SendTwoWayTokenEmailAsync(string token, string email, CancellationToken cancellationToken);
        Task SendForgetPasswordTokenEmailAsync(string token, string email, CancellationToken cancellationToken);
    }
}
