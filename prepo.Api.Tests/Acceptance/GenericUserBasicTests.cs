using System.Globalization;
using NUnit.Framework;
using prepo.Api.Tests.Builders;

namespace prepo.Api.Tests.Acceptance
{
    [TestFixture]
    public class GenericUserBasicTests : GenericBasicTests<UserBuilder>
    {
        public GenericUserBasicTests()
        {
            LinkRelList = new[] { "users" };
            UrlPrefix = "/users";
            ResourceName = "user";
            ResourceList = "users";

            FullResourceId = "sam";
            FullResourceJson = "{'id':'sam', 'name': 'Sam Plews', 'age': 31}";
            FullResource = MakeUserResource("sam", "Sam Plews", 31);
            
            Builder = new UserBuilder();
        }

        protected override string MakeResource(string id)
        {
            return MakeUserResource(id);
        }

        private string MakeUserResource(string id, string name = null, int age = 0)
        {
            var resource = (@"
            {
                '_links': { 
                    'self': {'href': '" + UrlPrefix + @"/$id$'},
                    'personas': {'href': '" + UrlPrefix + @"/$id$/personas'}
                },
                'id' : '$id$',
                'name' : $name$,
                'age' : $age$,
            }").Replace("$id$", id)
                                                                                                                                                                                                                                                                                                                                                                  .Replace("$name$", name == null ? "null" : "'" + name + "'")
                                                                                                                                                                                                                                                                                                                                                                  .Replace("$age$", age.ToString(CultureInfo.InvariantCulture));

            return resource;
        }
    }
}