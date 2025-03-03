using E_BangApplication.Mapper;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace E_BangApplication.Exetensions
{
    public static class ServiceExtension
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ProjectMapper));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<ProjectMapper>());
            services.AddFluentValidationAutoValidation();
        }
    }
}
