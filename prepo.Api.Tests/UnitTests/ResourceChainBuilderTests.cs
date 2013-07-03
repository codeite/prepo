using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using prepo.Api.Controllers;
using prepo.Api.Resources;
using prepo.Api.Services;

namespace prepo.Api.Tests.UnitTests
{
    [TestFixture]
    public class ResourceChainBuilderTests
    {
        private ResourceChainBuilder _builder;

        [SetUp]
        public void SetUp()
        {
            _builder = new ResourceChainBuilder();
        }

        [Test]
        public void BlankPathReturnsRoot()
        {
            // Arrange

            // Act
            var resource = _builder.Build("/");
            var head = resource.Head;

            // Assert
            resource.Should().BeOfType<RootResource>();
            head.Should().BeOfType<RootResource>();
            resource.Should().BeSameAs(head);
        }

        [TestCase("/users")]
        [TestCase("/users/")]
        [TestCase("/Users")]
        [TestCase("/usErs/")]
        public void CanGetUsersCollectionResource(string path)
        {
            // Arrange

            // Act
            var root = _builder.Build(path);
            var head = root.Head;

            // Assert
            root.Should().BeOfType<RootResource>();
            head.Should().BeOfType<UserCollectionResource>();

            head.Owner.Should().BeOfType<RootResource>();
            head.Owner.Should().Be(root);
        }

        [TestCase("/users/sam", "sam")]
        [TestCase("/users/robert", "robert")]
        [TestCase("/users/sam/", "sam")]
        [TestCase("/users/robert/", "robert")]
        public void CanGetUsersItemResource(string path, string id)
        {
            // Arrange

            // Act
            var root = _builder.Build(path);
            var head = root.Head;

            // Assert
            root.Should().BeOfType<RootResource>();
            head.Should().BeOfType<UserItemResource>();

            var user = head as UserItemResource;
            user.Id.Should().Be(id);
        }

        [TestCase("/users/Sam/personas")]
        [TestCase("/users/Sam/personas/")]
        public void CanGetPersonaCollectionResource(string path)
        {
            // Arrange

            // Act
            var root = _builder.Build(path);
            var head = root.Head;

            // Assert
            root.Should().BeOfType<RootResource>();
            head.Should().BeOfType<PersonaCollectionResource>();
        }

        [TestCase("/users/Sam/personas/MrFantastic", "Sam", "MrFantastic")]
        [TestCase("/users/Sam/personas/MrFantastic/", "Sam", "MrFantastic")]
        [TestCase("/users/Robert/personas/Bob", "Robert", "Bob")]
        [TestCase("/users/Robert/personas/Bob/", "Robert", "Bob")]
        public void CanGetPersonasItemResource(string path, string parentId, string id)
        {
            // Arrange

            // Act
            var root = _builder.Build(path);
            var head = root.Head;

            // Assert
            root.Should().BeOfType<RootResource>();
            head.Should().BeOfType<PersonaItemResource>();

            var persona = head as PersonaItemResource;
            persona.Id.Should().Be(id);
        }
    }
}
