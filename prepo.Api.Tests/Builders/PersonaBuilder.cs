using System.Globalization;
using Everest.Content;

namespace prepo.Api.Tests.Builders
{

    public class PersonaBuilder : IResourceBuilder
    {
        private string _id;

        public PersonaBuilder()
        {
            _id = "0";
        }

        public PersonaBuilder(int id)
        {
            _id = id.ToString();
        }

        public PersonaBuilder(string id)
        {
            _id = id;
        }

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public IResourceBuilder WithId(string id)
        {
            _id = id;
            return this;
        }

        public IResourceBuilder WithId(int id)
        {
            _id = id.ToString(CultureInfo.InvariantCulture);
            return this;
        }

        public string Build()
        {
            return "{'id':'" + _id + "'}";
        }

        public JsonBodyContent BuildAsContent()
        {
            return new JsonBodyContent(Build());
        }
    }
}