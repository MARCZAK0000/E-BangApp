using E_BangDomain.Entities;
using E_BangDomain.HelperRepository;
using E_BangDomain.MaybePattern;
using E_BangDomain.Repository;
using E_BangDomain.RequestDtos.AccountRepositoryDtos;
using E_BangDomain.ResponseDtos.Account;
using MyCustomMediator.Interfaces;

namespace E_BangApplication.CQRS.Query.AccountHandler
{
    public class ForgotPasswordTokenQuery : ForgotPasswordTokenDto, IRequest<ForgotPasswordTokenResponseDto>
    {

    }
    public class ForgetPasswordTokenQueryHandler : IRequestHandler<ForgotPasswordTokenQuery, ForgotPasswordTokenResponseDto>
    {
        private readonly IAccountRepository _accountRepository;

        private readonly IEmailRepository _emailRepository;

        public ForgetPasswordTokenQueryHandler(IAccountRepository accountRepository, IEmailRepository emailRepository)
        {
            _accountRepository = accountRepository;
            _emailRepository = emailRepository;
        }

        public async Task<ForgotPasswordTokenResponseDto> Handle(ForgotPasswordTokenQuery request, CancellationToken token)
        {
            ForgotPasswordTokenResponseDto response = new();
            Maybe<Account> maybe = await _accountRepository.FindAccountByEmailAsync(request.Email, token);
            if (!maybe.HasValue || maybe.Value is null)
            {
                return response;
            }
            string forgetToken = await _accountRepository.GenerateForgetPasswordTokenAsync(maybe.Value);
            await _emailRepository.SendForgetPasswordTokenEmailAsync(maybe.Value.Email!, forgetToken, token);
            response.IsSuccess = true;
            return response;
        }
    }
}
