using System;
using System.Collections.Generic;
using MongoDB.Configuration;
using MongoDB.Connections;

namespace MongoDB
{
    /// <summary>
    /// </summary>
    public class MongoCollection : IMongoCollection
    {
        private readonly MongoCollection<Document> _collection;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "MongoCollection&lt;T&gt;" /> class.
        /// </summary>
        /// <param name = "configuration">The configuration.</param>
        /// <param name = "connection">The connection.</param>
        /// <param name = "databaseName">Name of the database.</param>
        /// <param name = "name">The name.</param>
        internal MongoCollection(MongoConfiguration configuration, Connection connection, string databaseName, string name)
        {
            //Todo: add public constrcutor for users to call
            _collection = new MongoCollection<Document>(configuration, connection, databaseName, name);
        }

        /// <summary>
        ///   Gets the database.
        /// </summary>
        /// <value>The database.</value>
        public IMongoDatabase Database
        {
            get { return _collection.Database; }
        }

        /// <summary>
        ///   Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return _collection.Name; }
        }

        /// <summary>
        ///   Gets the name of the database.
        /// </summary>
        /// <value>The name of the database.</value>
        public string DatabaseName
        {
            get { return _collection.DatabaseName; }
        }

        /// <summary>
        ///   Gets the full name.
        /// </summary>
        /// <value>The full name.</value>
        public string FullName
        {
            get { return _collection.FullName; }
        }

        /// <summary>
        ///   Gets the meta data.
        /// </summary>
        /// <value>The meta data.</value>
        public CollectionMetadata Metadata
        {
            get { return _collection.Metadata; }
        }

        /// <summary>
        ///   Finds the one.
        /// </summary>
        /// <param name = "spec">The spec.</param>
        /// <returns></returns>
        public Document FindOne(Document spec)
        {
            return _collection.FindOne(spec);
        }

        /// <summary>
        ///   Finds all.
        /// </summary>
        /// <returns></returns>
        public ICursor FindAll()
        {
            return new Cursor(_collection.FindAll());
        }

        /// <summary>
        ///   Finds the specified where.
        /// </summary>
        /// <param name = "where">The where.</param>
        /// <returns></returns>
        public ICursor Find(string @where)
        {
            return new Cursor(_collection.Find(@where));
        }

        /// <summary>
        ///   Finds the specified spec.
        /// </summary>
        /// <param name = "spec">The spec.</param>
        /// <returns></returns>
        public ICursor Find(Document spec)
        {
            return new Cursor(_collection.Find(spec));
        }

        /// <summary>
        ///   Finds the specified spec.
        /// </summary>
        /// <param name = "spec">The spec.</param>
        /// <param name = "limit">The limit.</param>
        /// <param name = "skip">The skip.</param>
        /// <returns></returns>
        public ICursor Find(Document spec, int limit, int skip)
        {
            return new Cursor(_collection.Find(spec, limit, skip));
        }

        /// <summary>
        ///   Finds the specified spec.
        /// </summary>
        /// <param name = "spec">The spec.</param>
        /// <param name = "limit">The limit.</param>
        /// <param name = "skip">The skip.</param>
        /// <param name = "fields">The fields.</param>
        /// <returns></returns>
        public ICursor Find(Document spec, int limit, int skip, Document fields)
        {
            return new Cursor(_collection.Find(spec, limit, skip, fields));
        }

        /// <summary>
        ///   Executes a query and atomically applies a modifier operation to the first document returning the original document
        ///   by default.
        /// </summary>
        /// <param name = "document">The document.</param>
        /// <param name = "spec"><see cref = "Document" /> to find the document.</param>
        /// <returns>A <see cref = "Document" /></returns>
        public Document FindAndModify(Document document, Document spec)
        {
            return _collection.FindAndModify(document, spec);
        }

        /// <summary>
        ///   Executes a query and atomically applies a modifier operation to the first document returning the original document
        ///   by default.
        /// </summary>
        /// <param name = "document">The document.</param>
        /// <param name = "spec"><see cref = "Document" /> to find the document.</param>
        /// <param name = "sort"><see cref = "Document" /> containing the names of columns to sort on with the values being the</param>
        /// <returns>A <see cref = "Document" /></returns>
        /// <see cref = "IndexOrder" />
        public Document FindAndModify(Document document, Document spec, Document sort)
        {
            return _collection.FindAndModify(document, spec, sort);
        }

        /// <summary>
        ///   Executes a query and atomically applies a modifier operation to the first document returning the original document
        ///   by default.
        /// </summary>
        /// <param name = "document">The document.</param>
        /// <param name = "spec"><see cref = "Document" /> to find the document.</param>
        /// <param name = "returnNew">if set to <c>true</c> [return new].</param>
        /// <returns>A <see cref = "Document" /></returns>
        public Document FindAndModify(Document document, Document spec, bool returnNew)
        {
            return _collection.FindAndModify(document, spec, returnNew);
        }

        /// <summary>
        /// Executes a query and atomically applies a modifier operation to the first document returning the original document
        /// by default.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="spec"><see cref="Document"/> to find the document.</param>
        /// <param name="sort"><see cref="Document"/> containing the names of columns to sort on with the values being the
        /// <see cref="IndexOrder"/></param>
        /// <param name="returnNew">if set to <c>true</c> [return new].</param>
        /// <returns>A <see cref="Document"/></returns>
        public Document FindAndModify(Document document, Document spec, Document sort, bool returnNew)
        {
            return _collection.FindAndModify(document, spec, sort, null, false, returnNew, false);
        }

        /// <summary>
        /// Executes a query and atomically applies a modifier operation to the first document returning the original document
        /// by default.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="spec"><see cref="Document"/> to find the document.</param>
        /// <param name="sort"><see cref="Document"/> containing the names of columns to sort on with the values being the
        /// <see cref="IndexOrder"/></param>
        /// <param name="fields">The fields.</param>
        /// <param name="remove">if set to <c>true</c> [remove].</param>
        /// <param name="returnNew">if set to <c>true</c> [return new].</param>
        /// <param name="upsert">if set to <c>true</c> [upsert].</param>
        /// <returns>A <see cref="Document"/></returns>
        public Document FindAndModify(Document document, Document spec, Document sort, Document fields, bool remove, bool returnNew, bool upsert)
        {
            return _collection.FindAndModify(document, spec, sort, fields, remove, returnNew, upsert);
        }

        /// <summary>
        ///   Maps the reduce.
        /// </summary>
        /// <returns></returns>
        public MapReduce MapReduce()
        {
            return _collection.MapReduce();
        }

        /// <summary>
        ///   Counts this instance.
        /// </summary>
        /// <returns></returns>
        public long Count()
        {
            return _collection.Count();
        }

        /// <summary>
        ///   Counts the specified spec.
        /// </summary>
        /// <param name = "spec">The spec.</param>
        /// <returns></returns>
        public long Count(Document spec)
        {
            return _collection.Count(spec);
        }

        /// <summary>
        ///   Inserts the specified doc.
        /// </summary>
        /// <param name = "document">The document.</param>
        public void Insert(Document document)
        {
            _collection.Insert(document);
        }

        /// <summary>
        ///   Inserts the specified doc.
        /// </summary>
        /// <param name = "document">The document.</param>
        /// <param name = "safemode">if set to <c>true</c> [safemode].</param>
        public void Insert(Document document, bool safemode)
        {
            _collection.Insert(document, safemode);
        }

        /// <summary>
        ///   Inserts the specified docs.
        /// </summary>
        /// <param name = "documents">The docs.</param>
        public void Insert(IEnumerable<Document> documents)
        {
            _collection.Insert(documents);
        }

        /// <summary>
        ///   Inserts the specified docs.
        /// </summary>
        /// <param name = "documents">The docs.</param>
        /// <param name = "safemode">if set to <c>true</c> [safemode].</param>
        public void Insert(IEnumerable<Document> documents, bool safemode)
        {
            _collection.Insert(documents, safemode);
        }

        /// <summary>
        ///   Deletes the specified selector.
        /// </summary>
        /// <param name = "selector">The selector.</param>
        [Obsolete("Use Remove instead")]
        public void Delete(Document selector)
        {
            _collection.Delete(selector);
        }

        /// <summary>
        ///   Removes the specified selector.
        /// </summary>
        /// <param name = "selector">The selector.</param>
        public void Remove(Document selector)
        {
            _collection.Remove(selector);
        }

        /// <summary>
        ///   Deletes the specified selector.
        /// </summary>
        /// <param name = "selector">The selector.</param>
        /// <param name = "safemode">if set to <c>true</c> [safemode].</param>
        [Obsolete("Use Remove instead")]
        public void Delete(Document selector, bool safemode)
        {
            _collection.Delete(selector, safemode);
        }

        /// <summary>
        ///   Removes the specified selector.
        /// </summary>
        /// <param name = "selector">The selector.</param>
        /// <param name = "safemode">if set to <c>true</c> [safemode].</param>
        public void Remove(Document selector, bool safemode)
        {
            _collection.Remove(selector, safemode);
        }

        /// <summary>
        ///   Updates the specified doc.
        /// </summary>
        /// <param name = "document">The document.</param>
        /// <param name = "safemode">if set to <c>true</c> [safemode].</param>
        public void Update(Document document, bool safemode)
        {
            _collection.Save(document, safemode);
        }

        /// <summary>
        ///   Updates the specified doc.
        /// </summary>
        /// <param name = "document">The document.</param>
        public void Update(Document document)
        {
            _collection.Save(document);
        }

        /// <summary>
        ///   Updates the specified doc.
        /// </summary>
        /// <param name = "document">The document.</param>
        /// <param name = "selector">The selector.</param>
        public void Update(Document document, Document selector)
        {
            _collection.Update(document, selector);
        }

        /// <summary>
        ///   Updates the specified doc.
        /// </summary>
        /// <param name = "document">The document.</param>
        /// <param name = "selector">The selector.</param>
        /// <param name = "safemode">if set to <c>true</c> [safemode].</param>
        public void Update(Document document, Document selector, bool safemode)
        {
            _collection.Update(document, selector, safemode);
        }

        /// <summary>
        ///   Updates the specified doc.
        /// </summary>
        /// <param name = "document">The document.</param>
        /// <param name = "selector">The selector.</param>
        /// <param name = "flags">The flags.</param>
        public void Update(Document document, Document selector, UpdateFlags flags)
        {
            _collection.Update(document, selector, flags);
        }

        /// <summary>
        ///   Updates the specified doc.
        /// </summary>
        /// <param name = "document">The document.</param>
        /// <param name = "selector">The selector.</param>
        /// <param name = "flags">The flags.</param>
        /// <param name = "safemode">if set to <c>true</c> [safemode].</param>
        public void Update(Document document, Document selector, UpdateFlags flags, bool safemode)
        {
            _collection.Update(document, selector, flags, safemode);
        }

        /// <summary>
        ///   Updates all.
        /// </summary>
        /// <param name = "document">The document.</param>
        /// <param name = "selector">The selector.</param>
        public void UpdateAll(Document document, Document selector)
        {
            _collection.UpdateAll(document, selector);
        }

        /// <summary>
        ///   Updates all.
        /// </summary>
        /// <param name = "document">The document.</param>
        /// <param name = "selector">The selector.</param>
        /// <param name = "safemode">if set to <c>true</c> [safemode].</param>
        public void UpdateAll(Document document, Document selector, bool safemode)
        {
            _collection.UpdateAll(document, selector, safemode);
        }

        /// <summary>
        ///   Saves the specified doc.
        /// </summary>
        /// <param name = "document">The document.</param>
        public void Save(Document document)
        {
            _collection.Save(document);
        }

        /// <summary>
        ///   Saves the specified doc.
        /// </summary>
        /// <param name = "document">The document.</param>
        /// <param name = "safemode">if set to <c>true</c> [safemode].</param>
        public void Save(Document document, bool safemode)
        {
            _collection.Save(document, safemode);
        }
    }
}