using WorkingGood.Infrastructure.Common.Extensions;

namespace WorkingGood.WebApi.Common.Extensions.Configuration;

public static class WebApiConfiguration
{
    public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddInfrastructureConfiguration(configuration)
            .ConfigureSwagger();
        return services;
    }
}