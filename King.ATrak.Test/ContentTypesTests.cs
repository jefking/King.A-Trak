namespace King.ATrak.Test
{
    using King.ATrak;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class ContentTypesTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ContentTypeFilePathInvalid()
        {
            ContentTypes.ContentType(null);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ContentTypeFilePathWierd()
        {
            ContentTypes.ContentType(". ");
        }

        [TestCase("image/jpeg", ".jpg")]
        [TestCase("text/css", ".css")]
        [TestCase("image/png", ".png")]
        [TestCase("image/x-icon", ".ico")]
        [TestCase("text/plain", ".txt")]
        [TestCase("", ".jrnl")]
        public void Types(string expected, string extension)
        {
            Assert.AreEqual(expected, ContentTypes.ContentType(extension));
        }

        [TestCase("image/jpeg", ".jpg")]
        [TestCase("text/css", ".css")]
        [TestCase("image/png", ".png")]
        [TestCase("image/x-icon", ".ico")]
        [TestCase("text/plain", ".txt")]
        public void TypesCached(string expected, string extension)
        {
            Assert.AreEqual(expected, ContentTypes.ContentType(extension));
            Assert.AreEqual(expected, ContentTypes.ContentType(extension));
            Assert.AreEqual(expected, ContentTypes.ContentType(extension));
        }
    }
}