using Microsoft.Extensions.DependencyInjection;
using System;
using TPICAP.API.Interfaces;
using TPICAP.API.Services;
using TPICAP.Data.Interfaces;
using TPICAP.Data.Repositories;

namespace TPICAP.API.Extensions
{
    public static class CoreExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddScoped<IPersonsRepository, PersonsSqlServerExpressRepository>();

            services.AddScoped<IPersonsService, PersonsService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
        }
    }
}