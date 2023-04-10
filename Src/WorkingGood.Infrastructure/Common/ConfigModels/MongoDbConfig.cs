using System;
namespace WorkingGood.Infrastructure.Common.ConfigModels
{
	public class MongoDbConfig
	{
        public string ConnectionString { get; set; } = string.Empty;
        public string Database { get; set; } = string.Empty;
    }
}

