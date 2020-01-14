using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Scheduledo.Core.Enums;
using Scheduledo.Service.Models;

namespace Scheduledo.Service.Extensions
{
    public static class Queryables
    {
        private static readonly int _defaultPage = 1;
        private static readonly int _defaultPageSize = 10;

        public static IQueryable<T> Sort<T>(this IQueryable<T> baseQuery, ICollection<SortInput> sortProperties, bool clear = false)
        {
            if (sortProperties != null && sortProperties.Any())
            {
                foreach (var sortColumn in sortProperties)
                {
                    baseQuery = baseQuery.OrderBy(
                        sortColumn.Column, sortColumn.Direction,
                        sortColumn == sortProperties.First() && clear);
                }
            }

            return baseQuery;
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string sortProperty, SortDirection sortOrder, bool clear = false)
        {
            var hasProperty = sortProperty != null && HasProperty(typeof(T), sortProperty, true);

            if (hasProperty)
            {
                var lambda = (dynamic)CreateExpression(typeof(T), sortProperty);

                if (clear || !source.IsOrdered())
                {
                    return sortOrder == SortDirection.Ascending
                        ? Queryable.OrderBy(source, lambda)
                        : Queryable.OrderByDescending(source, lambda);
                }
                else
                {
                    return sortOrder == SortDirection.Ascending
                        ? Queryable.ThenBy(source, lambda)
                        : Queryable.ThenByDescending(source, lambda);
                }
            }

            return source;
        }

        public static bool IsOrdered<T>(this IQueryable<T> queryable)
        {
            if (queryable == null)
            {
                throw new ArgumentNullException(nameof(queryable));
            }

            return queryable.Expression.Type == typeof(IOrderedQueryable<T>);
        }

        public static IQueryable<T> Paging<T>(this IQueryable<T> query, int page, int pageSize)
        {
            if (page < 1)
            {
                page = _defaultPage;
            }

            if (pageSize < 1)
            {
                page = _defaultPageSize;
            }

            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public static IQueryable<T> Paging<T>(this IOrderedQueryable<T> query, int page, int pageSize)
        {
            return Paging((IQueryable<T>)query, page, pageSize);
        }

        public static IQueryable<T> Paging<T>(this IQueryable<T> query, int? page, int? pageSize)
        {
            if (page.HasValue && page < 1)
            {
                page = _defaultPage;
            }

            if (pageSize.HasValue && pageSize < 1)
            {
                page = _defaultPageSize;
            }

            if (page.HasValue && pageSize.HasValue)
                return query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);

            return query;
        }

        public static IQueryable<T> Paging<T>(this IOrderedQueryable<T> query, int? page, int? pageSize)
        {
            return Paging((IQueryable<T>)query, page, pageSize);
        }

        private static bool HasProperty(Type obj, string propertyName, bool includeNestedObjects = false)
        {
            if (includeNestedObjects)
            {
                return GetPropertyWithNested(obj, propertyName) != null;
            }

            return GetProperty(obj, propertyName) != null;
        }

        private static PropertyInfo GetProperty(Type obj, string propertyName)
        {
            return obj.GetProperty(propertyName, BindingFlags.IgnoreCase
                | BindingFlags.Public | BindingFlags.Instance);
        }

        private static PropertyInfo GetPropertyWithNested(Type objType, string propertyName)
        {
            PropertyInfo propInfo = GetProperty(objType, propertyName);

            if (propInfo == null && propertyName.Contains("."))
            {
                string firstProp = propertyName.Substring(0, propertyName.IndexOf('.'));
                propInfo = GetProperty(objType, firstProp);

                if (propInfo == null)
                {
                    throw new ArgumentNullException(nameof(firstProp));
                }

                return GetPropertyWithNested(
                    propInfo.PropertyType,
                    propertyName.Substring(propertyName.IndexOf('.') + 1));
            }

            return propInfo;
        }

        private static LambdaExpression CreateExpression(Type type, string propertyName)
        {
            var param = Expression.Parameter(type, "x");

            Expression body = param;
            foreach (var member in propertyName.Split('.'))
            {
                body = Expression.PropertyOrField(body, member);
            }

            return Expression.Lambda(body, param);
        }
    }
}
