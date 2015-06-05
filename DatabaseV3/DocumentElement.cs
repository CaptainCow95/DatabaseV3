using System;

namespace DatabaseV3
{
    /// <summary>
    /// Represents an element in a document.
    /// </summary>
    public class DocumentElement : DocumentValue
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
        public override bool? AsBool
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
        public override byte[] AsByteArray
        {
            get
            {
                if (_data is byte[])
                {
                    return (byte[])_data;
                }

                return null;
            }
        }

        /// <inheritdoc />
        public override Document AsDocument
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
        public override DocumentArray AsDocumentArray
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
        public override int? AsInt32
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
        public override long? AsInt64
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
        public override string AsString
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
        public override string ToString()
        {
            return _data.ToString();
        }
    }
}