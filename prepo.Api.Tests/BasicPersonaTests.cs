using System;
using Everest.Content;
using Everest.Status;
using FluentAssertions;
using NUnit.Framework;
using prepo.Client;

namespace prepo.Api.Tests
{
    [TestFixture]
    public class BasicPersonaTests
    {
        private PrepoRestClient _client;

        [SetUp]
        public void SetUp()
        {
            _client = new PrepoRestClient();
            _client.GetRoot().Delete();
        }

        [Test]
        public void GetFirstTenPersonas()
        {
            // Arrange
            var client = new PrepoRestClient();
            var personasRd = client
                .GetRoot()
                .FollowRel("personas"); ;

            AddPersonas(personasRd, 1, 20);

            // Act
            var personas = personasRd
                .FollowRel("first");

            // Assert
            Console.WriteLine(personas.Body);
            personas.Body.ShouldBeJson(@"
            {
                '_links': { 
                    'self': {'href': '/personas'},
                    'persona': {'href': '/personas/{id}'},
                    'first': {'href': '/personas?page=1&count=10'},
                    'next': {'href': '/personas?page=2&count=10'},
                    'page': {'href': '/personas?page={page}&count={count}'},
                    'personas':[
                        { 'href': '/personas/1'},
                        { 'href': '/personas/2'},
                        { 'href': '/personas/3'},
                        { 'href': '/personas/4'},
                        { 'href': '/personas/5'},
                        { 'href': '/personas/6'},
                        { 'href': '/personas/7'},
                        { 'href': '/personas/8'},
                        { 'href': '/personas/9'},
                        { 'href': '/personas/10'}   
                    ]
                }
            }");
        }

        private static void AddPersonas(ApiResource personasRd, int first, int? last = null)
        {
            var realLast = last ?? first;
            for (var i = first; i <= realLast; i++)
            {
                personasRd.PutToRel("persona", new {id = i}, new PersonaBuilder(i).BuildAsContent());
            }
        }

        [Test]
        public void GetSecondsFivePersonas()
        {
            // Arrange
            var client = new PrepoRestClient();
            var personasRd = client.GetRoot()
                                .FollowRel("personas");
            AddPersonas(personasRd, 1, 20);

            // Act

            var personas = personasRd.FollowRel("page", new { page = 2, count = 5 });

            // Assert
            Console.WriteLine(personas.Body);
            personas.Body.ShouldBeJson(@"
            {
                '_links': { 
                    'self': {'href': '/personas'},
                    'persona': {'href': '/personas/{id}'},
                    'first': {'href': '/personas?page=1&count=10'},
                    'page': {'href': '/personas?page={page}&count={count}'},
                    'next': {'href': '/personas?page=3&count=5'},
                    'prev': {'href': '/personas?page=1&count=5'},
                    'personas':[
                        { 'href': '/personas/6'},
                        { 'href': '/personas/7'},
                        { 'href': '/personas/8'},
                        { 'href': '/personas/9'},
                        { 'href': '/personas/10'}   
                    ]
                }
            }");
        }


        [Test]
        public void GetPersonas()
        {
            // Arrange
            var client = new PrepoRestClient();

            // Act
            var personas = client.GetRoot().FollowRel("personas");

            // Assert
            Console.WriteLine(personas.Body);
            personas.Body.ShouldBeJson(@"
            {
                '_links': { 
                    'self': {'href': '/personas'},
                    'persona': {'href': '/personas/{id}'},
                    'first': {'href': '/personas?page=1&count=10'},
                    'page': {'href': '/personas?page={page}&count={count}'},
                }
            }");
        }

        [Test]
        public void GetPersona()
        {
            // Arrange
            var client = new PrepoRestClient();
            var personasRd = client.GetRoot().FollowRel("personas");
            AddPersonas(personasRd, 15);

            // Act
            var personas = personasRd.FollowRel("persona", new {id = 15});

            // Assert
            Console.WriteLine(personas.Body);
            personas.Body.ShouldBeJson(@"
            {
                '_links': { 
                    'self': {'href': '/personas/15'}
                },
                'id' : '15'
            }");
        }

        [Test]
        public void PutPersona()
        {
            // Arrange
            var client = new PrepoRestClient();

            // Act
            var location = client
                .GetRoot()
                .FollowRel("personas")
                .PutToRel("persona", new { id = 22 }, new JsonBodyContent("{'id':22}"));

            // Assert
            location.Should().Be("http://dev.prepo.codeite.com/personas/22");
        }

        [Test]
        public void PostPersona()
        {
            // Arrange
            var client = new PrepoRestClient();

            // Act
            var location = client
                .GetRoot()
                .PostToRel("personas", null, new JsonBodyContent("{'id':77}"));

            // Assert
            location.Should().Be("http://dev.prepo.codeite.com/personas/77");
        }

        [Test]
        public void PutThenGetPersona()
        {
            // Arrange
            var personaResource = new PersonaBuilder(65).BuildAsContent();
            var client = new PrepoRestClient();
            var personas = client
                .GetRoot()
                .FollowRel("personas");

            // Act
            var location = personas.PutToRel("persona", new { id = 65 }, personaResource);

            // Assert
            location.Should().Be("http://dev.prepo.codeite.com/personas/65");


            // Act

            var persona = personas.FollowRel("persona", new {id = 65});

            // Assert
            Console.WriteLine(persona.Body);
            persona.Body.ShouldBeJson(@"
            {
                '_links': { 
                    'self': {'href': '/personas/65'}
                },
                'id' : '65'
            }");
        }

        [Test]
        public void PostThenGetPersona()
        {
            // Arrange
            var personaResource = new PersonaBuilder(96).BuildAsContent();
            var client = new PrepoRestClient();
            var personas = client
                .GetRoot()
                .FollowRel("personas");

            // Act
            var location = personas.Post(personaResource);

            // Assert
            location.Should().Be("http://dev.prepo.codeite.com/personas/96");


            // Act
            var persona = personas.FollowRel("persona", new { id = 96 });

            // Assert
            Console.WriteLine(persona.Body);
            persona.Body.ShouldBeJson(@"
            {
                '_links': { 
                    'self': {'href': '/personas/96'}
                },
                'id' : '96'
            }");
        }

        [Test]
        public void DeletePersona()
        {
            // Arrange
            var client = new PrepoRestClient();
            var personasRd = client
                .GetRoot()
                .FollowRel("personas");

            AddPersonas(personasRd, 876);

            // Act
            personasRd.FollowRel("persona", new { id = 876 });
            personasRd.DeleteRel("persona", new { id = 876 });
            Action act = () => personasRd.FollowRel("persona", new { id = 876 });

            // Assert
            act.ShouldThrow<UnexpectedStatusException>()
               .Where(x => x.StatusCode == 404);
        }
    }
}