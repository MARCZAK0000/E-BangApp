using E_BangApplication.Authentication;
using E_BangApplication.Mapper;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using MyCustomMediator.Classes;

namespace E_BangApplication.Exetensions
{
    public static class ServiceExtension
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ProjectMapper));
            services.AddMyCustomMediator();
            services.AddFluentValidationAutoValidation();
            services.AddScoped<IUserContext, UserContext>();
        }
    }
}
