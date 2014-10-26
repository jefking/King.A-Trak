namespace King.ATrak.Test.Azure
{
    using King.ATrak.Azure;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class BlobWriterTests
    {
        [Test]
        public void Constructor()
        {
            new BlobWriter("test", "UseDevelopmentStorage=true;");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorContainerNull()
        {
            new BlobWriter(null);
        }
    }
}