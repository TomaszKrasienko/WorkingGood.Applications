using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using WorkingGood.Domain.Interfaces;
using WorkingGood.Domain.Interfaces.Valida;
using WorkingGood.Infrastructure.Common.ConfigModels;
using WorkingGood.Infrastructure.Common.Statics;
using WorkingGood.Infrastructure.Persistance;
using WorkingGood.Infrastructure.Persistance.Repositories;
using WorkingGood.Infrastructure.Validation;

namespace WorkingGood.Infrastructure.Common.Extensions
{
	public static class InfrastructureConfiguration
	{
		public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
		{
			services
				.AddServices()
				.AddConfigs(configuration)
				.AddHttpClient(configuration);
			return services;
		}
		private static IServiceCollection AddServices(this IServiceCollection services)
		{
			services.AddScoped<IMongoDbContext, MongoDbContext>();
			services.AddScoped<IApplicationRepository, ApplicationRepository>();
			services.AddScoped<IOfferChecker, OfferChecker>();
			return services;
		}
		private static IServiceCollection AddConfigs(this IServiceCollection services, IConfiguration configuration)
		{
			MongoDbConfig mongoDbConfig = new();
			configuration.Bind("MongoDbConnection", mongoDbConfig);
			services.AddSingleton(mongoDbConfig);
			return services;
		}
		private static IServiceCollection AddHttpClient(this IServiceCollection services, IConfiguration configuration)
		{
			var retryPolicy = Policy.HandleResult<HttpResponseMessage>(
				r => !r.IsSuccessStatusCode)
				.RetryAsync(3);
			string offersClientAddress = configuration.GetValue<string>("OffersClientAddress") ?? throw new Exception();
			services.AddHttpClient(HttpClients.OffersClient, client =>
			{
				client.BaseAddress = new Uri(offersClientAddress);
				client.Timeout = new TimeSpan(0, 0, 30);
				client.DefaultRequestHeaders.Clear();
			}).AddPolicyHandler(retryPolicy);
			return services;
		}
	}
}

