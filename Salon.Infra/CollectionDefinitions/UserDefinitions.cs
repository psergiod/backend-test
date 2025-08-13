using MongoDB.Driver;
using Salon.Domain.Users.Entities;
using Salon.Infra.DbContext;
using System.Linq;

namespace Salon.Infra.CollectionDefinitions
{
    public class UserDefinitions : ICollectionDefinitions<User>
    {
        private readonly IMongoDbContext _mongoDbContext;
        public UserDefinitions(IMongoDbContext mongoDbContext) 
        {
            _mongoDbContext = mongoDbContext;
        }

        public void BuildIndexes()
        {
            var coll = _mongoDbContext.GetCollection<User>(typeof(User).Name);

            var hasIndexes = coll.Indexes.List().ToList().Count;
            if (hasIndexes <= 1)
            {
                var indexBuilder = Builders<User>.IndexKeys;
                var keys = indexBuilder
                    .Ascending(user => user.Login)
                    .Ascending(user => user.Removed);
                var options = new CreateIndexOptions
                {
                    Background = true,
                    Unique = true
                };

                var indexModel = new CreateIndexModel<User>(keys, options);
                coll.Indexes.CreateOne(indexModel);
            }
        }
    }
}
