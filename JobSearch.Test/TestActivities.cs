using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSearch.Test
{
    /// <summary>
    /// Test instances of <see cref="Activity"/>.
    /// </summary>
    public static class TestActivities
    {
        static TestActivities ()
        {
            JobInterview = new Activity(83, DateTime.Now, TimeSpan.FromMinutes(55),
                TestContacts.PeterSmith, "Job Interview 1");
            FollowUpRecruiter =  new Activity(203, DateTime.Now, TimeSpan.FromMinutes(117),
                TestContacts.SarahBillingsley, "Follow-up with recruiter");
        }

        /// <summary>
        /// A test contact
        /// </summary>
        public static Activity JobInterview
        {
            get;
            private set;
        }

        /// <summary>
        /// A test contact
        /// </summary>
        public static Activity FollowUpRecruiter
        {
            get;
            private set;
        }
    }
}
