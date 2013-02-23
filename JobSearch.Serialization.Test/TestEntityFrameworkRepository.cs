using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using JobSearch.Test;
using JobSearch.Interfaces;
using NUnit.Framework;

namespace JobSearch.Serialization.Test
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <typeparam name="TId"></typeparam>
    /// <typeparam name="TItem"></typeparam>
    public class TestEntityFrameworkRepository<TDbContext, TId, TItem>
        where TDbContext : DbContext, new()
        where TItem : class, IEquatable<TItem>
    {
        /// <summary>
        /// Create a new <see cref="TestEntityFrameworkRepository{TDbContext, TId, TItem}"/>.
        /// </summary>
        /// <param name="testItem1">
        /// </param>
        /// <param name="testItem2">
        /// </param>
        /// <param name="identifyingProperty">
        /// 
        /// </param>
        /// <param name="uniqueProperty">
        /// </param>
        /// <param name="varyItem">
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// No argument can be null.
        /// </exception>
        protected TestEntityFrameworkRepository(TItem testItem1, TItem testItem2, 
            Expression<Func<TItem, TId>> identifyingProperty,
            Expression<Func<TItem, object>> uniqueProperty,
            Action<TItem> varyItem)
        {
            if (testItem1 == null)
            {
                throw new ArgumentNullException("testItem1");
            }
            if (testItem2 == null)
            {
                throw new ArgumentNullException("testItem2");
            }
            if (identifyingProperty == null)
            {
                throw new ArgumentNullException("identifyingProperty");
            }
            if (uniqueProperty == null)
            {
                throw new ArgumentNullException("uniqueProperty");
            }
            if (varyItem == null)
            {
                throw new ArgumentNullException("varyItem");
            }

            try
            {
                IdentifyingPropertyName =
                    EntityFrameworkRepositoryTestHelper.GetPropertyName(identifyingProperty,
                    PropertyAccessors.CanRead);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message, "identifyingProperty", ex);
            }

            try
            {
                UniquePropertyName =
                    EntityFrameworkRepositoryTestHelper.GetPropertyName(uniqueProperty,
                    PropertyAccessors.CanRead | PropertyAccessors.CanWrite);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message, "uniqueProperty", ex);
            }

            TestItem1 = testItem1;
            TestItem2 = testItem2;
            Vary = varyItem;
        }

        /// <summary>
        /// A test <typeparamref name="TItem"/>.
        /// </summary>
        public TItem TestItem1
        {
            get; 
            private set;
        }

        /// <summary>
        /// A test <typeparamref name="TItem"/>.
        /// </summary>
        public TItem TestItem2
        {
            get; 
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string IdentifyingPropertyName
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string UniquePropertyName
        {
            get; 
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public Action<TItem> Vary
        {
            get; 
            private set;
        }



        [Test]
        public void TestCreation()
        {
            Assert.DoesNotThrow(() => new EntityFrameworkRepository<TDbContext, TId, TItem>());
        }

        [Test]
        public void TestCreation_Argument()
        {
            EntityFrameworkRepository<TDbContext, TId, TItem> repository;
            TDbContext context;

            using (context = new TDbContext())
            using (repository = new EntityFrameworkRepository<TDbContext, TId, TItem>(context))
            {
                Assert.That(repository.DbContext, Is.EqualTo(context), "Incorrect DbContext");
                Assert.That(repository.Dirty, Is.False, "Incorrect Dirty flag");
                Assert.That(repository.GetItemId, Is.Not.Null, "GetItemID is null");
                Assert.That(repository.GetItemDbSet, Is.Not.Null, "GetItemDbSet is null");
            }
        }

        [Test]
        public void TestGetItemId()
        {
            EntityFrameworkRepository<TDbContext, TId, TItem> repository;

            using (repository = new EntityFrameworkRepository<TDbContext, TId, TItem>())
            {
                Assert.That(repository.GetItemId(TestItem2),
                    Is.EqualTo(repository.GetItemId(TestItem2)));
            }
        }

        [Test]
        public void TestGetItemDbSet()
        {
            EntityFrameworkRepository<TDbContext, TId, TItem> repository;

            using (repository = new EntityFrameworkRepository<TDbContext, TId, TItem>())
            {
                Assert.That(repository.GetItemDbSet(), Is.Not.Null);
            }
        }

        [Test]
        public void TestCreate()
        {
            TItem first;
            IEnumerable<PropertyInfo> properties;

            using (EntityFrameworkRepository<TDbContext, TId, TItem> repository = new EntityFrameworkRepository<TDbContext, TId, TItem>())
            using (RepositoryWiper<TId, TItem> wiper 
                = new RepositoryWiper<TId, TItem>(repository, repository.GetItemId))
            {
                wiper.Wipe();

                Assert.That(repository.Exists(repository.GetItemId(TestItem1)), Is.False,
                            "Test contact exists");
                repository.Create(TestItem1);
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
                                Is.EqualTo(property.GetMethod.Invoke(TestItem1, new object[0])),
                                string.Format("Incorrect {0}", property.Name));
                }
            }
        }

        [Test]
        public void TestCreate_Null()
        {
            using (EntityFrameworkRepository<TDbContext, TId, TItem> repository = new EntityFrameworkRepository<TDbContext, TId, TItem>())
            using (new RepositoryWiper<TId, TItem>(repository, repository.GetItemId))
            {
                Assert.That(() => repository.Create(null), 
                    Throws.TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("item"));
            }
        }

        [Test]
        public void TestUpdate()
        {
            TItem updatedContact;

            using (EntityFrameworkRepository<TDbContext, TId, TItem> repository = new EntityFrameworkRepository<TDbContext, TId, TItem>())
            using (RepositoryWiper<TId, TItem> wiper = 
                new RepositoryWiper<TId, TItem>(repository, repository.GetItemId))
            {
                wiper.Wipe();

                repository.Create(TestItem1);
                repository.Save();

                updatedContact = repository.GetAll().First(EntityFrameworkRepositoryHelper.GetIdMatchesExpression(TId, ));
                updatedContact.Notes = "Completely different notes.";
                updatedContact.Phone = "Do not dial this.";
                updatedContact.Organization = "Completely different organization.";
                repository.Update(updatedContact);
                repository.Save();

                Assert.That(updatedContact, Is.EqualTo(repository.Get(repository.GetItemId(updatedContact))));
            }
        }

        [Test]
        public void TestUpdate_EmptyRepository()
        {
            EntityFrameworkRepository<TDbContext, TId, TItem> repository;

            using (repository = new EntityFrameworkRepository<TDbContext, TId, TItem>())
            using (new RepositoryWiper<TId, TItem>(repository, repository.GetItemId))
            {
                Assert.That(repository.Exists(repository.GetItemId(TestItem1)), Is.False,
                            "Test contact exists");
                Assert.That(() => repository.Update(TestItem1),
                            Throws.ArgumentException.And.Property("ParamName").EqualTo("item"));
            }
        }

        [Test]
        public void TestUpdate_Null()
        {
            using (EntityFrameworkRepository<TDbContext, TId, TItem> repository = new EntityFrameworkRepository<TDbContext, TId, TItem>())
            using (new RepositoryWiper<TId, TItem>(repository, repository.GetItemId))
            {
                Assert.That(() => repository.Update(null),
                    Throws.TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("item"));
            }
        }

        [Test]
        public void TestDelete()
        {
            using (EntityFrameworkRepository<TDbContext, TId, TItem> repository = new EntityFrameworkRepository<TDbContext, TId, TItem>())
            using (new RepositoryWiper<TId, TItem>(repository, repository.GetItemId))
            {
                Assert.That(repository.Exists(repository.GetItemId(TestItem1)), Is.False,
                            "Test contact exists");
                repository.Create(TestItem1);
                Assert.That(repository.Dirty, Is.True,
                            "Not dirty");
                repository.Save();
                Assert.That(repository.Dirty, Is.False,
                            "Dirty");
                Assert.That(repository.GetAll().Count(), Is.EqualTo(1),
                            "Incorrect count");
                repository.Delete(repository.GetAll().First(c => c.Name == TestItem1.Name).Id);
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
            using (EntityFrameworkRepository<TDbContext, TId, TItem> repository = new EntityFrameworkRepository<TDbContext, TId, TItem>())
            using (new RepositoryWiper<TId, TItem>(repository, repository.GetItemId))
            {
                Assert.That(repository.Exists(repository.GetItemId(TestItem1)), Is.False,
                            "Test contact exists");
                Assert.That(() => repository.Delete(repository.GetItemId(TestItem1)),
                    Throws.ArgumentException.And.Property("ParamName").EqualTo("id"));
            }
        }

        [Test]
        public void TestDelete_DoesNotExist()
        {
            TId id;

            using (EntityFrameworkRepository<TDbContext, TId, TItem> repository = new EntityFrameworkRepository<TDbContext, TId, TItem>())
            using (new RepositoryWiper<TId, TItem>(repository, repository.GetItemId))
            {
                repository.Create(TestItem1);
                repository.Save();

                Assert.That(repository.GetAll().Any(c => c.Name == TestItem1.Name),
                    Is.True, "Test contact does not exist");
                Assert.That(!repository.GetAll().Any(c => c.Name == TestItem2.Name), 
                    Is.True, "Test contact exists");

                id = repository.GetAll().First(c => c.Name == TestItem1.Name).Id + 1; 
                Assert.That(() => repository.Delete(id),
                    Throws.ArgumentException.And.Property("ParamName").EqualTo("id"));
            }
        }
    }
}

