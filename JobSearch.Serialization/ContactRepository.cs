﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobSearch.Core;

namespace JobSearch.Serialization
{
    /// <summary>
    /// A repository for <see cref="Contact"/>.
    /// </summary>
    public class ContactRepository: EntityFrameworkRepository<JobSearch, int, IContact>
    {
        private readonly JobSearch jobSearch;

        /// <summary>
        /// Create a <see cref="ContactRepository"/>.
        /// </summary>
        public ContactRepository()
        {
            jobSearch = new JobSearch();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            jobSearch.Dispose();
        }

        /// <summary>
        /// Does an item with the given <paramref name="id"/> exist in the repository?
        /// </summary>
        /// <param name="id">
        /// The identifier to check for.
        /// </param>
        /// <returns>
        /// True if it exists, false otherwise.
        /// </returns>
        public bool Exists(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Delegate to get the item's unique identifier.
        /// </summary>
        public Func<IContact, int> GetItemId { get; private set; }

        /// <summary>
        /// Get all items.
        /// </summary>
        /// <returns>
        /// The locations in the repository.
        /// </returns>
        public IQueryable<IContact> GetAll()
        {
            return jobSearch.Contacts.AsQueryable();
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
            return jobSearch.Contacts.FirstOrDefault(c => c.Id == id);
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
        /// <paramref name="item"/> csannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="item"/> already exists or is otherwise invalid.
        /// </exception>
        public void Create(IContact item)
        {
            Contact contact;
            contact = item as Contact;
            if (contact == null)
            {
                throw new ArgumentException("Not a Contact", "item");
            }
            jobSearch.Contacts.Add(contact);
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
            Contact oldContact;
            Contact newContact;

            oldContact = jobSearch.Contacts.FirstOrDefault(c => c.Id == item.Id);
            if(oldContact == null)
            {
                throw new ArgumentException("Not found", "item");
            }

            newContact = item as Contact;
            if (newContact == null)
            {
                throw new ArgumentException();
            }

            jobSearch.Contacts.Remove(oldContact);
            jobSearch.Contacts.Add(newContact);
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
            Contact contact;

            contact = jobSearch.Contacts.FirstOrDefault(c => c.Id == id);
            if (contact == null)
            {
                throw new ArgumentException("Not found", "id");    
            }

            jobSearch.Contacts.Remove(contact);
        }

        /// <summary>
        /// Save changes.
        /// </summary>
        public void Save()
        {
            jobSearch.SaveChanges();
        }

        /// <summary>
        /// Are there unsaved changes?
        /// </summary>
        /// <seealso cref="IRepository{TId,TItem}.Save"/>
        public bool Dirty { get; private set; }
    }
}
