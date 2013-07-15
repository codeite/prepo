using System.Globalization;
using Everest.Content;

namespace prepo.Api.Tests.Builders
{
    public class UserBuilder : ResourceBuilder<UserBuilder>, IResourceBuilder
    {
        private string _id;

        public UserBuilder()
        {
            _id = "0";
        }

        public UserBuilder(int id)
        {
            _id = id.ToString();
        }

        public UserBuilder(string id)
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