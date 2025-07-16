using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Mapster;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Shared.Utils.Params;

namespace Shared.Utils;

public static class Helper
{
    #region Pagination

    /// <summary>
    /// Paginate a queryable
    /// </summary>
    /// <param name="source"></param>
    /// <param name="paginationParams"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<PaginationResult<T>> ToPaginatedListAsync<T>(
        this IQueryable<T> source,
        PaginationParams paginationParams)
    {
        var totalItems = await source.CountAsync();

        var items = await source
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();

        return new PaginationResult<T>(
            items,
            totalItems,
            paginationParams.PageNumber,
            paginationParams.PageSize
        );
    }

    /// <summary>
    /// Paginate a queryable and project it to another type
    /// </summary>
    /// <param name="source"></param>
    /// <param name="paginationParams"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    public static async Task<PaginationResult<TResult>> ProjectToPaginatedListAsync<TSource, TResult>(
        this IQueryable<TSource> source,
        PaginationParams paginationParams)
    {
        var totalItems = await source.CountAsync();

        var items = await source
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ProjectToType<TResult>()
            .ToListAsync();

        return new PaginationResult<TResult>(
            items,
            totalItems,
            paginationParams.PageNumber,
            paginationParams.PageSize
        );
    }

    #endregion

    #region MongoDB Pagination

    public static async Task<PaginationResult<T>> ToPaginatedListAsync<T>(
        this IFindFluent<T, T> findFluent,
        PaginationParams paginationParams)
    {
        var totalItems = await findFluent.CountDocumentsAsync();

        var items = await findFluent
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Limit(paginationParams.PageSize)
            .ToListAsync();

        return new PaginationResult<T>(
            items,
            (int)totalItems,
            paginationParams.PageNumber,
            paginationParams.PageSize
        );
    }

    // Pagination with mapping for IFindFluent
    public static async Task<PaginationResult<TResult>> ToPaginatedListAsync<T, TResult>(
        this IFindFluent<T, T> findFluent,
        PaginationParams paginationParams,
        Func<T, TResult> mapper)
    {
        var totalItems = await findFluent.CountDocumentsAsync();

        var items = await findFluent
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Limit(paginationParams.PageSize)
            .ToListAsync();

        var mappedItems = items.Select(mapper).ToList();

        return new PaginationResult<TResult>(
            mappedItems,
            (int)totalItems,
            paginationParams.PageNumber,
            paginationParams.PageSize
        );
    }

    #endregion

    #region Combine Expressions

    /// <summary>
    ///     Combine two expressions with AndAlso operator (&&)
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Expression<Func<T, bool>> CombineAndAlsoExpressions<T>(this Expression<Func<T, bool>> first,
        Expression<Func<T, bool>> second)
    {
        return CombineExpressions(first, second, Expression.AndAlso);
    }

    /// <summary>
    ///     Combine two expressions with OrElse operator (||)
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Expression<Func<T, bool>> CombineOrExpressions<T>(this Expression<Func<T, bool>> first,
        Expression<Func<T, bool>> second)
    {
        return CombineExpressions(first, second, Expression.OrElse);
    }

    private static Expression<Func<T, bool>> CombineExpressions<T>(this Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2, Func<Expression, Expression, BinaryExpression> combiner)
    {
        var parameter = Expression.Parameter(typeof(T));

        var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
        var left = leftVisitor.Visit(expr1.Body);

        var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
        var right = rightVisitor.Visit(expr2.Body);

        return Expression.Lambda<Func<T, bool>>(combiner(left, right), parameter);
    }

    #endregion

    #region Sorting

    /// <summary>
    /// Apply first sorting
    /// </summary>
    /// <param name="source"></param>
    /// <param name="isDescending"></param>
    /// <param name="sortProperty"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IOrderedQueryable<T> ApplySorting<T>(this IQueryable<T> source, bool isDescending,
        Expression<Func<T, object>> sortProperty)
    {
        return isDescending ? source.OrderByDescending(sortProperty) : source.OrderBy(sortProperty);
    }


    /// <summary>
    /// Apply secondary sorting
    /// </summary>
    /// <param name="source"></param>
    /// <param name="isDescending"></param>
    /// <param name="sortProperty"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, bool isDescending,
        Expression<Func<T, object>> sortProperty)
    {
        return isDescending ? source.ThenByDescending(sortProperty) : source.ThenBy(sortProperty);
    }

    #endregion
}

internal class ReplaceExpressionVisitor(Expression oldValue, Expression newValue) : ExpressionVisitor
{
    public override Expression Visit(Expression? node)
    {
        return (node == oldValue ? newValue : base.Visit(node))!;
    }
}