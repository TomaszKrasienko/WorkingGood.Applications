using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WorkingGood.Domain.Interfaces;
using WorkingGood.Infrastructure.Common.ConfigModels;
using WorkingGood.Infrastructure.Persistance;
using WorkingGood.Infrastructure.Persistance.Repositories;

namespace WorkingGood.Infrastructure.Common.Extensions
{
	public static class InfrastructureConfiguration
	{
		public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
		{
			services
				.AddServices()
				.AddConfigs(configuration);
			return services;
		}
		private static IServiceCollection AddServices(this IServiceCollection services)
		{
			services.AddScoped<IMongoDbContext, MongoDbContext>();
			services.AddScoped<IApplicationRepository, ApplicationRepository>();
			return services;
		}
		private static IServiceCollection AddConfigs(this IServiceCollection services, IConfiguration configuration)
		{
			MongoDbConfig mongoDbConfig = new();
			configuration.Bind("MongoDbConnection", mongoDbConfig);
			services.AddSingleton(mongoDbConfig);
			return services;
		}
	}
}

