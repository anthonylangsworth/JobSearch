using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSearch.Core
{
    /// <summary>
    /// Code contracts for <see cref="IJobOpening"/>.
    /// </summary>
    [ContractClassFor(typeof(IJobOpening))]
    public abstract class JobOpeningContract: IJobOpening
    {
        /// <summary>
        /// Class invariants.
        /// </summary>
        [ContractInvariantMethod]
        private void ClassInvariants()
        {
            Contract.Invariant(!AdditionalContacts.Contains(null));
            Contract.Invariant(!Activities.Contains(null));
        }

        /// <summary>
        /// Additional contacts associated with the role (beyond or including those referenced
        /// by Activities).
        /// </summary>
        public IList<IContact> AdditionalContacts { get; private set; }

        /// <summary>
        /// URL for the job advertisement (if any).
        /// </summary>
        public Uri Url { get; set; }

        /// <summary>
        /// The job title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The name of the company, department or organization the role is with.
        /// </summary>
        public string Organization { get; set; }

        /// <summary>
        /// User supplied notes (if any).
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Past and upcoming activities associated with this job opening.
        /// </summary>
        public IList<IActivity> Activities { get; private set; }

        /// <summary>
        /// When it was advertised (if applicable).
        /// </summary>
        public DateTime AdvertisedDate { get; set; }

        /// <summary>
        /// Unique identifier.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Add an interview <see cref="IActivity"/> to <see cref="IJobOpening.Activities"/> along with 
        /// a follow up.
        /// </summary>
        /// <param name="start">
        /// The <see cref="DateTime"/> the interview starts.
        /// </param>
        /// <param name="duration">
        /// The duration of the interview.
        /// </param>
        /// <param name="contact">
        /// The <see cref="IContact"/> the interview is with, including their address and
        /// contact details. This must be already an element in <see cref="IJobOpening.AdditionalContacts"/>, 
        /// ensuring contacts need not be reentered for multiple activities.
        /// </param>
        /// <param name="description">
        /// An optional description or notes for the interview. This cannot be null, empty or
        /// white space.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="contact"/> cannot be null. <paramref name="description"/> cannot
        /// be null, empty or whitespace.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="duration"/> must be positive.
        /// </exception>
        /// <seealso cref="IJobOpening.Activities"/>
        public void AddInterview(DateTime start, TimeSpan duration, IContact contact, string description)
        {
            Contract.Requires<ArgumentException>(duration == duration.Duration(), "duration");
            Contract.Requires<ArgumentNullException>(contact != null, "contact");
            Contract.Requires<ArgumentNullException>(!String.IsNullOrWhiteSpace(description), "description");
            Contract.Ensures(Url == Contract.OldValue(Url));
            Contract.Ensures(Title == Contract.OldValue(Title));
            Contract.Ensures(Notes == Contract.OldValue(Notes));
            Contract.Ensures(Organization == Contract.OldValue(Organization));
            Contract.Ensures(AdditionalContacts.Equals(Contract.OldValue(AdditionalContacts)));
            Contract.Ensures(Activities.Count >= 2);
        }

        /// <summary>
        /// Apply for a job opening. This adds a completed activity for the application
        /// and a follow up later.
        /// </summary>
        /// <param name="applicationTime">
        /// The time and date the job was applied for.
        /// </param>
        /// <param name="contact">
        /// Details of the contact the application was made through. This cannot be null.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///  <paramref name="contact"/> cannot be null.
        /// </exception>
        /// <seealso cref="IJobOpening.Activities"/>
        public void Apply(DateTime applicationTime, IContact contact)
        {
            Contract.Requires<ArgumentNullException>(contact != null, "contact");
            Contract.Ensures(Url == Contract.OldValue(Url));
            Contract.Ensures(Title == Contract.OldValue(Title));
            Contract.Ensures(Notes == Contract.OldValue(Notes));
            Contract.Ensures(Organization == Contract.OldValue(Organization));
            Contract.Ensures(AdditionalContacts.Equals(Contract.OldValue(AdditionalContacts)));
            Contract.Ensures(Activities.Count >= 2);
        }
    }
}
