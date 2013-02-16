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
            EntityFrameworkRepository<JobSearch, int, string> repository;

            repository = new EntityFrameworkRepository<JobSearch, int, string>();
        }
    }
}
