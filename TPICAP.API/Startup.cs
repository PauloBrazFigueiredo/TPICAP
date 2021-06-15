using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;
using TPICAP.API.Extensions;
using TPICAP.API.Middleware;

namespace TPICAP.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddServices();

            services.AddControllers();

            services.AddSwaggers();

            services.AddDbContextPools(Configuration);

            services.AddAutoMapper();

            var key = Encoding.UTF8.GetBytes(Configuration["ApplicationSettings:JWTSecret"].ToString());
            services.AddAuthentication(key);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUIs();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            

            app.UseMiddleware<RequestResponseLoggingMiddleware>();

            app.UseAuthorization();

            bool authenticationEnabled = Configuration.GetValue<bool>("ApplicationSettings:AuthenticationEnabled");
            app.UseEndpoints(endpoints =>
            {
                if (authenticationEnabled)
                { 
                    endpoints.MapControllers();
                }
                else
                {
                    endpoints.MapControllers().WithMetadata(new AllowAnonymousAttribute());
                }
            });
        }
    }
}
