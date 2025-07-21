using E_BangApplication.Exceptions;
using E_BangDomain.Entities;
using E_BangDomain.HelperRepository;
using E_BangDomain.IQueueService;
using E_BangDomain.Repository;
using E_BangDomain.RequestDtos.AccountRepositoryDtos;
using E_BangDomain.ResponseDtos.Account;
using MyCustomMediator.Interfaces;

namespace E_BangApplication.CQRS.Command.AccountCommand
{
    public class RegisterAccountCommand : RegisterAccountDto, IRequest<RegisterAccountResponseDto>
    {

    }
    public class RegisterAccountCommandHandler : IRequestHandler<RegisterAccountCommand, RegisterAccountResponseDto>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IEmailRepository _emailRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMessageSenderHandlerQueue _messageSenderHandlerQueue;
        public RegisterAccountCommandHandler(IAccountRepository accountRepository, 
            IEmailRepository emailRepository, IRoleRepository roleRepository,
            IMessageSenderHandlerQueue messageSenderHandlerQueue)
        {
            _accountRepository = accountRepository;
            _emailRepository = emailRepository;
            _roleRepository = roleRepository;
            _messageSenderHandlerQueue = messageSenderHandlerQueue;
        }

        public async Task<RegisterAccountResponseDto> Handle(RegisterAccountCommand request, CancellationToken cancellationToken)
        {
            RegisterAccountResponseDto response = new();
            Account? account = await _accountRepository.RegisterAccountAsync(request, cancellationToken);
            if(account is null)
            {
                return response;
            }
            Roles? role = await _roleRepository.GetRolesAsync(cancellationToken)
                .ContinueWith(x => x.Result.OrderBy(y => y.RoleLevel).FirstOrDefault());

            if (role is null)
            {
                throw new InternalServerErrorException("No roles available to assign to the account.");
            }
            bool isRoleAssigned = await _roleRepository.AddToRoleLevelZeroAsync(account.Id, cancellationToken);
            if (!isRoleAssigned)
            {
                throw new InternalServerErrorException("Failed to assign role to the account.");
            }
            string confirmEmailToken = await _accountRepository.GenerateConfirmEmailToken(account);
            _messageSenderHandlerQueue.QueueBackgroundWorkItem(async token =>
            {
                await _emailRepository.SendRegistrationConfirmAccountEmailAsync(confirmEmailToken, account.Email!, token);
            });
            response.IsSuccess = true;
            return response;
        }
    }

}

