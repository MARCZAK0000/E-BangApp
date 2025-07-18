using E_BangDomain.Entities;
using E_BangDomain.HelperRepository;
using E_BangDomain.IQueueService;
using E_BangDomain.MaybePattern;
using E_BangDomain.Repository;
using E_BangDomain.RequestDtos.AccountRepositoryDtos;
using E_BangDomain.RequestDtos.TokenRepostitoryDtos;
using E_BangDomain.ResponseDtos.Account;
using E_BangDomain.Settings;
using Microsoft.AspNetCore.Http;
using MyCustomMediator.Interfaces;
using System.Security.Claims;

namespace E_BangApplication.CQRS.Command
{
    public class VerifyCredentialsCommand : LoginAccountDto, IRequest<TwoWayTokenResponseDto>
    {

    }
    public class ValidateCredentialsCommandHandler : IRequestHandler<VerifyCredentialsCommand, TwoWayTokenResponseDto>
    {
        private readonly IAccountRepository _accountRepository;

        private readonly IRoleRepository _roleRepository;

        private readonly ITokenRepository _tokenRepository;

        private readonly HttpOnlyTokenOptions _httpOnlyTokenOptions;

        private readonly IMessageSenderHandlerQueue _messageSenderHandlerQueue;

        private readonly IEmailRepository _emailRepository;

        public ValidateCredentialsCommandHandler(IAccountRepository accountRepository,
            IRoleRepository roleRepository,
            ITokenRepository tokenRepository,
            HttpOnlyTokenOptions httpOnlyTokenOptions, IMessageSenderHandlerQueue messageSenderHandlerQueue, IEmailRepository emailRepository)
        {
            _accountRepository = accountRepository;
            _roleRepository = roleRepository;
            _tokenRepository = tokenRepository;
            _httpOnlyTokenOptions = httpOnlyTokenOptions;
            _messageSenderHandlerQueue = messageSenderHandlerQueue;
            _emailRepository = emailRepository;
        }

        public async Task<TwoWayTokenResponseDto> Handle(VerifyCredentialsCommand request, CancellationToken token)
        {
            TwoWayTokenResponseDto response = new()
            {
                IsSuccess = false,
                TwoWayToken = string.Empty
            };
            Maybe<Account> maybe = await _accountRepository.FindAccountByEmailAsync(request.Email, token);
            if (!maybe.HasValue || maybe.Value == null)
            {
                return response;
            }
            bool isCredentialsValid = await _accountRepository.ValidateLoginCredentialsAsync(maybe.Value, request);
            if (!isCredentialsValid)
            {
                return response;
            }
            if (maybe.Value.TwoFactorEnabled)
            {
                string twoWayToken = _tokenRepository.GenerateTwoWayFactoryToken();
                bool isSaved = await _tokenRepository.SaveTwoWayFactoryTokenAsync(maybe.Value.Id, twoWayToken, token);
                if (!isSaved)
                {
                    return response;
                }
                response.TwoWayToken = twoWayToken;

                ///EMAIL SENDING LOGIC HERE !!!!!!!!!!!!
                _messageSenderHandlerQueue.QueueBackgroundWorkItem(async (cancellationToken) =>
                {
                    await _emailRepository.SendTwoWayTokenEmailAsync(
                        twoWayToken, maybe.Value.Email!, cancellationToken);
                });

            }
            else
            {
                List<string> roles = await _roleRepository.GetRoleByAccountIdAsync(maybe.Value.Id, token);
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
                            cfg.Expires = _httpOnlyTokenOptions.AccessTokenExpireDate;
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
                            cfg.Expires = _httpOnlyTokenOptions.RefreshTokenExpireDate;
                            cfg.IsEssential = true;
                            cfg.HttpOnly = _httpOnlyTokenOptions.IsHttpOnly;
                            cfg.Secure = false;
                            cfg.SameSite = SameSiteMode.Lax;
                        }
                    }
                    ];
                bool isSaved = _tokenRepository.SaveCookies(saveCookies);
                if (!isSaved)
                {
                    return response;
                }
                response.IsSuccess = true;
            }

            return response;
        }
    }
}
