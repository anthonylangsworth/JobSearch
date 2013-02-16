using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSearch.Interfaces
{
    /// <summary>
    /// An <see cref="Contact"/> repository.
    /// </summary>
    public interface IContactRepository: IRepository<int, Contact>
    {
        // No members
    }
}
