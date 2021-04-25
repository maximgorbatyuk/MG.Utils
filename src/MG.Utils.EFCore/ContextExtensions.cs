using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MG.Utils.Abstract;
using MG.Utils.Abstract.Entities;
using MG.Utils.Abstract.Exceptions;
using MG.Utils.Entities;
using MG.Utils.Exceptions;
using MG.Utils.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace MG.Utils.EFCore
{
    public static class ContextExtensions
    {
        public static IQueryable<TEntity> Active<TEntity>(this IQueryable<TEntity> set)
            where TEntity : class, IHasDeletedAt
        {
            return set.Where(x => x.DeletedAt == null);
        }

        public static async Task<TEntity> AddEntityAsync<TContext, TEntity>(this TContext set, TEntity entity)
            where TContext : DbContext
            where TEntity : class
        {
            var entry = await set.AddAsync(entity);

            return entry.Entity;
        }

        public static async Task AddRangeAsync<TContext, TEntity>(this TContext set, IEnumerable<TEntity> entity)
            where TContext : DbContext
            where TEntity : class
        {
            await set.AddRangeAsync(entity);
        }

        /// <summary>
        /// Returns an array with no tracking entities.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="query">Query.</param>
        /// <returns>Array.</returns>
        public static Task<T[]> AllAsync<T>(this IQueryable<T> query)
            where T : class
        {
            return query
                .AsNoTracking()
                .ToArrayAsync();
        }

        public static async Task<TResultEntity[]> AllAsync<TEntity, TResultEntity>(this IQueryable<TEntity> query, Func<TEntity, TResultEntity> transform)
            where TEntity : class
            where TResultEntity : class
        {
            return (await query
                .AllAsync())
                .Select(transform)
                .ToArray();
        }

        public static Task<T> ByIdOrNullAsync<T>(this IQueryable<T> query, long id)
            where T : class, IHasId
        {
            return query.FirstOrDefaultAsync(x => x.Id == id);
        }

        public static async Task HasEntityOrFailAsync<T>(this IQueryable<T> query, long id)
            where T : class, IHasId
        {
            if (!await query.AnyAsync(x => x.Id == id))
            {
                throw ResourceNotFoundException.CreateFromEntity<T>(id);
            }
        }

        public static async Task HasEntitiesOrFailAsync<T>(this IQueryable<T> query, IReadOnlyCollection<long> ids)
            where T : class, IHasId
        {
            if (ids.Except(await query.Select(x => x.Id).ToArrayAsync()).Any())
            {
                throw new BadRequestException($"Incorrect {typeof(T).Name} ids");
            }
        }

        public static async Task<IReadOnlyCollection<TDto>> MapAsync<TEntity, TDto>(
            this IQueryable<TEntity> query, Func<TEntity, TDto> converter)
            where TEntity : class
        {
            return (await query.AllAsync())
                .Select(converter)
                .ToArray();
        }

        public static async Task<T> ByIdOrFailAsync<T>(this IQueryable<T> query, long id)
            where T : class, IHasId
        {
            return await query.FirstOrDefaultAsync(x => x.Id == id)
                   ?? throw ResourceNotFoundException.CreateFromEntity<T>(id);
        }

        public static async Task<Pageable<TEntity>> AsPaginatedAsync<TEntity>(
            this IQueryable<TEntity> query,
            PageModel pageModelOrNull = null)
            where TEntity : class
        {
            pageModelOrNull ??= PageModel.Default;

            return new Pageable<TEntity>(
                currentPage: pageModelOrNull.Page,
                pageSize: pageModelOrNull.PageSize,
                totalItems: await query.CountAsync(),
                results: await query
                    .Skip(pageModelOrNull.ToSkip)
                    .Take(pageModelOrNull.PageSize)
                    .AllAsync());
        }

        public static async Task<Pageable<TResultEntity>> AsPaginatedAsync<TEntity, TResultEntity>(
            this IQueryable<TEntity> query,
            Func<TEntity, TResultEntity> transform,
            PageModel pageModelOrNull = null)
            where TEntity : class
            where TResultEntity : class
        {
            pageModelOrNull ??= PageModel.Default;

            return new Pageable<TResultEntity>(
                currentPage: pageModelOrNull.Page,
                pageSize: pageModelOrNull.PageSize,
                totalItems: await query.CountAsync(),
                results: (await query
                    .Skip(pageModelOrNull.ToSkip)
                    .Take(pageModelOrNull.PageSize)
                    .AsNoTracking()
                    .ToArrayAsync())
                .Select(transform)
                .ToArray());
        }

        public static IQueryable<T> When<T>(
            this IQueryable<T> query,
            bool condition,
            Expression<Func<T, bool>> whereExpression)
            where T : class
        {
            return condition ? query.Where(whereExpression) : query;
        }

        public static async Task AnyOrFailAsync<T>(
            this IQueryable<T> context,
            Expression<Func<T, bool>> expression,
            string errorMessage = null)
            where T : class
        {
            if (!await context.AnyAsync(expression))
            {
                throw new ResourceNotFoundException(errorMessage ?? "There is no item found by a passed condition");
            }
        }

        public static async Task NoItemsByConditionOrFailAsync<T>(
            this IQueryable<T> context,
            Expression<Func<T, bool>> expression,
            string errorMessage = null)
            where T : class
        {
            if (await context.AnyAsync(expression))
            {
                throw new BadRequestException(errorMessage ?? "There are items found by a passed condition");
            }
        }

        public static async Task NoItemsByConditionOrFailAsync<T>(
            this IQueryable<T> context,
            Expression<Func<T, bool>> expression,
            Exception exception)
            where T : class
        {
            if (await context.AnyAsync(expression))
            {
                throw exception;
            }
        }

        public static async Task<int> TrySaveChangesAsync<TContext>(this TContext context)
            where TContext : DbContext
        {
            try
            {
                return await context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                throw new DatabaseException(exception);
            }
        }

        public static IQueryable<TEntity> IncludeWhen<TEntity, TProperty>(
            this IQueryable<TEntity> query,
            bool condition,
            Expression<Func<TEntity, TProperty>> navigationPropertyPath)
            where TEntity : class
        {
            return condition ? query.Include(navigationPropertyPath) : query;
        }

        public static IQueryable<TEntity> IncludeWhen<TEntity, TProperty>(
            this IQueryable<TEntity> query,
            bool condition,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, TProperty>> include)
            where TEntity : class
        {
            return condition ? include(query) : query;
        }

        public static IQueryable<long> GetIds<TEntity>(
            this IQueryable<TEntity> query)
            where TEntity : BaseModel
        {
            return query.Select(x => x.Id);
        }

        public static async Task<TResult> DoWithinTransactionAsync<TContext, TResult>(
            this TContext context,
            Func<Task<TResult>> action,
            string errorMessage = null)
            where TContext : DbContext
        {
            action.ThrowIfNull(nameof(action));

            try
            {
                await context.Database.BeginTransactionAsync();

                TResult result = await action();

                await context.Database.CommitTransactionAsync();

                return result;
            }
            catch (Exception exception)
            {
                await context.Database.RollbackTransactionAsync();
                const string defaultError = "Cannot execute transaction due to database error";
                throw new InvalidOperationException(errorMessage ?? defaultError, exception);
            }
        }
    }
}