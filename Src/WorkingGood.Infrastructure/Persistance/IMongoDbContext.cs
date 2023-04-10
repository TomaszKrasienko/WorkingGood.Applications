using System;
using MongoDB.Driver;

namespace WorkingGood.Infrastructure.Persistance
{
	public interface IMongoDbContext
	{
		IMongoDatabase GetDatabase();
	}
}

