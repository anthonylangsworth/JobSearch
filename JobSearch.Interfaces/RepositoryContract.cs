using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSearch.Interfaces
{
    /// <summary>
    /// Contracts for <see cref="IRepository{TId, TItem}"/>.
    /// </summary>
    [ContractClassFor(typeof(IRepository<,>))]
    public abstract class RepositoryContract<TId, TItem>: IRepository<TId, TItem>
        where TItem : class, IEquatable<TItem>
    {
        /// <summary>
        /// Class invariants.
        /// </summary>
        [ContractInvariantMethod]
        private void ClassInariants()
        {
            Contract.Invariant(GetItemId != null);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public abstract void Dispose();

        /// <summary>
        /// Does an item with the given <paramref name="id"/> exist in the repository?
        /// </summary>
        /// <param name="id">
        /// The identifier to check for.
        /// </param>
        /// <returns>
        /// True if it exists, false otherwise.
        /// </returns>
        public bool Exists(TId id)
        {
            // Contract.Ensures(Contract.Result<bool>().Equals(GetAll().Any(item => GetItemId(item).Equals(id))));
            Contract.Ensures(Contract.OldValue<bool>(Dirty).Equals(Dirty));
            return false;
        }

        /// <summary>
        /// Delegate to get the item's unique identifier.
        /// </summary>
        public abstract Func<TItem, TId> GetItemId
        {
            get; 
        }

        /// <summary>
        /// Get all items.
        /// </summary>
        /// <returns>
        /// The TItems in the repository.
        /// </returns>
        public IQueryable<TItem> GetAll()
        {
            Contract.Ensures(Contract.Result<IQueryable<TId>>() != null);
            Contract.Ensures(Contract.OldValue<bool>(Dirty) == Dirty);
            return null;
        }

        /// <summary>
        /// Retrieve the TItem identified by <paramref name="id"/>.
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
            Contract.Ensures(Exists(id) ?
                Contract.Result<TItem>() != null && GetItemId(Contract.Result<TItem>()).Equals(id)
                : Contract.Result<TItem>() == null);
            Contract.Ensures(Contract.OldValue<bool>(Dirty) == Dirty);
            return null;
        }

        /// <summary>
        /// Create a new TItem.
        /// </summary>
        /// <param name="item">
        /// The item to create.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="item"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="item"/> already exists or is otherwise invalid.
        /// </exception>
        public void Create(TItem item)
        {
            Contract.Requires<ArgumentNullException>(item != null, "item");
            Contract.Requires<ArgumentException>(!Exists(GetItemId(item)), "item");
            Contract.Ensures(Exists(GetItemId(item)));
            Contract.Ensures(Get(GetItemId(item)).Equals(item));
            Contract.Ensures(Dirty);
        }

        /// <summary>
        /// Update or modify an existing item.
        /// </summary>
        /// <param name="item">
        /// The item to modify.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="item"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="item"/> already exists or is otherwise invalid.
        /// </exception>
        public void Update(TItem item)
        {
            Contract.Requires<ArgumentNullException>(item != null, "item");
            Contract.Requires<ArgumentException>(Exists(GetItemId(item)), "item");
            Contract.Ensures(Exists(GetItemId(item)));
            Contract.Ensures(Get(GetItemId(item)).Equals(item));
            Contract.Ensures(Dirty);
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
            Contract.Requires<ArgumentException>(Exists(id), "item");
            Contract.Ensures(!Exists(id));
            Contract.Ensures(Dirty);
        }

        /// <summary>
        /// Save changes.
        /// </summary>
        public void Save()
        {
            Contract.Ensures(!Dirty);
        }

        /// <summary>
        /// Are there unsaved changes?
        /// </summary>
        /// <seealso cref="IRepository{TId,TItem}.Save"/>
        public abstract bool Dirty
        {
            get;
        }
    }
}
