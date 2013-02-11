using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace JobSearch
{
    /// <summary>
    /// An job opening.
    /// </summary>
    public class JobOpening
    {
        private readonly List<Contact> additionalContacts;

        private readonly List<Activity> activities;

        /// <summary>
        /// Create a new <see name="JobOpening"/>.
        /// </summary>
        /// <param name="id">
        /// The job opening's unique identifier.
        /// </param>
        public JobOpening(int id)
            : this()
        {
            Contract.Ensures(Id == id);

            this.Id = id;
        }

        /// <summary>
        /// Create a new <see name="JobOpening"/>.
        /// </summary>
        public JobOpening()
        {
            Contract.Ensures(!Activities.Any());
            Contract.Ensures(!AdditionalContacts.Any());
            Contract.Ensures(Url == null);
            Contract.Ensures(Title == null);
            Contract.Ensures(Organization == null);
            Contract.Ensures(Notes == null);

            additionalContacts = new List<Contact>();
            activities = new List<Activity>();
        }

        /// <summary>
        /// Class invariants.
        /// </summary>
        [ContractInvariantMethod]
        private void ClassInvariants()
        {
            Contract.Invariant(this.additionalContacts != null);
            Contract.Invariant(this.activities != null);
            Contract.Invariant(!additionalContacts.Contains(null));
            Contract.Invariant(!activities.Contains(null));
        }

        /// <summary>
        /// Additional contacts associated with the role (beyond or including those referenced
        /// by Activities).
        /// </summary>
        public IList<Contact> AdditionalContacts
        {
            get
            {
                return additionalContacts;
            }
        }

        /// <summary>
        /// URL for the job advertisement (if any).
        /// </summary>
        public Uri Url
        {
            get;
            set;
        }

        /// <summary>
        /// The job title.
        /// </summary>
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// The name of the company, department or organization the role is with.
        /// </summary>
        public string Organization
        {
            get;
            set;
        }

        /// <summary>
        /// User supplied notes (if any).
        /// </summary>
        public string Notes
        {
            get; set;
        }

        /// <summary>
        /// Past and upcoming activities associated with this job opening.
        /// </summary>
        public IList<Activity> Activities
        {
            get
            {
                return activities;
            }
        }

        /// <summary>
        /// When it was advertised (if applicable).
        /// </summary>
        public DateTime AdvertisedDate
        {
            get;
            set;
        }

        /// <summary>
        /// Unique identifier.
        /// </summary>
        public int Id
        {
            get; 
            private set; 
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Id: {0}, Organization: {1}, Title: {2}, Url: {3}, AdvertisedDate: {4}", 
                Id, Organization, Title, Url, AdvertisedDate);
        }
    }
}
