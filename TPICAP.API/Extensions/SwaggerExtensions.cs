using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;

namespace TPICAP.API.Extensions
{
    public static class SwaggerExtensions
    {
        private const string SwaggerTitle = "IYDATA Cleaning Service API V1";
        private const string SwaggerVersion = "v1";

        public static void AddSwaggers(this IServiceCollection services)
        {
            services.AddSwaggerGen(SwaggerAction);
        }

        public static void UseSwaggerUIs(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            app.UseSwaggerUI(SwaggerUiOptions);
        }

        private static void SwaggerAction(SwaggerGenOptions c)
        {
            c.SwaggerDoc(SwaggerVersion, new OpenApiInfo
            {
                Title = SwaggerTitle,
                Version = SwaggerVersion
            });
            // Bearer token authentication
            var securityDefinition = new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                BearerFormat = "JWT",
                Scheme = "bearer",
                Description = "JWT Authorization header using the Bearer scheme.",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
            };
            c.AddSecurityDefinition("Bearer", securityDefinition);

            // Make sure swagger UI requires a Bearer token specified
            var securityScheme = new OpenApiSecurityScheme()
            {
                Reference = new OpenApiReference()
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            };
            var securityRequirements = new OpenApiSecurityRequirement()
            {
                {
                    securityScheme,
                    Array.Empty<string>()
                }
            };
            c.AddSecurityRequirement(securityRequirements);
        }

        private static void SwaggerUiOptions(SwaggerUIOptions options)
        {
            options.SwaggerEndpoint($"/swagger/{SwaggerVersion}/swagger.json", SwaggerTitle);
            options.InjectStylesheet("/swagger-ui-dark/swagger-dark.css");
        }
    }
}
