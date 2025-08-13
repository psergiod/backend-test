using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Salon.Infra.DbContext
{
    public class MongoDbContext : IMongoDbContext
    {
        private readonly string _databaseName;
        private IMongoDatabase _db { get; set; }
        private IMongoClient _mongoClient { get; set; }
        public MongoDbContext(IMongoClient mongoClient, IConfiguration configuration)
        {
            _databaseName = configuration["ConnectionStrings:DataBase"];
            _mongoClient = mongoClient;
            _db = _mongoClient.GetDatabase(_databaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _db.GetCollection<T>(name);
        }

        public void Destroy()
        {
            _mongoClient.DropDatabase(_databaseName);
        }
    }
}
