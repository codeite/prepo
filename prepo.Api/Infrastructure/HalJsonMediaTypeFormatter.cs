using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using prepo.Api.Resources;

namespace prepo.Api.Infrastructure
{
    public class HalJsonMediaTypeFormatter : BufferedMediaTypeFormatter 
    {
        public override bool CanReadType(Type type)
        {
            var cwt = typeof(HalResource).IsAssignableFrom(type);
            return cwt;
        }

        public override bool CanWriteType(Type type)
        {
            var cwt = typeof(HalResource).IsAssignableFrom(type);
            return cwt;
        }

        public override void WriteToStream(Type type, object value, System.IO.Stream writeStream, System.Net.Http.HttpContent content)
        {
            content.Headers.Add("Content-Type", "aplication/hal+json");
            var writer = new StreamWriter(writeStream);
            
            writer.Write("Bo!");
            writer.Flush();
        }

        public override object ReadFromStream(Type type, Stream readStream, System.Net.Http.HttpContent content, IFormatterLogger formatterLogger)
        {
            return base.ReadFromStream(type, readStream, content, formatterLogger);
        }
    }
}