using E_BangDomain.Entities;
using E_BangDomain.MaybePattern;
using E_BangDomain.RequestDtos.AccountRepositoryDtos;

namespace E_BangDomain.Repository
{
    public interface IAccountRepository
    {
        /// <summary>
        /// Create new Account
        /// </summary>
        /// <param name="registerAccountDto"></param>
        /// <param name="token"></param>
        /// <returns><see cref="Account"/> Account Details</returns>
        Task<Account> RegisterAccountAsync(RegisterAccountDto registerAccountDto, CancellationToken token);

        /// <summary>
        /// Check UserName and Password without TwoWayToken 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="login"></param>
        /// <returns><see cref="bool"/> True of False</returns>s
        Task<bool> ValidateLoginCredentialsAsync(Account user, LoginAccountDto login);

       
        /// <summary>
        /// Validates a user's login credentials using a two-way factory code.
        /// </summary>
        /// <remarks>This method performs validation using a two-way factory code mechanism. Ensure that
        /// the  <paramref name="login"/> object contains all required fields for validation.</remarks>
        /// <param name="user">The account object representing the user attempting to log in.</param>
        /// <param name="login">The login details provided by the user, including the factory code.</param>
        /// <returns><see langword="true"/> if the login is successfully validated; otherwise, <see langword="false"/>.</returns>
        Task<bool> ValidateLoginWithTwoWayFactoryCodeAsync(Account user, LoginAccountDto login);

        /// <summary>
        /// Generates a confirmation email token for the specified user.
        /// </summary>
        /// <param name="user">The account for which the confirmation email token is generated. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the confirmation email token as
        /// a string.</returns>
        Task<string> GenerateConfirmEmailTokenAsync(Account user);

        /// <summary>
        /// Confirm Email for the specifed user
        /// </summary>
        /// <param name="account">The account for which the confirmation is accepted. Cannot be null</param>
        /// <param name="confirmToken">Token</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the confirmation email result</returns>
        Task<bool> ConfirmEmailAsync(Account account, string confirmToken);
        /// <summary>
        /// Find User Account with email
        /// </summary>
        /// <param name="email"></param>
        /// <param name="token"></param>
        /// <returns>/><see cref="Account>"/> Account Info</returns>
        Task<Maybe<Account>> FindAccountByEmailAsync(string email, CancellationToken token);

        /// <summary>
        /// Find User Account with Id
        /// </summary>
        /// <param name="accountId">Account ID</param>
        /// <returns><see cref="Account>"/> Account Info</returns>
        Task<Maybe<Account>> FindAccountByIdAsync(string accountId);
        /// <summary>
        /// Generates a token for resetting the password of the specified account.
        /// </summary>
        /// <param name="account"></param>
        /// <returns>string</returns>
        Task<string> GenerateForgetPasswordTokenAsync(Account account);
        /// <summary>
        /// Resets the password for the specified account using the provided new password and token.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="newPassword"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<bool> SetNewPasswordAsync(Account account, string newPassword, string token);
    }
}
