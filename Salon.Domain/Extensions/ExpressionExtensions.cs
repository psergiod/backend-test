using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace VideoStore.Domain.Extensions
{
    public static class ExpressionExtensions
    {
        public static Expression<Func<TParameter, bool>> And<TParameter>(this Expression<Func<TParameter, bool>> firstExpression, Expression<Func<TParameter, bool>> secondExpression)
        {
            return PredicateBuilder.New(firstExpression).And(secondExpression);
        }
        public static Expression<Func<TParameter, bool>> ConcatWithAnd<TParameter>(this IList<Expression<Func<TParameter, bool>>> expressions)
        {
            Expression<Func<TParameter, bool>> seed = expressions.FirstOrDefault();
            return expressions.Skip(1).Aggregate(seed, (Expression<Func<TParameter, bool>> current, Expression<Func<TParameter, bool>> expression) => current.And(expression));
        }
    }
}
