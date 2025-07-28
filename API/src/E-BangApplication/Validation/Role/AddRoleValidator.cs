using E_BangApplication.CQRS.Command.RoleHandler;
using E_BangDomain.Repository;
using FluentValidation;

namespace E_BangApplication.Validation.Role
{
    public class AddRoleValidator: AbstractValidator<AddRoleCommand>
    {
        private readonly IRoleRepository _roleRepository;

        public AddRoleValidator(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;

            RuleFor(x => x.RoleName)
             .NotEmpty().WithMessage("Role name is required.")
             .MaximumLength(50).WithMessage("Role name must be at most 50 characters long.")
             .MustAsync(RoleExistsAsync).WithMessage("Role already exists.");

            RuleFor(x => x.RoleDescription)
                .NotEmpty().WithMessage("Role description is required.")
                .MaximumLength(200).WithMessage("Role description must be at most 200 characters long.");

            RuleFor(x => x.RoleLevel)
                .GreaterThanOrEqualTo(0).WithMessage("Role level must be greater than or equal to 0");


        }
        private async Task<bool> RoleExistsAsync(string roleName, CancellationToken token)
        {
            var resul =  await _roleRepository.GetRolesAsync(token);
            return !resul.Any(pr=>pr.RoleName==roleName);
        } 
    }
}
