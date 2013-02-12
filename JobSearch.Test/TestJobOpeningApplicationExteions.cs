using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace JobSearch.Test
{
    [TestFixture]
    public class TestJobOpeningApplicationExteions
    {
        [Test]
        public void TestApply()
        {
            JobOpening jobOpening;
            DateTime applicationTime;
            Contact contact;

            jobOpening = new JobOpening();
            applicationTime = DateTime.Now;
            contact = TestContacts.PeterSmith;

            jobOpening.Apply(applicationTime, contact);

            Assert.That(jobOpening.Activities.Count(), Is.EqualTo(2), "Incorrect activity count");
            Assert.That(jobOpening.Activities, Contains.Item(new Activity(applicationTime, TimeSpan.Zero, contact, 
                JobOpeningApplicationExtensions.ApplicationDescription, true)),
                "Missing application activity");
            Assert.That(jobOpening.Activities, Contains.Item(new Activity(
                applicationTime + JobOpeningApplicationExtensions.ApplicationFollowUpDelay,
                JobOpeningApplicationExtensions.FollowUpDuration, contact,
                JobOpeningApplicationExtensions.FollowUpDescription)),
                "Missing followup activity");
        }

        [Test]
        public void TestAddInterview_NullJobOpening()
        {
            JobOpening jobOpening = null;
            Assert.That(() => jobOpening.Apply(DateTime.Now, TestContacts.PeterSmith),
                Throws.TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("jobOpening"));
        }

        [Test]
        public void TestAddInterview_NullContact()
        {
            Assert.That(() => new JobOpening().Apply(DateTime.Now, null),
                Throws.TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("contact"));
        }
    }
}
