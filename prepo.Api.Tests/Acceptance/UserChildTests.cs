using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
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
        public void CanSeeOwnedPersonasLink()
        {
            // Arrange
            var userSam = new UserBuilder("sam");

            // Act
            var sam = _client.GetRoot().FollowRel("users").PutToRel("user", new { id = userSam.Id }, userSam.BuildAsContent());
            Console.WriteLine(sam.Body);
            var json = sam.BodyAsJson();

            // Assert
            var links = json["_links"] as Dictionary<string, dynamic>;
            links.Should().ContainKey("self");
            links.Should().ContainKey("personas", reason: "User object did not contain link to owned personas");
            var personas = links["personas"] as Dictionary<string, dynamic>;
            personas.Should().ContainKey("href");
            ((string)personas["href"]).Should().Be("/users/sam/personas");
        }

        [Test]
        public void CanFollowOwnedPersonasLink()
        {
            // Arrange
            var userSam = new UserBuilder("sam");
            var sam = _client.GetRoot().FollowRel("users").PutToRel("user", new { id = userSam.Id }, userSam.BuildAsContent());

            // Act
            var samOwnedPersonas = sam.FollowRel("personas");
            Console.WriteLine(samOwnedPersonas.Body);

            // Assert
        }
    }
}
