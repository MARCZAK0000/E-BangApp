using E_BangDomain.Entities;
using E_BangDomain.MaybePattern;
using E_BangDomain.RequestDtos.AccountRepositoryDtos;
using E_BangDomain.ResultsPattern;
using Microsoft.AspNetCore.Identity;

namespace E_BangDomain.Repository
{
    public interface IAccountRepository
    {
        /// <summary>
        /// Creates a new account with the specified password.
        /// </summary>
        /// <param name="account">The account entity to create.</param>
        /// <param name="password">The password for the new account.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="IdentityResult"/> of the creation process.</returns>
        Task<IdentityResult> RegisterAccountAsync(Account account, string password);

        /// <summary>
        /// Validates the user's login credentials (username and password only, no two-factor code).
        /// </summary>
        /// <param name="user">The account to validate.</param>
        /// <param name="login">The login details provided by the user.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="Result"/> indicating success or failure.</returns>
        Task<Result> ValidateLoginCredentialsAsync(Account user, LoginAccountDto login);

        /// <summary>
        /// Validates a user's login credentials using a two-way factory code.
        /// </summary>
        /// <remarks>
        /// This method performs validation using a two-way factory code mechanism.
        /// Ensure that the <paramref name="login"/> object contains all required fields for validation.
        /// </remarks>
        /// <param name="user">The account object representing the user attempting to log in.</param>
        /// <param name="login">The login details provided by the user, including the two-way factory code.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="Result"/> indicating success or failure.</returns>
        Task<Result> ValidateLoginWithTwoWayFactoryCodeAsync(Account user, LoginAccountDto login);

        /// <summary>
        /// Generates a confirmation email token for the specified user.
        /// </summary>
        /// <param name="user">The account for which the confirmation email token is generated. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the confirmation email token as a string.</returns>
        Task<string> GenerateConfirmEmailTokenAsync(Account user);

        /// <summary>
        /// Confirms the email address for the specified account using the provided confirmation token.
        /// Returns a <see cref="Result"/> indicating success or failure, with error details if applicable.
        /// </summary>
        /// <param name="account">The account for which the email confirmation is being performed. Cannot be null.</param>
        /// <param name="confirmToken">The confirmation token to validate the email address.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="Result"/> indicating the outcome of the confirmation.</returns>
        Task<Result> ConfirmEmailAsync(Account account, string confirmToken);

        /// <summary>
        /// Finds a user account by email.
        /// </summary>
        /// <param name="email">The email address to search for.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="Maybe{Account}"/> with account info if found.</returns>
        Task<Maybe<Account>> FindAccountByEmailAsync(string email, CancellationToken token);

        /// <summary>
        /// Finds a user account by account ID.
        /// </summary>
        /// <param name="accountId">The account ID to search for.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="Maybe{Account}"/> with account info if found.</returns>
        Task<Maybe<Account>> FindAccountByIdAsync(string accountId);

        /// <summary>
        /// Generates a token for resetting the password of the specified account.
        /// </summary>
        /// <param name="account">The account for which to generate the reset token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the password reset token as a string.</returns>
        Task<string> GenerateForgetPasswordTokenAsync(Account account);

        /// <summary>
        /// Resets the password for the specified account using the provided new password and token.
        /// </summary>
        /// <param name="account">The account to reset the password for.</param>
        /// <param name="newPassword">The new password to set.</param>
        /// <param name="token">The password reset token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is true if the password was reset successfully; otherwise, false.</returns>
        Task<bool> SetNewPasswordAsync(Account account, string newPassword, string token);

        /// <summary>
        /// Changes the password for the specified account.
        /// </summary>
        /// <param name="account">The account to change the password for.</param>
        /// <param name="oldPassword">The current password.</param>
        /// <param name="newPassword">The new password to set.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is true if the password was changed successfully; otherwise, false.</returns>
        Task<bool> ChangePasswordAsync(Account account, string oldPassword, string newPassword);

        /// <summary>
        /// Updates the last update time for the specified account.
        /// </summary>
        /// <param name="accountID">The account ID to update.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is true if the update was successful; otherwise, false.</returns>
        Task<bool> LastUdateTimeAsync(string accountID, CancellationToken token);

        /// <summary>
        /// Gets the account ID associated with the specified email address.
        /// </summary>
        /// <param name="email">The email address to search for.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the account ID as a string, or an empty string if not found.</returns>
        Task<string> GetAccountIdFromEmail(string email, CancellationToken token);
    }
}
