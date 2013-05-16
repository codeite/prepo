using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            root.Body.ShouldBeJson(@"{""_links"":{ },""self"":{""href"":""/""},""users"":{""href"":""/users""}}");
        }

    }

    public static class JsonAssertionHelper
    {
        public static void ShouldBeJson(this string actual, string expected)
        {
            var cannonicalizer = new Codeite.Core.Json.JsonCannonicalizer();
            var actualCannonical = cannonicalizer.Cannonicalize(actual);
            var expectedCannonical = cannonicalizer.Cannonicalize(expected);
            ShouldBeTestExtensions.ShouldBe(actualCannonical, expectedCannonical);
        }
    }
}
