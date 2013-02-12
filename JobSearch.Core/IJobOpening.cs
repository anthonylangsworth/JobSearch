using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using JobSearch.Core;

namespace JobSearch.Core
{
    /// <summary>
    /// Job openings.
    /// </summary>
    [ContractClass(typeof(JobOpeningContract))]
    public interface IJobOpening
    {
        /// <summary>
        /// Additional contacts associated with the role (beyond or including those referenced
        /// by Activities).
        /// </summary>
        IList<IContact> AdditionalContacts { get; }

        /// <summary>
        /// URL for the job advertisement (if any).
        /// </summary>
        Uri Url { get; set; }

        /// <summary>
        /// The job title.
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// The name of the company, department or organization the role is with.
        /// </summary>
        string Organization { get; set; }

        /// <summary>
        /// User supplied notes (if any).
        /// </summary>
        string Notes { get; set; }

        /// <summary>
        /// Past and upcoming activities associated with this job opening.
        /// </summary>
        IList<IActivity> Activities { get; }

        /// <summary>
        /// When it was advertised (if applicable).
        /// </summary>
        DateTime AdvertisedDate { get; set; }

        /// <summary>
        /// Unique identifier.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Add an interview <see cref="IActivity"/> to <see cref="Activities"/> along with 
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
        /// contact details. This must be already an element in <see cref="AdditionalContacts"/>, 
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
        void AddInterview(DateTime start, TimeSpan duration, IContact contact, string description);

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
        void Apply(DateTime applicationTime, IContact contact);
    }
}