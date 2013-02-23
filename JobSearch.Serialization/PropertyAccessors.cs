using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSearch.Serialization
{
    /// <summary>
    /// Whether the requested property should have a 'get' (read) or
    /// 'set' (write) accessor.
    /// </summary>
    /// <seealso cref="EntityFrameworkRepositoryHelper"/>
    [Flags]
    public enum PropertyAccessors
    {
        /// <summary>
        /// 'get'.
        /// </summary>
        CanRead = 1,
        /// <summary>
        /// 'set'.
        /// </summary>
        CanWrite = 2
    }
}
