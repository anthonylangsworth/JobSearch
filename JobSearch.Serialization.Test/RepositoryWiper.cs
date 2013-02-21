using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobSearch.Interfaces;

namespace JobSearch.Serialization.Test
{
    /// <summary>
    /// Normally used within a using statement for automated unit tests, 
    /// this will clear the contents of a repository during <see cref="Dispose"/>.
    /// </summary>
    public class RepositoryWiper<TId, TItem>: IDisposable
        where TItem: class, IEquatable<TItem>
    {
        /// <summary>
        /// Create a new <see cref="RepositoryWiper{TId, TItem}"/>.
        /// </summary>
        /// <param name="repository">
        /// The repository to empty during <see cref="Dispose"/>. This
        /// cannot be null.
        /// </param>
        /// <param name="getItemId">
        /// A <see cref="Func{TItem, TId}"/> that extracts the ID from an item.
        /// This cannot be null.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// No argument can be null.
        /// </exception>
        public RepositoryWiper(IRepository<TId, TItem> repository, Func<TItem, TId> getItemId)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }
            if (getItemId == null)
            {
                throw new ArgumentNullException("getItemId");
            }

            Repository = repository;
            GetItemId = getItemId;
        }

        /// <summary>
        /// The repository to wipe during <see cref="Dispose"/>.
        /// </summary>
        public IRepository<TId, TItem> Repository
        {
            get;
            private set;
        }

        /// <summary>
        /// Get the ID from an item.
        /// </summary>
        public Func<TItem, TId> GetItemId
        {
            get; 
            private set;
        }

        /// <summary>
        /// Empty the repository.
        /// </summary>
        public void Wipe()
        {
            foreach (TItem item in Repository.GetAll())
            {
                Repository.Delete(GetItemId(item));
            }
            Repository.Save();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Wipe();
        }
    }
}
