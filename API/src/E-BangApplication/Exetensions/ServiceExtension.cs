using E_BangApplication.Authentication;
using E_BangApplication.CQRS.Command.AccountHandler;
using E_BangApplication.Mapper;
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

            return services;
        }
    }
}
