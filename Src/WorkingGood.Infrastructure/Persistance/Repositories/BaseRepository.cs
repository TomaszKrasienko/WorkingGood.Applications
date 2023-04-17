using System;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using WorkingGood.Domain.Interfaces;
using WorkingGood.Domain.Models;
using WorkingGood.Infrastructure.Common.ConfigModels;

namespace WorkingGood.Infrastructure.Persistance.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T : Entity
    {
        protected readonly ILogger<BaseRepository<T>> Logger;
        protected readonly IMongoDbContext MongoDbContext;
        protected readonly MongoDbConfig MongoDbConfig;
        protected BaseRepository(ILogger<BaseRepository<T>> logger, IMongoDbContext mongoDbContext, MongoDbConfig mongoDbConfig)
        {
            Logger = logger;
            MongoDbContext = mongoDbContext;
            MongoDbConfig = mongoDbConfig;
        }
        public async Task<T> AddAsync(T entity)
        {
            Logger.LogInformation($"Adding {typeof(T)} to database");
            var db = MongoDbContext.GetDatabase();
            var collection = db.GetCollection<T>(MongoDbConfig.Database);
            await collection.InsertOneAsync(entity);
            return entity;
        }
        public async Task<T> GetByIdAsync(Guid id)
        {
            Logger.LogInformation($"Getting Application by {id}");
            var db = MongoDbContext.GetDatabase();
            var collection = db.GetCollection<T>(MongoDbConfig.Database);
            var filter = Builders<T>.Filter.Eq("_id", id);
            var entity = await collection.Find<T>(filter).FirstOrDefaultAsync();
            return entity;
        }
        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> includeProperties)
        {
            var db = MongoDbContext.GetDatabase();
            var collection = db.GetCollection<T>(MongoDbConfig.Database);
            var entities = await collection.FindAsync(includeProperties);
            return await entities.ToListAsync();
        }
    }
}

