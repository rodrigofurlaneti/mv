using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ApiInfox.Resolvers
{
    public class EntitySerializeResolver : DefaultContractResolver
    {
        private readonly EntitySerializeSetting _setting;
        public EntitySerializeResolver(EntitySerializeSetting setting)
            : base()
        {
            _setting = setting;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization serialization)
        {
            var property = base.CreateProperty(member, serialization);
            property.ShouldSerialize = x => _setting.ShouldSerialize(property.DeclaringType, property.PropertyName);
            return property;
        }
    }
}