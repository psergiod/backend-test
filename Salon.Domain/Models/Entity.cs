using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace Salon.Domain.Models
{
    public abstract class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        [BsonId]
        public TPrimaryKey Id { get; set; }

        public bool Removed { get; private set; }

        public Entity<TPrimaryKey> Remove() 
        {
            Removed = true;
            return this;
        }

        public Entity<TPrimaryKey> Enable()
        {
            Removed = false;
            return this;
        }
    }
}
