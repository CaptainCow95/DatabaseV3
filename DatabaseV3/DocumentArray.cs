using System.Collections.Generic;
using System.Text;

namespace DatabaseV3
{
    /// <summary>
    /// Represents an array in a document.
    /// </summary>
    public class DocumentArray : DocumentValue
    {
        /// <summary>
        /// The data in the array.
        /// </summary>
        private List<DocumentValue> _data = new List<DocumentValue>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentArray"/> class.
        /// </summary>
        /// <param name="data">The data in the array.</param>
        public DocumentArray(IEnumerable<object> data)
        {
            foreach (var item in data)
            {
                if (item is DocumentValue)
                {
                    _data.Add((DocumentValue)item);
                }
                else
                {
                    _data.Add(new DocumentElement(item));
                }
            }
        }

        /// <inheritdoc />
        public override bool? AsBool
        {
            get
            {
                return null;
            }
        }

        /// <inheritdoc />
        public override byte[] AsByteArray
        {
            get
            {
                return null;
            }
        }

        /// <inheritdoc />
        public override Document AsDocument
        {
            get
            {
                return null;
            }
        }

        /// <inheritdoc />
        public override DocumentArray AsDocumentArray
        {
            get
            {
                return this;
            }
        }

        /// <inheritdoc />
        public override int? AsInt32
        {
            get
            {
                return null;
            }
        }

        /// <inheritdoc />
        public override long? AsInt64
        {
            get
            {
                return null;
            }
        }

        /// <inheritdoc />
        public override string AsString
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the value at the specified index.
        /// </summary>
        /// <param name="i">The index to get.</param>
        /// <returns>The value at the specified index.</returns>
        public DocumentValue this[int i]
        {
            get
            {
                return _data[i];
            }
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            DocumentArray value = obj as DocumentArray;
            return value != null && Equals(_data, value._data);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return _data.GetHashCode();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("[");
            bool first = true;
            foreach (var item in _data)
            {
                if (!first)
                {
                    builder.Append(",");
                }
                else
                {
                    first = false;
                }

                builder.Append(item);
            }

            builder.Append("]");
            return builder.ToString();
        }
    }
}