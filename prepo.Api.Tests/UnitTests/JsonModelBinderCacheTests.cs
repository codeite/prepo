using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using prepo.Api.Infrastructure;

namespace prepo.Api.Tests.UnitTests
{
    [TestFixture]
    public class JsonModelBinderCacheTests
    {
        [Test]
        public void TestName()
        {
            // Arrange
            var cache = new JsonModelBinderCache();
            var json = new Dictionary<string, object>
            {
                {"alpha", "first"}
            };

            // Act
            var binder = cache.CreateBinderFor(typeof (TestClassA));
            var instance = binder(json);

            // Assert
            instance.Should().BeOfType<TestClassA>();
            var typedInstance = instance as TestClassA;
            typedInstance.Alpha.Should().Be("first");
        } 
    }

    public class TestClassA
    {
        public string Alpha { get; set; }
    }
}