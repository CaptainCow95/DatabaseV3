namespace DatabaseV3
{
    /// <summary>
    /// Represents a value in a document.
    /// </summary>
    public abstract class DocumentValue
    {
        /// <summary>
        /// Gets the value as a boolean.
        /// </summary>
        public abstract bool? AsBool
        {
            get;
        }

        /// <summary>
        /// Gets the value as a byte array.
        /// </summary>
        public abstract byte[] AsByteArray
        {
            get;
        }

        /// <summary>
        /// Gets the value as a <see cref="Document"/>.
        /// </summary>
        public abstract Document AsDocument
        {
            get;
        }

        /// <summary>
        /// Gets the value as a <see cref="DocumentArray"/>.
        /// </summary>
        public abstract DocumentArray AsDocumentArray
        {
            get;
        }

        /// <summary>
        /// Gets the value as a 32-bit integer.
        /// </summary>
        public abstract int? AsInt32
        {
            get;
        }

        /// <summary>
        /// Gets the value as a 64-bit integer.
        /// </summary>
        public abstract long? AsInt64
        {
            get;
        }

        /// <summary>
        /// Gets the value as a string.
        /// </summary>
        public abstract string AsString
        {
            get;
        }
    }
}