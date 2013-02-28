using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
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

        /// <summary>
        /// Customize model creation.
        /// </summary>
        /// <param name="modelBuilder">
        /// The <see cref="DbModelBuilder"/> used to configure the model.
        /// </param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Contact>().Property(c => c.Name).IsRequired();
            modelBuilder.Entity<Contact>().Property(c => c.Email).IsOptional();
            modelBuilder.Entity<Contact>().Property(c => c.Notes).IsOptional();
            modelBuilder.Entity<Contact>().Property(c => c.Organization).IsOptional();
            modelBuilder.Entity<Contact>().Property(c => c.Phone).IsOptional();
            modelBuilder.Entity<Contact>().Property(c => c.Role).IsOptional();
            modelBuilder.Entity<Contact>().HasKey(c => c.Id);

            modelBuilder.Entity<Activity>().HasRequired(a => a.Contact)
                .WithOptional();
            modelBuilder.Entity<Activity>().Property(a => a.Completed).IsRequired();
            modelBuilder.Entity<Activity>().Property(a => a.Description).IsOptional();
            modelBuilder.Entity<Activity>().Property(a => a.Duration).IsRequired();
            modelBuilder.Entity<Activity>().Property(a => a.Start).IsRequired();
            modelBuilder.Entity<Activity>().HasKey(a => a.Id);

            modelBuilder.Entity<JobOpening>().Property(jo => jo.Url).HasColumnType("nvarchar(max)");
            modelBuilder.Entity<JobOpening>().Property(jo => jo.Title).IsRequired();
            modelBuilder.Entity<JobOpening>().Property(jo => jo.Organization).IsRequired();
            modelBuilder.Entity<JobOpening>().Property(jo => jo.Notes).IsOptional();
            modelBuilder.Entity<JobOpening>().Property(jo => jo.AdvertisedDate).IsRequired();
            modelBuilder.Entity<JobOpening>().HasMany(jo => jo.Activities)
                .WithOptional();
            modelBuilder.Entity<JobOpening>().HasMany(jo => jo.AdditionalContacts)
                .WithOptional();
            modelBuilder.Entity<JobOpening>().HasKey(jo => jo.Id);
        }
    }
}
