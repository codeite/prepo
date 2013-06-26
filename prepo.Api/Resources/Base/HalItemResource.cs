using System.Collections.Generic;
using System.Globalization;
using prepo.Api.Contracts.Models;

namespace prepo.Api.Resources.Base
{
    public abstract class HalItemResource<TDbo> 
        : HalCollectionResource<TDbo>
        where TDbo : DbObject
    {
        private readonly TDbo _instance;

        protected HalItemResource(string selfHref, TDbo instance) : base(selfHref)
        {
            _instance = instance;
        }

        public TDbo Instance
        {
            get { return _instance; }
        }

        protected override void AddProperties(Dictionary<string, object> dictionary)
        {
            var type = typeof (TDbo);

            foreach (var propertyInfo in type.GetProperties())
            {
                var name = FixName(propertyInfo.Name);
                var value = propertyInfo.GetValue(_instance);

                dictionary.Add(name, value);
            }
        }

        private string FixName(string name)
        {
            if (name.Length > 0)
            {
                var first = name[0].ToString(CultureInfo.InvariantCulture).ToLower()[0];

                name = first + name.Substring(1);
            }

            return name;
        }
    }
}