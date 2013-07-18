using System;
using System.Collections.Generic;
using prepo.Api.Contracts.Models;
using prepo.Api.Models;
using prepo.Api.Services;

namespace prepo.Api.Resources.Base
{
    public interface IHalResource
    {
        object ToDynamicJson(DbObject instance, IEnumerable<ResourceLink> additionalLinks = null);
        string ToXml(DbObject instance, IEnumerable<ResourceLink> additionalLinks = null);
        ResourceLink SelfLink { get; }
        IEnumerable<ResourceLink> GetRelatedResources();
        IEnumerable<IHalResource> GetEmbededResources();

        void ReadChildResources(Stack<string> partsStack);
        IHalResource Child { get; }
        IHalResource Head { get; }
        IHalResource Owner { get; }

        Type DboType { get; }
        string ResourceName { get; }
        string RebuildPath();
    }
}