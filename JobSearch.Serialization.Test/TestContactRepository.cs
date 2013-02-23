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
    public class TestContactRepository
    {
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
            using (RepositoryWiper<int, Contact> wiper 
                = new RepositoryWiper<int, Contact>(repository, repository.GetItemId))
            {
                wiper.Wipe();

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
        public void TestUpdate()
        {
            EntityFrameworkRepository<JobSearchContext, int, Contact> repository;
            Contact updatedContact;

            using (repository = new EntityFrameworkRepository<JobSearchContext, int, Contact>())
            using (RepositoryWiper<int, Contact> wiper = 
                new RepositoryWiper<int, Contact>(repository, repository.GetItemId))
            {
                wiper.Wipe();

                repository.Create(TestContacts.SarahBillingsley);
                repository.Save();

                updatedContact = repository.GetAll().First(c => c.Name == TestContacts.SarahBillingsley.Name);
                updatedContact.Notes = "Completely different notes.";
                updatedContact.Phone = "Do not dial this.";
                updatedContact.Organization = "Completely different organization.";
                repository.Update(updatedContact);
                repository.Save();

                Assert.That(updatedContact, Is.EqualTo(repository.Get(updatedContact.Id)));
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

        [Test]
        public void TestDelete()
        {
            EntityFrameworkRepository<JobSearchContext, int, Contact> repository;

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
                            "Dirty");
                Assert.That(repository.GetAll().Count(), Is.EqualTo(1),
                            "Incorrect count");
                repository.Delete(repository.GetAll().First(c => c.Name == TestContacts.SarahBillingsley.Name).Id);
                Assert.That(repository.Dirty, Is.True,
                            "Not dirty");
                repository.Save();
                Assert.That(repository.Dirty, Is.False,
                            "Dirty");
                Assert.That(repository.GetAll().Count(), Is.EqualTo(0),
                            "Incorrect count");
            }
        }

        [Test]
        public void TestDelete_Empty()
        {
            EntityFrameworkRepository<JobSearchContext, int, Contact> repository;

            using (repository = new EntityFrameworkRepository<JobSearchContext, int, Contact>())
            using (new RepositoryWiper<int, Contact>(repository, repository.GetItemId))
            {
                Assert.That(repository.Exists(TestContacts.SarahBillingsley.Id), Is.False,
                            "Test contact exists");
                Assert.That(() => repository.Delete(TestContacts.SarahBillingsley.Id),
                    Throws.ArgumentException.And.Property("ParamName").EqualTo("id"));
            }
        }

        [Test]
        public void TestDelete_DoesNotExist()
        {
            EntityFrameworkRepository<JobSearchContext, int, Contact> repository;
            int id;

            using (repository = new EntityFrameworkRepository<JobSearchContext, int, Contact>())
            using (new RepositoryWiper<int, Contact>(repository, repository.GetItemId))
            {
                repository.Create(TestContacts.SarahBillingsley);
                repository.Save();

                Assert.That(repository.GetAll().Any(c => c.Name == TestContacts.SarahBillingsley.Name),
                    Is.True, "Test contact does not exist");
                Assert.That(!repository.GetAll().Any(c => c.Name == TestContacts.PeterSmith.Name), 
                    Is.True, "Test contact exists");

                id = repository.GetAll().First(c => c.Name == TestContacts.SarahBillingsley.Name).Id + 1; 
                Assert.That(() => repository.Delete(id),
                    Throws.ArgumentException.And.Property("ParamName").EqualTo("id"));
            }
        }
    }
}

