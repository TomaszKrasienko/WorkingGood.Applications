using System;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using WorkingGood.Domain.Interfaces;
using WorkingGood.Domain.Models;
using WorkingGood.Infrastructure.Common.ConfigModels;
using WorkingGood.Log;

namespace WorkingGood.Infrastructure.Persistance.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T : Entity
    {
        protected readonly IWgLog<BaseRepository<T>> Logger;
        protected readonly IMongoDbContext MongoDbContext;
        protected readonly MongoDbConfig MongoDbConfig;
        protected BaseRepository(IWgLog<BaseRepository<T>> logger, IMongoDbContext mongoDbContext, MongoDbConfig mongoDbConfig)
        {
            Logger = logger;
            MongoDbContext = mongoDbContext;
            MongoDbConfig = mongoDbConfig;
        }
        
        public async Task<T> AddAsync(T entity)
        {
            Logger.Info($"Executing {nameof(AddAsync)} for type {typeof(T)}");
            IMongoCollection<T> collection = GetCollection();
            await collection.InsertOneAsync(entity);
            return entity;
        }
        
        public async Task<T> GetByIdAsync(Guid id)
        {
            Logger.Info($"Executing {nameof(GetByIdAsync)} for type {typeof(T)}");
            IMongoCollection<T> collection = GetCollection();
            var filter = Builders<T>.Filter.Eq("_id", id);
            var entity = await collection.Find<T>(filter).FirstOrDefaultAsync();
            return entity;
        }
        
        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> includeProperties)
        {
            Logger.Info($"Executing {nameof(GetAllAsync)} for type {typeof(T)}");
            IMongoCollection<T> collection = GetCollection();
            var entities = await collection.FindAsync(includeProperties);
            return await entities.ToListAsync();
        }

        protected IMongoCollection<T> GetCollection()
        {
            IMongoDatabase db = MongoDbContext.GetDatabase();
            return db.GetCollection<T>(MongoDbConfig.Database);
        }

    }
}

