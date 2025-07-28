using E_BangDomain.Entities;
using E_BangDomain.MaybePattern;
using E_BangDomain.Repository;
using E_BangDomain.RequestDtos.AccountRepositoryDtos;
using E_BangDomain.ResponseDtos.Account;
using MyCustomMediator.Interfaces;

namespace E_BangApplication.CQRS.Command.AccountHandler
{
    public class ConfirmEmailCommand : ConfirmEmailDto, IRequest<ConfirmEmailResponseDto>
    {

    }
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, ConfirmEmailResponseDto>
    {
        private readonly IAccountRepository _accountRepository;

        public ConfirmEmailCommandHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<ConfirmEmailResponseDto> Handle(ConfirmEmailCommand request, CancellationToken token)
        {
            ConfirmEmailResponseDto response = new();
            Maybe<Account> account = await _accountRepository.FindAccountByEmailAsync(request.Email, token);
            if (!account.HasValue || account.Value is null)
            {
                return response;
            }
            response.IsSuccess = await _accountRepository.ConfirmEmailAsync(account.Value, request.Token)
                   && await _accountRepository.LastUdateTimeAsync(account.Value.Id, token);
            return response;
        }
    }
}
