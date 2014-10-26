namespace King.ATrak.Test.Azure
{
    using King.ATrak.Azure;
    using King.Azure.Data;
    using NSubstitute;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class BlobItemTests
    {
        [Test]
        public void Constructor()
        {
            var container = Substitute.For<IContainer>();
            new BlobItem(container, new Uri("http://google.com"));
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorContainerNull()
        {
            new BlobItem(null, new Uri("http://google.com"));
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorObjectIdNull()
        {
            var container = Substitute.For<IContainer>();
            new BlobItem(container, null);
        }

        [Test]
        public void IsIStorageItem()
        {
            var container = Substitute.For<IContainer>();
            Assert.IsNotNull(new BlobItem(container, new Uri("http://google.com")) as IStorageItem);
        }
    }
}