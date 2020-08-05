using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Shared.Models;
using Shared.Models.Filter;

namespace App.Extensions
{
    public static class FilterExtensions
    {
        /// <summary>
        /// FilterModel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TFilterType"></typeparam>
        /// <param name="query"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IQueryable<T> Filter<T, TFilterType>(this IQueryable<T> query, FilterModel<TFilterType> filter)
        {
            (Expression Body, ParameterExpression Parameter) value;

            if (filter.FilterType is NumberFilterType numberFilterType)
            {
                value = CreateNumberFilter<T, TFilterType>(filter, numberFilterType);
            }
            else if(filter.FilterType is DateTimeFilterType dateTimeFilterType)
            {
                value = CreateDateTimeFilter<T, TFilterType>(filter, dateTimeFilterType);
            }
            else if(filter.FilterType is StringFilterType stringFilterType)
            {
                value = CreateStringFilter<T, TFilterType>(filter, stringFilterType);
            }
            else if (filter.FilterType is BoolFilterType)
            {
                value = CreateBoolFilter<T, TFilterType>(filter);
            }
            else if (filter.FilterType is GuidFilterType guidFilterType)
            {
                value = CreateGuidFilter<T, TFilterType>(filter, guidFilterType);
            }
            else
            {
                throw new NotImplementedException(filter.FilterType.ToString());
            }

            return query.Where(Expression.Lambda<Func<T, bool>>(value.Body, value.Parameter));

        }

        public static IQueryable<T> Filter<T>(this IQueryable<T> query, FilterPagingParameters parameters)
        {
            query = query.Filter(parameters.NumberFilters);
            query = query.Filter(parameters.DateTimeFilters);
            query = query.Filter(parameters.StringFilters);
            query = query.Filter(parameters.BoolFilters);
            query = query.Filter(parameters.GuidFilters);

            return query;
        }

        private static IQueryable<T> Filter<T, TFilterType>(this IQueryable<T> query, FilterModel<TFilterType>[] filters)
        {
            if (filters != null && filters.Any())
            {
                foreach (var filter in filters)
                {
                    query = query.Filter(filter);
                }
            }

            return query;
        }

        private static (Expression Body, ParameterExpression Parameter) CreateNumberFilter<T, TFilterType>(FilterModel<TFilterType> filter, NumberFilterType filterType)
        {
            var propertyExpression = GetExpression<T, decimal>(filter.Name);
            var parameter = propertyExpression.Parameters[0];
            var value1 = Expression.Constant(Convert.ToDecimal(filter.Values[0]));
            Expression body;

            if (filterType == NumberFilterType.Between)
            {
                var value2 = Expression.Constant(Convert.ToDecimal(filter.Values[1]));
                var bodyMin = Expression.GreaterThanOrEqual(propertyExpression.Body, value1);
                var bodyMax = Expression.LessThanOrEqual(propertyExpression.Body, value2);

                body = Expression.AndAlso(bodyMin, bodyMax);

            }
            else if (filterType == NumberFilterType.Equals)
            {
                body = Expression.Equal(propertyExpression.Body, value1);
            }
            else if (filterType == NumberFilterType.GreaterThan)
            {
                body = Expression.GreaterThan(propertyExpression.Body, value1);
            }
            else if (filterType == NumberFilterType.GreaterThanOrEqual)
            {
                body = Expression.GreaterThanOrEqual(propertyExpression.Body, value1);
            }
            else if (filterType == NumberFilterType.LessThan)
            {
                body = Expression.LessThan(propertyExpression.Body, value1);
            }
            else if (filterType == NumberFilterType.LessThanOrEqual)
            {
                body = Expression.LessThanOrEqual(propertyExpression.Body, value1);
            }
            else
            {
                throw new NotImplementedException(filterType.ToString());
            }

            return (body, parameter);
        }

        private static (Expression Body, ParameterExpression Parameter) CreateDateTimeFilter<T, TFilterType>(FilterModel<TFilterType> filter, DateTimeFilterType filterType)
        {
            var propertyExpression = GetExpression<T, DateTime>(filter.Name);
            var parameter = propertyExpression.Parameters[0];
            var value1 = Expression.Constant(Convert.ToDateTime(filter.Values[0])); // TODO: ToDate Required?
            Expression body;

            if (filterType == DateTimeFilterType.Between)
            {
                var value2 = Expression.Constant(Convert.ToDateTime(filter.Values[1]));
                var bodyMin = Expression.GreaterThanOrEqual(propertyExpression.Body, value1);
                var bodyMax = Expression.LessThanOrEqual(propertyExpression.Body, value2);

                body = Expression.AndAlso(bodyMin, bodyMax);

            }
            else if (filterType == DateTimeFilterType.Equals)
            {
                body = Expression.Equal(propertyExpression.Body, value1);
            }
            else if (filterType == DateTimeFilterType.GreaterThan)
            {
                body = Expression.GreaterThan(propertyExpression.Body, value1);
            }
            else if (filterType == DateTimeFilterType.GreaterThanOrEqual)
            {
                body = Expression.GreaterThanOrEqual(propertyExpression.Body, value1);
            }
            else if (filterType == DateTimeFilterType.LessThan)
            {
                body = Expression.LessThan(propertyExpression.Body, value1);
            }
            else if (filterType == DateTimeFilterType.LessThanOrEqual)
            {
                body = Expression.LessThanOrEqual(propertyExpression.Body, value1);
            }
            else
            {
                throw new NotImplementedException(filterType.ToString());
            }

            return (body, parameter);
        }

        private static (Expression Body, ParameterExpression Parameter) CreateStringFilter<T, TFilterType>(FilterModel<TFilterType> filter, StringFilterType filterType)
        {
            // TODO: Try to make it the same way everywhere
            string methodName = Enum.GetName(typeof(StringFilterType), filterType);
            MethodInfo methodInfo = typeof(string).GetMethod(methodName, new Type[] { typeof(string) });

            if(methodInfo == null)
            {
                throw new NotImplementedException(filterType.ToString());
            }

            var propertyExpression = GetExpression<T, string>(filter.Name);
            var parameter = propertyExpression.Parameters[0];
            var value = Expression.Constant(filter.Values[0] as string);
            Expression body = Expression.Call(propertyExpression.Body, methodInfo, value);

            return (body, parameter);
        }

        private static (Expression Body, ParameterExpression Parameter) CreateGuidFilter<T, TFilterType>(FilterModel<TFilterType> filter, GuidFilterType filterType)
        {
            Expression body;
            var propertyExpression = GetExpression<T, Guid>(filter.Name);
            var parameter = propertyExpression.Parameters[0];


            if (filterType == GuidFilterType.Equals)
            {
                var value = Expression.Constant(Guid.Parse(filter.Values[0] as string));
                body = Expression.Equal(propertyExpression.Body, value);
            }
            else if (filterType == GuidFilterType.Contains)
            {
                var arrExpr = Expression.Constant(filter.Values.Select(obj => Guid.Parse(obj as string)).ToArray());
                MethodInfo methodInfo = typeof(ICollection<Guid>).GetMethod("Contains");
                body = Expression.Call(arrExpr, methodInfo, propertyExpression.Body);
            }
            else
            {
                throw new NotImplementedException(filterType.ToString());
            }

            return (body, parameter);
        }

        private static (Expression Body, ParameterExpression Parameter) CreateBoolFilter<T, TFilterType>(FilterModel<TFilterType> filter)
        {
            var propertyExpression = GetExpression<T, bool>(filter.Name);
            var parameter = propertyExpression.Parameters[0];
            var value = Expression.Constant((bool)filter.Values[0]);
            Expression body = Expression.Equal(propertyExpression.Body, value);

            return (body, parameter);
        }

        private static Expression<Func<T, TProperty>> GetExpression<T, TProperty>(string propertyName)
        {
            // x =>
            var parameter = Expression.Parameter(typeof(T));
            // x.Name
            var mapProperty = Expression.Property(parameter, propertyName);
            // (object)x.Name
            var convertedExpression = Expression.Convert(mapProperty, typeof(TProperty));
            // x => (object)x.Name
            return Expression.Lambda<Func<T, TProperty>>(convertedExpression, parameter);
        }
    }
}
