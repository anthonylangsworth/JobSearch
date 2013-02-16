using System;
using System.Data.Entity;
using NUnit.Framework;

namespace JobSearch.Serialization.Test
{
    [TestFixture]
    public class TestEntityFrameworkRepository
    {
        [Test]
        public void TestCreation()
        {
            Assert.DoesNotThrow(() => new EntityFrameworkRepository<JobSearchContext, int, Contact>());
        }
    }
}
