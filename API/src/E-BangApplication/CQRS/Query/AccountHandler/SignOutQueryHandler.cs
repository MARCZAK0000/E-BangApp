using E_BangDomain.Repository;
using E_BangDomain.ResponseDtos.Account;
using E_BangDomain.Settings;
using MyCustomMediator.Interfaces;

namespace E_BangApplication.CQRS.Query.AccountHandler
{
    public class SignOutQuery : IRequest<SignOutResponseDto>
    {

    }
    public class SignOutQueryHandler : IRequestHandler<SignOutQuery, SignOutResponseDto>
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly HttpOnlyTokenOptions _httpOnlyTokenOptions;

        public SignOutQueryHandler(ITokenRepository tokenRepository, HttpOnlyTokenOptions httpOnlyTokenOptions)
        {
            _tokenRepository = tokenRepository;
            _httpOnlyTokenOptions = httpOnlyTokenOptions;
        }
        public Task<SignOutResponseDto> Handle(SignOutQuery request, CancellationToken cancellationToken)
        {
            SignOutResponseDto response = new();
            List<string> cookies = 
                [
                    _httpOnlyTokenOptions.AccessToken, 
                    _httpOnlyTokenOptions.RefreshToken
                ];

            bool IsRemoved = _tokenRepository.RemoveCookies(cookies);

            if (!IsRemoved)
                return Task.FromResult(response);

            response.IsSuccess = true;
            return Task.FromResult(response);

        }
    }
}
