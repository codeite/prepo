using System.Collections.Generic;

namespace prepo.Api.Resources
{
    public interface HalResource
    {
        object ToDynamicJson();
        ResourceLink SelfLink { get; }
        IEnumerable<ResourceLink> GetRelatedResources();
        IEnumerable<HalResource> GetEmbededResources();
    }
}