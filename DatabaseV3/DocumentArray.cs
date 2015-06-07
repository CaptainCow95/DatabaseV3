using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatabaseV3
{
    /// <summary>
    /// Represents an array in a document.
    /// </summary>
    public class DocumentArray : IDocumentElement
    {
        /// <summary>
        /// The data in the array.
        /// </summary>
        private List<IDocumentElement> _data = new List<IDocumentElement>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentArray"/> class.
        /// </summary>
        /// <param name="data">The data in the array.</param>
        public DocumentArray(IEnumerable<object> data)
        {
            foreach (var item in data)
            {
                if (item is IDocumentElement)
                {
                    _data.Add((IDocumentElement)item);
                }
                else
                {
                    _data.Add(new DocumentElement(item));
                }
            }
        }

        /// <inheritdoc />
        public bool? AsBool
        {
            get
            {
                return null;
            }
        }

        /// <inheritdoc />
        public Document AsDocument
        {
            get
            {
                return null;
            }
        }

        /// <inheritdoc />
        public DocumentArray AsDocumentArray
        {
            get
            {
                return this;
            }
        }

        /// <inheritdoc />
        public int? AsInt32
        {
            get
            {
                return null;
            }
        }

        /// <inheritdoc />
        public long? AsInt64
        {
            get
            {
                return null;
            }
        }

        /// <inheritdoc />
        public string AsString
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
        public IDocumentElement this[int i]
        {
            get
            {
                return _data[i];
            }
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            DocumentArray array = obj as DocumentArray;
            return array != null && _data.Count == array._data.Count && _data.Except(array._data).Any();
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