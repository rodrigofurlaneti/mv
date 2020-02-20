using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Api.Resolvers
{
    public class EntitySerializeCamelCaseResolver : CamelCasePropertyNamesContractResolver
    {
        private readonly EntitySerializeAdapter _adapter;
        public EntitySerializeCamelCaseResolver(EntitySerializeSetting setting)
            : base()
        {
            _adapter = new EntitySerializeAdapter(setting);
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization serialization)
        {
            var property = base.CreateProperty(member, serialization);
            return _adapter.CreateProperty(property, member, serialization);
        }
    }
}