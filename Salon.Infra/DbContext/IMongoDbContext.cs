using MongoDB.Driver;
using System;

namespace Salon.Infra.DbContext
{
    public interface IMongoDbContext
    {
        IMongoCollection<T> GetCollection<T>(string name);
        void Destroy();
    }
}