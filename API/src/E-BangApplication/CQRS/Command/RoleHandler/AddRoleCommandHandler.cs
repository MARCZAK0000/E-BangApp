using CustomLogger.Abstraction;
using E_BangDomain.Entities;
using E_BangDomain.Repository;
using E_BangDomain.RequestDtos.Role;
using E_BangDomain.ResponseDtos.Role;
using Microsoft.Extensions.Logging;
using MyCustomMediator.Interfaces;

namespace E_BangApplication.CQRS.Command.RoleHandler
{
    public class AddRoleCommand : AddRoleDto, IRequest<AddRoleResponseDto>
    {
    }

    public class AddRoleCommandHandler : IRequestHandler<AddRoleCommand, AddRoleResponseDto>
    {
        private readonly IRoleRepository _roleRepository;

        private readonly ICustomLogger<AddRoleCommandHandler> _logger;

        public AddRoleCommandHandler(IRoleRepository roleRepository, ICustomLogger<AddRoleCommandHandler> logger)
        {
            _roleRepository = roleRepository;
            _logger = logger;
        }

        public async Task<AddRoleResponseDto> Handle(AddRoleCommand request, CancellationToken token)
        {
            AddRoleResponseDto response = new();
            Roles roles = new()
            {
                RoleName = request.RoleName,
                RoleDescription = request.RoleDescription,
                RoleLevel = request.RoleLevel,
            };
            bool isRoleAdded = await _roleRepository.AddRoleAsync(roles, token);
            bool isUpdateRoleLevelAsync = await _roleRepository.UpdateRoleLevelAsync(roles.RoleLevel, token);
            response.IsSuccess = isRoleAdded && isUpdateRoleLevelAsync;
            if (response.IsSuccess)
            {
                _logger.LogInformation("Role has been add: {roleName}", roles.RoleName);
            }
            return response;
        }
    }

}
