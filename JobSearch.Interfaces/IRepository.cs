using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSearch.Interfaces
{
    /// <summary>
    /// A repository for loading or saving <typeparamref name="TItem"/>s, each 
    /// uniquely identified by a <typeparamref name="TId"/>.
    /// </summary>
    /// <typeparam name="TId">
    /// The type of identifier for <typeparamref name="TItem"/>.
    /// </typeparam>
    /// <typeparam name="TItem">
    /// The class exposed by the repository.
    /// </typeparam>
    public interface IRepository<TId, TItem>: IDisposable
        where TItem : class, IEquatable<TItem>
    {
        /// <summary>
        /// Does an item with the given <paramref name="id"/> exist in the repository?
        /// </summary>
        /// <param name="id">
        /// The identifier to check for.
        /// </param>
        /// <returns>
        /// True if it exists, false otherwise.
        /// </returns>
        [Pure]
        bool Exists(TId id);

        /// <summary>
        /// Delegate to get the item's unique identifier.
        /// </summary>
        [Pure]
        Func<TItem, TId> GetItemId
        {
            get;
        }

        /// <summary>
        /// Get all items.
        /// </summary>
        /// <returns>
        /// The <typeparamref name="TItem"/>s in the repository.
        /// </returns>
        [Pure]
        IQueryable<TItem> GetAll();

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
        [Pure]
        TItem Get(TId id);

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
        void Create(TItem item);

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
        void Update(TItem item);

        /// <summary>
        /// Delete item identified by <paramref name="id"/>.
        /// </summary>
        /// <param name="id">
        /// The identifier of the item.
        /// </param>
        /// <exception cref="ArgumentException">
        /// An item identified by <paramref name="id"/> does not exist.
        /// </exception>
        void Delete(TId id);

        /// <summary>
        /// Save changes (if any).
        /// </summary>
        /// <seealso cref="Dirty"/>
        void Save();

        /// <summary>
        /// Are there unsaved changes?
        /// </summary>
        /// <seealso cref="Save"/>
        [Pure]
        bool Dirty
        {
            get;
        }
    }
}
