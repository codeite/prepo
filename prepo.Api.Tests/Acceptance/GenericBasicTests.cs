using System;
using Everest.Content;
using Everest.Status;
using FluentAssertions;
using NUnit.Framework;
using prepo.Api.Tests.Builders;
using prepo.Api.Tests.Helpers;
using prepo.Client;

namespace prepo.Api.Tests.Acceptance
{
    public abstract class GenericBasicTests<T>
        where T : IResourceBuilder
    {
        public string[] LinkRelList { get; set; }
        public string UrlPrefix { get; set; }
        public string ResourceName { get; set; }
        public string ResourceList { get; set; }
        public string FullResourceJson { get; set; }
        public string FullResource { get; set; }
        public string FullResourceId { get; set; }
        public ResourceBuilder<T> Builder { get; set; }

        protected abstract string MakeResource(string id);
        protected virtual void OnSetup(PrepoRestClient client)
        {
            
        }

        private PrepoRestClient _client;

        [SetUp]
        public void SetUp()
        {
            _client = new PrepoRestClient();
            _client.GetRoot().Delete();

            OnSetup(_client);
        }

        [Test]
        public void GetFirstTenResources()
        {
            // Arrange
            var client = new PrepoRestClient();
            var resource = client
                .GetRoot()
                .FollowRelList(LinkRelList);

            AddResource(resource, 1, 20);

            // Act
            var resources = resource
                .FollowRel("first");

            // Assert
            resources.Body.ShouldBeJson(@"
            {
                '_links': { 
                    'self': {'href': '" + UrlPrefix + @"'},
                    '" + ResourceName + @"': {'href': '" + UrlPrefix + @"/{id}'},
                    'first': {'href': '" + UrlPrefix + @"?page=1&count=10'},
                    'next': {'href': '" + UrlPrefix + @"?page=2&count=10'},
                    'page': {'href': '" + UrlPrefix + @"?page={page}&count={count}'},
                    '" + ResourceList + @"':[
                        { 'href': '" + UrlPrefix + @"/1'},
                        { 'href': '" + UrlPrefix + @"/2'},
                        { 'href': '" + UrlPrefix + @"/3'},
                        { 'href': '" + UrlPrefix + @"/4'},
                        { 'href': '" + UrlPrefix + @"/5'},
                        { 'href': '" + UrlPrefix + @"/6'},
                        { 'href': '" + UrlPrefix + @"/7'},
                        { 'href': '" + UrlPrefix + @"/8'},
                        { 'href': '" + UrlPrefix + @"/9'},
                        { 'href': '" + UrlPrefix + @"/10'}   
                    ]
                }
            }");
        }

        private void AddResource(ApiResource resourcesRd, int first, int? last = null)
        {
            var realLast = last ?? first;
            for (var i = first; i <= realLast; i++)
            {
                resourcesRd.PutToRel(ResourceName, new { id = i }, Builder.New().WithId(i).BuildAsContent());
            }
        }

        [Test]
        public void GetSecondsFiveResources()
        {
            // Arrange
            var client = new PrepoRestClient();
            var resourcesRd = client.GetRoot()
                                .FollowRelList(LinkRelList);
            AddResource(resourcesRd, 1, 20);

            // Act

            var resources = resourcesRd.FollowRel("page", new { page = 2, count = 5 });

            // Assert
            resources.Body.ShouldBeJson(@"
            {
                '_links': { 
                    'self': {'href': '" + UrlPrefix + @"'},
                    '" + ResourceName + @"': {'href': '" + UrlPrefix + @"/{id}'},
                    'first': {'href': '" + UrlPrefix + @"?page=1&count=10'},
                    'next': {'href': '" + UrlPrefix + @"?page=3&count=5'},
                    'prev': {'href': '" + UrlPrefix + @"?page=1&count=5'},
                    'page': {'href': '" + UrlPrefix + @"?page={page}&count={count}'},
                    '" + ResourceList + @"':[
                        { 'href': '" + UrlPrefix + @"/6'},
                        { 'href': '" + UrlPrefix + @"/7'},
                        { 'href': '" + UrlPrefix + @"/8'},
                        { 'href': '" + UrlPrefix + @"/9'},
                        { 'href': '" + UrlPrefix + @"/10'}   
                    ]
                }
            }");
        }


        [Test]
        public void GetResources()
        {
            // Arrange
            var client = new PrepoRestClient();

            // Act
            var resources = client.GetRoot().FollowRelList(LinkRelList);

            // Assert
            resources.Body.ShouldBeJson(@"
            {
                '_links': { 
                    'self': {'href': '" + UrlPrefix + @"'},
                    '" + ResourceName + @"': {'href': '" + UrlPrefix + @"/{id}'},
                    'first': {'href': '" + UrlPrefix + @"?page=1&count=10'},
                    'page': {'href': '" + UrlPrefix + @"?page={page}&count={count}'},
                }
            }");
        }

        [Test]
        public void GetResource()
        {
            // Arrange
            var client = new PrepoRestClient();
            var resourcesRd = client.GetRoot().FollowRelList(LinkRelList);
            AddResource(resourcesRd, 15);

            // Act
            var resources = resourcesRd.FollowRel(ResourceName, new { id = 15 });

            // Assert
            resources.Body.ShouldBeJson(MakeResource("15"));
        }

        [Test]
        public void PutResource()
        {
            // Arrange
            var client = new PrepoRestClient();

            // Act
            var resource = client
                .GetRoot()
                .FollowRelList(LinkRelList)
                .PutToRel(ResourceName, new { id =FullResourceId }, new JsonBodyContent(FullResourceJson));

            // Assert
            resource.Response.Location.Should().Be("http://dev.prepo.codeite.com" + UrlPrefix + @"/" + FullResourceId);
            resource.Body.ShouldBeJson(FullResource);
        }

        [Test]
        public void PostResource()
        {
            // Arrange
            var client = new PrepoRestClient();

            // Act
            var resource = client
                .GetRoot()
                .PostToRel(ResourceList, null, new JsonBodyContent("{'id':77}"));

            // Assert
            resource.Location.Should().Be("http://dev.prepo.codeite.com" + UrlPrefix + @"/77");
        }

        [Test]
        public void PutThenGetResource()
        {
            // Arrange
            var resourceResource = Builder.New().WithId(65).BuildAsContent();
            var client = new PrepoRestClient();
            var resources = client
                .GetRoot()
                .FollowRelList(LinkRelList);

            // Act
            var resource = resources.PutToRel(ResourceName, new { id = 65 }, resourceResource);

            // Assert
            resource.Response.Location.Should().Be("http://dev.prepo.codeite.com" + UrlPrefix + @"/65");


            // Act

            var resourceGet = resources.FollowRel(ResourceName, new { id = 65 });

            // Assert
            resourceGet.Body.ShouldBeJson(MakeResource("65"));
        }

        [Test]
        public void PutRespondsWithResource()
        {
            // Arrange
            var resourceResource = Builder.New().WithId(65).BuildAsContent();
            var client = new PrepoRestClient();
            var resources = client
                .GetRoot()
                .FollowRelList(LinkRelList);

            // Act
            var resourceFromPut = resources.PutToRel(ResourceName, new { id = 65 }, resourceResource);
            var resourceFromGet = resources.FollowRel(ResourceName, new { id = 65 });

            // Assert
            resourceFromPut.Body.ShouldBeJson(resourceFromGet.Body);
        }

        [Test]
        public void PostRespondsWithResource()
        {
            // Arrange
            var resourceResource = Builder.New().WithId(66).BuildAsContent();
            var client = new PrepoRestClient();
            var resources = client
                .GetRoot()
                .FollowRelList(LinkRelList);

            // Act
            var resourceFromPost = resources.Post(resourceResource);
            var resourceFromGet = resources.FollowRel(ResourceName, new { id = 66 });

            // Assert
            resourceFromPost.Body.ShouldBeJson(resourceFromGet.Body);
        }

        [Test]
        public void PostThenGetResource()
        {
            // Arrange
            var resourceResource = Builder.New().WithId(96).BuildAsContent();
            var client = new PrepoRestClient();
            var resources = client
                .GetRoot()
                .FollowRelList(LinkRelList);

            // Act
            var location = resources.Post(resourceResource).Location;

            // Assert
            location.Should().Be("http://dev.prepo.codeite.com" + UrlPrefix + @"/96");


            // Act
            var resource = resources.FollowRel(ResourceName, new { id = 96 });

            // Assert
            resource.Body.ShouldBeJson(MakeResource("96"));
        }

        [Test]
        public void DeleteResource()
        {
            // Arrange
            var client = new PrepoRestClient();
            var resourcesRd = client
                .GetRoot()
                .FollowRelList(LinkRelList);

            AddResource(resourcesRd, 876);

            // Act
            resourcesRd.FollowRel(ResourceName, new { id = 876 });
            resourcesRd.DeleteRel(ResourceName, new { id = 876 });
            Action act = () => resourcesRd.FollowRel(ResourceName, new { id = 876 });

            // Assert
            act.ShouldThrow<UnexpectedStatusException>()
               .Where(x => x.StatusCode == 404);
        }
    }
}