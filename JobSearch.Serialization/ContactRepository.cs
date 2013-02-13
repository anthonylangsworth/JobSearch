using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobSearch.Core;

namespace JobSearch.Serialization
{
    public class ContactRepository: IContactRepository
    {
        private readonly JobSearchEntities jobSearchEntities;

        public ContactRepository()
        {
            jobSearchEntities = new JobSearchEntities();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            jobSearchEntities.Dispose();
        }

        /// <summary>
        /// Get all items.
        /// </summary>
        /// <returns>
        /// The locations in the repository.
        /// </returns>
        public IQueryable<IContact> GetAll()
        {
            throw new NotImplementedException();
            // return jobSearchEntities.Contacts.AsQueryable();
        }

        /// <summary>
        /// Retrieve the location identified by <paramref name="id"/>.
        /// </summary>
        /// <param name="id">
        /// The ID of the item to load.
        /// </param>
        /// <returns>
        /// The I<see cref="IContact"/> or null,
        /// if no item matches.
        /// </returns>
        public IContact Get(int id)
        {
            throw new NotImplementedException();
            // return jobSearchEntities.Contacts.SingleOrDefault(c => c.Id == id);
        }

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
        public int Create(IContact item)
        {
            throw new NotImplementedException();
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
        public void Update(IContact item)
        {
            throw new NotImplementedException();
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
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Save changes.
        /// </summary>
        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
