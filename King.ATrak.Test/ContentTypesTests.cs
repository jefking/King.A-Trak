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
    }
}