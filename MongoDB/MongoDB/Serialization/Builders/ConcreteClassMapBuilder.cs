﻿using System;

using MongoDB.Configuration.Mapping.Model;
using System.Collections.Generic;

namespace MongoDB.Serialization.Builders
{
    internal class ConcreteClassMapBuilder : IObjectBuilder
    {
        private readonly IClassMap _classMap;
        private readonly object _instance;
        private readonly IDictionary<string, object> _extendedProperties;

        public ConcreteClassMapBuilder(IClassMap classMap)
        {
            _classMap = classMap;
            _instance = classMap.CreateInstance();

            if(!_classMap.HasExtendedProperties)
                return;
            
            var extPropType = _classMap.ExtendedPropertiesMap.MemberReturnType;
            if (extPropType == typeof(IDictionary<string, object>))
                extPropType = typeof(Dictionary<string, object>);
            _extendedProperties = (IDictionary<string, object>)Activator.CreateInstance(extPropType);
            _classMap.ExtendedPropertiesMap.SetValue(_instance, _extendedProperties);
        }

        public void AddProperty(string name, object value)
        {
            var memberMap = _classMap.GetMemberMapFromAlias(name);
            if (memberMap != null)
                memberMap.SetValue(_instance, value);
            else if ((!_classMap.HasDiscriminator || _classMap.DiscriminatorAlias != name) && _extendedProperties != null)
                _extendedProperties.Add(name, value);
        }

        public object BuildObject()
        {
            return _instance;
        }

        public PropertyDescriptor GetPropertyDescriptor(string name)
        {
            var memberMap = _classMap.GetMemberMapFromAlias(name);
            if (memberMap == null)
                return null;

            var type = memberMap.MemberReturnType;
            var isDictionary = false;
            if (memberMap is CollectionMemberMap)
                type = ((CollectionMemberMap)memberMap).ElementType;
            else if (memberMap is DictionaryMemberMap)
            {
                type = ((DictionaryMemberMap)memberMap).ValueType;
                isDictionary = true;
            }

            return new PropertyDescriptor { Type = type, IsDictionary = isDictionary };
        }
    }
}