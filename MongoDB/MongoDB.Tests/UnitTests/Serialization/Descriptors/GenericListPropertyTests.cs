using System.Collections.Generic;
using NUnit.Framework;

namespace MongoDB.UnitTests.Serialization.Descriptors
{
    [TestFixture]
    public class GenericListPropertyTests : SerializationTestBase
    {
        public class GenericListOfObjectsProperty
        {
            public List<object> A { get; set; }
        }

        [Test]
        public void CanSerializeAGenericListOfObjects()
        {
            var bson = Serialize<GenericListOfObjectsProperty>(new GenericListOfObjectsProperty() { A = new List<object> { 1, 2 } });
            Assert.AreEqual("GwAAAARBABMAAAAQMAABAAAAEDEAAgAAAAAA", bson);
        }

        [Test]
        public void CanSerializeAGenericListOfObjectsUsingAnonymousType()
        {
            var bson = Serialize<GenericListOfObjectsProperty>(new { A = new[] { 1, 2 } });
            Assert.AreEqual("GwAAAARBABMAAAAQMAABAAAAEDEAAgAAAAAA", bson);
        }

        public class GenericListOfIntegerProperty
        {
            public List<int> A { get; set; }
        }

        [Test]
        public void CanSerializeAGenericListOfIntegers()
        {
            var bson = Serialize<GenericListOfIntegerProperty>(new GenericListOfIntegerProperty() { A = new List<int> { 1, 2 } });
            Assert.AreEqual("GwAAAARBABMAAAAQMAABAAAAEDEAAgAAAAAA", bson);
        }

        [Test]
        public void CanSerializeAGenericListOfIntegersUsingAnonymousType()
        {
            var bson = Serialize<GenericListOfIntegerProperty>(new { A = new[] { 1, 2 } });
            Assert.AreEqual("GwAAAARBABMAAAAQMAABAAAAEDEAAgAAAAAA", bson);
        }

        public class GenericListOfClasses
        {
            public IList<GenericListOfClassesA> A { get; set; }
        }

        public class GenericListOfClassesA
        {
            public string B { get; set; }
        }

        [Test]
        public void CanSerializeAGenericListOfClasses()
        {
            var doc = new Document("A", new[] { new Document("B", "b") });
            var o = new GenericListOfClasses();
            o.A = new List<GenericListOfClassesA> { new GenericListOfClassesA() { B = "b" } };
            string bson = Serialize<GenericListOfClasses>(o);
            string expected = Serialize(doc);
            Assert.AreEqual(expected, bson);
        }

        [Test]
        public void CanSerializeAGenericListOfClassesUsingAnonymousType()
        {
            var doc = new Document("A", new[] { new Document("B", "b") });
            string bson = Serialize<GenericListOfClasses>(new { A = new[] { new { B = "b" } } });
            string expected = Serialize(doc);
            Assert.AreEqual(expected, bson);
        }

        public class GenericListOfEmbeddedDocuments
        {
            public IList<Document> A { get; set; }
        }

        [Test]
        public void CanSerializeAListOfEmbeddedDocuments()
        {
            var doc = new Document("A", new[] { new Document("B", "b" ) });
            var o = new GenericListOfEmbeddedDocuments();
            o.A = new List<Document> { new Document().Append("B", "b") };
            string bson = Serialize<GenericListOfEmbeddedDocuments>(o);
            string expected = Serialize(doc);
            Assert.AreEqual(expected, bson);
        }

        [Test]
        public void CanSerializeAListOfEmbeddedDocumentsUsingAnonymousType()
        {
            var doc = new Document("A", new[] { new Document("B", "b") });
            string bson = Serialize<GenericListOfEmbeddedDocuments>(new { A = new[] { new Document("B", "b") } });
            string expected = Serialize(doc);
            Assert.AreEqual(expected, bson);
        }
    }
}