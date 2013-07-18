using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Reflection;
using Codeite.Core.Json;
using prepo.Api.Contracts.Models;
using prepo.Api.Infrastructure.Reflecting;
using prepo.Api.Resources.Base;

namespace prepo.Api.Infrastructure
{
    public class HalXmlMediaTypeFormatter : BufferedMediaTypeFormatter
    {
        private readonly JsonModelBinderCache _jsonModelBinderCache;

        public HalXmlMediaTypeFormatter(JsonModelBinderCache jsonModelBinderCache)
        {
            _jsonModelBinderCache = jsonModelBinderCache;
            this.SupportedMediaTypes.Add(new MediaTypeWithQualityHeaderValue("application/hal+xml"));
            this.SupportedMediaTypes.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
        }

        public override bool CanReadType(Type type)
        {
            var cwt = typeof (IHalResourceInstance).IsAssignableFrom(type);
            cwt |= typeof (DbObject).IsAssignableFrom(type);
            return cwt;
        }

        public override bool CanWriteType(Type type)
        {
            var cwt = typeof (IHalResourceInstance).IsAssignableFrom(type);
            return cwt;
        }

        public override void WriteToStream(Type type, object value, Stream writeStream, HttpContent content)
        {
            var halResource = value as IHalResourceInstance;

            if (halResource == null)
            {
                return;
            }

            var writer = new StreamWriter(writeStream);

            var xml = halResource.ToXml();
            writer.Write(xml);
            writer.Flush();
        }

        public override object ReadFromStream(Type type, Stream readStream, HttpContent content,
                                              IFormatterLogger formatterLogger)
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

            return dboInstance;
        }

        private Type GetDbObjectType(Type resourceType)
        {
            ConstructorInfo ctor = resourceType.GetConstructors().Single();
            return ctor.GetParameters().Single().ParameterType;
        }
    }
}