using System;
using Microsoft.Extensions.Logging;
using WorkingGood.Domain.Interfaces;
using WorkingGood.Infrastructure.Common.ConfigModels;

namespace WorkingGood.Infrastructure.Persistance.Repositories
{
    public class BaseRepository<T> : IRepository<T>
    {
        private readonly ILogger<BaseRepository<T>> _logger;
        private readonly IMongoDbContext _mongoDbContext;
        private readonly MongoDbConfig _mongoDbConfig;
        public BaseRepository(ILogger<BaseRepository<T>> logger, IMongoDbContext mongoDbContext, MongoDbConfig mongoDbConfig)
        {
            _logger = logger;
            _mongoDbContext = mongoDbContext;
            _mongoDbConfig = mongoDbConfig;
        }
        public async Task<T> AddAsync(T entity)
        {
            _logger.LogInformation($"Adding {typeof(T)} to databse");
            var db = _mongoDbContext.GetDatabase();
            var collection = db.GetCollection<T>(_mongoDbConfig.Database);
            await collection.InsertOneAsync(entity);
            return entity;
        }
    }
}

