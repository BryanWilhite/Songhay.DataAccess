using Songhay.Diagnostics;
using Songhay.Extensions;
using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Songhay.DataAccess.Repository.Extensions
{
    /// <summary>
    /// Extensions of <see cref="DbContext"/>
    /// </summary>
    public static partial class DbContextExtensions
    {
        static DbContextExtensions()
        {
            traceSource = TraceSources.Instance
                .GetTraceSourceFromConfiguredName()
                .WithAllSourceLevels()
                .EnsureTraceSource();
        }

        static readonly TraceSource traceSource;

        /// <summary>
        /// Detaches the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        public static void Detach<TEntity>(this DbContext context, TEntity entity) where TEntity : class
        {
            if (context == null) return;

            context.Entry(entity).State = EntityState.Detached;
        }

        /// <summary>
        /// Returns <see cref="DbContext" /> with default configuration.
        /// </summary>
        /// <typeparam name="TContext">The type of the context.</typeparam>
        /// <param name="context">The context.</param>
        public static TContext WithDefaultConfiguration<TContext>(this DbContext context) where TContext : DbContext
        {
            if (context == null) return null;

            context.Configuration.AutoDetectChangesEnabled = true;
            context.Configuration.ProxyCreationEnabled = true;
            context.Configuration.LazyLoadingEnabled = true;
            context.Configuration.UseDatabaseNullSemantics = false;
            context.Configuration.ValidateOnSaveEnabled = true;

            return context as TContext;
        }

        /// <summary>
        /// Returns <see cref="DbContext" />
        /// with <see cref="DbContext.Configuration.LazyLoadingEnabled"/>
        /// set to <c>false</c>.
        /// </summary>
        /// <typeparam name="TContext">The type of the context.</typeparam>
        /// <param name="context">The context.</param>
        public static TContext WithoutLazyLoadingEnabled<TContext>(this DbContext context) where TContext : DbContext
        {
            if (context == null) return null;

            context.Configuration.LazyLoadingEnabled = false;

            return context as TContext;
        }

        /// <summary>
        /// Returns <see cref="DbContext" />
        /// with <see cref="DbContext.Configuration.ProxyCreationEnabled"/>
        /// set to <c>false</c>.
        /// </summary>
        /// <typeparam name="TContext">The type of the context.</typeparam>
        /// <param name="context">The context.</param>
        public static TContext WithoutProxyCreationEnabled<TContext>(this DbContext context) where TContext : DbContext
        {
            if (context == null) return null;

            context.Configuration.ProxyCreationEnabled = false;

            return context as TContext;
        }

        /// <summary>
        /// Returns <see cref="DbContext" />
        /// with <see cref="DbContext.Configuration.LazyLoadingEnabled"/>
        /// and <see cref="DbContext.Configuration.ProxyCreationEnabled"/>
        /// set to <c>false</c>.
        /// </summary>
        /// <typeparam name="TContext">The type of the context.</typeparam>
        /// <param name="context">The context.</param>
        public static TContext WithoutLazyLoadingAndProxyCreationEnabled<TContext>(this DbContext context) where TContext : DbContext
        {
            if (context == null) return null;

            context
                .WithoutLazyLoadingEnabled<TContext>()
                .WithoutProxyCreationEnabled<TContext>();

            return context as TContext;
        }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <param name="context">The context.</param>
        public static void SaveDbContextChanges(this DbContext context)
        {
            if (context == null)
            {
                traceSource.TraceError($"{nameof(DbContextExtensions)}.{nameof(SaveDbContextChanges)}(): the expected context is not here.");
                return;
            }

            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                traceSource.TraceError(ex);

                Action<DbValidationError> handleValidationError = i =>
                {
                    var errorMessage = string.Format("PropertyName: {0}, ErrorMessage: {1}", i.PropertyName, i.ErrorMessage);
                    traceSource.TraceError(errorMessage);
                };

                ex.EntityValidationErrors.ForEachInEnumerable(i => i.ValidationErrors.ForEachInEnumerable(handleValidationError));
            }
        }

        /// <summary>
        /// Saves the changes asynchronously.
        /// </summary>
        /// <param name="context">The context.</param>
        public static async Task SaveDbContextChangesAsync(this DbContext context)
        {
            if (context == null)
            {
                traceSource.TraceError($"{nameof(DbContextExtensions)}.{nameof(SaveDbContextChangesAsync)}(): the expected context is not here.");
                return;
            }

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbEntityValidationException ex)
            {
                traceSource.TraceError(ex);

                Action<DbValidationError> handleValidationError = i =>
                {
                    var errorMessage = string.Format("PropertyName: {0}, ErrorMessage: {1}", i.PropertyName, i.ErrorMessage);
                    traceSource.TraceError(errorMessage);
                };

                ex.EntityValidationErrors.ForEachInEnumerable(i => i.ValidationErrors.ForEachInEnumerable(handleValidationError));
            }
        }
    }
}
