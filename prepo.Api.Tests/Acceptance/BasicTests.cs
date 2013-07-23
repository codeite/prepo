using System;
using System.Net.Http;
using System.Web.Http;
using Everest.Headers;
using NUnit.Framework;
using prepo.Api.Tests.Helpers;
using prepo.Client;

namespace prepo.Api.Tests.Acceptance
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
            root.Body.ShouldBeJson(@"
            {
                '_links': { 
                    'self': {'href': '/'},
                    'users': {'href': '/users'},
                    'personas': {'href': '/personas'},
                    'settings': {'href': '/settings'},
                }
            }");
        }
        
        [Test]
        public void GetRootAsXml()
        {
            // Arrange
            var client = new PrepoRestClient(null, new Accept("application/xml"));

            // Act
            var root = client.GetRoot();

            // Assert
            root.Body.ShouldBeXml(@"
           <resource href=""/"">
              <link rel=""users"" href=""/users"" />
              <link rel=""personas"" href=""/personas"" />
              <link rel=""settings"" href=""/settings"" />
            </resource>");
        }
    }
}
