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
    /// Helper methods for <see cref="EntityFrameworkRepository{TDbContext,TId,TItem}"/>.
    /// </summary>
    internal static class EntityFrameworkRepositoryHelper
    {
        /// <summary>
        /// Given a <see cref="DbContext"/> (usually a derived or subclass),
        /// find a public instance property that returns a <see cref="DbSet{TItem}"/> and
        /// return a <see cref="Func{T}"/> that returns the result.
        /// </summary>
        /// <typeparam name="TDbContext">
        /// The type of <see cref="DbContext"/> used.
        /// </typeparam>
        /// <typeparam name="TItem">
        /// The type of the property to look for.
        /// </typeparam>
        /// <param name="dbContext">
        /// The <typeparamref name="TDbContext"/> (usually a derived or subclass of TDbContext)
        /// tp examine.
        /// </param>
        /// <returns>
        /// A <see cref="Func{T}"/> that returns a <see cref="DbSet{TItem}"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="dbContext"/> cannot be null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Zero or multiple matching properties exist on <paramref name="dbContext"/>.
        /// </exception>
        internal static Func<DbSet<TItem>> GetDbSet<TDbContext, TItem>(TDbContext dbContext)
            where TDbContext : DbContext
            where TItem: class
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException("dbContext");
            }

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
        /// Given a <paramref name="propertyName"/> of type <typeparamref name="TId"/>, 
        /// construct a <see cref="Func{TItem, TId}"/> that returns the value of property
        /// on an object of type <typeparamref name="TItem"/>.
        /// </summary>
        /// <typeparam name="TItem">
        /// The type of item the ID will be extracted from.
        /// </typeparam>
        /// <typeparam name="TId">
        /// The type of the ID.
        /// </typeparam>
        /// <param name="propertyName">
        /// Optional property name. This cannot be null, empty or whitespace.
        /// </param>
        /// <returns>
        /// A <see cref="Func{TItem, TId}"/> that returns the ID.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// 
        /// </exception>
        internal static Func<TItem, TId> GetId<TItem, TId>(string propertyName = "Id")
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentNullException("propertyName");
            }

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
        /// Return an <see cref="Expression"/> that can be used in a LINQ to entities
        /// query that matches the property<paramref name="propertyName"/> on an item
        /// of type <typeparamref name="TItem"/>.
        /// </summary>
        /// <typeparam name="TItem">
        /// The type of item the property is called on.
        /// </typeparam>
        /// <typeparam name="TId">
        /// The property type.
        /// </typeparam>
        /// <param name="id">
        /// The ID to match.
        /// </param>
        /// <param name="propertyName">
        /// The name of the property to match. This cannot be null, empty or whitespace.
        /// </param>
        /// <returns>
        /// An expression that can be used in a LINQ query.
        /// </returns>
        internal static Expression<Func<TItem, bool>> GetIdMatchesExpression<TItem, TId>(TId id, string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException("propertyName");                
            }

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
