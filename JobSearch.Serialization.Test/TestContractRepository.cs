using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
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
            // Database.SetInitializer(new DropCreateDatabaseAlways<JobSearchContext>());
            // Database.SetInitializer(new CreateDatabaseIfNotExists<JobSearchContext>());
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
            Contact first;
            IEnumerable<PropertyInfo> properties;

            using (repository = new EntityFrameworkRepository<JobSearchContext, int, Contact>())
            using (new RepositoryWiper<int, Contact>(repository, repository.GetItemId))
            {
                Assert.That(repository.Exists(TestContacts.SarahBillingsley.Id), Is.False,
                            "Test contact exists");
                repository.Create(TestContacts.SarahBillingsley);
                Assert.That(repository.Dirty, Is.True,
                            "Not dirty");
                repository.Save();
                Assert.That(repository.Dirty, Is.False,
                            "Not dirty");
                Assert.That(repository.GetAll().Count(), Is.EqualTo(1),
                            "Incorrect count");

                // Ensure the element matches, ignoring the ID
                first = repository.GetAll().First();
                properties = first.GetType().GetProperties().Where(pi => pi.Name != "Id");
                foreach (PropertyInfo property in properties)
                {
                    Assert.That(property.GetMethod.Invoke(first, new object[0]),
                                Is.EqualTo(property.GetMethod.Invoke(TestContacts.SarahBillingsley, new object[0])),
                                string.Format("Incorrect {0}", property.Name));
                }
            }
        }

        [Test]
        public void TestCreate_Null()
        {
            EntityFrameworkRepository<JobSearchContext, int, Contact> repository;

            using (repository = new EntityFrameworkRepository<JobSearchContext, int, Contact>())
            using (new RepositoryWiper<int, Contact>(repository, repository.GetItemId))
            {
                Assert.That(() => repository.Create(null), 
                    Throws.TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("item"));
            }
        }

        [Test]
        public void TestUpdate_EmptyRepository()
        {
            EntityFrameworkRepository<JobSearchContext, int, Contact> repository;
            // Contact first;
            // IEnumerable<PropertyInfo> properties;

            using (repository = new EntityFrameworkRepository<JobSearchContext, int, Contact>())
            using (new RepositoryWiper<int, Contact>(repository, repository.GetItemId))
            {
                Assert.That(repository.Exists(TestContacts.SarahBillingsley.Id), Is.False,
                            "Test contact exists");
                Assert.That(() => repository.Update(TestContacts.SarahBillingsley),
                            Throws.ArgumentException.And.Property("ParamName").EqualTo("item"));
            }
        }

        [Test]
        public void TestUpdate_Null()
        {
            EntityFrameworkRepository<JobSearchContext, int, Contact> repository;

            using (repository = new EntityFrameworkRepository<JobSearchContext, int, Contact>())
            using (new RepositoryWiper<int, Contact>(repository, repository.GetItemId))
            {
                Assert.That(() => repository.Update(null),
                    Throws.TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("item"));
            }
        }
    }
}

