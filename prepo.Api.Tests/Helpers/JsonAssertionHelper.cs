using System;
using System.IO;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using Codeite.Core.Json;
using FluentAssertions;

namespace prepo.Api.Tests.Helpers
{
    public static class AssertionHelper
    {
        public static void ShouldBeXml(this string actual, string expected)
        {
            string actualCannonical;
            string expectedCannonical;

            try
            {
                expectedCannonical = ToCanonicalXml(expected);
            }
            catch (DynamicJsonObjectReadException e)
            {
                throw new Exception("Expected JSON invalid, " + e.Message + "\n" + expected);
            }

            try
            {
                actualCannonical = ToCanonicalXml(actual);
            }
            catch (DynamicJsonObjectReadException e)
            {
                throw new Exception("Actual JSON invalid, " + e.Message + "\n" + actual);
            }

            Console.WriteLine("e: " + expectedCannonical);
            Console.WriteLine("a: " + actualCannonical);

            actualCannonical.Should().Be(expectedCannonical);
        }

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

        private static string ToCanonicalXml(string xml)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.PreserveWhitespace = false;
            xDoc.LoadXml(xml);
            
            //Instantiate an XmlNamespaceManager object. 
            var xmlnsManager = new XmlNamespaceManager(xDoc.NameTable);

            // Create a list of nodes to have the Canonical treatment
            XmlNodeList nodeList = xDoc.SelectNodes("/", xmlnsManager);

            //Initialise the stream to read the node list
            var nodeStream = new MemoryStream();
            XmlWriter xw = XmlWriter.Create(nodeStream);
            nodeList[0].WriteTo(xw);
            xw.Flush();
            nodeStream.Position = 0;

            // Perform the C14N transform on the nodes in the stream
            var transform = new XmlDsigC14NTransform();
            transform.LoadInput(nodeStream);

            // use a new memory stream for output of the transformed xml 
            // this could be done numerous ways if you don't wish to use a memory stream
            var outputStream = (MemoryStream)transform.GetOutput(typeof(Stream));
            return Encoding.UTF8.GetString(outputStream.ToArray());
        }

    }
}