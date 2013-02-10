using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSearch.Core
{
    /// <summary>
    /// A repository for loading or saving <typeparamref name="TItem"/>s, each 
    /// uniquely identified by a <typeparamref name="TId"/>.
    /// </summary>
    /// <remarks>
    /// TODO: Add a Save/Commit style method for use when backed up by a database.
    /// </remarks>
    /// <typeparam name="TId">
    /// The type of identifier for <typeparamref name="TItem"/>.
    /// </typeparam>
    /// <typeparam name="TItem">
    /// The object type the repository loads and saves.
    /// </typeparam>
    public interface IRepository<TId, TItem>: IDisposable
    {
        /// <summary>
        /// Get all items.
        /// </summary>
        /// <returns>
        /// The locations in the repository.
        /// </returns>
        IQueryable<TItem> GetAll();

        /// <summary>
        /// Retrieve the location identified by <paramref name="id"/>.
        /// </summary>
        /// <param name="id">
        /// The ID of the item to load.
        /// </param>
        /// <returns>
        /// The <typeparamref name="TItem"/> for <paramref name="id"/> or null
        /// if no item matches.
        /// </returns>
        TItem Get(TId id);

        /// <summary>
        /// Create a new location.
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
        TId Create(TItem item);

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
        /// Save changes.
        /// </summary>
        void Save();
    }
}
