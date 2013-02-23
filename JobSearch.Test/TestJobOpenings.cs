using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSearch.Test
{
    /// <summary>
    /// Test instances of <see cref="JobOpening"/>.
    /// </summary>
    public static class TestJobOpenings
    {
        /// <summary>
        /// An empty (no activities or contacts) job opening.
        /// </summary>
        public static JobOpening Empty
        {
            get
            {
                JobOpening result;

                result = new JobOpening()
                    {
                        AdvertisedDate = DateTime.Now,
                        Notes = "Notes",
                        Organization = "Acme Inc",
                        Title = "Road runner catcher",
                        Url = "www.wikipedia.org"
                    };

                return result;
            }
        }

        /// <summary>
        /// An job opening at Microsoft.
        /// </summary>
        public static JobOpening MicrosoftCeo
        {
            get
            {
                JobOpening result;

                result = new JobOpening()
                {
                    AdvertisedDate = DateTime.Now,
                    Notes = "Need Steve Balmer replacement. Quick!",
                    Organization = "Microsoft",
                    Title = "CEO",
                    Url = "www.microsoft.org"
                };
                result.Apply(DateTime.Now, TestContacts.PeterSmith);
                result.AddInterview(DateTime.Now + TimeSpan.FromDays(1), TimeSpan.FromMinutes(130), 
                    TestContacts.SarahBillingsley, "First round interview.");

                return result;
            }
        }

    }
}
