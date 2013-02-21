using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSearch.Serialization
{
    /// <summary>
    /// A repository for <see cref="JobOpening"/>.
    /// </summary>
    public class JobOpeningRepository : EntityFrameworkRepository<JobSearchContext, int, Contact>
    {
        // No members
    }
}
