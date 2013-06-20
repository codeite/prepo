using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Everest.Content;
using NUnit.Framework;
using Shouldly;
using prepo.Client;

namespace prepo.Api.Tests
{
    [TestFixture]
    public class BasicTests
    {
        [Test]
        public void GetRoot()
        {
            // Arrange
            var client = new PrepoRestClient();

            // Act
            var root = client.GetRoot();

            // Assert
            Console.WriteLine(root.Body);
            root.Body.ShouldBeJson(@"
            {
                '_links': { 
                    'self': {'href': '/'},
                    'users': {'href': '/users'}
                }
            }");
        }

        [Test]
        public void GetFirstTenUsers()
        {
            // Arrange
            var client = new PrepoRestClient();
            var usersRd = client.GetRoot()
                              .FollowRel("users"); ;

            for (var i = 1; i < 20; i++)
            {
                usersRd.PutToRel("user", new { id = i }, new JsonBodyContent("{'id':" + i + "}"));
            }

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

        [Test]
        public void GetSecondsFiveUsers()
        {
            // Arrange
            var client = new PrepoRestClient();

            // Act
            var users = client.GetRoot()
                .FollowRel("users")
                .FollowRel("page", new {page = 2, count = 5});

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

            // Act
            var users = client
                .GetRoot()
                .FollowRel("users")
                .FollowRel("user", new {id = 15});

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
            location.ShouldBe("http://dev.prepo.codeite.com/users/22");
        }

        [Test]
        public void PostUser()
        {
            // Arrange
            var client = new PrepoRestClient();

            // Act
            var location = client
                .GetRoot()
                .PostToRel("users", new JsonBodyContent("{'id':77}"));

            // Assert
            location.ShouldBe("http://dev.prepo.codeite.com/users/77");
        }

        [Test]
        public void PutThenGetUser()
        {
            // Arrange
            var client = new PrepoRestClient();
            var users = client
                .GetRoot()
                .FollowRel("users");

            // Act
            var location = users.PutToRel("user", new {id = 65}, new JsonBodyContent("{'id':65}"));

            // Assert
            location.ShouldBe("http://dev.prepo.codeite.com/users/65");


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
    }
}
