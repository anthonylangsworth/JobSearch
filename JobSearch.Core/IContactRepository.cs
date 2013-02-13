using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSearch.Core
{
    /// <summary>
    /// An <see cref="IContact"/> repository.
    /// </summary>
    public interface IContactRepository: IRepository<int, IContact>
    {
        // No members
    }
}
