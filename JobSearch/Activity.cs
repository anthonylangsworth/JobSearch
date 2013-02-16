using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace JobSearch
{
    /// <summary>
    /// An activity, such as an interview or follow-up.
    /// </summary>
    public class Activity : IEquatable<Activity>
    {
        /// <summary>
        /// Create a new <see cref="Activity"/>.
        /// </summary>
        /// <param name="id">
        /// The ID of the activity.
        /// </param>
        /// <param name="start">
        /// The time the activity starts.
        /// </param>
        /// <param name="duration">
        /// The duration of the activity.
        /// </param>
        /// <param name="contact">
        /// The <see cref="Contact"/> involved with the activity, including
        /// contact details and address.
        /// </param>
        /// <param name="description">
        /// Option description or notes.
        /// </param>
        /// <param name="completed">
        /// True if the activity is created completed, false otherwise (the default).
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="contact"/> cannot be null.
        /// </exception>
        public Activity(int id, DateTime start, TimeSpan duration, Contact contact, string description, bool completed = false)
            : this(start, duration, contact, description, completed)
        {
            Contract.Ensures(this.Id == id, "Incorrect Id");

            this.Id = id;
        }

        /// <summary>
        /// Create a new <see cref="Activity"/>.
        /// </summary>
        /// <param name="start">
        /// The time the activity starts.
        /// </param>
        /// <param name="duration">
        /// The duration of the activity. This must be positive.
        /// </param>
        /// <param name="contact">
        /// The <see cref="Contact"/> involved with the activity, including
        /// contact details and address.
        /// </param>
        /// <param name="description">
        /// Option description or notes. This cannot be null, empty or
        /// white space.
        /// </param>
        /// <param name="completed">
        /// True if the activity is created completed, false otherwise (the default).
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="contact"/> cannot be null. <paramref name="description"/> cannot
        /// be null, empty or whitespace.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="duration"/> must be positive.
        /// </exception>
        public Activity(DateTime start, TimeSpan duration, Contact contact, string description, bool completed = false)
        {
            Contract.Requires<ArgumentNullException>(contact != null, "contact");
            Contract.Requires<ArgumentException>(duration == duration.Duration(), "duration");
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(description), "description");
            Contract.Ensures(Start == start);
            Contract.Ensures(Duration == duration);
            Contract.Ensures(Contact.Equals(contact));
            Contract.Ensures(Description == description);
            Contract.Ensures(Completed == completed);

            this.Start = start;
            this.Contact = contact;
            this.Description = description;
            this.Duration = duration;
            this.Completed = completed;
        }

        /// <summary>
        /// Class invariants.
        /// </summary>
        [ContractInvariantMethod]
        private void ClassInvariants()
        {
            Contract.Invariant(Contact != null,
                "Contact cannot be null");
            Contract.Invariant(Duration == Duration.Duration(),
                "Duration must be positive");
        }

        /// <summary>
        /// The date/time this activity starts. 
        /// </summary>
        public DateTime Start
        {
            get; 
            private set;
        }

        /// <summary>
        /// The name of this activity.
        /// </summary>
        public string Description
        {
            get; 
            set;
        }

        /// <summary>
        /// How long the activity takes.
        /// </summary>
        public TimeSpan Duration
        {
            get;
            private set;
        }

        /// <summary>
        /// The person involved in the activity.
        /// </summary>
        public Contact Contact
        {
            get;
            private set;
        }

        /// <summary>
        /// Is the activity completed or done?
        /// </summary>
        public bool Completed
        {
            get;
            set;
        }

        /// <summary>
        /// ID.
        /// </summary>
        public int Id
        {
            get;
            private set;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(Activity other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Start.Equals(other.Start) && string.Equals(Description, other.Description) && Duration.Equals(other.Duration) && Equals(Contact, other.Contact) && Completed.Equals(other.Completed) && Id == other.Id;
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
            return Equals((Activity) obj);
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
                int hashCode = Start.GetHashCode();
                hashCode = (hashCode*397) ^ (Description != null ? Description.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ Duration.GetHashCode();
                hashCode = (hashCode*397) ^ (Contact != null ? Contact.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ Completed.GetHashCode();
                hashCode = (hashCode*397) ^ Id;
                return hashCode;
            }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Id: {5}, Start: {0}, Description: {1}, Contact: {2}, Duration: {3}, Completed: {4}", 
                Start, Description, Contact, Duration, Completed, Id);
        }
    }
}
