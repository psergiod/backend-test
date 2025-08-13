using MongoDB.Bson;
using MongoDB.Driver;
using Salon.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Salon.Domain.Base
{
    public interface IRepository<TEntity> where TEntity : class, IEntity<ObjectId>
    {
        Task InsertAsync(TEntity entity);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<bool> ExistAsync(ObjectId id);
        Task<TEntity> GetByIdAsync(ObjectId id);
        Task<bool> UpdateAsync(TEntity entity);
        Task<bool> RemoveAsync(ObjectId id);
        Task<TEntity> GetFirstOrDefaultByFilterAsync(FilterDefinition<TEntity> filter);
        Task<IEnumerable<TSelect>> GetByExpressionAsync<TSelect>(
           Expression<Func<TEntity, bool>> where,
           Expression<Func<TEntity, TSelect>> selector,
           int? top = null);
        Task<TSelect> GetByFirstOrDefaultExpressionAsync<TSelect>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TSelect>> selector);
    }
}