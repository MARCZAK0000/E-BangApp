using E_BangApplication.CQRS.Command.AccountHandler;
using E_BangApplication.Validation.Shared;
using FluentValidation;

namespace E_BangApplication.Validation.Account
{
    public class RegisterAccountValidator : AbstractValidator<RegisterAccountCommand>
    {
        public RegisterAccountValidator()
        {
            Include(new CredentialsValidator());

        }
    }
}
