using E_BangApplication.Authentication;
using E_BangDomain.Repository;
using E_BangDomain.ResponseDtos.Account;
using MyCustomMediator.Interfaces;

namespace E_BangApplication.CQRS.Command.AccountHandler
{
    public class RefreshTokenCommand : IRequest<RefreshTokenResponseDto>
    {
    }
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResponseDto>
    {
        private readonly IUserContext _userContext;

        private readonly ITokenRepository _tokenRepository;

        public RefreshTokenCommandHandler(IUserContext userContext, 
            ITokenRepository tokenRepository)
        {
            _userContext = userContext;
            _tokenRepository = tokenRepository;
        }

        public async Task<RefreshTokenResponseDto> Handle(RefreshTokenCommand request, CancellationToken token)
        {
            RefreshTokenResponseDto response = new();
            CurrentUser currentUser = _userContext.GetCurrentUser();
            string refreshToken = _userContext.GetRefreshToken();
            string refreshTokenDb = await _tokenRepository.GetRefreshTokenAsync(currentUser.AccountID, token);
            if(!refreshToken.Equals(refreshTokenDb, StringComparison.CurrentCultureIgnoreCase))
            {
                return response;
            }
            string newRefreshToken = _tokenRepository.GenerateRefreshToken();
            bool isSaved = await _tokenRepository.SaveRefreshTokenAsync(currentUser.AccountID, newRefreshToken, token);
            response.IsSuccess = true;
            return response;
        }
    }

}
