using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JobSearch.Core
{
    /// <summary>
    /// A <see cref="IContact"/>'s role.
    /// </summary>
    public enum ContactRole
    {
        /// <summary>
        /// A recruiter, head-hunter or otherwise works for a placement firm. Often the first contact for an
        /// </summary>
        Recruiter,
        /// <summary>
        /// Someone from HR, usually in the hiring organization.
        /// </summary>
        HumanResources,
        /// <summary>
        /// A hiring manager.
        /// </summary>
        HiringManager,
    }
}
