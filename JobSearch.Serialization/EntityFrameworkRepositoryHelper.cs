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
                    String.Format("No property found of type DbSet<{0}> on {1}",
                                  typeof(TItem).Name, dbContext.GetType().Name));
            }
            else if (propertyGetMethods.Count() > 1)
            {
                throw new InvalidOperationException(
                    String.Format("Multiple properties found of type DbSet<{0}> on {1}",
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
            if (String.IsNullOrWhiteSpace(propertyName))
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
                    String.Format("No property called '{0}' found of type '{1}' on type '{2}'",
                                  propertyName, typeof(TId).Name, typeof(TItem).Name));
            }
            else if (idPropertyGetMethods.Count() > 1)
            {
                throw new InvalidOperationException(
                    String.Format("Multiple properties called '{0}' found of type '{1}' on type '{2}'",
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
        /// <typeparam name="TProperty">
        /// The property type.
        /// </typeparam>
        /// <param name="expectedValue">
        /// The value the property should match.
        /// </param>
        /// <param name="propertyName">
        /// The name of the property to match. This cannot be null, empty or whitespace.
        /// </param>
        /// <returns>
        /// An expression that can be used in a LINQ query.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="propertyName"/> cannot be null, empty or whitespace.
        /// ></exception>
        internal static Expression<Func<TItem, bool>> GetMatchesExpression<TItem, TProperty>(
            TProperty expectedValue, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
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
                        Expression.Constant(expectedValue)),
                    new[] { parameter });
        }

        /// <summary>
        /// Return an <see cref="Expression"/> that can be used in a LINQ to entities
        /// query that matches the property<paramref name="propertyName"/> on an item
        /// of type <typeparamref name="TItem"/>.
        /// </summary>
        /// <typeparam name="TItem">
        /// The type of item the property is called on.
        /// </typeparam>
        /// <param name="item">
        /// The item whose property should be matched.
        /// </param>
        /// <param name="propertyName">
        /// The name of the property to match. This cannot be null, empty or whitespace.
        /// </param>
        /// <returns>
        /// An expression that can be used in a LINQ query.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// No argument can be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="propertyName"/> is not a property on <typeparamref name="TItem"/>
        /// or there is no 'get' accessor.
        /// </exception>
        internal static Expression<Func<TItem, bool>> GetMatchesExpression<TItem>(
            TItem item, string propertyName)
            where TItem: class
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentNullException("propertyName");
            }
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            if (typeof (TItem).GetProperty(propertyName) == null
                || !typeof (TItem).GetProperty(propertyName).CanRead)
            {
                throw new ArgumentException(
                    string.Format("'{0}' is not a property on '{1}' or lacks a 'get' accessor", 
                        propertyName, typeof(TItem).FullName),
                    "propertyName");
            }

            ParameterExpression parameter;
            object expectedValue;

            expectedValue = typeof(TItem).GetProperty(propertyName).GetMethod.Invoke(item, new object[0]);

            // Must be same instance (e.g. http://msdn.microsoft.com/en-us/library/bb882637.aspx)
            parameter = Expression.Parameter(typeof(TItem), "item");

            return
                Expression.Lambda<Func<TItem, bool>>(
                    Expression.Equal(
                        Expression.MakeMemberAccess(
                            parameter,
                            typeof(TItem).GetProperty(propertyName)),
                        Expression.Constant(expectedValue)),
                    new[] { parameter });
        }

        /// <summary>
        /// Does the given <paramref name="expression"/> contain a lambda experssion
        /// referring to a property (only)?
        /// </summary>
        /// <param name="expression">
        /// The <see cref="Expression"/> to test.
        /// </param>
        /// <param name="accessors">
        /// The property name.
        /// </param>
        /// <returns>
        /// True if it is a property, false otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="expression"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Either:
        /// <list type="bullet">
        ///     <item>
        ///         <description>
        ///             <paramref name="expression"/> is not a <see cref="LambdaExpression"/>.
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <paramref name="expression"/>'s body does not containly only a member access.
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             <paramref name="expression"/>'s body calls a member that is not a property.
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             The property called in <paramref name="expression"/> lacks a get or set
        ///             accessor as requested by <paramref name="accessors"/>.
        ///         </description>
        ///     </item>
        /// </list>
        /// </exception>
        public static string GetPropertyName<TItem, TId>(Expression<Func<TItem, TId>> expression, PropertyAccessors accessors)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            LambdaExpression lambdaExpression;
            MemberExpression memberExpression;
            PropertyInfo propertyInfo;
            string result;

            lambdaExpression = expression as LambdaExpression;
            if (expression != null)
            {
                memberExpression = expression.Body as MemberExpression;
                if (memberExpression != null)
                {
                    propertyInfo = memberExpression.Member as PropertyInfo;
                    if (propertyInfo != null)
                    {
                        if ((accessors & PropertyAccessors.CanRead) == PropertyAccessors.CanRead
                            && !propertyInfo.CanRead)
                        {
                            throw new ArgumentException(
                                String.Format("No 'get' accessor on property '{0}'", propertyInfo.Name),
                                "expression");
                        }

                        if ((accessors & PropertyAccessors.CanWrite) == PropertyAccessors.CanWrite
                            && !propertyInfo.CanWrite)
                        {
                            throw new ArgumentException(
                                String.Format("No 'set' accessor on property '{0}'", propertyInfo.Name),
                                "expression");
                        }

                        result = propertyInfo.Name;
                    }
                    else
                    {
                        throw new ArgumentException(
                            String.Format("Member '{0}' is not a property", memberExpression.Member.Name),
                            "expression");
                    }
                }
                else
                {
                    throw new ArgumentException("Not a member access (e.g. property or method call)", "expression");
                }
            }
            else
            {
                throw new ArgumentException("Not a LambdaExpression", "expression");
            }

            return result;
        }
    }
}
