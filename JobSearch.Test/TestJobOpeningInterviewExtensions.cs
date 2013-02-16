using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace JobSearch.Test
{
    [TestFixture]
    public class TestJobOpeningInterviewExtensions
    {
        [Test]
        public void TestAddInterview()
        {
            JobOpening jobOpening;
            DateTime start;
            TimeSpan duration;
            string description;
            Contact contact;

            jobOpening = new JobOpening();
            start = DateTime.Now;
            duration = TimeSpan.FromMinutes(42);
            description = "foo";
            contact = TestContacts.PeterSmith;

            jobOpening.AddInterview(start, duration, contact, description);

            Assert.That(jobOpening.Activities.Count(), Is.EqualTo(2), "Incorrect activity count");
            Assert.That(jobOpening.Activities, Contains.Item(new Activity(start, duration, contact, description)),
                "Missing interview activity");
            Assert.That(jobOpening.Activities, Contains.Item(new Activity(
                start + JobOpeningInterviewExtensions.FollowUpDelay,
                 JobOpeningInterviewExtensions.FollowUpDuration, contact,
                 JobOpeningInterviewExtensions.FollowUpDescription)),
                "Missing followup activity");
        }

        [Test]
        public void TestAddInterview_NullContact()
        {
            Assert.That(() => new JobOpening().AddInterview(DateTime.Now, TimeSpan.FromMinutes(1), null, "foo"),
                Throws.TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("contact"));
        }

        [Test]
        public void TestAddInterview_NegativeDuration()
        {
            Assert.That(() => new JobOpening().AddInterview(DateTime.Now, TimeSpan.FromMinutes(-1), TestContacts.PeterSmith, "foo"),
                Throws.TypeOf<ArgumentException>().And.Property("ParamName").EqualTo("duration"));
        }

        [Test]
        public void TestAddInterview_NullDescription()
        {
            Assert.That(() => new JobOpening().AddInterview(DateTime.Now, TimeSpan.FromMinutes(1), TestContacts.PeterSmith, null),
                Throws.TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("description"));
        }

        [Test]
        public void TestAddInterview_EmptyDescription()
        {
            Assert.That(() => new JobOpening().AddInterview(DateTime.Now, TimeSpan.FromMinutes(1), TestContacts.PeterSmith, string.Empty),
                Throws.TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("description"));
        }

        [Test]
        public void TestAddInterview_WhiteSpaceDescription()
        {
            Assert.That(() => new JobOpening().AddInterview(DateTime.Now, TimeSpan.FromMinutes(1), TestContacts.PeterSmith, " "),
                Throws.TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("description"));
        }
    }
}
