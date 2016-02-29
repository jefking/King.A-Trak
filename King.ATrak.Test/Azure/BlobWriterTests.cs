namespace King.ATrak.Test.Azure
{
    using King.ATrak.Azure;
    using King.Azure.Data;
    using Microsoft.WindowsAzure.Storage.Blob;
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
        public void ConstructorCacheControllNegative()
        {
            new BlobWriter("test", "UseDevelopmentStorage=true;", false, -100);
        }

        [Test]
        public void ConstructorContainerNull()
        {
            Assert.That(() => new BlobWriter(null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void IsIDataWriter()
        {
            Assert.IsNotNull(new BlobWriter("test", "UseDevelopmentStorage=true;") as IDataWriter);
        }

        [Test]
        public async Task Initialize()
        {
            var container = Substitute.For<IContainer>();
            container.CreateIfNotExists().Returns(Task.FromResult(true));

            var w = new BlobWriter(container);
            var result = await w.Initialize();

            Assert.IsTrue(result);

            await container.Received().CreateIfNotExists();
        }

        [Test]
        public async Task InitializeNotCreate()
        {
            var container = Substitute.For<IContainer>();
            container.CreateIfNotExists().Returns(Task.FromResult(false));

            var w = new BlobWriter(container);
            var result = await w.Initialize();

            Assert.IsFalse(result);

            await container.Received().CreateIfNotExists();
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
            item.LoadMD5();
            item.Load();

            var container = Substitute.For<IContainer>();
            container.Save(relPath, data, contentType);
            container.Exists(relPath).Returns(Task.FromResult(false));

            var w = new BlobWriter(container);
            await w.Store(new[] { item });

            var x = item.Received().RelativePath;
            var y = item.Received().Data;
            var z = item.Received().ContentType;
            item.Received().LoadMD5();
            item.Received().Load();
            container.Received().Save(relPath, data, contentType);
            container.Received().Exists(relPath);
        }

        [Test]
        public async Task StoreCreateSnapshot()
        {
            var random = new Random();
            var data = new byte[64];
            random.NextBytes(data);
            var relPath = Guid.NewGuid().ToString();
            var contentType = Guid.NewGuid().ToString();

            var p = new BlobProperties()
            {
                ContentType = Guid.NewGuid().ToString(),
                ContentMD5 = Guid.NewGuid().ToString(),
            };

            var item = Substitute.For<IStorageItem>();
            item.RelativePath.Returns(relPath);
            item.Data.Returns(data);
            item.ContentType.Returns(contentType);
            item.LoadMD5();
            item.Load();

            var container = Substitute.For<IContainer>();
            container.Save(relPath, data, contentType);
            container.Exists(relPath).Returns(Task.FromResult(true));
            container.Snapshot(relPath);
            container.Properties(relPath).Returns(Task.FromResult(p));

            var w = new BlobWriter(container, true);
            await w.Store(new[] { item });

            var x = item.Received().RelativePath;
            var y = item.Received().Data;
            var z = item.Received().ContentType;
            item.Received().LoadMD5();
            item.Received().Load();
            container.Received().Save(relPath, data, contentType);
            container.Received().Exists(relPath);
            container.Received().Snapshot(relPath);
            container.Received().Properties(relPath);
        }

        [Test]
        public async Task StoreCreateSnapshotDoesntExist()
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
            item.LoadMD5();
            item.Load();

            var container = Substitute.For<IContainer>();
            container.Save(relPath, data, contentType);
            container.Exists(relPath).Returns(Task.FromResult(false));

            var w = new BlobWriter(container, true);
            await w.Store(new[] { item });

            var x = item.Received().RelativePath;
            var y = item.Received().Data;
            var z = item.Received().ContentType;
            item.Received().LoadMD5();
            item.Received().Load();
            container.Received().Save(relPath, data, contentType);
            container.Received().Exists(relPath);
            container.Received(0).Snapshot(relPath);
            container.Received().Exists(relPath);
        }

        [Test]
        public async Task StoreSameMd5()
        {
            var random = new Random();
            var data = new byte[64];
            random.NextBytes(data);
            var relPath = Guid.NewGuid().ToString();
            var contentType = Guid.NewGuid().ToString();

            var p = new BlobProperties()
            {
                ContentType = Guid.NewGuid().ToString(),
                ContentMD5 = Guid.NewGuid().ToString(),
            };

            var item = Substitute.For<IStorageItem>();
            item.RelativePath.Returns(relPath);
            item.MD5.Returns(p.ContentMD5);
            item.LoadMD5();

            var container = Substitute.For<IContainer>();
            container.Save(relPath, data, contentType);
            container.Exists(relPath).Returns(Task.FromResult(true));
            container.Snapshot(relPath);
            container.Properties(relPath).Returns(Task.FromResult(p));

            var w = new BlobWriter(container, true);
            await w.Store(new[] { item });

            var u = item.Received().MD5;
            var x = item.Received().RelativePath;
            var y = item.Received(0).Data;
            item.Received().LoadMD5();
            item.Received(0).Load();
            container.Received().Save(relPath, data, contentType);
            container.Received().Exists(relPath);
            container.Received().Snapshot(relPath);
            container.Received().Properties(relPath);
        }
    }
}