namespace King.ATrak.Test.Azure
{
    using King.ATrak.Azure;
    using King.Azure.Data;
    using NSubstitute;
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

        [Test]
        public void List()
        {
            var container = Substitute.For<IContainer>();
            container.List();

            var w = new BlobReader(container);
            w.List();
            
            container.Received().List();
        }
    }
}