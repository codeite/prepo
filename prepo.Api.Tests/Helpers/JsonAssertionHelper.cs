using System;
using Codeite.Core.Json;
using FluentAssertions;

namespace prepo.Api.Tests.Helpers
{
    public static class JsonAssertionHelper
    {
        public static void ShouldBeJson(this string actual, string expected)
        {
            var cannonicalizer = new Codeite.Core.Json.JsonCannonicalizer();
            string actualCannonical;
            string expectedCannonical;

            try
            {
                expectedCannonical = cannonicalizer.Cannonicalize(expected);
            }
            catch (DynamicJsonObjectReadException e)
            {
                throw new Exception("Expected JSON invalid, " + e.Message + "\n" + expected);
            }

            try
            {
                actualCannonical = cannonicalizer.Cannonicalize(actual);
            }
            catch (DynamicJsonObjectReadException e)
            {
                throw new Exception("Actual JSON invalid, " + e.Message + "\n" + actual);
            }

            Console.WriteLine("e: " + expectedCannonical);
            Console.WriteLine("a: " + actualCannonical);

            actualCannonical.Should().Be(expectedCannonical);
        }
    }
}