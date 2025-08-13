using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using NSubstitute;
using Salon.Infra.DbContext;
using System;
using System.IO;

namespace Salon.Infra.Tests.MongoDbContext
{
    public class DbFixture : IDbFixture
    {
        private string _dbName;
        public const string CONNECTIONSTRING = "mongodb://localhost:27017/{0}";
        public IMongoClient MongoClient;

        public DbFixture(string dbName = "")
        {
            _dbName = string.IsNullOrEmpty(dbName) ? $"test_{Guid.NewGuid()}" : dbName;

            var clientSettings = MongoClientSettings.FromConnectionString(string.Format(CONNECTIONSTRING, _dbName));
            clientSettings.LinqProvider = MongoDB.Driver.Linq.LinqProvider.V3;

            MongoClient = new MongoClient(clientSettings);
            MongoClient.GetDatabase(string.Format(CONNECTIONSTRING, _dbName));

            var configuration = Substitute.For<IConfiguration>();
            configuration[Arg.Any<string>()].ReturnsForAnyArgs(_dbName);

            this.DbContext = new DbContext.MongoDbContext(MongoClient, configuration);
        }

        public IMongoDbContext DbContext { get; }

        public void Dispose()
        {
            MongoClient.DropDatabase(_dbName);
        }
    }
}
