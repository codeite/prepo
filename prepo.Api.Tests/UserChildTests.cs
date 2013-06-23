using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using prepo.Client;

namespace prepo.Api.Tests.Acceptance
{
    [TestFixture]
    public class UserChildTests
    {
        private PrepoRestClient _client;

        [SetUp]
        public void SetUp()
        {
            _client = new PrepoRestClient();
            _client.GetRoot().Delete();
        }

        [Test]
        public void CanSeeOwnedPersonas()
        {
            // Arrange
            var userSam = new UserBuilder("sam");
            var sam = _client.GetRoot().FollowRel("users").PutToRel("user", new {id = userSam.Id}, userSam.BuildAsContent());

            // Act

            // Assert
        }
    }
}
