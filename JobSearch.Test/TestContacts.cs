using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSearch.Test
{
    /// <summary>
    /// Test instances of <see cref="Contact"/>.
    /// </summary>
    public static class TestContacts
    {
        /// <summary>
        /// A test contact
        /// </summary>
        public static Contact PeterSmith
        {
            get
            {
                return new Contact(42)
                {
                    Name = "Peter Smith",
                    Email = "psmith@uberrecruiters.com",
                    Role = ContactRole.HumanResources,
                    Phone = "1234 5678",
                    Organization = "Uber Recruiters"
                };
            }
        }

        /// <summary>
        /// A test contact
        /// </summary>
        public static Contact SarahBillingsley
        {
            get
            {
                return new Contact(113)
                {
                    Name = "Sarah Billingsley",
                    Email = "sarah.billingsley@wehirem.com",
                    Role = ContactRole.Recruiter,
                    Phone = "8765 0641",
                    Organization = "We Hire 'Em Recruiters"
                };
            }
        }
    }
}
