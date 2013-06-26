using System.Net;
using FluentAssertions;
using NUnit.Framework;
using prepo.Api.Tests.Builders;
using prepo.Client;

namespace prepo.Api.Tests.Acceptance
{
    [TestFixture]
    public class SubtlyUserTests
    {
        [Test]
        public void PuttingExistingRespondsWith200()
        {
            // Arrange
            var usersRt = new PrepoRestClient().GetRoot().FollowRel("users");
            const int userId = 123;
            var user = new UserBuilder(userId).BuildAsContent();

            // Act
            var firstResponse = usersRt.PutToRel("user", new { id = userId }, user).Response;
            var secondsResponse = usersRt.PutToRel("user", new { id = userId }, user).Response;

            // Assert
            firstResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            secondsResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        }
    }
}