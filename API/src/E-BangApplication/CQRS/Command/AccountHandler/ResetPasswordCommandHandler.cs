using E_BangDomain.Entities;
using E_BangDomain.MaybePattern;
using E_BangDomain.Repository;
using E_BangDomain.RequestDtos.AccountRepositoryDtos;
using E_BangDomain.ResponseDtos.Account;
using MyCustomMediator.Interfaces;

namespace E_BangApplication.CQRS.Command.AccountHandler
{
    public class ResetPasswordCommand : ResetPasswordDto, IRequest<ResetPasswordResponseDto>
    {
    }
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ResetPasswordResponseDto>
    {
        private readonly IAccountRepository _accountRepository;

        public ResetPasswordCommandHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<ResetPasswordResponseDto> Handle(ResetPasswordCommand request, CancellationToken token)
        {
            ResetPasswordResponseDto response = new();
            Maybe<Account> account = await _accountRepository.FindAccountByEmailAsync(request.Email, token);
            if (!account.HasValue || account.Value is null)
            {
                return response;
            }
            response.IsSuccess = await _accountRepository.SetNewPasswordAsync(account.Value, request.Password, request.Token)
                && await _accountRepository.LastUdateTimeAsync(account.Value.Id, token);
            return response;
        }
    }
}
