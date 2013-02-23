using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace JobSearch.Serialization.Test
{
    [TestFixture]
    public class TestActivityRepository : TestEntityFrameworkRepository<JobSearchContext, int, Activity>
    {
        public TestActivityRepository()
            : base(
                testItem1: null, 
                testItem2: null, 
                identifyingProperty: activity => activity.Id, 
                uniqueProperty: activity => activity.Description, 
                varyItem: activity => activity.Completed = !activity.Completed)
        {
            // Do nothing    
        }
    }
}
