using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSearch.Core
{
    /// <summary>
    /// A contact in a job search, such as a recruiter, HR representative or a hiring manager.
    /// </summary>
    [ContractClass(typeof(ContactContract))]
    public interface IContact: IEquatable<IContact>
    {
        /// <summary>
        /// The peron's name.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Phone number.
        /// </summary>
        string Phone { get; set; }

        /// <summary>
        /// E-mail address.
        /// </summary>
        string Email { get; set; }

        /// <summary>
        /// Free form notes.
        /// </summary>
        string Notes { get; set; }

        /// <summary>
        /// The name of the peron's organization.
        /// </summary>
        string Organization { get; set; }

        /// <summary>
        /// Unique ID of the person.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// The contact role (e.g. recruiter, HR, hiring manager).
        /// </summary>
        ContactRole Role { get; set; }
    }
}
