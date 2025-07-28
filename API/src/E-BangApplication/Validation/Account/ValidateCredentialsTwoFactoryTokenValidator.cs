using E_BangApplication.CQRS.Command.AccountHandler;
using E_BangApplication.Validation.Shared;
using FluentValidation;

namespace E_BangApplication.Validation.Account
{
    public class ValidateCredentialsTwoFactoryTokenValidator : AbstractValidator<ValidateCredentialsTwoFactoryTokenCommand>
    {
        private readonly string[] digits = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        public ValidateCredentialsTwoFactoryTokenValidator()
        {
            Include(new CredentialsValidator());
            RuleFor(pr => pr.TwoFactorCode)
                .Length(6).WithMessage("Two-factor code must be 6 digits long.")
                .Custom((val, act) =>
                {
                    if(val.Any(c => !digits.Contains(c.ToString())))
                    {
                        act.AddFailure("Two-factor code must contain only digits.");
                    }
                });
        }
    }
}
