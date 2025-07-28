using E_BangApplication.CQRS.Command.UserHandler;
using FluentValidation;

namespace E_BangApplication.Validation.User
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("FirstName is required.")
            .MaximumLength(50).WithMessage("FirstName can't be longer than 50 characters.");

            RuleFor(x => x.SecondName)
                .MaximumLength(50).WithMessage("SecondName can't be longer than 50 characters.");

            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("Surname is required.")
                .MaximumLength(50).WithMessage("Surname can't be longer than 50 characters.");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required.")
                .MaximumLength(100).WithMessage("City can't be longer than 100 characters.");

            RuleFor(x => x.StreetNumber)
                .NotEmpty().WithMessage("StreetNumber is required.")
                .MaximumLength(10).WithMessage("StreetNumber can't be longer than 10 characters.");

            RuleFor(x => x.StreetName)
                .NotEmpty().WithMessage("StreetName is required.")
                .MaximumLength(100).WithMessage("StreetName can't be longer than 100 characters.");

            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("Country is required.")
                .MaximumLength(50).WithMessage("Country can't be longer than 50 characters.");

            RuleFor(x => x.PostalCode)
                .NotEmpty().WithMessage("PostalCode is required.")
                .Matches(@"^\d{2}-\d{3}$").WithMessage("PostalCode must be in the format XX-XXX (e.g. 00-123).");
        }
    }
}
