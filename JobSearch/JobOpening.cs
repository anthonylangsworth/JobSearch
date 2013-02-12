using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using JobSearch.Core;

namespace JobSearch
{
    /// <summary>
    /// An job opening.
    /// </summary>
    public class JobOpening : IJobOpening
    {
        private readonly List<IContact> additionalContacts;

        private readonly List<IActivity> activities;

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

            additionalContacts = new List<IContact>();
            activities = new List<IActivity>();
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

            // Interview constants
            Contract.Invariant(JobOpening.InterviewFollowUpDelay == JobOpening.InterviewFollowUpDelay.Duration());
            Contract.Invariant(JobOpening.InterviewFollowUpDuration == JobOpening.InterviewFollowUpDuration.Duration());
            Contract.Invariant(!string.IsNullOrWhiteSpace(JobOpening.InterviewFollowUpDescription));

            // Application constants
            Contract.Invariant(ApplicationFollowUpDelay == ApplicationFollowUpDelay.Duration());
            Contract.Invariant(ApplicationFollowUpDuration == ApplicationFollowUpDuration.Duration());
            Contract.Invariant(!string.IsNullOrWhiteSpace(ApplicationFollowUpDescription));
            Contract.Invariant(!string.IsNullOrWhiteSpace(ApplicationDescription));
        }

        /// <summary>
        /// Additional contacts associated with the role (beyond or including those referenced
        /// by Activities).
        /// </summary>
        public IList<IContact> AdditionalContacts
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
        public IList<IActivity> Activities
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
        /// The time after an interview the follow up is scheduled.
        /// </summary>
        /// <seealso cref="AddInterview"/>
        public static readonly TimeSpan InterviewFollowUpDelay = TimeSpan.FromDays(1);

        /// <summary>
        /// The duration of an interview follow up.
        /// </summary>
        /// <seealso cref="AddInterview"/>
        public static readonly TimeSpan InterviewFollowUpDuration = TimeSpan.FromMinutes(15);

        /// <summary>
        /// The description of the interview follow up.
        /// </summary>
        /// <seealso cref="AddInterview"/>
        public static readonly string InterviewFollowUpDescription =
            "Interview follow up. Thank the interviewers for their time and ask if you can help them further.";

        /// <summary>
        /// Add an interview <see cref="Activity"/> to <see cref="JobOpening.Activities"/> along with 
        /// a follow up.
        /// </summary>
        /// <param name="start">
        /// The <see cref="DateTime"/> the interview starts.
        /// </param>
        /// <param name="duration">
        /// The duration of the interview.
        /// </param>
        /// <param name="contact">
        /// The <see cref="Contact"/> the interview is with, including their address and
        /// contact details. This must be already an element in <see cref="JobOpening.AdditionalContacts"/>, 
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
        /// <seealso cref="Activities"/>
        /// <seealso cref="InterviewFollowUpDelay"/>
        /// <seealso cref="InterviewFollowUpDescription"/>
        /// <seealso cref="InterviewFollowUpDuration"/>
        public void AddInterview(DateTime start, TimeSpan duration, IContact contact, string description)
        {
            Contract.Ensures(Activities.Count(a => a.Equals(new Activity(
                start, duration, contact, description))) == 1);
            Contract.Ensures(Activities.Count(a => a.Equals(new Activity(
                start + InterviewFollowUpDelay, InterviewFollowUpDuration, contact, InterviewFollowUpDescription))) == 1);

            Activities.Add(new Activity(start, duration, contact, description));
            Activities.Add(new Activity(start + InterviewFollowUpDelay, InterviewFollowUpDuration,
                                                   contact, InterviewFollowUpDescription));
        }

        /// <summary>
        /// The time after an the follow up is scheduled.
        /// </summary>
        /// <seealso cref="Apply"/>
        public static readonly TimeSpan ApplicationFollowUpDelay = TimeSpan.FromDays(3);

        /// <summary>
        /// The duration of an follow up.
        /// </summary>
        /// <seealso cref="Apply"/>
        public static readonly TimeSpan ApplicationFollowUpDuration = TimeSpan.FromMinutes(15);

        /// <summary>
        /// The description of the follow up.
        /// </summary>
        /// <seealso cref="Apply"/>
        public static readonly string ApplicationFollowUpDescription =
            "Application follow up. Reach out to the advertiser to ensure they have your details and answer any questions.";

        /// <summary>
        /// The description of the inital application.
        /// </summary>
        /// <seealso cref="Apply"/>
        public static readonly string ApplicationDescription =
            "Applied.";

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
        /// <seealso cref="Activities"/>
        /// <seealso cref="ApplicationFollowUpDelay"/>
        /// <seealso cref="ApplicationFollowUpDescription"/>
        /// <seealso cref="ApplicationFollowUpDuration"/>
        /// <seealso cref="ApplicationDescription"/>
        public void Apply(DateTime applicationTime, IContact contact)
        {
            Contract.Ensures(Activities.Count(a => a.Equals(new Activity(
                applicationTime, TimeSpan.Zero, contact, "Applied.", true))) == 1);
            Contract.Ensures(Activities.Count(a => a.Equals(new Activity(
                applicationTime + ApplicationFollowUpDelay, ApplicationFollowUpDuration, contact, ApplicationFollowUpDescription))) == 1);

            Activities.Add(new Activity(applicationTime, TimeSpan.Zero, contact, ApplicationDescription, true));
            Activities.Add(new Activity(applicationTime + ApplicationFollowUpDelay, ApplicationFollowUpDuration, contact, ApplicationFollowUpDescription));
        }

    }
}
