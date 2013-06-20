using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
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
                    'first': {'href': '/users?page=1&count=10'}
                }
            }");
        }

        [Test]
        public void GetFirstTenUsers()
        {
            // Arrange
            var client = new PrepoRestClient();

            // Act
            var users = client.GetRoot()
                .FollowRel("users")
                .FollowRel("first");

            // Assert
            Console.WriteLine(users.Body);
            users.Body.ShouldBeJson(@"
            {
                '_links': { 
                    'self': {'href': '/users'},
                    'first': {'href': '/users?page=1&count=10'},
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

    }
}
