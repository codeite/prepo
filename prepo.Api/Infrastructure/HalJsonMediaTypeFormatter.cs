using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Reflection;
using System.Web;
using Codeite.Core.Json;
using Newtonsoft.Json;
using prepo.Api.Resources;
using prepo.Api.Resources.Base;

namespace prepo.Api.Infrastructure
{
    public class HalJsonMediaTypeFormatter : BufferedMediaTypeFormatter
    {
        public HalJsonMediaTypeFormatter()
        {
            this.SupportedMediaTypes.Add(new MediaTypeWithQualityHeaderValue("application/hal+json"));
            this.SupportedMediaTypes.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public override bool CanReadType(Type type)
        {
            var cwt = typeof(IHalResource).IsAssignableFrom(type);
            return cwt;
        }

        public override bool CanWriteType(Type type)
        {
            var cwt = typeof(IHalResource).IsAssignableFrom(type);
            return cwt;
        }

        public override void WriteToStream(Type type, object value, Stream writeStream, System.Net.Http.HttpContent content)
        {
            var halResource = value as IHalResource;

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
            var json = DynamicJsonObject.ReadJson(content.ReadAsStringAsync().Result);
            string id = json["id"].ToString();

            var dboType = GetDbObjectType(type);

            object dboInstance = Activator.CreateInstance(dboType, id);

            object resourceInstance = Activator.CreateInstance(type, dboInstance);

            return resourceInstance;
        }

        private Type GetDbObjectType(Type resourceType)
        {
            ConstructorInfo ctor = resourceType.GetConstructors().Single();
            return ctor.GetParameters().Single().ParameterType;
        }
    }
}