using System;

namespace DatabaseV3
{
    /// <summary>
    /// Represents an element in a document.
    /// </summary>
    public class DocumentElement : IDocumentElement
    {
        /// <summary>
        /// The data in the document element.
        /// </summary>
        private object _data;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentElement"/> class.
        /// </summary>
        /// <param name="data">The data in the document element.</param>
        public DocumentElement(object data)
        {
            _data = data;
        }

        /// <inheritdoc />
        public bool? AsBool
        {
            get
            {
                if (_data is bool)
                {
                    return (bool)_data;
                }
                else if (_data is string)
                {
                    bool result;
                    if (bool.TryParse((string)_data, out result))
                    {
                        return result;
                    }
                }

                return null;
            }
        }

        /// <inheritdoc />
        public Document AsDocument
        {
            get
            {
                if (_data is Document)
                {
                    return (Document)_data;
                }

                return null;
            }
        }

        /// <inheritdoc />
        public DocumentArray AsDocumentArray
        {
            get
            {
                if (_data is DocumentArray)
                {
                    return (DocumentArray)_data;
                }

                return null;
            }
        }

        /// <inheritdoc />
        public int? AsInt32
        {
            get
            {
                if (_data is int)
                {
                    return (int)_data;
                }
                else if (_data is long)
                {
                    if ((long)_data >= int.MinValue && (long)_data <= int.MaxValue)
                    {
                        return (int)(long)_data;
                    }
                }
                else if (_data is string)
                {
                    int result;
                    if (int.TryParse((string)_data, out result))
                    {
                        return result;
                    }
                }

                return null;
            }
        }

        /// <inheritdoc />
        public long? AsInt64
        {
            get
            {
                if (_data is int)
                {
                    return (int)_data;
                }
                else if (_data is long)
                {
                    return (long)_data;
                }
                else if (_data is string)
                {
                    long result;
                    if (long.TryParse((string)_data, out result))
                    {
                        return result;
                    }
                }

                return null;
            }
        }

        /// <inheritdoc />
        public string AsString
        {
            get
            {
                if (_data is int || _data is long || _data is string || _data is bool)
                {
                    return Convert.ToString(_data);
                }

                return null;
            }
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return _data.Equals(obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return _data.GetHashCode();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            if (_data is string)
            {
                return '\"' + _data.ToString() + '\"';
            }
            else if (_data is bool)
            {
                return _data.ToString().ToLowerInvariant();
            }
            else
            {
                return _data.ToString();
            }
        }
    }
}