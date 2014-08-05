using MongoDB.Attributes;

namespace MongoDB.Results
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class CommandResultBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandResultBase"/> class.
        /// </summary>
        protected CommandResultBase(){
            ExtendedProperties = new Document();
        }

        /// <summary>
        /// Gets or sets the server error message.
        /// </summary>
        /// <value>The error message.</value>
        [MongoAlias("errmsg")]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CommandResultBase"/> is success.
        /// </summary>
        /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
        [MongoAlias("ok")]
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the extended properties.
        /// </summary>
        /// <remarks>
        /// Stores all propertys which are not maped.
        /// </remarks>
        /// <value>The extended properties.</value>
        public Document ExtendedProperties { get; set; }
    }
}