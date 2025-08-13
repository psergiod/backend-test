using Microsoft.AspNetCore.Mvc.ApplicationModels;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Salon.Domain.Base;
using Salon.Domain.Models;
using Salon.Infra.DbContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Salon.Infra.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity<ObjectId>
    {
        private readonly IMongoCollection<TEntity> _collection;
        private readonly bool _showRemoved;

        public Repository(IMongoDbContext dbContext, bool showRemoved = false)
        {
            _collection = dbContext.GetCollection<TEntity>(typeof(TEntity).Name);
            _showRemoved = showRemoved;
        }

        public async Task InsertAsync(TEntity entity) => await _collection.InsertOneAsync(entity);

        public async Task<IEnumerable<TSelect>> GetByExpressionAsync<TSelect>(
            Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, TSelect>> selector,
            int? top = null)
        {
            IMongoQueryable<TEntity> collectionQueryable = _collection.AsQueryable();
            IMongoQueryable<TEntity> query;
            Expression<Func<TEntity, bool>> hideRemovedCriteria = x => !x.Removed;

            query =
                collectionQueryable
                 .Where(where);

            if (!_showRemoved)
                query = query.Where(hideRemovedCriteria);

            if (top != null)
                return await Task.Run(() => query.Select(selector).Take((int)top));

            return await Task.Run(() => query.Select(selector));
        }

        public async Task<TSelect> GetByFirstOrDefaultExpressionAsync<TSelect>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TSelect>> selector)
        {
            var resultList = await GetByExpressionAsync(where, selector);

            return resultList.FirstOrDefault();
        }

        public async Task<TEntity> GetByIdAsync(ObjectId id)
        {
            var filter = Builders<TEntity>.Filter.Eq(x => x.Id, id);
            filter = AddRemovedFilter(filter);
            var entity = await _collection.Find(filter).FirstOrDefaultAsync();

            return entity;
        }

        public async Task<TEntity> GetFirstOrDefaultByFilterAsync(FilterDefinition<TEntity> filter)
        {
            filter = AddRemovedFilter(filter);
            var entity = await _collection.Find(filter).FirstOrDefaultAsync();

            return entity;
        }

        public async Task<bool> ExistAsync(ObjectId id) => await GetByIdAsync(id) != null;

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            if (!_showRemoved)
                return await _collection.Find(x => !x.Removed).ToListAsync();

            return await _collection.Find(_ => true).ToListAsync();

        }

        public async Task<bool> UpdateAsync(TEntity entity)
        {
            var result = await _collection.ReplaceOneAsync(x => x.Id.Equals(entity.Id), entity, new ReplaceOptions { IsUpsert = true });

            return result.ModifiedCount == 1;
        }

        public async Task<bool> RemoveAsync(ObjectId id)
        {
            var filter = Builders<TEntity>.Filter.Eq(x => x.Id, id);
            var result = await _collection.DeleteOneAsync(filter);

            return result.DeletedCount == 1;
        }

        private FilterDefinition<TEntity> AddRemovedFilter(FilterDefinition<TEntity> filter)
        {
            if (!_showRemoved)
            {
                filter &= Builders<TEntity>.Filter.Eq(x => x.Removed, false);
            }
            return filter;
        }
    }
}
