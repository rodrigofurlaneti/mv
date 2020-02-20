using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Api.Resolvers
{
    public class EntitySerializeAdapter
    {
        private readonly EntitySerializeSetting _setting;
        public EntitySerializeAdapter(EntitySerializeSetting setting)
        {
            _setting = setting;
        }

        public JsonProperty CreateProperty(JsonProperty property, MemberInfo member, MemberSerialization serialization)
        {
            property.ShouldSerialize = x => _setting.ShouldSerialize(property.DeclaringType, property.PropertyName);
            return property;
        }
    }
}