using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using JobSearch.Test;
using NUnit.Framework;

namespace JobSearch.Serialization.Test
{
    [TestFixture]
    public class TestContactRepository: TestEntityFrameworkRepository<JobSearchContext, int, Contact>
    {
        public TestContactRepository()
            : base(
                testItem1: TestContacts.PeterSmith,
                testItem2: TestContacts.SarahBillingsley,
                identifyingProperty: contact => contact.Id,
                uniqueProperty: contact => contact.Name,
                varyItem: contact => contact.Notes = (contact.Notes != null ? contact.Notes + "foo" : "foo"))
        {
            // Do nothing    
        }
    }
}

