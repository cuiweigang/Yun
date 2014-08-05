﻿using System;

namespace MongoDB.Configuration.Mapping.Conventions
{
    /// <summary>
    /// 
    /// </summary>
    public class DefaultDefaultValueConvention : IDefaultValueConvention
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly DefaultDefaultValueConvention Instance = new DefaultDefaultValueConvention();

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultDefaultValueConvention"/> class.
        /// </summary>
        private DefaultDefaultValueConvention()
        {
        }

        /// <summary>
        /// Gets the default value.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public object GetDefaultValue(Type type){
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
    }
}
