using E_BangDomain.RequestDtos.AccountRepositoryDtos;
using FluentValidation;

namespace E_BangApplication.Validation.Shared
{
    public class CredentialsValidator : AbstractValidator<CredentialsAccountDto>
    {
        public CredentialsValidator()
        {
            RuleFor(pr => pr.Email)
                .NotEmpty().WithMessage("Email cannot be empty.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(250).WithMessage("Invalid length");

            RuleFor(pr => pr.Password)
                .NotEmpty().WithMessage("Password cannot be empty.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
                .MaximumLength(18).WithMessage("Password cannot exceed 100 characters.");

            RuleFor(pr => pr.ConfirmPassword)
             .NotEmpty().WithMessage("Password cannot be empty.")
             .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
             .MaximumLength(18).WithMessage("Password cannot exceed 100 characters.")
             .Equal(pr => pr.Password).WithMessage("Confirm password must be the same as Password");
        }
    }
}
