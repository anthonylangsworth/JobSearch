using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace JobSearch.Serialization
{
    /// <summary>
    /// Container class for a <see cref="TestFixtureSetup"/>.
    /// </summary>
    [TestFixture]
    public class TestFixtureSetup
    {
        /// <summary>
        /// Run once before tests start to setup the database.
        /// </summary>
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<JobSearchContext>());
            // Database.SetInitializer(new DropCreateDatabaseAlways<JobSearchContext>());
            // Database.SetInitializer(new CreateDatabaseIfNotExists<JobSearchContext>());
        }
    }
}
