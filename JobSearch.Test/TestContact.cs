using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace JobSearch.Test
{
    [TestFixture]
    public class TestContact
    {
        [Test]
        public void TestCreation()
        {
            Contact contact;
            const string name = "name";
            const string email = "E-mail";
            const int id = 42;
            const string notes = "notes";
            const string organization = "organization";
            const string phone = "phone";
            const ContactRole role = ContactRole.HumanResources;

            contact = new Contact()
                {
                    Name = name,
                    Email = email,
                    Id = id,
                    Notes = notes,
                    Organization = organization,
                    Phone = phone,
                    Role = role
                };

            Assert.That(contact.Name, Is.EqualTo(name), "Incorrect name");
            Assert.That(contact.Email, Is.EqualTo(email), "Incorrect E-mail");
            Assert.That(contact.Id, Is.EqualTo(id), "Incorrect id");
            Assert.That(contact.Notes, Is.EqualTo(notes), "Incorrect notes");
            Assert.That(contact.Organization, Is.EqualTo(organization), "Incorrect organization");
            Assert.That(contact.Phone, Is.EqualTo(phone), "Incorrect phone");
            Assert.That(contact.Role, Is.EqualTo(role), "Incorrect role");
        }
    }
}
