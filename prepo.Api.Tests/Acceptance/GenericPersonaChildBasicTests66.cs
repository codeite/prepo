﻿using NUnit.Framework;
using prepo.Api.Tests.Builders;

namespace prepo.Api.Tests.Acceptance
{
    [TestFixture]
    public class GenericPersonaChildBasicTests67 : GenericBasicTests<PersonaBuilder>
    {
        public GenericPersonaChildBasicTests67()
        {
            LinkRelList = new[] { "users", "user:id=sam", "personas" };
            UrlPrefix = "/user/sam/personas";
            ResourceName = "persona";
            ResourceList = "personas";

            FullResourceId = "bob";
            FullResourceJson = "{'id':'bob'}";
            FullResource = MakeUserResource("bob");

            Builder = new ResourceBuilder<PersonaBuilder>();
        }

        protected override void OnSetup(Client.PrepoRestClient client)
        {
            client.GetRoot().PostToRel("users", new {id = "sam"}, new UserBuilder().WithId("sam").BuildAsContent());
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