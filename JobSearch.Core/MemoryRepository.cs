using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSearch.Core
{
    /// <summary>
    /// A generic, in-memory repository.
    /// </summary>
    /// <typeparam name="TId">
    /// The type of the unique identifier of <typeparamref name="TItem"/>.
    /// </typeparam>
    /// <typeparam name="TItem">
    /// The items stored in the repository.
    /// </typeparam>
    public class MemoryRepository<TId, TItem> : IRepository<TId, TItem>
        where TItem : class, ICloneable
    {
        /// <summary>
        /// Create a new <see cref="MemoryRepository{TId, TItem}"/>.
        /// </summary>
        /// <param name="getId">
        /// A <see cref="Func{TItem, TId}"/> that extracts the ID from the item.
        /// </param>
        /// <param name="setId">
        /// An <see cref="Action{TItem, TId}"/> that sets the ID on the item.
        /// </param>B
        /// <param name="newId">
        /// A delegate that creates the ID of a new item. This can do nothing if
        /// the caller assigns the ID or it otherwise will already be unique.
        /// Alternatively, it can throw an exception (an <see cref="ArgumentException"/>
        /// is recommended) if the ID is controlled by the caller and is a duplicate.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="getId"/> cannot be null.
        /// </exception>
        public MemoryRepository(Func<TItem, TId> getId, Action<TItem, TId> setId, Func<IEnumerable<TItem>, TId> newId)
        {
            if (getId == null)
            {
                throw new ArgumentNullException("getId");
            }
            if (setId == null)
            {
                throw new ArgumentNullException("setId");
            }
            if (newId == null)
            {
                throw new ArgumentNullException("newId");
            }

            GetId = getId;
            SetId = setId;
            NewId = newId;
        }

        /// <summary>
        /// A <see cref="Func{TItem, TId}"/> that extracts the ID from the item.
        /// </summary>
        /// <returns></returns>
        public Func<TItem, TId> GetId
        {
            get;
            private set;
        }

        /// <summary>
        /// A <see cref="Func{TItem, TId}"/> that sets the item ID.
        /// </summary>
        /// <returns></returns>
        public Action<TItem, TId> SetId
        {
            get;
            private set;
        }

        /// <summary>
        /// A delegate that creates the ID of a new item.
        /// </summary>
        /// <returns></returns>
        public Func<IEnumerable<TItem>, TId> NewId
        {
            get;
            private set;
        }

        /// <summary>
        /// Items stored in the repository.
        /// </summary>
        protected List<TItem> Items = new List<TItem>();

        /// <summary>
        /// Get all items.
        /// </summary>
        /// <returns>
        /// The items in the repository.
        /// </returns>
        public IQueryable<TItem> GetAll()
        {
            return Items.AsQueryable();
        }

        /// <summary>
        /// Get an item.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TItem Get(TId id)
        {
            return Items.FirstOrDefault(item => GetId(item).Equals(id));
        }

        /// <summary>
        /// Insert a copy of the given item into the repository. Set ID
        /// of <paramref name="item"/> if it is assigned by <see cref="SetId"/>.
        /// </summary>
        /// <remarks>
        /// Inserting a copy prevents side effect modifications altering
        /// intems in the repository.
        /// </remarks>
        /// <param name="item"></param>
        public TId Create(TItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            TItem newItem = (TItem)item.Clone();
            SetId(item, NewId(Items));
            SetId(newItem, NewId(Items));
            Items.Add(newItem);
            return GetId(newItem);
        }

        /// <summary>
        /// Update an existing item.
        /// </summary>
        /// <param name="item"></param>
        public void Update(TItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            if (!Items.Exists(location => GetId(location).Equals(GetId(item))))
            {
                throw new ArgumentException(
                    string.Format("item '{0}' does not exist in repository", item), "item");
            }

            Items.Remove(Items.Find(location => GetId(location).Equals(GetId(item))));
            Items.Add((TItem)item.Clone());
        }

        /// <summary>
        /// Delete an item.
        /// </summary>
        /// <param name="id"></param>
        public void Delete(TId id)
        {
            if (!Items.Exists(location => GetId(location).Equals(id)))
            {
                throw new ArgumentException(
                    string.Format("item with ID '{0}' does not exist in repository", id), "id");
            }

            Items.Remove(Items.Find(location => GetId(location).Equals(id)));
        }

        /// <summary>
        /// Save changes.
        /// </summary>
        public void Save()
        {
            // Do nothing
        }

        /// <summary>
        /// Clean up.
        /// </summary>
        public void Dispose()
        {
            // Do nothing
        }
    }
}
