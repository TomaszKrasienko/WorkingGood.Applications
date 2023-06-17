using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using WorkingGood.Domain.Interfaces;
using WorkingGood.Domain.Models;
using WorkingGood.Infrastructure.Common.ConfigModels;

namespace WorkingGood.Infrastructure.Persistance.Repositories
{
    public class ApplicationRepository : BaseRepository<Application>, IApplicationRepository
    {
        public ApplicationRepository(
            ILogger<BaseRepository<Application>> logger, 
            IMongoDbContext mongoDbContext, 
            MongoDbConfig mongoDbConfig) : base(logger, mongoDbContext, mongoDbConfig)
        { }

        public async Task DeleteForOfferAsync(Guid offerId)
        {
            Logger.LogInformation($"Deleting all applications for offer with id {offerId}");
            var db = MongoDbContext.GetDatabase();
            var collection = db.GetCollection<Application>(MongoDbConfig.Database);
            await collection.DeleteManyAsync(x => x.OfferId == offerId);
        }
    }
}

