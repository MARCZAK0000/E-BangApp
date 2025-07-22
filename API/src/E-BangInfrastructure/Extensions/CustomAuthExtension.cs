using E_BangDomain.Settings;
using E_BangInfrastructure.Cors;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace E_BangInfrastructure.Extensions
{
    public static class CustomAuthExtension
    {
        public static void AddCustomAuthorization(this IServiceCollection services)
        {
            services.AddAuthorizationBuilder();
        } 

        public static void AddCorsPolicy(this IServiceCollection services, IConfiguration configuration) 
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

        public static void AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            AuthenticationSettings authenticationSettings = new();
            configuration.GetSection("AuthenticationSettings").Bind(authenticationSettings);

            HttpOnlyTokenOptions httpOnlyTokenOptions = new();
            configuration.GetSection("HttpOnlySettings").Bind(httpOnlyTokenOptions);

            services.AddSingleton(authenticationSettings);
            services.AddSingleton(httpOnlyTokenOptions);    
            services.AddAuthentication(options=>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                
                }).AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = authenticationSettings.Issure,
                        ValidAudience = authenticationSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.Key))
                    };

                    if (httpOnlyTokenOptions.IsHttpOnly)
                    {
                        options.Events = new()
                        {

                            OnMessageReceived = ctx =>
                            {
                                ctx.Request.Cookies.TryGetValue("accessToken", out string? accessToken);
                                if (!string.IsNullOrWhiteSpace(accessToken))
                                {
                                    ctx.Token = accessToken;
                                }
                                return Task.CompletedTask;
                            },
                        };
                    }
                });
        }
    }
}
