using E_BangDomain.Entities;
using E_BangDomain.MaybePattern;
using E_BangDomain.Repository;
using E_BangDomain.RequestDtos.AccountRepositoryDtos;
using E_BangDomain.RequestDtos.TokenRepostitoryDtos;
using E_BangDomain.ResponseDtos.Account;
using E_BangDomain.ResultsPattern;
using E_BangDomain.Settings;
using Microsoft.AspNetCore.Http;
using MyCustomMediator.Interfaces;
using System.Security.Claims;

namespace E_BangApplication.CQRS.Command.AccountHandler
{
    public class ValidateCredentialsTwoFactoryTokenCommand : LoginAccountDto, IRequest<SignInResponseDto>
    {

    }
    public class ValidateCredentialsTwoFactoryTokenCommandHandler
        : IRequestHandler<ValidateCredentialsTwoFactoryTokenCommand, SignInResponseDto>
    {
        private readonly IAccountRepository _accountRepository;

        private readonly IRoleRepository _roleRepository;

        private readonly ITokenRepository _tokenRepository;

        private readonly HttpOnlyTokenOptions _httpOnlyTokenOptions;

        public ValidateCredentialsTwoFactoryTokenCommandHandler(IAccountRepository accountRepository,
            IRoleRepository roleRepository,
            ITokenRepository tokenRepository,
            HttpOnlyTokenOptions httpOnlyTokenOptions)
        {
            _accountRepository = accountRepository;
            _roleRepository = roleRepository;
            _tokenRepository = tokenRepository;
            _httpOnlyTokenOptions = httpOnlyTokenOptions;
        }

        public async Task<SignInResponseDto> Handle(ValidateCredentialsTwoFactoryTokenCommand request, CancellationToken token)
        {
            SignInResponseDto response = new()
            {
                IsSuccess = false,
            };

            Maybe<Account> maybe = await _accountRepository.FindAccountByEmailAsync(request.Email, token);
            if (!maybe.HasValue || maybe.Value == null)
                return response;

            Result isCredentialsValid = await _accountRepository.ValidateLoginWithTwoWayFactoryCodeAsync(maybe.Value, request);
            if (!isCredentialsValid.IsSuccess)
            {
                response.IsSuccess = isCredentialsValid.IsSuccess;
                response.Message += string.Join(", ", response.Message, isCredentialsValid.ErrorMessage.ToString());
                return response;
            }


            List<string> roles = await _roleRepository.GetRoleNamesByAccountIdAsync(maybe.Value.Id, token);
            List<Claim> claims = _tokenRepository.GenerateClaimsList(maybe.Value, roles);
            string accessToken = _tokenRepository.GenerateToken(claims);
            string refreshToken = _tokenRepository.GenerateRefreshToken();
            List<SaveCookiesDtos> saveCookies =
                [
                    new SaveCookiesDtos
                    {
                        Token = accessToken,
                        Key = _httpOnlyTokenOptions.AccessToken,
                        CookiesOptions = cfg =>{
                            cfg.Expires = new DateTimeOffset(DateTime.Now.AddMinutes(_httpOnlyTokenOptions.AccessTokenExpireDate));
                            cfg.IsEssential = true;
                            cfg.HttpOnly = _httpOnlyTokenOptions.IsHttpOnly;
                            cfg.Secure = false;
                            cfg.SameSite = SameSiteMode.Lax;
                        }
                    },
                    new SaveCookiesDtos
                    {
                        Token = refreshToken,
                        Key = _httpOnlyTokenOptions.RefreshToken,
                        CookiesOptions = cfg =>{
                            cfg.Expires = new DateTimeOffset(DateTime.Now.AddMinutes(_httpOnlyTokenOptions.RefreshTokenExpireDate));
                            cfg.IsEssential = true;
                            cfg.HttpOnly = _httpOnlyTokenOptions.IsHttpOnly;
                            cfg.Secure = false;
                            cfg.SameSite = SameSiteMode.Lax;
                        }
                    }
                ];

            bool isRefreshTokenSaved = await _tokenRepository.SaveRefreshTokenAsync(maybe.Value.Id, refreshToken, token);
            if (!isRefreshTokenSaved)
                return response;

            bool isSaved = _tokenRepository.SaveCookies(saveCookies);
            if (!isSaved)
                return response;

            response.IsSuccess = true;
            return response;

        }
    }
}
