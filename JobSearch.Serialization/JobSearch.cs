using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobSearch.Core;

namespace JobSearch.Serialization
{
    public class JobSearch: DbContext
    {
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<JobOpening> JobOpenings { get; set; }
    }
}
