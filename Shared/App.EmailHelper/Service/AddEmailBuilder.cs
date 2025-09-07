using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_BangAppEmailHelper.Service
{
    public static class AddEmailBuilder
    {
        public static IServiceCollection AddEmailBuilder(this IServiceCollection services)
        {
            services.AddScoped<HtmlRenderer>();
            return services;
        }
    }
}
