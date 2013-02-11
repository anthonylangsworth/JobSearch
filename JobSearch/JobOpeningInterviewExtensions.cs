using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSearch
{
    /// <summary>
    /// Extension methods for <see cref="JobOpening"/> dealing with interviews.
    /// </summary>
    public static class JobOpeningInterviewExtensions
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
        }

        /// <summary>
        /// The time after an interview the follow up is scheduled.
        /// </summary>
        /// <seealso cref="AddInterview"/>
        public static readonly TimeSpan FollowUpDelay = TimeSpan.FromDays(1);

        /// <summary>
        /// The duration of an interview follow up.
        /// </summary>
        /// <seealso cref="AddInterview"/>
        public static readonly TimeSpan FollowUpDuration = TimeSpan.FromMinutes(15);

        /// <summary>
        /// The description of the interview follow up.
        /// </summary>
        /// <seealso cref="AddInterview"/>
        public static readonly string FollowUpDescription =
            "Interview follow up. Thank the interviewers for their time and ask if you can help them further.";

        /// <summary>
        /// Add an interview <see cref="Activity"/> to <see cref="JobOpening.Activities"/> along with 
        /// a follow up.
        /// </summary>
        /// <param name="jobOpening">
        /// The <see cref="JobOpening"/> this applies to.
        /// </param>
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
        /// <seealso cref="JobOpening.Activities"/>
        public static void AddInterview(this JobOpening jobOpening, DateTime start, TimeSpan duration, Contact contact, string description)
        {
            Contract.Requires<ArgumentNullException>(jobOpening != null, "jobOpening");
            Contract.Requires<ArgumentException>(duration == duration.Duration(), "duration");
            Contract.Requires<ArgumentNullException>(contact != null, "contact");
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(description), "description");
            Contract.Ensures(jobOpening.Url == Contract.OldValue(jobOpening.Url));
            Contract.Ensures(jobOpening.Title == Contract.OldValue(jobOpening.Title));
            Contract.Ensures(jobOpening.Organization == Contract.OldValue(jobOpening.Organization));
            Contract.Ensures(jobOpening.AdditionalContacts.Equals(Contract.OldValue(jobOpening.AdditionalContacts)));
            Contract.Ensures(jobOpening.Activities.Count >= 2);
            Contract.Ensures(jobOpening.Activities.Count(a => a.Equals(new Activity(
                start, duration, contact, description))) == 1);
            Contract.Ensures(jobOpening.Activities.Count(a => a.Equals(new Activity(
                start + FollowUpDelay, FollowUpDuration, contact, FollowUpDescription))) == 1);

            jobOpening.Activities.Add(new Activity(start, duration, contact, description));
            jobOpening.Activities.Add(new Activity(start + FollowUpDelay, FollowUpDuration,
                contact, FollowUpDescription));
        }

    }
}
