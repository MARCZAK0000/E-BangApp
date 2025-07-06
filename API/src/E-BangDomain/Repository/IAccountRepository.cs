using E_BangDomain.Entities;
using E_BangDomain.MaybePattern;
using E_BangDomain.RequestDtos.AccountRepositoryDtos;

namespace E_BangDomain.Repository
{
    public interface IAccountRepository
    {
        Task<Account> RegisterAccountAsync(RegisterAccountDto registerAccountDto, CancellationToken token);
        Task<bool> ValidateLoginCredentialsAsync(Account user, LoginAccountDto login);
        Task<Maybe<Account>> FindAccountByEmailAsync(string email, CancellationToken token);
        Task<bool> ValidateLoginWithTwoWayFactoryCodeAsync(Account user, LoginAccountDto login);
        Task<bool> LogoutAsync();
    }
}
