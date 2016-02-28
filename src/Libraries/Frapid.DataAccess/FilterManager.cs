using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Frapid.DataAccess.Models;
using Frapid.i18n;
using Frapid.NPoco;

namespace Frapid.DataAccess
{
    public class FilterManager
    {
        public static string GetColumnName<T>(T poco, string propertyName)
        {
            var type = poco.GetType();

            var a = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .FirstOrDefault(p => p.Name.Equals(propertyName));

            var attr = a?.GetCustomAttributes(typeof(ColumnAttribute), false)
                .Cast<ColumnAttribute>().FirstOrDefault();

            if (attr != null)
            {
                return attr.Name;
            }

            return string.Empty;
        }

        public static void AddFilters(ref Sql sql, List<Filter> filters)
        {
            if (filters == null || filters.Count().Equals(0))
            {
                return;
            }

            foreach (var filter in filters)
            {
                string column = Sanitizer.SanitizeIdentifierName(filter.ColumnName);

                if (string.IsNullOrWhiteSpace(column))
                {
                    continue;
                }

                string statement = filter.FilterStatement;

                if (statement == null || statement.ToUpperInvariant() != "OR")
                {
                    statement = "AND";
                }

                statement += " ";

                switch ((FilterCondition)filter.FilterCondition)
                {
                    case FilterCondition.IsEqualTo:
                        sql.Append(statement + Sanitizer.SanitizeIdentifierName(column) + " = @0", filter.FilterValue);
                        break;
                    case FilterCondition.IsNotEqualTo:
                        sql.Append(statement + Sanitizer.SanitizeIdentifierName(column) + " != @0", filter.FilterValue);
                        break;
                    case FilterCondition.IsLessThan:
                        sql.Append(statement + Sanitizer.SanitizeIdentifierName(column) + " < @0", filter.FilterValue);
                        break;
                    case FilterCondition.IsLessThanEqualTo:
                        sql.Append(statement + Sanitizer.SanitizeIdentifierName(column) + " <= @0", filter.FilterValue);
                        break;
                    case FilterCondition.IsGreaterThan:
                        sql.Append(statement + Sanitizer.SanitizeIdentifierName(column) + " > @0", filter.FilterValue);
                        break;
                    case FilterCondition.IsGreaterThanEqualTo:
                        sql.Append(statement + Sanitizer.SanitizeIdentifierName(column) + " >= @0", filter.FilterValue);
                        break;
                    case FilterCondition.IsBetween:
                        sql.Append(statement + Sanitizer.SanitizeIdentifierName(column) + " BETWEEN @0 AND @1", filter.FilterValue, filter.FilterAndValue);
                        break;
                    case FilterCondition.IsNotBetween:
                        sql.Append(statement + Sanitizer.SanitizeIdentifierName(column) + " NOT BETWEEN @0 AND @1", filter.FilterValue, filter.FilterAndValue);
                        break;
                    case FilterCondition.IsLike:
                        sql.Append(statement + " lower(" + Sanitizer.SanitizeIdentifierName(column) + ") LIKE @0",
                            "%" + filter.FilterValue.ToLower(CultureManager.GetCurrent()) + "%");
                        break;
                    case FilterCondition.IsNotLike:
                        sql.Append(statement + " lower(" + Sanitizer.SanitizeIdentifierName(column) + ") NOT LIKE @0",
                            "%" + filter.FilterValue.ToLower(CultureManager.GetCurrent()) + "%");
                        break;
                }
            }
        }

        public static void AddFilters<T>(ref Sql sql, T poco, List<Filter> filters)
        {
            if (filters == null || filters.Count().Equals(0))
            {
                return;
            }

            foreach (var filter in filters)
            {
                if (string.IsNullOrWhiteSpace(filter.ColumnName))
                {
                    if (!string.IsNullOrWhiteSpace(filter.PropertyName))
                    {
                        filter.ColumnName = GetColumnName(poco, filter.PropertyName);
                    }
                }

                string column = Sanitizer.SanitizeIdentifierName(filter.ColumnName);

                if (string.IsNullOrWhiteSpace(column))
                {
                    continue;
                }

                string statement = filter.FilterStatement;

                if (statement == null || statement.ToUpperInvariant() != "OR")
                {
                    statement = "AND";
                }

                statement += " ";

                switch ((FilterCondition)filter.FilterCondition)
                {
                    case FilterCondition.IsEqualTo:
                        sql.Append(statement + Sanitizer.SanitizeIdentifierName(column) + " = @0", filter.FilterValue);
                        break;
                    case FilterCondition.IsNotEqualTo:
                        sql.Append(statement + Sanitizer.SanitizeIdentifierName(column) + " != @0", filter.FilterValue);
                        break;
                    case FilterCondition.IsLessThan:
                        sql.Append(statement + Sanitizer.SanitizeIdentifierName(column) + " < @0", filter.FilterValue);
                        break;
                    case FilterCondition.IsLessThanEqualTo:
                        sql.Append(statement + Sanitizer.SanitizeIdentifierName(column) + " <= @0", filter.FilterValue);
                        break;
                    case FilterCondition.IsGreaterThan:
                        sql.Append(statement + Sanitizer.SanitizeIdentifierName(column) + " > @0", filter.FilterValue);
                        break;
                    case FilterCondition.IsGreaterThanEqualTo:
                        sql.Append(statement + Sanitizer.SanitizeIdentifierName(column) + " >= @0", filter.FilterValue);
                        break;
                    case FilterCondition.IsBetween:
                        sql.Append(statement + Sanitizer.SanitizeIdentifierName(column) + " BETWEEN @0 AND @1", filter.FilterValue, filter.FilterAndValue);
                        break;
                    case FilterCondition.IsNotBetween:
                        sql.Append(statement + Sanitizer.SanitizeIdentifierName(column) + " NOT BETWEEN @0 AND @1", filter.FilterValue, filter.FilterAndValue);
                        break;
                    case FilterCondition.IsLike:
                        sql.Append(statement + " lower(" + Sanitizer.SanitizeIdentifierName(column) + ") LIKE @0",
                            "%" + filter.FilterValue.ToString().ToLower(CultureManager.GetCurrent()) + "%");
                        break;
                    case FilterCondition.IsNotLike:
                        sql.Append(statement + " lower(" + Sanitizer.SanitizeIdentifierName(column) + ") NOT LIKE @0",
                            "%" + filter.FilterValue.ToString().ToLower(CultureManager.GetCurrent()) + "%");
                        break;
                }
            }
        }
    }
}