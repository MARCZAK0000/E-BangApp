using E_BangApplication.Authentication;
using E_BangDomain.Entities;
using E_BangDomain.HelperRepository;
using E_BangDomain.MaybePattern;
using E_BangDomain.Repository;
using E_BangDomain.ResponseDtos.Account;
using Microsoft.AspNetCore.Identity;
using MyCustomMediator.Interfaces;

namespace E_BangApplication.CQRS.Query.AccountHandler
{
    public class ReSendConfirmEmailQuery : IRequest<ReSendConfirmEmailResponseDto>
    {

    }
    public class ReSendConfimEmailQueryHandler : IRequestHandler<ReSendConfirmEmailQuery, ReSendConfirmEmailResponseDto>
    {
        private readonly IUserContext _userContext;
        private readonly IAccountRepository _accountRepository;
        private readonly IEmailRepository _emailRepository;

        public ReSendConfimEmailQueryHandler(IUserContext userContext, IAccountRepository accountRepository, IEmailRepository emailRepository)
        {
            _userContext = userContext;
            _accountRepository = accountRepository;
            _emailRepository = emailRepository;
        }

        public async Task<ReSendConfirmEmailResponseDto> Handle(ReSendConfirmEmailQuery request, CancellationToken token)
        {
            ReSendConfirmEmailResponseDto response = new();
            CurrentUser currentUser = _userContext.GetCurrentUser();
            if (currentUser is null || 
                    string.IsNullOrEmpty(currentUser.EmailAddress) 
                    || string.IsNullOrEmpty(currentUser.AccountID))
            {
                return response;
            }
            Maybe<Account> maybe = await _accountRepository.FindAccountByIdAsync(currentUser.AccountID);
            if (!maybe.HasValue || maybe.Value is null)
            {
                return response;
            }
            string confirmToken = await _accountRepository.GenerateConfirmEmailTokenAsync(maybe.Value);

            ///SEND EMAIL TOKEN 
            await _emailRepository.SendEmailConfirmAccountAsync(confirmToken, currentUser.EmailAddress, token);
            response.IsSuccess = true;
            return response;
        }
    }
}
