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
            new BlobItem(container, "/file.txt");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorContainerNull()
        {
            new BlobItem(null, "/file.txt");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorPathNull()
        {
            var container = Substitute.For<IContainer>();
            new BlobItem(container, null);
        }

        [Test]
        public void IsIStorageItem()
        {
            var container = Substitute.For<IContainer>();
            Assert.IsNotNull(new BlobItem(container, "/file.txt") as IStorageItem);
        }
    }
}