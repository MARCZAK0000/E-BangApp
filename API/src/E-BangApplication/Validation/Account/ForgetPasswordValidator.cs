using E_BangApplication.CQRS.Query.AccountHandler;
using FluentValidation;

namespace E_BangApplication.Validation.Account
{
    public class ForgetPasswordValidator : AbstractValidator<ForgotPasswordTokenQuery>
    {
        public ForgetPasswordValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(250).WithMessage("Invalid length");

        }
    }
}
