using E_BangApplication.CQRS.Command.UserHandler;
using E_BangApplication.Validation.Shared;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_BangApplication.Validation.User
{
    public class UpdatePasswordValidator : AbstractValidator<UpdatePasswordCommand>
    {
        public UpdatePasswordValidator()
        {

            RuleFor(pr => pr.OldPassword)
                .NotEmpty().WithMessage("Password cannot be empty.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
                .MaximumLength(18).WithMessage("Password cannot exceed 100 characters.");

            RuleFor(pr => pr.NewPassword)
              .NotEmpty().WithMessage("Password cannot be empty.")
              .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
              .MaximumLength(18).WithMessage("Password cannot exceed 100 characters.");

            RuleFor(pr => pr.ConfirmNewPassword)
             .NotEmpty().WithMessage("Password cannot be empty.")
             .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
             .MaximumLength(18).WithMessage("Password cannot exceed 100 characters.")
             .Equal(pr => pr.NewPassword).WithMessage("Confirm password must be the same as Password");
        }
    }
}
