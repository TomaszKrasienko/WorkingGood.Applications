using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using WorkingGood.Domain.Interfaces;
using WorkingGood.Domain.Models;
using WorkingGood.Infrastructure.Common.ConfigModels;
using WorkingGood.Log;

namespace WorkingGood.Infrastructure.Persistance.Repositories
{
    public class ApplicationRepository : BaseRepository<Application>, IApplicationRepository
    {
        public ApplicationRepository(
            IWgLog<BaseRepository<Application>> logger, 
            IMongoDbContext mongoDbContext, 
            MongoDbConfig mongoDbConfig) : base(logger, mongoDbContext, mongoDbConfig)
        { }

        public async Task DeleteForOfferAsync(Guid offerId)
        {
            Logger.Info($"Executing {nameof(DeleteForOfferAsync)} for {offerId}");
            var db = MongoDbContext.GetDatabase();
            var collection = db.GetCollection<Application>(MongoDbConfig.Database);
            await collection.DeleteManyAsync(x => x.OfferId == offerId);
        }
    }
}

