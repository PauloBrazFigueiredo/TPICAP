using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;
using TPICAP.Data.Models;

namespace TPICAP.API.Extensions
{
    public static class DbContextExtensions
    {
        private const string PersonsConnection = "PersonsConnection";

        public static void AddDbContextPools(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var directory = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
            services.AddDbContext<PersonsDatabaseContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString(PersonsConnection)));
        }
    }
}
