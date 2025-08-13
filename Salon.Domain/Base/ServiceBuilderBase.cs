using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Salon.Domain.Models;
using VideoStore.Domain.Extensions;

namespace Salon.Domain.Base
{
    public class ServiceBuilderBase<TEntity> : IServiceBuilderBase<TEntity> where TEntity : Entity<ObjectId>
    {
        private readonly IList<Expression<Func<TEntity, bool>>> _filters;

        public ServiceBuilderBase()
        {
            _filters = new List<Expression<Func<TEntity, bool>>>();
        }

        public Expression<Func<TEntity, bool>> Build()
        {
            var filter = _filters.Any() ? _filters.ConcatWithAnd() : EmptyExpression();
            _filters.Clear();
            return filter;
        }

        public void AddFilter(Expression<Func<TEntity, bool>> filter)
        {
            _filters.Add(filter);
        }

        private Expression<Func<TEntity, bool>> EmptyExpression()
        {
            Expression<Func<TEntity, bool>> expression = x => true;
            return expression;
        }
    }
}
