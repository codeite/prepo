using Everest.Content;

namespace prepo.Api.Tests.Builders
{
    public interface IResourceBuilder
    {
        string Id { get; set; }
        IResourceBuilder WithId(string id);
        IResourceBuilder WithId(int id);
        string Build();
        JsonBodyContent BuildAsContent();
    }
}