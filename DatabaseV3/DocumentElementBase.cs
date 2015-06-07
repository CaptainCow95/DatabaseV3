namespace DatabaseV3
{
    /// <summary>
    /// Represents a value in a document.
    /// </summary>
    public interface IDocumentElement
    {
        /// <summary>
        /// Gets the value as a boolean.
        /// </summary>
        bool? AsBool
        {
            get;
        }

        /// <summary>
        /// Gets the value as a <see cref="Document"/>.
        /// </summary>
        Document AsDocument
        {
            get;
        }

        /// <summary>
        /// Gets the value as a <see cref="DocumentArray"/>.
        /// </summary>
        DocumentArray AsDocumentArray
        {
            get;
        }

        /// <summary>
        /// Gets the value as a 32-bit integer.
        /// </summary>
        int? AsInt32
        {
            get;
        }

        /// <summary>
        /// Gets the value as a 64-bit integer.
        /// </summary>
        long? AsInt64
        {
            get;
        }

        /// <summary>
        /// Gets the value as a string.
        /// </summary>
        string AsString
        {
            get;
        }
    }
}