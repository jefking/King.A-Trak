namespace King.ATrak.Test.Azure
{
    using King.ATrak.Azure;
    using King.Azure.Data;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

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

        [Test]
        public async Task Initialize()
        {
            var container = Substitute.For<IContainer>();
            container.CreateIfNotExists().Returns(Task.FromResult(true));

            var w = new BlobWriter(container);
            var result = await w.Initialize();

            Assert.IsTrue(result);

            container.Received().CreateIfNotExists();
        }

        [Test]
        public async Task InitializeNotCreate()
        {
            var container = Substitute.For<IContainer>();
            container.CreateIfNotExists().Returns(Task.FromResult(false));

            var w = new BlobWriter(container);
            var result = await w.Initialize();

            Assert.IsFalse(result);

            container.Received().CreateIfNotExists();
        }

        [Test]
        public async Task Store()
        {
            var random = new Random();
            var data = new byte[64];
            random.NextBytes(data);
            var relPath = Guid.NewGuid().ToString();
            var contentType = Guid.NewGuid().ToString();

            var item = Substitute.For<IStorageItem>();
            item.RelativePath.Returns(relPath);
            item.Data.Returns(data);
            item.ContentType.Returns(contentType);
            item.Load();

            var container = Substitute.For<IContainer>();
            container.Save(relPath, data, contentType);

            var w = new BlobWriter(container);
            await w.Store(new[] { item });

            var x = item.Received().RelativePath;
            var y = item.Received().Data;
            var z = item.Received().ContentType;
            item.Received().Load();
            container.Received().Save(relPath, data, contentType);
        }
    }
}