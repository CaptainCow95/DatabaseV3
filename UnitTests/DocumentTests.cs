using DatabaseV3;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace UnitTests
{
    /// <summary>
    /// A suite of tests for the <see cref="Document"/> class.
    /// </summary>
    [TestFixture]
    public class DocumentTests
    {
        /// <summary>
        /// Tests using a boolean.
        /// </summary>
        [Test]
        public void BoolItem()
        {
            bool value = true;
            Document document = new Document(new Dictionary<string, object> { { "data", value } });
            CheckDocument(document, "data", value, null, null, null, null, value.ToString());
        }

        /// <summary>
        /// Tests using a string as a boolean.
        /// </summary>
        [Test]
        public void BoolStringItem()
        {
            string value = "true";
            Document document = new Document(new Dictionary<string, object> { { "data", value } });
            CheckDocument(document, "data", true, null, null, null, null, value);
        }

        /// <summary>
        /// Ensures a key in a document has the specified values.
        /// </summary>
        /// <param name="doc">The document to test.</param>
        /// <param name="key">The key to test.</param>
        /// <param name="boolValue">The boolean value.</param>
        /// <param name="documentValue">The document value.</param>
        /// <param name="documentArrayValue">The document array value.</param>
        /// <param name="intValue">The 32-bit integer value.</param>
        /// <param name="longValue">The 64-bit integer value.</param>
        /// <param name="stringValue">The string value.</param>
        public void CheckDocument(Document doc, string key, bool? boolValue, Document documentValue, DocumentArray documentArrayValue, int? intValue, long? longValue, string stringValue)
        {
            Assert.AreEqual(boolValue, doc[key].AsBool);
            Assert.AreEqual(documentValue, doc[key].AsDocument);
            Assert.AreEqual(documentArrayValue, doc[key].AsDocumentArray);
            Assert.AreEqual(intValue, doc[key].AsInt32);
            Assert.AreEqual(longValue, doc[key].AsInt64);
            Assert.AreEqual(stringValue, doc[key].AsString);
        }

        /// <summary>
        /// Tests using a <see cref="DocumentArray"/>.
        /// </summary>
        [Test]
        public void DocumentArrayItem()
        {
            DocumentArray value = new DocumentArray(new List<object> { 5, "test", true });
            Document document = new Document(new Dictionary<string, object> { { "data", value } });
            CheckDocument(document, "data", null, null, value, null, null, null);
        }

        /// <summary>
        /// Tests using a <see cref="Document"/>.
        /// </summary>
        [Test]
        public void DocumentItem()
        {
            Document value = new Document(new Dictionary<string, object> { { "Test", "Testing" } });
            Document document = new Document(new Dictionary<string, object> { { "data", value } });
            CheckDocument(document, "data", null, value, null, null, null, null);
        }

        /// <summary>
        /// Tests using a 32-bit integer.
        /// </summary>
        [Test]
        public void Int32Item()
        {
            int value = int.MaxValue;
            Document document = new Document(new Dictionary<string, object> { { "data", value } });
            CheckDocument(document, "data", null, null, null, value, value, value.ToString());
        }

        /// <summary>
        /// Tests using a string as a 32-bit integer.
        /// </summary>
        [Test]
        public void Int32StringItem()
        {
            string value = "1234";
            Document document = new Document(new Dictionary<string, object> { { "data", value } });
            CheckDocument(document, "data", null, null, null, 1234, 1234, value);
        }

        /// <summary>
        /// Tests using a 64-bit integer.
        /// </summary>
        [Test]
        public void Int64Item()
        {
            long value = long.MaxValue;
            Document document = new Document(new Dictionary<string, object> { { "data", value } });
            CheckDocument(document, "data", null, null, null, null, value, value.ToString());
        }

        /// <summary>
        /// Tests using a string as a 64-bit integer.
        /// </summary>
        [Test]
        public void Int64StringItem()
        {
            string value = "9876543210";
            Document document = new Document(new Dictionary<string, object> { { "data", value } });
            CheckDocument(document, "data", null, null, null, null, 9876543210, value);
        }

        /// <summary>
        /// Tests parsing JSON into a document.
        /// </summary>
        [Test]
        public void Parse()
        {
            string json = "{\"string\":\"test\",\"int\":12345,\"long\":9876543210,\"bool\":true,\"document\":{\"key\":\"value\",\"key2\":false},\"array\":[1,2,true,\"key\"]}";
            Document document = Document.Parse(json);
            Assert.AreEqual("test", document["string"].AsString);
            Assert.AreEqual(12345, document["int"].AsInt32);
            Assert.AreEqual(9876543210, document["long"].AsInt64);
            Assert.AreEqual(true, document["bool"].AsBool);
            Assert.AreEqual(new Document(new Dictionary<string, object> { { "key", "value" }, { "key2", false } }), document["document"].AsDocument);
            Assert.AreEqual(new DocumentArray(new List<object> { 1, 2, true, "key" }), document["array"].AsDocumentArray);
            Assert.AreEqual(json, document.ToString());
        }

        /// <summary>
        /// Tests parsing invalid JSON into a document.
        /// </summary>
        [Test]
        public void ParseInvalid()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Document.Parse("{\"key\":\"value\"");
            });

            Assert.Throws<ArgumentException>(() =>
            {
                Document.Parse("{\"key:\"value\"}");
            });

            Assert.Throws<ArgumentException>(() =>
            {
                Document.Parse("{\"key\":\"value\"}}");
            });
        }

        /// <summary>
        /// Tests using a string.
        /// </summary>
        [Test]
        public void StringItem()
        {
            string value = "Test";
            Document document = new Document(new Dictionary<string, object> { { "data", value } });
            CheckDocument(document, "data", null, null, null, null, null, value);
        }
    }
}