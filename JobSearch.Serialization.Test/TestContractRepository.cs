using System;
using System.Data.Entity;
using System.Linq;
using JobSearch.Test;
using NUnit.Framework;

namespace JobSearch.Serialization.Test
{
    [TestFixture]
    public class TestContractRepository
    {
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<JobSearchContext>());
        }

        [Test]
        public void TestCreation()
        {
            Assert.DoesNotThrow(() => new EntityFrameworkRepository<JobSearchContext, int, Contact>());
        }

        [Test]
        public void TestCreation_Argument()
        {
            EntityFrameworkRepository<JobSearchContext, int, Contact> repository;
            JobSearchContext jobSearchContext;

            jobSearchContext = new JobSearchContext();
            using (repository = new EntityFrameworkRepository<JobSearchContext, int, Contact>(jobSearchContext))
            {
                Assert.That(repository.DbContext, Is.EqualTo(jobSearchContext), "Incorrect DbContext");
                Assert.That(repository.Dirty, Is.False, "Incorrect Dirty flag");
                Assert.That(repository.GetItemId, Is.Not.Null, "GetItemID is null");
                Assert.That(repository.GetItemDbSet, Is.Not.Null, "GetItemDbSet is null");
            }
        }

        [Test]
        public void TestGetItemId()
        {
            EntityFrameworkRepository<JobSearchContext, int, Contact> repository;

            using (repository = new EntityFrameworkRepository<JobSearchContext, int, Contact>())
            {
                Assert.That(repository.GetItemId(TestContacts.PeterSmith), 
                    Is.EqualTo(TestContacts.PeterSmith.Id));
            }
        }

        [Test]
        public void TestGetItemDbSet()
        {
            EntityFrameworkRepository<JobSearchContext, int, Contact> repository;
            // DbSet<Contact> contacts;

            using (repository = new EntityFrameworkRepository<JobSearchContext, int, Contact>())
            {
                Assert.That(repository.GetItemDbSet(), Is.Not.Null);
            }
        }

        [Test]
        public void TestCreate()
        {
            EntityFrameworkRepository<JobSearchContext, int, Contact> repository;

            using (repository = new EntityFrameworkRepository<JobSearchContext, int, Contact>())
            {
                Assert.That(repository.Exists(TestContacts.SarahBillingsley.Id), Is.False, 
                    "Test contact exists");
                repository.Create(TestContacts.SarahBillingsley);
                Assert.That(repository.Exists(TestContacts.SarahBillingsley.Id), Is.True, 
                    "Test contact does not exist");
                Assert.That(repository.Get(TestContacts.SarahBillingsley.Id), Is.EqualTo(TestContacts.SarahBillingsley), 
                    "Incorrect Get result");
                Assert.That(repository.GetAll(), Is.EquivalentTo(new[] { TestContacts.SarahBillingsley }),
                    "Incorrect Get result");
                Assert.That(repository.Dirty, Is.True,
                    "Not dirty");
            }
        }
    }
}
