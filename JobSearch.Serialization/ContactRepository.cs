using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobSearch.Interfaces;

namespace JobSearch.Serialization
{
    /// <summary>
    /// A repository for <see cref="Contact"/>.
    /// </summary>
    public class ContactRepository: EntityFrameworkRepository<JobSearchContext, int, Contact>
    {
        // No members
    }
}
