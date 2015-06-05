using System.Collections.Generic;

namespace DatabaseV3
{
    /// <summary>
    /// Represents a document.
    /// </summary>
    public class Document
    {
        /// <summary>
        /// The data contained in the document.
        /// </summary>
        private Dictionary<string, DocumentValue> _data = new Dictionary<string, DocumentValue>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Document"/> class.
        /// </summary>
        /// <param name="data">The data in the document.</param>
        public Document(Dictionary<string, object> data)
        {
            foreach (var item in data)
            {
                if (item.Value is DocumentValue)
                {
                    _data.Add(item.Key, (DocumentValue)item.Value);
                }
                else
                {
                    _data.Add(item.Key, new DocumentElement(item.Value));
                }
            }
        }

        /// <summary>
        /// Gets the count of items in the document.
        /// </summary>
        public int Count
        {
            get
            {
                return _data.Count;
            }
        }

        /// <summary>
        /// Gets the value at the specified key.
        /// </summary>
        /// <param name="s">The key to get.</param>
        /// <returns>The value at the specified key.</returns>
        public DocumentValue this[string s]
        {
            get
            {
                return _data[s];
            }
        }
    }
}