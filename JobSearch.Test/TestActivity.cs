using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace JobSearch.Test
{
    [TestFixture]
    public class TestActivity
    {
        [Test]
        public void TestCreation()
        {
            Activity activity;
            DateTime start = DateTime.Now;
            TimeSpan duration = TimeSpan.FromMinutes(59);
            const string description = "foo";
            Contact contact  = new Contact();
            int id = 42;

            activity = new Activity(id, start, duration, contact, description);

            Assert.That(activity.Completed, Is.False, "Activity is completed");
            Assert.That(activity.Contact, Is.EqualTo(contact), "Incorrect contact");
            Assert.That(activity.Description, Is.EqualTo(description), "Incorrect description");
            Assert.That(activity.Duration, Is.EqualTo(duration), "Incorrect duration");
            Assert.That(activity.Id, Is.EqualTo(id), "Incorrect id");
            Assert.That(activity.Start, Is.EqualTo(start), "Incorrect start");
        }

        [Test]
        public void TestCreation_NullContact()
        {
            Assert.That(() => new Activity(0, DateTime.Now, TimeSpan.FromHours(1), null, "foo"),
                Throws.TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("contact"));
        }

        [Test]
        public void TestCreation_NegativeDuration()
        {
            Assert.That(() => new Activity(0, DateTime.Now, TimeSpan.FromHours(-1), new Contact(), "foo"),
                Throws.TypeOf<ArgumentException>().And.Property("ParamName").EqualTo("duration"));
        }

        [Test]
        public void TestCreation_NullDescription()
        {
            Assert.That(() => new Activity(0, DateTime.Now, TimeSpan.FromHours(1), new Contact(), null),
                Throws.TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("description"));
        }

        [Test]
        public void TestCreation_EmptyDescription()
        {
            Assert.That(() => new Activity(0, DateTime.Now, TimeSpan.FromHours(1), new Contact(), string.Empty),
                Throws.TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("description"));
        }

        [Test]
        public void TestCreation_WhiteSpaceDescription()
        {
            Assert.That(() => new Activity(0, DateTime.Now, TimeSpan.FromHours(1), new Contact(), " "),
                Throws.TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("description"));
        }
    }
}
