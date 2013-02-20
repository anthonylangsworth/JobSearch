using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace JobSearch.Serialization.Test
{
    [TestFixture]
    public class TestEntityFrameworkRepositoryHelper
    {
        [Test]
        public void TestGetExistsExpression()
        {
            Expression<Func<Contact, bool>> expression;

            expression = EntityFrameworkRepositoryHelper.GetIdMatchesExpression<Contact, int>(42, "Id");

            Assert.That(expression.ToString(), Is.EqualTo("item => (item.Id == 42)"));
        }
    }
}
