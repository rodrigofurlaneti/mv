using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Core.Extensions;

namespace Api.Resolvers
{
    public class EntitySerializeSetting
    {
        private readonly IDictionary<Type, IList<string>> _properties = new Dictionary<Type, IList<string>>();
        private readonly bool _enableCamelCase;

        public EntitySerializeSetting(bool enableCamelCase)
        {
            _enableCamelCase = enableCamelCase;
        }
        public void NotSerializeProperty<TEntity>(Expression<Func<TEntity, object>> expression)
        {
            Type type = typeof(TEntity);

            var member = expression.Body as MemberExpression;
            if (member == null)
                throw new ArgumentException($"Expression '{expression}' refers to a method, not a property.");

            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException($"Expression '{expression}' refers to a field, not a property.");

            if (type != propInfo.ReflectedType && !type.IsSubclassOf(propInfo.ReflectedType))
                throw new ArgumentException($"Expresion '{expression}' refers to a property that is not from type {type}.");

            //
            if (!_properties.ContainsKey(typeof(TEntity)))
                _properties.Add(typeof(TEntity), new List<string>());
            _properties[typeof(TEntity)].Add(_enableCamelCase ? propInfo.Name.ToLowerCamelCase() : propInfo.Name);
        }

        public bool ShouldSerialize(Type entiType, string name)
        {
            if (!_properties.ContainsKey(entiType))
                return true;
            return !_properties[entiType].Contains(name);
        }
    }
}