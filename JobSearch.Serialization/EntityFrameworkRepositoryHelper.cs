using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JobSearch.Serialization
{
    /// <summary>
    /// Helper methods for <see cref="EntityFrameworkRepository{TDbContext,TId,TItem,TInterface}"/>.
    /// </summary>
    internal static class EntityFrameworkRepositoryHelper
    {
        /// <summary>
        /// Given a <see cref="DbContext"/> (usually a derived or subclass),
        /// find a public instance property that returns a <see cref="DbSet{TItem}"/> and
        /// return a <see cref="Func{T}"/> that returns the result.
        /// </summary>
        /// <typeparam name="TItem">
        /// The type of the property to look for.
        /// </typeparam>
        /// <param name="dbContext">
        /// The <see cref="TDbContext"/> (usually a derived or subclass of TDbContext)
        /// tp examine.
        /// </param>
        /// <returns>
        /// A <see cref="Func{T}"/> that returns a <see cref="DbSet{TItem}"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="dbContext"/> cannot be null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Zero or multiple properties exist on <see cref="dbContext"/>
        /// </exception>
        internal static Func<DbSet<TItem>> GetDbSet<TDbContext, TItem>(TDbContext dbContext)
            where TDbContext : DbContext
            where TItem: class
        {
            Contract.Requires<ArgumentNullException>(dbContext != null);
            Contract.Ensures(Contract.Result<Func<DbSet<TItem>>>() != null);

            IEnumerable<MethodInfo> propertyGetMethods;

            propertyGetMethods = dbContext.GetType()
                                         .GetProperties(BindingFlags.GetProperty | BindingFlags.Public |
                                                        BindingFlags.Instance)
                                         .Select(pi => pi.GetGetMethod())
                                         .Where(gm => gm.ReturnType == typeof (DbSet<TItem>));
            if (!propertyGetMethods.Any())
            {
                throw new InvalidOperationException(
                    string.Format("No property found of type DbSet<{0}> on {1}",
                                  typeof(TItem).Name, dbContext.GetType().Name));
            }
            else if (propertyGetMethods.Count() > 1)
            {
                throw new InvalidOperationException(
                    string.Format("Multiple properties found of type DbSet<{0}> on {1}",
                                  typeof(TItem).Name, dbContext.GetType().Name));
            }

            return () => (DbSet<TItem>) propertyGetMethods.First().Invoke(dbContext, new object[0]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TId"></typeparam>
        /// <returns></returns>
        internal static Func<TItem, TId> GetId<TItem, TId>(string propertyName = "Id")
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(propertyName));
            Contract.Ensures(Contract.Result<Func<TItem, TId>>() != null);

            IEnumerable<MethodInfo> idPropertyGetMethods;

            idPropertyGetMethods = typeof (TItem)
                .GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance)
                .Where(pi => pi.Name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase))
                .Select(pi => pi.GetGetMethod())
                .Where(gm => gm.ReturnType == typeof (TId));
            if (!idPropertyGetMethods.Any())
            {
                throw new InvalidOperationException(
                    string.Format("No property called '{0}' found of type '{1}' on type '{2}'",
                                  propertyName, typeof(TId).Name, typeof(TItem).Name));
            }
            else if (idPropertyGetMethods.Count() > 1)
            {
                throw new InvalidOperationException(
                    string.Format("Multiple properties called '{0}' found of type '{1}' on type '{2}'",
                                  propertyName, typeof(TId).Name, typeof(TItem).Name));
            }

            return item => (TId)idPropertyGetMethods.First().Invoke(item, new object[0]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TId"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        internal static Expression<Func<TItem, bool>> GetExistsExpression<TItem, TId>(TId id, string propertyName)
        {
            ParameterExpression parameter;

            // Must be same instance (e.g. http://msdn.microsoft.com/en-us/library/bb882637.aspx)
            parameter = Expression.Parameter(typeof (TItem), "item");

            return 
                Expression.Lambda<Func<TItem, bool>>(
                    Expression.Equal(
                        Expression.MakeMemberAccess(
                            parameter, 
                            typeof(TItem).GetProperty(propertyName)),
                        Expression.Constant(id)),
                    new[] { parameter });
        }
    }
}
