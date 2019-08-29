using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Cig.YanFa.Account.Application.Common
{
    public static class EnumerableExtensions
    {
        //public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> query, bool condition, Expression<Func<T, bool>> predicate)
        //{
        //    return condition ? query.Where(predicate) : query;
        //}

        //public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> query, bool condition, Expression<Func<T, int, bool>> predicate)
        //{
        //    return condition ? query.Where(predicate) : query;
        //}

        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> query, bool condition, Func<T, bool> predicate)
        {
            return condition ? query.Where(predicate) : query;
        }
    }
}
