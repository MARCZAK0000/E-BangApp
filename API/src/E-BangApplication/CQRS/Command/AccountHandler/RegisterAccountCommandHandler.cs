using E_BangApplication.Exceptions;
using E_BangDomain.Entities;
using E_BangDomain.HelperRepository;
using E_BangDomain.Repository;
using E_BangDomain.RequestDtos.AccountRepositoryDtos;
using E_BangDomain.ResponseDtos.Account;
using MyCustomMediator.Interfaces;

namespace E_BangApplication.CQRS.Command.AccountHandler
{
    public class RegisterAccountCommand : RegisterAccountDto, IRequest<RegisterAccountResponseDto>
    {

    }
    public class RegisterAccountCommandHandler : IRequestHandler<RegisterAccountCommand, RegisterAccountResponseDto>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IEmailRepository _emailRepository;
        private readonly IRoleRepository _roleRepository;
        public RegisterAccountCommandHandler(IAccountRepository accountRepository,
            IEmailRepository emailRepository, IRoleRepository roleRepository)
        {
            _accountRepository = accountRepository;
            _emailRepository = emailRepository;
            _roleRepository = roleRepository;
        }

        public async Task<RegisterAccountResponseDto> Handle(RegisterAccountCommand request, CancellationToken cancellationToken)
        {
            RegisterAccountResponseDto response = new();
            Account account = new();
            account.Email = request.Email;
            account.UserName = request.Email;
            account.NormalizedEmail = request.Email.ToUpperInvariant();
            account.NormalizedUserName = request.Email.ToUpperInvariant();
            account.TwoFactorEnabled = request.TwoFactorEnable;

            //Add account
            bool isAccount = await _accountRepository.RegisterAccountAsync(account, request.Password);
            if (!isAccount)
            {
                return response;
            }

            bool isRoleAssigned = await _roleRepository.AddToRoleLevelZeroAsync(account.Id, cancellationToken);
            if (!isRoleAssigned)
            {
                throw new InternalServerErrorException("Failed to assign role to the account.");
            }
            string confirmEmailToken = await _accountRepository.GenerateConfirmEmailTokenAsync(account);
            await _emailRepository.SendRegistrationConfirmAccountEmailAsync(confirmEmailToken, account.Email, cancellationToken);
            response.IsSuccess = true;
            return response;
        }
    }

}

