using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSearch
{
    /// <summary>
    /// Extension methods for <see cref="JobOpening"/> around applying for jobs.
    /// </summary>
    public static class JobOpeningApplicationExtensions
    {
        /// <summary>
        /// Class invariants.
        /// </summary>
        [ContractInvariantMethod]
        private static void ClassInvariants()
        {
            Contract.Invariant(FollowUpDelay == FollowUpDelay.Duration());
            Contract.Invariant(FollowUpDuration == FollowUpDuration.Duration());
            Contract.Invariant(!string.IsNullOrWhiteSpace(FollowUpDescription));
            Contract.Invariant(!string.IsNullOrWhiteSpace(ApplicationDescription));
        }

        /// <summary>
        /// The time after an the follow up is scheduled.
        /// </summary>
        /// <seealso cref="Apply"/>
        public static readonly TimeSpan FollowUpDelay = TimeSpan.FromDays(3);

        /// <summary>
        /// The duration of an follow up.
        /// </summary>
        /// <seealso cref="Apply"/>
        public static readonly TimeSpan FollowUpDuration = TimeSpan.FromMinutes(15);

        /// <summary>
        /// The description of the follow up.
        /// </summary>
        /// <seealso cref="Apply"/>
        public static readonly string FollowUpDescription =
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
        /// <param name="jobOpening">
        /// The <see cref="JobOpening"/> to apply for. This cannot be null.
        /// </param>
        /// <param name="applicationTime">
        /// The time and date the job was applied for.
        /// </param>
        /// <param name="contact">
        /// Details of the contact the application was made through. This cannot be null.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Neither <paramref name="jobOpening"/> nor <paramref name="contact"/> can be null.
        /// </exception>
        public static void Apply(this JobOpening jobOpening, DateTime applicationTime, Contact contact)
        {
            Contract.Requires<ArgumentNullException>(jobOpening != null, "jobOpening");
            Contract.Requires<ArgumentNullException>(contact != null, "contact");
            Contract.Ensures(jobOpening.Url == Contract.OldValue(jobOpening.Url));
            Contract.Ensures(jobOpening.Title == Contract.OldValue(jobOpening.Title));
            Contract.Ensures(jobOpening.Notes == Contract.OldValue(jobOpening.Notes));
            Contract.Ensures(jobOpening.Organization == Contract.OldValue(jobOpening.Organization));
            Contract.Ensures(jobOpening.AdditionalContacts.Equals(Contract.OldValue(jobOpening.AdditionalContacts)));
            Contract.Ensures(jobOpening.Activities.Count >= 2);
            Contract.Ensures(jobOpening.Activities.Count(a => a.Equals(new Activity(
                applicationTime, TimeSpan.Zero, contact, "Applied.", true))) == 1);
            Contract.Ensures(jobOpening.Activities.Count(a => a.Equals(new Activity(
                applicationTime + FollowUpDelay, FollowUpDuration, contact, FollowUpDescription))) == 1);

            jobOpening.Activities.Add(new Activity(applicationTime, TimeSpan.Zero, contact, ApplicationDescription) { Completed = true });
            jobOpening.Activities.Add(new Activity(applicationTime + FollowUpDelay, FollowUpDuration, contact, FollowUpDescription));
        }
    }
}
