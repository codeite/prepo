using System.Collections.Generic;

namespace prepo.Api.Resources.Base
{
    public interface IHalResource
    {
        object ToDynamicJson();
        ResourceLink SelfLink { get; }
        IEnumerable<ResourceLink> GetRelatedResources();
        IEnumerable<IHalResource> GetEmbededResources();

        void ReadChildResources(Stack<string> partsStack);
        IHalResource Child { get; }
        IHalResource Head { get; }
        IHalResource Owner { get; }
    }
}