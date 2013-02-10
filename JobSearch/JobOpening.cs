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
        /// <remarks>
        /// Using <see cref="Tuple{Key, TValue}"/> rather than <see cref="KeyValuePair{Key, TValue}"/> because
        /// classes are generally faster than structs (not that speed is critical here).
        /// </remarks>
        private readonly List<Contact> contacts;

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
            Contract.Ensures(!Contacts.Any());
            Contract.Ensures(Url == null);
            Contract.Ensures(Title == null);
            Contract.Ensures(Organization == null);
            Contract.EndContractBlock();

            contacts = new List<Contact>();
            activities = new List<Activity>();
        }

        /// <summary>
        /// Class invariants.
        /// </summary>
        [ContractInvariantMethod]
        private void ClassInvariants()
        {
            Contract.Invariant(this.contacts != null);
            Contract.Invariant(this.activities != null);
            Contract.Invariant(!contacts.Contains(null));
            Contract.Invariant(!activities.Contains(null));

            Contract.Invariant(activities.Select(a => a.Contact).All(c => contacts.Contains(c)),
                "Every person associated with an activity is in the contacts");
        }

        /// <summary>
        /// Contacts associated with the role.
        /// </summary>
        public IEnumerable<Contact> Contacts
        {
            get
            {
                return contacts.AsReadOnly();
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
        /// Past and upcoming activities associated with this job opening.
        /// </summary>
        public IEnumerable<Activity> Activities
        {
            get
            {
                return activities.AsReadOnly();
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
        /// Add an interview <see cref="Activity"/> to <see cref="Activities"/> along with 
        /// a follow up.
        /// </summary>
        /// <param name="start">
        /// The <see cref="DateTime"/> the interview starts.
        /// </param>
        /// <param name="contact">
        /// The <see cref="Contact"/> the interview is with, including their address and
        /// contact details. This must be already an element in <see cref="Contacts"/>, 
        /// ensuring contacts need not be reentered for multiple activities.
        /// </param>
        /// <param name="description">
        /// An optional description or notes for the interview.
        /// </param>
        /// <param name="duration">
        /// The duration of the interview.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="contact"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="contact"/> not in <see cref="Contacts"/>.
        /// </exception>
        public void AddInterview(DateTime start, Contact contact, string description, TimeSpan duration)
        {
            Contract.Requires<ArgumentNullException>(contact != null, "contact");
            Contract.Requires<ArgumentException>(Contacts.Contains(contact), "Unknown contact");
            Contract.Ensures(Url == Contract.OldValue(Url));
            Contract.Ensures(Title == Contract.OldValue(Title));
            Contract.Ensures(Organization == Contract.OldValue(Organization));
            Contract.Ensures(contacts.Equals(Contract.OldValue(contacts)));
            Contract.Ensures(activities.Count >= 2);
            Contract.Ensures(activities.Count(a => a.Equals(new Activity(
                start, duration, contact, description))) == 1);
            Contract.Ensures(activities.Count(a => a.Equals(new Activity(
                start + TimeSpan.FromDays(1), TimeSpan.FromMinutes(15), contact, "Interview follow up"))) == 1);
            Contract.EndContractBlock();

            activities.Add(new Activity(start, duration, contact, description));
            activities.Add(new Activity(start + TimeSpan.FromDays(1), TimeSpan.FromMinutes(15), contact, "Interview follow up"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activity"></param>
        public void RemoveActivity(Activity activity)
        {
            throw new System.NotImplementedException();
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
