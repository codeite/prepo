using System.Globalization;
using NUnit.Framework;
using prepo.Api.Tests.Builders;

namespace prepo.Api.Tests.Acceptance
{
    [TestFixture]
    public class GenericPersonaBasicTests2 : GenericBasicTests<PersonaBuilder>
    {
        public GenericPersonaBasicTests2()
        {
            LinkRelList = new[] { "personas" };
            UrlPrefix = "/personas";
            ResourceName = "persona";
            ResourceList = "personas";

            FullResourceId = "sam";
            FullResourceJson = "{'id':'sam'}";
            FullResource = MakeUserResource("sam");

            Builder = new ResourceBuilder<PersonaBuilder>();
        }

        protected override string MakeResource(string id)
        {
            return MakeUserResource(id);
        }

        private string MakeUserResource(string id)
        {
            var resource = (@"
            {
                '_links': { 
                    'self': {'href': '" + UrlPrefix + @"/$id$'}
                },
                'id' : '$id$'
            }").Replace("$id$", id);

            return resource;
        }
    }
}