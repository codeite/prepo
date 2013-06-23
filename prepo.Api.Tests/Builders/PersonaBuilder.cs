using Everest.Content;

namespace prepo.Api.Tests
{
    public class PersonaBuilder
    {
        private int _id;

        public PersonaBuilder()
        {
            _id = 0;
        }
        
        public PersonaBuilder(int id)
        {
            _id = id;
        }

        public PersonaBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public string Build()
        {
            return "{'id':" + _id + "}";
        }

        public JsonBodyContent BuildAsContent()
        {
            return new JsonBodyContent(Build());
        }
    }
}