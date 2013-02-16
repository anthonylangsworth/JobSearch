using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSearch.Core
{
    /// <summary>
    /// Code contracts for <see cref="IContact"/>.
    /// </summary>
    [ContractClassFor(typeof(IContact))]
    internal abstract class ContactContract : IContact
    {
        /// <summary>
        /// The peron's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Phone number.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// E-mail address.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Free form notes.
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// The name of the peron's organization.
        /// </summary>
        public string Organization { get; set; }

        /// <summary>
        /// Unique ID of the person.
        /// </summary>
        public int Id { get; private set;  }

        /// <summary>
        /// The contact role (e.g. recruiter, HR, hiring manager).
        /// </summary>
        public ContactRole Role { get; set; }

        /// <summary>
        /// Are two <see cref="IContact"/>s equal?
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public abstract bool Equals(IContact other);
    }
}
