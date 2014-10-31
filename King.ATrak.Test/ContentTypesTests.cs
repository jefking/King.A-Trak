namespace King.ATrak.Test
{
    using King.ATrak;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class ContentTypesTests
    {
        [Test]
        public void Contstructor()
        {
            new ContentTypes();
        }

        [Test]
        public void IsIContentTypes()
        {
            Assert.IsNotNull(new ContentTypes() as IContentTypes);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ContentTypeFilePathInvalid()
        {
            var c = new ContentTypes();
            c.ContentType(null);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ContentTypeFilePathWierd()
        {
            var c = new ContentTypes();
            c.ContentType(". ");
        }

        [TestCase("image/jpeg", ".jpg")]
        [TestCase("text/css", ".css")]
        [TestCase("image/png", ".png")]
        [TestCase("image/x-icon", ".ico")]
        [TestCase("text/plain", ".txt")]
        [TestCase("", ".jrnl")]
        public void Types(string expected, string extension)
        {
            var c = new ContentTypes();
            Assert.AreEqual(expected, c.ContentType(extension));
        }

        [TestCase("image/jpeg", ".jpg")]
        [TestCase("text/css", ".css")]
        [TestCase("image/png", ".png")]
        [TestCase("image/x-icon", ".ico")]
        [TestCase("text/plain", ".txt")]
        public void TypesCached(string expected, string extension)
        {
            var c = new ContentTypes();
            Assert.AreEqual(expected, c.ContentType(extension));
            Assert.AreEqual(expected, c.ContentType(extension));
            Assert.AreEqual(expected, c.ContentType(extension));
        }
    }
}