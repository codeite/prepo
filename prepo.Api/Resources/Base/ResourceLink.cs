namespace prepo.Api.Resources.Base
{
    public class ResourceLink
    {
        public ResourceLink(string name, string href, string title = null)
        {
            Name = name;
            Href = href;
            Title = title;
        }
        
        public ResourceLink(string name, HalResource resource, string title = null)
        {
            Name = name;
            Href = resource.SelfLink.Href;
            Title = title;
        }

        public string Name { get; set; }
        public string Href { get; set; }
        public string Title { get; set; }
    }
}