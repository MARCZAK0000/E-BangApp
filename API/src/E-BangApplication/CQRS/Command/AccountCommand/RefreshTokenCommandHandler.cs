using E_BangApplication.Authentication;
using E_BangDomain.Repository;
using E_BangDomain.ResponseDtos.Account;
using MyCustomMediator.Interfaces;

namespace E_BangApplication.CQRS.Command.AccountCommand
{
    public class RefreshTokenCommand : IRequest<RefreshTokenResponseDto>
    {
    }
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResponseDto>
    {
        private readonly IUserContext _userContext;

        private readonly ITokenRepository _tokenRepository;
        private readonly IAccountRepository _accountRepository;

        public RefreshTokenCommandHandler(IUserContext userContext, ITokenRepository tokenRepository, IAccountRepository accountRepository)
        {
            _userContext = userContext;
            _tokenRepository = tokenRepository;
            _accountRepository = accountRepository;
        }

        public async Task<RefreshTokenResponseDto> Handle(RefreshTokenCommand request, CancellationToken token)
        {
            RefreshTokenResponseDto response = new();
            CurrentUser currentUser = _userContext.GetCurrentUser();
            string refreshToken = _userContext.GetRefreshToken();


        }
    }

}
