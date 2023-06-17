using WorkingGood.Infrastructure.Common.Extensions;

namespace WorkingGood.WebApi.Common.Extensions.Configuration;

public static class WebApiConfiguration
{
    public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddInfrastructureConfiguration(configuration)
            .ConfigureSwagger()
            .AddCorsPolicy()
            .SetAutoMapper();
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
    private static IServiceCollection SetAutoMapper(this IServiceCollection services)
    {
        return services
            .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    }
}