using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using prepo.Api.Infrastructure;

namespace prepo.Api.Tests.UnitTests
{
    [TestFixture]
    public class UriBuilderHelperTests
    {
        [TestCase("left", "right", "left/right")]
        [TestCase("leftA/leftB", "rightA/rightB", "leftA/leftB/rightA/rightB")]
        [TestCase("/left/", "/right/", "/left/right/")]
        [TestCase("/", "/right/", "/right/")]
        [TestCase("/", "right/", "/right/")]
        public void CombinesTwoPartsCorrectly(string left, string right, string expected)
        {
            // Arrange

            // Act
            var actual = UriBuilderHelper.Combine(left, right);

            // Assert
            actual.Should().Be(expected);
        }
    }
}
