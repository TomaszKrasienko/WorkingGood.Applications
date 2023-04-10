using System;
using MongoDB.Driver;
using WorkingGood.Infrastructure.Common.ConfigModels;

namespace WorkingGood.Infrastructure.Persistance
{
    public class MongoDbContext : IMongoDbContext
    {
        private readonly MongoDbConfig _mongoDbConfig;
        public MongoDbContext(MongoDbConfig mongoDbConfig)
        {
            _mongoDbConfig = mongoDbConfig;
        }
        public IMongoDatabase GetDatabase()
        {
            var client = new MongoClient(_mongoDbConfig.ConnectionString);
            return client.GetDatabase(_mongoDbConfig.Database);
        }
    }
}

