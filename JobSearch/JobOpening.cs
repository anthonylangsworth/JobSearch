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
    public class JobOpening : IEquatable<JobOpening>
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
            Contract.Invariant(!AdditionalContacts.Contains(null));
            Contract.Invariant(!Activities.Contains(null));
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
        public string Url
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
            return String.Format("Id: {0}, Organization: {1}, Title: {2}, Url: {3}, AdvertisedDate: {4}", 
                Id, Organization, Title, Url, AdvertisedDate);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(JobOpening other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(additionalContacts, other.additionalContacts) && Equals(activities, other.activities) && string.Equals(Url, other.Url) && string.Equals(Title, other.Title) && string.Equals(Organization, other.Organization) && string.Equals(Notes, other.Notes) && AdvertisedDate.Equals(other.AdvertisedDate) && Id == other.Id;
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((JobOpening)obj);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (additionalContacts != null ? additionalContacts.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (activities != null ? activities.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Url != null ? Url.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Title != null ? Title.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Organization != null ? Organization.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Notes != null ? Notes.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ AdvertisedDate.GetHashCode();
                hashCode = (hashCode * 397) ^ Id;
                return hashCode;
            }
        }
    }
}
