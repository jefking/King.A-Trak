namespace King.ATrak.Test.Azure
{
    using King.ATrak.Azure;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class BlobReaderTests
    {
        [Test]
        public void Constructor()
        {
            new BlobReader("test", "UseDevelopmentStorage=true;");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorContainerNull()
        {
            new BlobReader(null);
        }
    }
}