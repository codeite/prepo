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
            root.Body.ShouldBe(@"{""_links"":{},""self"":{""href"":""/""},""users"":{""href"":""/users""}}");


        }
    }
}
