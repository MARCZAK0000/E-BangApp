using E_BangApplication.Mapper;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using MyCustomMediator.Classes;
using System.Reflection;

namespace E_BangApplication.Exetensions
{
    public static class ServiceExtension
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ProjectMapper));
            services.AddMyCustomMediator();
            services.AddFluentValidationAutoValidation();
        }
    }
}
