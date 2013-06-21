using System;
using Everest.Content;
using Everest.Status;
using FluentAssertions;
using NUnit.Framework;
using prepo.Client;

namespace prepo.Api.Tests
{
    [TestFixture]
    public class BasicUserTests
    {
        private PrepoRestClient _client;

        [SetUp]
        public void SetUp()
        {
            _client = new PrepoRestClient();
            _client.GetRoot().Delete();
        }

        [Test]
        public void GetFirstTenUsers()
        {
            // Arrange
            var client = new PrepoRestClient();
            var usersRd = client
                .GetRoot()
                .FollowRel("users"); ;

            AddUsers(usersRd, 1, 20);

            // Act
            var users = usersRd
                .FollowRel("first");

            // Assert
            Console.WriteLine(users.Body);
            users.Body.ShouldBeJson(@"
            {
                '_links': { 
                    'self': {'href': '/users'},
                    'user': {'href': '/users/{id}'},
                    'first': {'href': '/users?page=1&count=10'},
                    'page': {'href': '/users?page={page}&count={count}'},
                    'users':[
                        { 'href': '/users/1'},
                        { 'href': '/users/2'},
                        { 'href': '/users/3'},
                        { 'href': '/users/4'},
                        { 'href': '/users/5'},
                        { 'href': '/users/6'},
                        { 'href': '/users/7'},
                        { 'href': '/users/8'},
                        { 'href': '/users/9'},
                        { 'href': '/users/10'}   
                    ]
                }
            }");
        }

        private static void AddUsers(ApiResource usersRd, int first, int? last = null)
        {
            var realLast = last ?? first;
            for (var i = first; i <= realLast; i++)
            {
                usersRd.PutToRel("user", new {id = i}, new UserBuilder(i).BuildAsContent());
            }
        }

        [Test]
        public void GetSecondsFiveUsers()
        {
            // Arrange
            var client = new PrepoRestClient();
            var usersRd = client.GetRoot()
                                .FollowRel("users");
            AddUsers(usersRd, 1, 20);

            // Act

            var users = usersRd.FollowRel("page", new { page = 2, count = 5 });

            // Assert
            Console.WriteLine(users.Body);
            users.Body.ShouldBeJson(@"
            {
                '_links': { 
                    'self': {'href': '/users'},
                    'user': {'href': '/users/{id}'},
                    'first': {'href': '/users?page=1&count=10'},
                    'page': {'href': '/users?page={page}&count={count}'},
                    'users':[
                        { 'href': '/users/6'},
                        { 'href': '/users/7'},
                        { 'href': '/users/8'},
                        { 'href': '/users/9'},
                        { 'href': '/users/10'}   
                    ]
                }
            }");
        }


        [Test]
        public void GetUsers()
        {
            // Arrange
            var client = new PrepoRestClient();

            // Act
            var users = client.GetRoot().FollowRel("users");

            // Assert
            Console.WriteLine(users.Body);
            users.Body.ShouldBeJson(@"
            {
                '_links': { 
                    'self': {'href': '/users'},
                    'user': {'href': '/users/{id}'},
                    'first': {'href': '/users?page=1&count=10'},
                    'page': {'href': '/users?page={page}&count={count}'},
                }
            }");
        }

        [Test]
        public void GetUser()
        {
            // Arrange
            var client = new PrepoRestClient();
            var usersRd = client.GetRoot().FollowRel("users");
            AddUsers(usersRd, 15);

            // Act
            var users = usersRd.FollowRel("user", new {id = 15});

            // Assert
            Console.WriteLine(users.Body);
            users.Body.ShouldBeJson(@"
            {
                '_links': { 
                    'self': {'href': '/users/15'}
                },
                'id' : '15',
                'name' : 'Number 15'
            }");
        }

        [Test]
        public void PutUser()
        {
            // Arrange
            var client = new PrepoRestClient();

            // Act
            var location = client
                .GetRoot()
                .FollowRel("users")
                .PutToRel("user", new { id = 22 }, new JsonBodyContent("{'id':22}"));

            // Assert
            location.Should().Be("http://dev.prepo.codeite.com/users/22");
        }

        [Test]
        public void PostUser()
        {
            // Arrange
            var client = new PrepoRestClient();

            // Act
            var location = client
                .GetRoot()
                .PostToRel("users", null, new JsonBodyContent("{'id':77}"));

            // Assert
            location.Should().Be("http://dev.prepo.codeite.com/users/77");
        }

        [Test]
        public void PutThenGetUser()
        {
            // Arrange
            var userResource = new UserBuilder(65).BuildAsContent();
            var client = new PrepoRestClient();
            var users = client
                .GetRoot()
                .FollowRel("users");

            // Act
            var location = users.PutToRel("user", new { id = 65 }, userResource);

            // Assert
            location.Should().Be("http://dev.prepo.codeite.com/users/65");


            // Act

            var user = users.FollowRel("user", new {id = 65});

            // Assert
            Console.WriteLine(user.Body);
            user.Body.ShouldBeJson(@"
            {
                '_links': { 
                    'self': {'href': '/users/65'}
                },
                'id' : '65',
                'name' : 'Number 65'
            }");
        }

        [Test]
        public void PostThenGetUser()
        {
            // Arrange
            var userResource = new UserBuilder(96).BuildAsContent();
            var client = new PrepoRestClient();
            var users = client
                .GetRoot()
                .FollowRel("users");

            // Act
            var location = users.Post(userResource);

            // Assert
            location.Should().Be("http://dev.prepo.codeite.com/users/96");


            // Act
            var user = users.FollowRel("user", new { id = 96 });

            // Assert
            Console.WriteLine(user.Body);
            user.Body.ShouldBeJson(@"
            {
                '_links': { 
                    'self': {'href': '/users/96'}
                },
                'id' : '96',
                'name' : 'Number 96'
            }");
        }

        [Test]
        public void DeleteUser()
        {
            // Arrange
            var client = new PrepoRestClient();
            var usersRd = client
                .GetRoot()
                .FollowRel("users");

            AddUsers(usersRd, 876);

            // Act
            usersRd.FollowRel("user", new { id = 876 });
            usersRd.DeleteRel("user", new { id = 876 });
            Action act = () => usersRd.FollowRel("user", new { id = 876 });

            // Assert
            act.ShouldThrow<UnexpectedStatusException>()
               .Where(x => x.StatusCode == 404);
        }
    }
}