using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSearch.Serialization
{
    /// <summary>
    /// A repository for <see cref="Activity"/>.
    /// </summary>
    public class ActivityRepository : EntityFrameworkRepository<JobSearchContext, int, Contact>
    {
        // No members
    }
}
