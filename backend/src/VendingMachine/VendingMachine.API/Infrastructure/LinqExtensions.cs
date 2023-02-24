using System.Linq.Expressions;

namespace VendingMachine.API.Infrastructure
{
    public static class LinqExtensions
    {
        #region Methods

        public static IOrderedQueryable<TSource> Order<TSource, TKey>(this IQueryable<TSource> query, Expression<Func<TSource, TKey>> keySelector, bool ascending)
        {
            if (ascending)
            {
                return query.OrderBy(keySelector);
            }

            return query.OrderByDescending(keySelector);
        }

        #endregion Methods
    }
}