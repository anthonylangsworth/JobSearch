using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using JobSearch.Interfaces;

namespace JobSearch.Serialization
{
    /// <summary>
    /// A repository class for <typeparamref name="TItem"/>, which has a unique
    /// identifier of type <typeparamref name="TId"/>.
    /// </summary>
    /// <remarks>
    /// This class is not thread safe.
    /// </remarks>
    /// <typeparam name="TDbContext">
    /// The type of the <see cref="System.Data.Entity.DbContext"/>.
    /// </typeparam>
    /// <typeparam name="TId">
    /// The type of the unique identifier of <typeparamref name="TItem"/>.
    /// </typeparam>
    /// <typeparam name="TItem">
    /// The item type stored in the repository.
    /// </typeparam>
    public class EntityFrameworkRepository<TDbContext, TId, TItem> : IRepository<TId, TItem>
        where TDbContext : DbContext, new()
        where TItem : class, IEquatable<TItem>
    {
        /// <summary>
        /// The property name on TItem of type <typeparamref name="TId "/>that returns a unique
        /// identifier. Future versions should use a list, including additional entries
        /// like typeof(TItem).Name, and store the property chosen in a field or property.
        /// </summary>
        private readonly string propertyName = "Id";

        /// <summary>
        /// Create a new <see cref="EntityFrameworkRepository{TDbContext, TId, TItem}"/>.
        /// </summary>
        /// <param name="dbContext">
        /// The <typeparamref name="TDbContext"/> to use. If null, a new DbContext will be creatd.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// Either a property on <typeparamref name="TDbContext"/> that returns a
        /// <see cref="DbSet{TItem}"/> does not exist or there was no property 
        /// called "Id" on <typeparamref name="TItem"/> of type <typeparamref name="TId"/>.
        /// </exception>
        public EntityFrameworkRepository(TDbContext dbContext = null)
        {
            Contract.Ensures(dbContext == null ? DbContext != null : ReferenceEquals(DbContext, dbContext));
            Contract.Ensures(!Dirty);

            // Add overrides to take in Funcs for getItemDbSet and getItemId
            // later, if needed.

            DbContext = dbContext ?? new TDbContext();
            GetItemDbSet = EntityFrameworkRepositoryHelper.GetDbSet<TDbContext, TItem>(DbContext);
            GetItemId = EntityFrameworkRepositoryHelper.GetId<TItem, TId>(propertyName);
            Dirty = false;
        }

        /// <summary>
        /// Class invariants.
        /// </summary>
        [ContractInvariantMethod]
        private void ClassInariants()
        {
            Contract.Invariant(DbContext != null);
            Contract.Invariant(GetItemDbSet != null);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            DbContext.Dispose();
        }

        /// <summary>
        /// The <typeparamref name="TDbContext"/> used.
        /// </summary>
        public TDbContext DbContext
        {
            get; 
            private set;
        }

        /// <summary>
        /// Delegate to get the <see cref="DbSet{Item}"/> from <see cref="DbContext"/>.
        /// </summary>
        public Func<DbSet<TItem>> GetItemDbSet
        {
            get; 
            private set;
        }

        /// <summary>
        /// Delegate to get the item's unique identifier.
        /// </summary>
        public Func<TItem, TId> GetItemId
        {
            get; 
            private set;
        }

        /// <summary>
        /// Does an item with the given <typeparamref name="TId"/> already exist?
        /// </summary>
        /// <param name="id">
        /// The ID to check.
        /// </param>
        /// <returns>
        /// True if it already exists, false otherwise.
        /// </returns>
        [Pure]
        public bool Exists(TId id)
        {
            // Invoking a lambda or delegate is not allowed within a LINQ to Entities expression
            // Expression<Func<TItem, bool>> expression = item => GetItemId(item).Equals(id);
            // return GetItemDbSet().Any(item => GetItemId(item).Equals(id));

            return GetItemDbSet().Any(EntityFrameworkRepositoryHelper.GetIdMatchesExpression<TItem, TId>(id, propertyName))
                || GetItemDbSet().Local.Any(item => GetItemId(item).Equals(id));
        }

        /// <summary>
        /// Get all items.
        /// </summary>
        /// <returns>
        /// The items in the repository.
        /// </returns>
        public IQueryable<TItem> GetAll()
        {
            return GetItemDbSet().AsQueryable();
        }

        /// <summary>
        /// Retrieve the item identified by <paramref name="id"/>.
        /// </summary>
        /// <param name="id">
        /// The ID of the item to load.
        /// </param>
        /// <returns>
        /// The <typeparamref name="TItem"/> for <paramref name="id"/> or null
        /// if no item matches.
        /// </returns>
        public TItem Get(TId id)
        {
            return GetItemDbSet().FirstOrDefault(item => GetItemId(item).Equals(id));
        }

        /// <summary>
        /// Create a new item.
        /// </summary>
        /// <param name="item">
        /// The item to create.
        /// </param>
        /// <returns>
        /// The ID of the new item (if any).
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="item"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="item"/> already exists or is otherwise invalid.
        /// </exception>
        public void Create(TItem item)
        {
            // Contract.Requires<ArgumentNullException>(item != null, "item");

            GetItemDbSet().Add(item);
            Dirty = true;
        }

        /// <summary>
        /// Update or modify an existing item.
        /// </summary>
        /// <param name="item">
        /// The item to modify. This cannot be null.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="item"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="item"/> already exists or is otherwise invalid.
        /// </exception>
        public void Update(TItem item)
        {
            Delete(GetItemId(item));
            Create(item);
            Dirty = true;
        }

        /// <summary>
        /// Delete item identified by <paramref name="id"/>.
        /// </summary>
        /// <param name="id">
        /// The identifier of the item.
        /// </param>
        /// <exception cref="ArgumentException">
        /// An item identified by <paramref name="id"/> does not exist.
        /// </exception>
        public void Delete(TId id)
        {
            TItem itemToDelete;

            itemToDelete = GetItemDbSet().FirstOrDefault(
                EntityFrameworkRepositoryHelper.GetIdMatchesExpression<TItem, TId>(id, propertyName));
            if (itemToDelete == null)
            {
                // Should not happen due to precondition but just in case
                throw new ArgumentException(
                    string.Format("Item with id '{0}' does not exist", id), "id");
            }

            GetItemDbSet().Remove(itemToDelete);
            Dirty = true;
        }

        /// <summary>
        /// Save changes.
        /// </summary>
        /// <seealso cref="Dirty"/>
        public void Save()
        {
            DbContext.SaveChanges();
            Dirty = false;
        }

        /// <summary>
        /// Are there unsaved changes?
        /// </summary>
        /// <seealso cref="Save"/>
        public bool Dirty
        {
            get;
            private set;
        }
    }
}
