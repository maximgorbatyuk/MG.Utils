using MG.Utils.Abstract.NonNullableObjects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MG.WebHost.Infrastructure.Config
{
    public static class RedisConfig
    {
        public static void Setup(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDistributedRedisCache(options =>
            {
                // substitute with "localhost" for local redis
                options.Configuration = new NonNullableString(configuration.GetConnectionString("Redis")).Value();

                // options.Configuration = "localhost";
                options.InstanceName = "master";
            });
        }
    }
}