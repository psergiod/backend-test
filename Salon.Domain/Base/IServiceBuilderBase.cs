using MongoDB.Bson;
using System;
using System.Linq.Expressions;
using Salon.Domain.Models;

namespace Salon.Domain.Base
{
    public interface IServiceBuilderBase<TEntity> where TEntity : Entity<ObjectId>
    {
        Expression<Func<TEntity, bool>> Build();
        void AddFilter(Expression<Func<TEntity, bool>> filter);

    }
}