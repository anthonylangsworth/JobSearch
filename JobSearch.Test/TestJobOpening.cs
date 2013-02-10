using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace JobSearch
{
    [TestFixture]
    public class TestJobOpening
    {
        [Test]
        public void TestCreation()
        {
            JobOpening jobOpening;
            jobOpening = new JobOpening(42);
            Assert.That(jobOpening.Id, Is.EqualTo(42));
        }
    }
}
