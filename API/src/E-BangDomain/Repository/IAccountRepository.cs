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
        /// Find User Account with email
        /// </summary>
        /// <param name="email"></param>
        /// <param name="token"></param>
        /// <returns>/><see cref="Account>"/> Account Info</returns>
        Task<Maybe<Account>> FindAccountByEmailAsync(string email, CancellationToken token);
        Task<bool> ValidateLoginWithTwoWayFactoryCodeAsync(Account user, LoginAccountDto login);
    }
}
