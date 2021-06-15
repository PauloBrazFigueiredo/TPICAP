using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using TPICAP.API.Mapping;

namespace TPICAP.API.Extensions
{
    public static class AutoMapperExtensions
    {
        public static void AddAutoMapper(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(configuration =>
            {
                configuration.AddProfile(new PersonProfile());
            });

            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
