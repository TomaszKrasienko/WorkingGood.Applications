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
    }
}

