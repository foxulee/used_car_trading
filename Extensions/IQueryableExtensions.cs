using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using project_vega.Core.Models;

namespace project_vega.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<Vehicle> ApplyFiltering(this IQueryable<Vehicle> query, VehicleQuery vehicleQuery)
        {
            if (vehicleQuery.MakeId.HasValue)
            {
                query = query.Where(v => v.Model.MakeId == vehicleQuery.MakeId.Value);
            }

            if (vehicleQuery.ModelId.HasValue)
            {
                query = query.Where(v => v.Model.Id == vehicleQuery.ModelId);
            }

            return query;
        }

        public static IQueryable<T> ApplyOrdering<T>(this IQueryable<T> query, IQueryObject queryObj, Dictionary<string, Expression<Func<T, object>>> columnsMap)
        {
            if (String.IsNullOrWhiteSpace(queryObj.SortBy) || !columnsMap.ContainsKey(queryObj.SortBy))
            {
                return query;
            }

            if (queryObj.IsSortAscending)
            {
                return query.OrderBy(columnsMap[queryObj.SortBy]);
            }
            else
            {
                return query.OrderByDescending(columnsMap[queryObj.SortBy]);
            }

        }

        public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, IQueryObject queryObject)
        {
            if (queryObject.Page <= 0)
            {
                queryObject.Page = 1;
            }
            if (queryObject.PageSize <= 0)
            {
                queryObject.PageSize = 10;
            }

            return query.Skip((queryObject.Page - 1) * queryObject.PageSize)
                .Take(queryObject.PageSize);
        }
    }
}