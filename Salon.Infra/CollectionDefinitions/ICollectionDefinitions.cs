using MongoDB.Bson;
using MongoDB.Driver;
using Salon.Domain.Models;

namespace Salon.Infra.CollectionDefinitions
{
    public interface ICollectionDefinitions<TEntity> where TEntity : Entity<ObjectId>
    {
        void BuildIndexes();
    }
}
