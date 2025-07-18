using E_BangDomain.Entities;
using E_BangDomain.HelperRepository;
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

        public RegisterAccountCommandHandler(IAccountRepository accountRepository, IEmailRepository emailRepository, IRoleRepository roleRepository)
        {
            _accountRepository = accountRepository;
            _emailRepository = emailRepository;
            _roleRepository = roleRepository;
        }

        public async Task<RegisterAccountResponseDto> Handle(RegisterAccountCommand request, CancellationToken cancellationToken)
        {
            RegisterAccountResponseDto response = new();
            Account? account = await _accountRepository.RegisterAccountAsync(request, cancellationToken);
            if(account is null)
            {
                response.IsSuccess = false;
                return response;
            }

            response.IsSuccess = true;
        }
    }

}

