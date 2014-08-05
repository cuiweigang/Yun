﻿using System;
using System.IO;
using MongoDB.Bson;
using MongoDB.Configuration.Mapping;
using MongoDB.Serialization;

namespace MongoDB.UnitTests.Serialization
{
    public abstract class SerializationTestBase
    {
        public const string EmptyDocumentBson = "BQAAAAA=";

        protected virtual IMappingStore MappingStore
        {
            get { return new AutoMappingStore(); }
        }

        protected T Deserialize<T>(string base64)
        {
            using (var mem = new MemoryStream(Convert.FromBase64String(base64)))
            {
                var reader = new BsonReader(mem, new BsonClassMapBuilder(MappingStore, typeof(T)));
                return (T)reader.ReadObject();
            }
        }

        protected string Serialize<T>(object instance)
        {
            using (var mem = new MemoryStream())
            {
                var writer = new BsonWriter(mem, new BsonClassMapDescriptor(MappingStore, typeof(T)));
                writer.WriteObject(instance);
                writer.Flush();
                return Convert.ToBase64String(mem.ToArray());
            }
        }

        protected string Serialize(Document document)
        {
            using (var mem = new MemoryStream())
            {
                var writer = new BsonWriter(mem, new BsonDocumentDescriptor());
                writer.WriteObject(document);
                writer.Flush();
                return Convert.ToBase64String(mem.ToArray());
            }
        }
    }
}
