using System;
using System.Collections.Generic;
using System.Linq;
using prepo.Api.Resources;
using prepo.Api.Resources.Base;

namespace prepo.Api.Services
{
    public class ResourceChainBuilder
    {
        public IHalResource Build(string path)
        {
            path = path ?? "";

            var rootResource = new RootResource();

            var partsStack = new Stack<string>(path.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries).Reverse());

            rootResource.ReadChildResources(partsStack);

            return rootResource;
        }
    }
}