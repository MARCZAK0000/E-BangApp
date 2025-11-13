namespace E_BangDomain.HelperRepository
{
    public interface IEmailRepository
    {
        Task SendRegistrationConfirmAccountEmailAsync(string accountId, string token, string email, CancellationToken cancellationToken);
        Task SendEmailConfirmAccountAsync(string accountID, string token, string email, CancellationToken cancellationToken);
        Task SendTwoWayTokenEmailAsync(string accountID, string token, string email, CancellationToken cancellationToken);
        Task SendForgetPasswordTokenEmailAsync(string accountID, string token, string email, CancellationToken cancellationToken);
    }
}
