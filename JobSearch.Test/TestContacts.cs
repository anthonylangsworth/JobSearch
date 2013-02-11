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
                return new Contact()
                {
                    Name = "Peter Smith",
                    Email = "psmith@uberrecruiters.com",
                    Phone = "1234 5678",
                    Organization = "Uber Recruiters"
                };
            }
        }
    }
}
