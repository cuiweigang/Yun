﻿using System;

using MongoDB.Configuration.IdGenerators;

namespace MongoDB.Configuration.Mapping.Model
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class IdMap : PersistentMemberMap
    {
        private readonly IIdGenerator _generator;

        /// <summary>
        /// Gets the id's unsaved value.
        /// </summary>
        /// <value>The unsaved value.</value>
        public object UnsavedValue { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdMap"/> class.
        /// </summary>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="memberType">Type of the member.</param>
        /// <param name="getter">The getter.</param>
        /// <param name="setter">The setter.</param>
        /// <param name="generator">The generator.</param>
        /// <param name="unsavedValue">The unsaved value.</param>
        public IdMap(string memberName, Type memberType, Func<object, object> getter, Action<object, object> setter, IIdGenerator generator, object unsavedValue)
            : base(memberName, memberType, getter, setter, null, "_id", true)
        {
            _generator = generator;
            UnsavedValue = unsavedValue;
        }

        /// <summary>
        /// Generates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public object Generate(object entity)
        {
            return _generator.Generate(entity, this);
        }
    }
}