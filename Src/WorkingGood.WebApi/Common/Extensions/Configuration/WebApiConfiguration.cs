using WorkingGood.Infrastructure.Common.Extensions;

namespace WorkingGood.WebApi.Common.Extensions.Configuration;

public static class WebApiConfiguration
{
    public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddInfrastructureConfiguration(configuration)
            .ConfigureSwagger()
            .AddCorsPolicy();
        return services;
    }
    private static IServiceCollection AddCorsPolicy(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(Common.Statics.ConfigurationConst.CORS_POLICY_NAME, builder =>
            {
                builder
                    .SetIsOriginAllowed(_ => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });
        return services;
    }
}