using System;
using System.Collections.Generic;
using MongoDB.Commands;
using MongoDB.Results;

namespace MongoDB
{
    /// <summary>
    ///   Provides a Fluent interface to build and execute Map/Reduce calls.
    /// </summary>
    public class MapReduce : IDisposable
    {
        private readonly IMongoDatabase _database;
        private readonly Type _rootType;
        private bool _disposing;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "MapReduce" /> class.
        /// </summary>
        /// <param name = "database">The database.</param>
        /// <param name = "name">The name.</param>
        /// <param name = "rootType">Type of the root.</param>
        public MapReduce(IMongoDatabase database, string name, Type rootType)
        {
            IsModifiable = true;
            if(database == null)
                throw new ArgumentNullException("database");
            if(name == null)
                throw new ArgumentNullException("name");
            if(rootType == null)
                throw new ArgumentNullException("rootType");

            _rootType = rootType;
            _database = database;
            Command = new MapReduceCommand(name);
        }

        /// <summary>
        /// Gets a value indicating whether this instance is modifiable.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is modifiable; otherwise, <c>false</c>.
        /// </value>
        public bool IsModifiable { get; private set; }

        /// <summary>
        ///   Gets the result.
        /// </summary>
        /// <value>The result.</value>
        internal MapReduceResult Result { get; private set; }

        /// <summary>
        ///   Gets the command.
        /// </summary>
        /// <value>The command.</value>
        public MapReduceCommand Command { get; private set; }

        /// <summary>
        ///   Gets the documents.
        /// </summary>
        /// <value>The documents.</value>
        public IEnumerable<Document> Documents
        {
            get
            {
                if(Result == null)
                    RetrieveData();
                if(Result == null || Result.Ok == false)
                    throw new InvalidOperationException("Documents cannot be iterated when an error was returned from execute.");

                var docs = _database.GetCollection<Document>(Result.CollectionName).FindAll().Documents;
                using((IDisposable)docs)
                {
                    foreach(var doc in docs)
                        yield return doc;
                }
            }
        }

        /// <summary>
        ///   Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if(Command.KeepTemp || Command.Out != null || _disposing)
                return;

            _disposing = true;

            if(Result == null || Result.Ok == false)
                return; //Nothing to do.

            //Drop the temporary collection that was created as part of results.
            _database.Metadata.DropCollection(Result.CollectionName);
        }

        /// <summary>
        ///   The map function references the variable this to inspect the current object under consideration.
        ///   A map function must call emit(key,value) at least once, but may be invoked any number of times, 
        ///   as may be appropriate.
        /// </summary>
        public MapReduce Map(string function)
        {
            return Map(new Code(function));
        }

        /// <summary>
        ///   The map function references the variable this to inspect the current object under consideration.
        ///   A map function must call emit(key,value) at least once, but may be invoked any number of times, 
        ///   as may be appropriate.
        /// </summary>
        public MapReduce Map(Code function)
        {
            TryModify();
            Command.Map = function;
            return this;
        }

        /// <summary>
        ///   The reduce function receives a key and an array of values. To use, reduce the received values, 
        ///   and return a result.
        /// </summary>
        /// <remarks>
        ///   The MapReduce engine may invoke reduce functions iteratively; thus, these functions 
        ///   must be idempotent. If you need to perform an operation only once, use a finalize function.
        /// </remarks>
        public MapReduce Reduce(string function)
        {
            return Reduce(new Code(function));
        }

        /// <summary>
        ///   The reduce function receives a key and an array of values. To use, reduce the received values, 
        ///   and return a result.
        /// </summary>
        /// <remarks>
        ///   The MapReduce engine may invoke reduce functions iteratively; thus, these functions 
        ///   must be idempotent. If you need to perform an operation only once, use a finalize function.
        /// </remarks>
        public MapReduce Reduce(Code function)
        {
            TryModify();
            Command.Reduce = function;
            return this;
        }

        /// <summary>
        ///   Query filter object
        /// </summary>
        public MapReduce Query(Document query)
        {
            TryModify();
            Command.Query = query;
            return this;
        }

        /// <summary>
        ///   Sort the query.  Useful for optimization
        /// </summary>
        public MapReduce Sort(Document sort)
        {
            TryModify();
            Command.Sort = sort;
            return this;
        }

        /// <summary>
        ///   Number of objects to return from collection
        /// </summary>
        public MapReduce Limit(long limit)
        {
            TryModify();
            Command.Limit = limit;
            return this;
        }

        /// <summary>
        ///   Name of the final collection the results should be stored in.
        /// </summary>
        /// <remarks>
        ///   A temporary collection is still used and then renamed to the target name atomically.
        /// </remarks>
        public MapReduce Out(String name)
        {
            TryModify();
            Command.Out = name;
            return this;
        }

        /// <summary>
        ///   When true the generated collection is not treated as temporary.  Specifying out automatically makes
        ///   the collection permanent
        /// </summary>
        public MapReduce KeepTemp(bool keep)
        {
            TryModify();
            Command.KeepTemp = keep;
            return this;
        }

        /// <summary>
        ///   Provides statistics on job execution time.
        /// </summary>
        public MapReduce Verbose(bool val)
        {
            TryModify();
            Command.Verbose = val;
            return this;
        }

        /// <summary>
        ///   Function to apply to all the results when finished.
        /// </summary>
        public MapReduce Finalize(Code function)
        {
            TryModify();
            Command.Finalize = function;
            return this;
        }

        /// <summary>
        ///   Document where fields go into javascript global scope
        /// </summary>
        public MapReduce Scope(Document scope)
        {
            TryModify();
            Command.Scope = scope;
            return this;
        }

        /// <summary>
        ///   Retrieves the data.
        /// </summary>
        internal void RetrieveData()
        {
            if(Command.Command.ContainsKey("map") == false || Command.Command.ContainsKey("reduce") == false)
                throw new InvalidOperationException("Cannot execute without a map and reduce function");

            IsModifiable = false;

            try
            {
                Result = new MapReduceResult(_database.SendCommand(_rootType, Command.Command));
            }
            catch(MongoCommandException exception)
            {
                Result = new MapReduceResult(exception.Error);
                throw new MongoMapReduceException(exception);
            }
        }

        /// <summary>
        ///   Tries the modify.
        /// </summary>
        private void TryModify()
        {
            if(IsModifiable == false)
                throw new InvalidOperationException("Cannot modify a map/reduce that has already executed");
        }
    }
}