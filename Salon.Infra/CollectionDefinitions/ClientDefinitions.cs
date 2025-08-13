using MongoDB.Driver;
using Salon.Domain.Clients.Entities;
using Salon.Infra.DbContext;
using System.Linq;

namespace Salon.Infra.CollectionDefinitions
{
    public class ClientDefinitions : ICollectionDefinitions<Client>
    {
        private readonly IMongoDbContext _mongoDbContext;
        public ClientDefinitions(IMongoDbContext mongoDbContext)
        {
            _mongoDbContext = mongoDbContext;
        }
        public void BuildIndexes()
        {
            var coll = _mongoDbContext.GetCollection<Client>(typeof(Client).Name);

            var hasIndexes = coll.Indexes.List().ToList().Count;
            if (hasIndexes <= 1)
            {
                var indexBuilder = Builders<Client>.IndexKeys;
                var keys = indexBuilder
                    .Ascending(movie => movie.Name)
                    .Ascending(movie => movie.Email);
                var options = new CreateIndexOptions
                {
                    Background = true,
                    Unique = true
                };
                var indexModel = new CreateIndexModel<Client>(keys, options);
                coll.Indexes.CreateOne(indexModel);
            }
        }
    }
}
