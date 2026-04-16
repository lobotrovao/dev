namespace Wex.PurchaseTransaction.Infrastructure.Extensions
{
    using System.Linq.Expressions;
    using System.Text.Json;
    using Wex.PurchaseTransaction.Application.SeedWork;

    /// <summary>
    /// Exntesion methods to apply dynamic filtering, sorting and pagination to an IQueryable based on JSON input models.
    /// </summary>
    public static class EfFilterBuilder
    {
        /// <summary>
        /// Apply filters on fields of T based on a JSON string representing a FilterModel. 
        /// Supports operators like contains, equals, startsWith, endsWith, isEmpty, isNotEmpty and logical combination of filters using And/Or.
        /// </summary>
        /// <typeparam name="T">Generic.</typeparam>
        /// <param name="query">Query used.</param>
        /// <param name="filterModelJson">Json with Filter Model.</param>
        /// <returns>IQueryable with filter.</returns>
        public static IQueryable<T> ApplyFilters<T>(
            this IQueryable<T> query,
            string filterModelJson)
        {
            if (string.IsNullOrWhiteSpace(filterModelJson))
                return query;

            FilterModel? model;
            try
            {
                model = JsonSerializer.Deserialize<FilterModel>(filterModelJson);
            }
            catch
            {
                return query; // invalid JSON
            }

            if (model?.Items == null || model.Items.Count == 0)
                return query;

            var parameter = Expression.Parameter(typeof(T), "x");
            Expression? finalExpr = null;

            foreach (var item in model.Items)
            {
                var expr = BuildExpression<T>(parameter, item);
                if (expr == null) continue;

                finalExpr = finalExpr == null
                    ? expr
                    : model.LogicOperator == "And"
                        ? Expression.AndAlso(finalExpr, expr)
                        : Expression.OrElse(finalExpr, expr);
            }

            if (finalExpr == null)
                return query;

            var lambda = Expression.Lambda<Func<T, bool>>(finalExpr, parameter);
            return query.Where(lambda);
        }

        /// <summary>
        /// Apply pagination to the query based on page number and page size.
        /// </summary>
        /// <typeparam name="T">Generic.</typeparam>
        /// <param name="query">Query used.</param>
        /// <param name="page">Page index.</param>
        /// <param name="pageSize">Page Size.</param>
        /// <returns>IQueryable with pagination.</returns>
        public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, int page, int pageSize)
        {
            if (page < 0) page = 0;
            if (pageSize <= 0) pageSize = 10;

            int startIndex = page * pageSize;

            return query.Skip(startIndex).Take(pageSize);
        }

        /// <summary>
        /// Apply sorting to the query based on a JSON string representing a list of SortItem, where each item specifies a field and sort direction (asc/desc).
        /// </summary>
        /// <typeparam name="T">Generic.</typeparam>
        /// <param name="query">Query used.</param>
        /// <param name="sortModelJson">Json with Sort Model.</param>
        /// <returns>IQueryable with sorting.</returns>
        public static IQueryable<T> ApplySorting<T>(
            this IQueryable<T> query,
            string sortModelJson)
        {
            if (string.IsNullOrWhiteSpace(sortModelJson))
                return query;

            List<SortItem>? sorts;
            try
            {
                sorts = JsonSerializer.Deserialize<List<SortItem>>(sortModelJson);
            }
            catch
            {
                return query;
            }

            if (sorts == null || sorts.Count == 0)
                return query;

            IOrderedQueryable<T>? ordered = null;

            foreach (var sort in sorts)
            {
                var param = Expression.Parameter(typeof(T), "x");
                var prop = Expression.Property(param, sort.Field);
                var lambda = Expression.Lambda(prop, param);

                bool desc = sort.Sort == "desc";

                ordered = ordered == null
                    ? (desc
                        ? Queryable.OrderByDescending(query, (dynamic)lambda)
                        : Queryable.OrderBy(query, (dynamic)lambda))
                    : (desc
                        ? Queryable.ThenByDescending(ordered, (dynamic)lambda)
                        : Queryable.ThenBy(ordered, (dynamic)lambda));
            }

            return ordered ?? query;
        }

        /// <summary>
        /// Build an expression for a single filter item based on the specified operator. 
        /// Supports string operations like contains, equals, startsWith, endsWith, isEmpty and isNotEmpty.
        /// </summary>
        /// <typeparam name="T">Generic.</typeparam>
        /// <param name="param">Parameter Expression.</param>
        /// <param name="filter">FilterItem.</param>
        /// <returns>Expression.</returns>
        private static Expression? BuildExpression<T>(
            ParameterExpression param,
            FilterItem filter)
        {
            var prop = Expression.Property(param, filter.Field);
            var constant = Expression.Constant(filter.Value);
            var toStringCall = Expression.Call(prop, "ToString", null);
            
            try
            {
                Expression? body = filter.Operator switch
                {
                    "contains" =>
                        Expression.Call(
                            toStringCall,
                            nameof(string.Contains),
                            null,
                            Expression.Call(constant, nameof(string.ToLower), null)
                        ),

                    "equals" =>
                        Expression.Equal(toStringCall, constant),

                    "startsWith" =>
                        Expression.Call(
                            toStringCall,
                            nameof(string.StartsWith),
                            null,
                            constant
                        ),

                    "endsWith" =>
                        Expression.Call(
                            toStringCall,
                            nameof(string.EndsWith),
                            null,
                            constant
                        ),

                    "isEmpty" =>
                        Expression.Equal(
                            Expression.Call(toStringCall, nameof(string.Trim), null),
                            Expression.Constant("")
                        ),

                    "isNotEmpty" =>
                        Expression.NotEqual(
                            Expression.Call(toStringCall, nameof(string.Trim), null),
                            Expression.Constant("")
                        ),

                    _ => null
                };
                return body;
            }
            catch (Exception)
            {
            }

            return null;
        }
    }
}
