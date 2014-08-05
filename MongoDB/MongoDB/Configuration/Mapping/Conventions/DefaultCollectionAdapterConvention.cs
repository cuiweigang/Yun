﻿using System;
using System.Collections;
using System.Collections.Generic;
using MongoDB.Configuration.CollectionAdapters;

namespace MongoDB.Configuration.Mapping.Conventions
{
    /// <summary>
    /// </summary>
    public class DefaultCollectionAdapterConvention : ICollectionAdapterConvention
    {
        /// <summary>
        /// </summary>
        private static readonly Dictionary<Type, CollectionTypeFactoryDelegate> CollectionTypes = new Dictionary<Type, CollectionTypeFactoryDelegate>
        {
            {typeof(ArrayList), CreateArrayListCollectionType},
            {typeof(IList), CreateArrayListCollectionType},
            {typeof(ICollection), CreateArrayListCollectionType},
            {typeof(IEnumerable), CreateArrayListCollectionType},
            {typeof(HashSet<>), CreateGenericSetCollectionType},
            {typeof(List<>), CreateGenericListCollectionType},
            {typeof(IList<>), CreateGenericListCollectionType},
            {typeof(ICollection<>), CreateGenericListCollectionType},
            {typeof(IEnumerable<>), CreateGenericListCollectionType}
        };

        /// <summary>
        /// </summary>
        private static readonly Dictionary<Type, ElementTypeFactoryDelegate> ElementTypes = new Dictionary<Type, ElementTypeFactoryDelegate>
        {
            {typeof(ArrayList), GetArrayListElementType},
            {typeof(IList), GetArrayListElementType},
            {typeof(ICollection), GetArrayListElementType},
            {typeof(IEnumerable), GetArrayListElementType},
            {typeof(HashSet<>), GetGenericSetElementType},
            {typeof(List<>), GetGenericListElementType},
            {typeof(IList<>), GetGenericListElementType},
            {typeof(ICollection<>), GetGenericListElementType},
            {typeof(IEnumerable<>), GetGenericListElementType}
        };

        /// <summary>
        /// </summary>
        public static readonly DefaultCollectionAdapterConvention Instance = new DefaultCollectionAdapterConvention();

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultCollectionAdapterConvention"/> class.
        /// </summary>
        private DefaultCollectionAdapterConvention()
        {
        }

        /// <summary>
        ///   Gets the type of the collection.
        /// </summary>
        /// <param name = "type">The type.</param>
        /// <returns></returns>
        public ICollectionAdapter GetCollectionAdapter(Type type)
        {
            CollectionTypeFactoryDelegate factory;
            if(CollectionTypes.TryGetValue(type, out factory))
                return factory();

            if(type.IsArray && type != typeof(byte[]))
                return new ArrayCollectionAdapter();

            if(type.IsGenericType && !type.IsGenericTypeDefinition)
            {
                var genericType = type.GetGenericTypeDefinition();
                if(CollectionTypes.TryGetValue(genericType, out factory))
                    return factory();
            }

            return null;
        }

        /// <summary>
        ///   Gets the type of the element.
        /// </summary>
        /// <param name = "type">The type.</param>
        /// <returns></returns>
        public Type GetElementType(Type type)
        {
            ElementTypeFactoryDelegate factory;
            if(ElementTypes.TryGetValue(type, out factory))
                return factory(type);

            if(type.IsArray)
                return type.GetElementType();

            if(type.IsGenericType && !type.IsGenericTypeDefinition)
            {
                var genericType = type.GetGenericTypeDefinition();
                if(ElementTypes.TryGetValue(genericType, out factory))
                    return factory(type);
            }

            return null;
        }

        private static ArrayListCollectionAdapter CreateArrayListCollectionType()
        {
            return new ArrayListCollectionAdapter();
        }

        private static Type GetArrayListElementType(Type type)
        {
            return typeof(object);
        }

        private static GenericListCollectionAdapter CreateGenericListCollectionType()
        {
            return new GenericListCollectionAdapter();
        }

        private static Type GetGenericListElementType(Type type)
        {
            return type.GetGenericArguments()[0];
        }

        private static GenericSetCollectionAdapter CreateGenericSetCollectionType()
        {
            return new GenericSetCollectionAdapter();
        }

        private static Type GetGenericSetElementType(Type type)
        {
            return type.GetGenericArguments()[0];
        }

        /// <summary>
        /// </summary>
        private delegate ICollectionAdapter CollectionTypeFactoryDelegate();

        private delegate Type ElementTypeFactoryDelegate(Type type);
    }
}