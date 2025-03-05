using E_BangInfrastructure.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace E_BangInfrastructure.Extensions
{
    internal static class CustomAuthExtension
    {
        internal static void AddCustomAuthorization(this IServiceCollection services)
        {
            services.AddAuthorizationBuilder();
        } 

        internal static void AddCorsPolicy(this IServiceCollection services, IConfiguration configuration) 
        {
            var corsOptions = new CorsSettings();
            configuration.GetSection("Cors").Bind(corsOptions);
            services.AddCors(pr => pr.AddPolicy("corsPolicy", options =>
            {
                options.WithOrigins(origins: [.. corsOptions.CorsDomain])
                     .AllowCredentials()
                     .AllowAnyMethod()
                     .AllowAnyHeader();
            }));
        }

        internal static void AddCustomAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication()
                .AddBearerToken(IdentityConstants.BearerScheme, options => 
                {
                    //options.Events = new Microsoft.AspNetCore.Authentication.BearerToken.BearerTokenEvents()
                    //{
                    //    OnMessageReceived = ctx =>
                    //    {
                    //        ctx.Request
                    //    }

                    //}
                });
        }
    }
}
