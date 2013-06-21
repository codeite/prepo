﻿using Everest.Content;

namespace prepo.Api.Tests
{
    public class UserBuilder
    {
        private int _id;

        public UserBuilder()
        {
            _id = 0;
        }
        
        public UserBuilder(int id)
        {
            _id = id;
        }

        public UserBuilder WithId(int id)
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