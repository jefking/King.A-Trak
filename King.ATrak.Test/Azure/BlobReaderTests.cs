namespace King.ATrak.Test.Azure
{
    using King.ATrak.Azure;
    using King.Azure.Data;
    using Microsoft.WindowsAzure.Storage.Blob;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
        public void IsIDataLister()
        {
            Assert.IsNotNull(new BlobReader("test", "UseDevelopmentStorage=true;") as IDataLister);
        }

        [Test]
        public void List()
        {
            var uri = new Uri("http://google.com/container/item.txt");
            var container = Substitute.For<IContainer>();
            container.List(null, true).Returns(new[] { new CloudBlockBlob(uri) });
            container.Name.Returns("container");
            
            var w = new BlobReader(container);
            var items = w.List();

            Assert.AreEqual(1, items.Count());
            Assert.AreEqual("item.txt", items.First().RelativePath);

            container.Received().List(null, true);
        }

        [Test]
        public void ListNull()
        {
            var container = Substitute.For<IContainer>();
            container.List(null, true).Returns((IEnumerable<IListBlobItem>)null);

            var w = new BlobReader(container);
            var result = w.List();

            Assert.IsNull(result);

            container.Received().List(null, true);
        }
    }
}