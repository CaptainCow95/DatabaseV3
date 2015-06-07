using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DatabaseV3
{
    /// <summary>
    /// Represents a document.
    /// </summary>
    public class Document : IDocumentElement
    {
        /// <summary>
        /// The data contained in the document.
        /// </summary>
        private Dictionary<string, IDocumentElement> _data = new Dictionary<string, IDocumentElement>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Document"/> class.
        /// </summary>
        /// <param name="data">The data in the document.</param>
        public Document(Dictionary<string, object> data)
        {
            foreach (var item in data)
            {
                if (item.Value is IDocumentElement)
                {
                    _data.Add(item.Key, (IDocumentElement)item.Value);
                }
                else
                {
                    _data.Add(item.Key, new DocumentElement(item.Value));
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Document"/> class.
        /// </summary>
        public Document()
        {
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
                return this;
            }
        }

        /// <inheritdoc />
        public DocumentArray AsDocumentArray
        {
            get
            {
                return null;
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
        public IDocumentElement this[string s]
        {
            get
            {
                return _data[s];
            }

            set
            {
                _data[s] = value;
            }
        }

        /// <summary>
        /// Parses JSON into a <see cref="Document"/>.
        /// </summary>
        /// <param name="json">The JSON to parse.</param>
        /// <returns>The <see cref="Document"/> represented by the JSON.</returns>
        public static Document Parse(string json)
        {
            Document document;
            using (JsonTextReader reader = new JsonTextReader(new StringReader(json)))
            {
                try
                {
                    reader.Read();
                    document = Parse(reader);

                    if (reader.Read())
                    {
                        throw new ArgumentException("Parameter contains invalid JSON.", nameof(json));
                    }
                }
                catch (JsonReaderException)
                {
                    throw new ArgumentException("Parameter contains invalid JSON.", nameof(json));
                }
            }

            return document;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            Document value = obj as Document;
            return value != null && _data.Count == value.Count && _data.Except(value._data).Any();
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
            builder.Append("{");

            foreach (var item in _data)
            {
                builder.Append("\"");
                builder.Append(item.Key);
                builder.Append("\":");
                builder.Append(item.Value);
                builder.Append(",");
            }

            builder.Length = builder.Length - 1;
            builder.Append("}");

            return builder.ToString();
        }

        /// <summary>
        /// Parses a <see cref="JsonTextReader"/> into a <see cref="Document"/>.
        /// </summary>
        /// <param name="reader">The <see cref="JsonTextReader"/> to parse.</param>
        /// <returns>The <see cref="Document"/> represented by the <see cref="JsonTextReader"/>.</returns>
        private static Document Parse(JsonTextReader reader)
        {
            Document document = new Document();
            if (reader.TokenType != JsonToken.StartObject)
            {
                throw new ArgumentException("Parameter contains invalid JSON.", nameof(reader));
            }

            while (reader.Read() && reader.TokenType != JsonToken.EndObject)
            {
                string key = (string)reader.Value;
                reader.Read();
                document[key] = ParseItem(reader);
            }

            if (reader.TokenType != JsonToken.EndObject)
            {
                throw new ArgumentException("Parameter contains invalid JSON.", nameof(reader));
            }

            return document;
        }

        /// <summary>
        /// Parses a <see cref="JsonTextReader"/> into a <see cref="IDocumentElement"/>.
        /// </summary>
        /// <param name="reader">The <see cref="JsonTextReader"/> to parse.</param>
        /// <returns>The <see cref="IDocumentElement"/> represented by the <see cref="JsonTextReader"/>.</returns>
        private static IDocumentElement ParseItem(JsonTextReader reader)
        {
            switch (reader.TokenType)
            {
                case JsonToken.StartArray:
                    var arrayContents = new List<IDocumentElement>();
                    while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                    {
                        arrayContents.Add(ParseItem(reader));
                    }

                    return new DocumentArray(arrayContents);

                case JsonToken.StartObject:
                    return Parse(reader);

                default:
                    return new DocumentElement(reader.Value);
            }
        }
    }
}