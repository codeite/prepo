using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Reflection;
using System.Web;
using Codeite.Core.Json;
using Newtonsoft.Json;
using prepo.Api.Contracts.Models;
using prepo.Api.Infrastructure.Reflecting;
using prepo.Api.Resources;
using prepo.Api.Resources.Base;

namespace prepo.Api.Infrastructure
{
    public class HalJsonMediaTypeFormatter : BufferedMediaTypeFormatter
    {
        private readonly JsonModelBinderCache _jsonModelBinderCache;

        public HalJsonMediaTypeFormatter(JsonModelBinderCache jsonModelBinderCache)
        {
            _jsonModelBinderCache = jsonModelBinderCache;
            this.SupportedMediaTypes.Add(new MediaTypeWithQualityHeaderValue("application/hal+json"));
            this.SupportedMediaTypes.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public override bool CanReadType(Type type)
        {
            var cwt = typeof(IHalResourceInstance).IsAssignableFrom(type);
            cwt |= typeof(DbObject).IsAssignableFrom(type);
            return cwt;
        }

        public override bool CanWriteType(Type type)
        {
            var cwt = typeof(IHalResourceInstance).IsAssignableFrom(type);
            return cwt;
        }

        public override void WriteToStream(Type type, object value, Stream writeStream, System.Net.Http.HttpContent content)
        {
            var halResource = value as IHalResourceInstance;

            if (halResource == null)
            {
                return;
            }

            var writer = new StreamWriter(writeStream);

            var json = halResource.ToDynamicJson().ToJsonString();
            writer.Write(json);
            writer.Flush();
        }

        public override object ReadFromStream(Type type, Stream readStream, System.Net.Http.HttpContent content, IFormatterLogger formatterLogger)
        {
            if (typeof (DbObject).IsAssignableFrom(type))
            {
                return ReadDbObject(type, content);
            }
            else
            {
                return ReadResource(type, content);
            }
        }

        private IHalResourceInstance ReadResource(Type type, HttpContent content)
        {
            var json = DynamicJsonObject.ReadJson(content.ReadAsStringAsync().Result);
            string id = json["id"].ToString();

            var dboType = GetDbObjectType(type);

            object dboInstance = Activator.CreateInstance(dboType, id);

            object resourceInstance = Activator.CreateInstance(type, dboInstance);

            return resourceInstance as IHalResourceInstance;
        }

        private DbObject ReadDbObject(Type type, HttpContent content)
        {
            var json = DynamicJsonObject.ReadJson(content.ReadAsStringAsync().Result) as Dictionary<string, object>;
            string id = json["id"].ToString();

            var modelBinder = _jsonModelBinderCache.GetModelFinderFor(type);

            var dboInstance = modelBinder(json, id) as DbObject;

            /*
             
           
            var dboInstance = Activator.CreateInstance(type, id) as DbObject;
            
            foreach (var propertyInfo in type.GetProperties())
            {
                var name = propertyInfo.Name.ToLowerInvariant();

                if (json.ContainsKey(name))
                {
                    SetValue(propertyInfo, dboInstance, json[name]);
                }
            }
            */

            return dboInstance;
        }

        private static void SetValue(PropertyInfo propertyInfo, DbObject dboInstance, object value)
        {
            if (propertyInfo.PropertyType == typeof (int))
            {
                value = (int) (long) value;
            }

            propertyInfo.SetValue(dboInstance, value);
        }

        private Type GetDbObjectType(Type resourceType)
        {
            ConstructorInfo ctor = resourceType.GetConstructors().Single();
            return ctor.GetParameters().Single().ParameterType;
        }
    }
}