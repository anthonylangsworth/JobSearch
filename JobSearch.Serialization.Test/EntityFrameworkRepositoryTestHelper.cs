using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JobSearch.Serialization.Test
{
    [Flags]
    public enum PropertyAccessors
    {
        CanRead = 1,
        CanWrite = 2
    }

    /// <summary>
    /// Helper methods for <see cref="TestEntityFrameworkRepository{TDbContext, TId, TItem}"/>.
    /// </summary>
    public static class EntityFrameworkRepositoryTestHelper
    {
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
        public static string GetPropertyName<TItem, TId>(Expression<Func<TItem, TId>> expression, EntityFrameworkRepositoryHelper.PropertyAccessors accessors)
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
                        if ((accessors & EntityFrameworkRepositoryHelper.PropertyAccessors.CanRead) == EntityFrameworkRepositoryHelper.PropertyAccessors.CanRead
                            && !propertyInfo.CanRead)
                        {
                            throw new ArgumentException(
                                string.Format("No 'get' accessor on property '{0}'", propertyInfo.Name),
                                "expression");
                        }

                        if ((accessors & EntityFrameworkRepositoryHelper.PropertyAccessors.CanWrite) == EntityFrameworkRepositoryHelper.PropertyAccessors.CanWrite
                            && !propertyInfo.CanWrite)
                        {
                            throw new ArgumentException(
                                string.Format("No 'set' accessor on property '{0}'", propertyInfo.Name),
                                "expression");
                        }

                        result = propertyInfo.Name;
                    }
                    else
                    {
                        throw new ArgumentException(
                            string.Format("Member '{0}' is not a property", memberExpression.Member.Name),
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
