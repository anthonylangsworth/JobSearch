using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSearch.Serialization
{
    /// <summary>
    /// A <see cref="DbContext"/> class (Entity Framework) for the JobSearch assembly.
    /// </summary>
    public class JobSearchContext: DbContext
    {
        /// <summary>
        /// <see cref="Contact"/>s.
        /// </summary>
        public DbSet<Contact> Contacts { get; set; }

        /// <summary>
        /// <see cref="Activity">Activities</see>.
        /// </summary>
        public DbSet<Activity> Activities { get; set; }

        /// <summary>
        /// <see cref="JobOpening"/>s.
        /// </summary>
        public DbSet<JobOpening> JobOpenings { get; set; }
    }
}
