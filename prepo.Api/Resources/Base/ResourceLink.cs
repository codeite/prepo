using System;

namespace prepo.Api.Resources.Base
{
    public class ResourceLink
    {
        private string _name;

        public ResourceLink(string name, string href, string title = null)
        {
            Name = name;
            Href = href;
            Title = title;
        }
        
        public ResourceLink(string name, IHalResource resource, string title = null)
        {
            Name = name;
            Href = resource.SelfLink.Href;
            Title = title;
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Can not set name to null");
                }
                _name = value;
            }
        }

        public string Href { get; set; }
        public string Title { get; set; }
    }
}