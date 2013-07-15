using System;
using Everest.Content;

namespace prepo.Api.Tests.Builders
{
    public class ResourceBuilder<T> where T : IResourceBuilder
    {
        public IResourceBuilder New()
        {
            return Activator.CreateInstance<T>();
        }
    }
}