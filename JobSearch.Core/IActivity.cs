using System;

namespace JobSearch.Core
{
    /// <summary>
    /// A job search activity, such as applying for a job, an interview or a follow up.
    /// </summary>
    public interface IActivity
    {
        /// <summary>
        /// The date/time this activity starts. 
        /// </summary>
        DateTime Start { get; }

        /// <summary>
        /// The name of this activity.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// How long the activity takes.
        /// </summary>
        TimeSpan Duration { get; }

        /// <summary>
        /// The person involved in the activity.
        /// </summary>
        IContact Contact { get; }

        /// <summary>
        /// Is the activity completed or done?
        /// </summary>
        bool Completed { get; set; }

        /// <summary>
        /// ID.
        /// </summary>
        int Id { get; }
    }
}