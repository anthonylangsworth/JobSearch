using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JobSearch
{
    /// <summary>
    /// A person involved in a job search.
    /// </summary>
    public class Contact : IEquatable<Contact>
    {
        /// <summary>
        /// The peron's name.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Phone number.
        /// </summary>
        public string Phone
        {
            get;
            set;
        }

        /// <summary>
        /// E-mail address.
        /// </summary>
        public string Email
        {
            get;
            set;
        }

        /// <summary>
        /// Free form notes.
        /// </summary>
        public string Notes
        {
            get;
            set;
        }

        /// <summary>
        /// The name of the peron's organization.
        /// </summary>
        public string Organization
        {
            get;
            set;
        }

        /// <summary>
        /// Unique ID of the person.
        /// </summary>
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// The contact role (e.g. recruiter, HR, hiring manager).
        /// </summary>
        public ContactRole Role
        {
            get; 
            set;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(Contact other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Name, other.Name) && string.Equals(Phone, other.Phone) && string.Equals(Email, other.Email) && string.Equals(Notes, other.Notes) && string.Equals(Organization, other.Organization) && Id == other.Id && Role == other.Role;
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
            return Equals((Contact) obj);
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
                int hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Phone != null ? Phone.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Email != null ? Email.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Notes != null ? Notes.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Organization != null ? Organization.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ Id;
                hashCode = (hashCode*397) ^ (int) Role;
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
            return string.Format("Id: {5}, Name: {0}, Phone: {1}, Email: {2}, Notes: {3}, Organization: {4}, Role: {6}", 
                Name, Phone, Email, Notes, Organization, Id, Role);
        }
    }
}
