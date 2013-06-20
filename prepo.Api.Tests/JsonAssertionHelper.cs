using System;
using Codeite.Core.Json;
using Shouldly;

namespace prepo.Api.Tests
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

            actualCannonical.ShouldBe(expectedCannonical);
        }
    }
}