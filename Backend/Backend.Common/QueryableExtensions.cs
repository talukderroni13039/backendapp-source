using Backend.Common.Models;
using System;
using System.Linq;


namespace Backend.Common
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> PageBy<T>(this IQueryable<T> source,PageRequest pageRequest)
        {
            return source.Skip(pageRequest.SkipCount).Take(pageRequest.TakeCount);
        }
    }
}
