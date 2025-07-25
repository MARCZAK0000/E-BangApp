using E_BangApplication.Authentication;
using E_BangDomain.Entities;
using E_BangDomain.MaybePattern;
using E_BangDomain.Repository;
using E_BangDomain.RequestDtos.User;
using E_BangDomain.ResponseDtos.User;
using MyCustomMediator.Interfaces;

namespace E_BangApplication.CQRS.Command.UserHandler
{
    public class UpdatePasswordCommand : UpdatePasswordDto, IRequest<UpdatePasswordResponseDto>
    {

    }

    public class UpdatePasswordCommandHandler : IRequestHandler<UpdatePasswordCommand, UpdatePasswordResponseDto>
    {
        private readonly IUserContext _userContext;
        private readonly IAccountRepository _accountRepository;

        public UpdatePasswordCommandHandler(IUserContext userContext, IAccountRepository accountRepository)
        {
            _userContext = userContext;
            _accountRepository = accountRepository;
        }

        public async Task<UpdatePasswordResponseDto> Handle(UpdatePasswordCommand request, CancellationToken token)
        {
            UpdatePasswordResponseDto response = new(); 
            CurrentUser currentUser = _userContext.GetCurrentUser();
            Maybe<Account> maybeAccount = await _accountRepository.FindAccountByIdAsync(currentUser.AccountID);
            if(!maybeAccount.HasValue || maybeAccount.Value is null)
            {
                return response;
            }

            response.IsSuccess = 
                await _accountRepository
                .ChangePasswordAsync(maybeAccount.Value, request.OldPassword, request.NewPassword)
                && await _accountRepository.LastUdateTimeAsync(maybeAccount.Value.Id, token);
            return response;
        }
    }

}
