using E_BangApplication.Authentication;
using E_BangApplication.CQRS.Command.AccountHandler;
using E_BangApplication.Mapper;
using E_BangApplication.Validation.Account;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
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

            return services;
        }
    }
}
