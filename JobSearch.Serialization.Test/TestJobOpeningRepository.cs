using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobSearch.Test;
using NUnit.Framework;

namespace JobSearch.Serialization.Test
{
    [TestFixture]
    public class TestJobOpeningRepository : TestEntityFrameworkRepository<JobSearchContext, int, JobOpening>
    {
        public TestJobOpeningRepository()
            : base(
                testItem1: TestJobOpenings.MicrosoftCeo,
                testItem2: TestJobOpenings.Empty,
                identifyingProperty: jobOpening => jobOpening.Id,
                uniqueProperty: jobOpening => jobOpening.Url,
                varyItem: jobOpening => jobOpening.Notes = (jobOpening.Notes != null ? jobOpening.Notes + "foo" : "foo"))
        {
            // Do nothing    
        }
    }
}
