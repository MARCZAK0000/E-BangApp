using E_BangApplication.Authentication;
using E_BangApplication.CQRS.Command.AccountHandler;
using E_BangApplication.CQRS.Command.RoleHandler;
using E_BangApplication.CQRS.Command.UserHandler;
using E_BangApplication.CQRS.Query.AccountHandler;
using E_BangApplication.Mapper;
using E_BangApplication.Validation.Account;
using E_BangApplication.Validation.Role;
using E_BangApplication.Validation.User;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using MyCustomMediator.Classes;
using System.Reflection;

namespace E_BangApplication.Exetensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ProjectMapper));
            services.AddMyCustomMediator(Assembly.GetAssembly(typeof(RegisterAccountCommand)));
            services.AddFluentValidationAutoValidation();
            services.AddScoped<IUserContext, UserContext>();

            services.AddScoped<IValidator<RegisterAccountCommand>, RegisterAccountValidator>();
            services.AddScoped<IValidator<VerifyCredentialsCommand>, VerifyCredentialsValidator>();
            services.AddScoped<IValidator<ValidateCredentialsTwoFactoryTokenCommand>, ValidateCredentialsTwoFactoryTokenValidator>();
            services.AddScoped<IValidator<ConfirmEmailCommand>, ConfirmEmailValidator>();
            services.AddScoped<IValidator<ResetPasswordCommand>, ResetPasswordValidator>();
            services.AddScoped<IValidator<ForgotPasswordTokenQuery>, ForgetPasswordValidator>();
            services.AddScoped<IValidator<UpdatePasswordCommand>, UpdatePasswordValidator>();
            services.AddScoped<IValidator<UpdateUserCommand>, UpdateUserValidator>();
            services.AddScoped<IValidator<AddRoleCommand>, AddRoleValidator>();

            return services;
        }
    }
}
