using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace WagsReaderLibrary.Json
{
    public class StandardContractResolver : DefaultContractResolver
    {
        public bool UseJsonPropertyName { get; }

        public StandardContractResolver(bool useJsonPropertyName)
        {
            UseJsonPropertyName = useJsonPropertyName;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            
            if (!UseJsonPropertyName)
            {
                property.PropertyName = property.UnderlyingName;
            }

            return property;
        }
    }
}
