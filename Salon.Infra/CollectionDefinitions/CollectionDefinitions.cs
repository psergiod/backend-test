using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using Salon.Domain.Models;
using Salon.Infra.DbContext;

namespace Salon.Infra.CollectionDefinitions

{
    public abstract class CollectionDefinitions<TEntity> : ICollectionDefinitions<TEntity> where TEntity : Entity<ObjectId>
    {
        protected CollectionDefinitions(IMongoDbContext mongoDbContext) { }
        public abstract IMongoCollection<TEntity> GetCollection();
        public abstract void BuildIndexes();
    }
}
