namespace King.ATrak.Integration.Test.Azure
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
        private readonly string ConnectionString = "UseDevelopmentStorage=true;";

        [Test]
        public async Task Store()
        {
            var containerName = 'a' + Guid.NewGuid().ToString().Replace("-", string.Empty);
            var to = new Container(containerName, ConnectionString);
            await to.CreateIfNotExists();

            var file = string.Format("{0}.bin", Guid.NewGuid());

            var random = new Random();
            var bytes = new byte[8];
            random.NextBytes(bytes);

            var item = Substitute.For<IStorageItem>();
            item.LoadMD5();
            item.Load();
            item.Data.Returns(bytes);
            item.RelativePath.Returns(file);

            var writer = new BlobWriter(to);
            await writer.Store(new[] { item });

            var result = await to.Get(file);
            Assert.AreEqual(bytes, result);

            await to.Delete();
        }

        [Test]
        public async Task StoreBackSlash()
        {
            var containerName = 'a' + Guid.NewGuid().ToString().Replace("-", string.Empty);
            var to = new Container(containerName, ConnectionString);
            await to.CreateIfNotExists();

            var file = string.Format("{0}\\{0}\\{0}.bin", Guid.NewGuid());

            var random = new Random();
            var bytes = new byte[8];
            random.NextBytes(bytes);

            var item = Substitute.For<IStorageItem>();
            item.LoadMD5();
            item.Load();
            item.Data.Returns(bytes);
            item.RelativePath.Returns(file);

            var writer = new BlobWriter(to);
            await writer.Store(new[] { item });

            var result = await to.Get(file.Replace("\\", "/"));
            Assert.AreEqual(bytes, result);

            await to.Delete();
        }

        [Test]
        public async Task StoreForwardSlash()
        {
            var containerName = 'a' + Guid.NewGuid().ToString().Replace("-", string.Empty);
            var to = new Container(containerName, ConnectionString);
            await to.CreateIfNotExists();

            var file = string.Format("{0}/{0}/{0}.bin", Guid.NewGuid());

            var random = new Random();
            var bytes = new byte[8];
            random.NextBytes(bytes);

            var item = Substitute.For<IStorageItem>();
            item.LoadMD5();
            item.Load();
            item.Data.Returns(bytes);
            item.RelativePath.Returns(file);

            var writer = new BlobWriter(to);
            await writer.Store(new[] { item });

            var result = await to.Get(file.Replace("\\", "/"));
            Assert.AreEqual(bytes, result);

            await to.Delete();
        }

        [Test]
        public async Task StoreCacheControl()
        {
            var random = new Random();
            var cache = random.Next(1, int.MaxValue - 1);
            var containerName = 'a' + Guid.NewGuid().ToString().Replace("-", string.Empty);
            var to = new Container(containerName, ConnectionString);
            await to.CreateIfNotExists();

            var file = string.Format("{0}.bin", Guid.NewGuid());

            var bytes = new byte[8];
            random.NextBytes(bytes);

            var item = Substitute.For<IStorageItem>();
            item.LoadMD5();
            item.Load();
            item.Data.Returns(bytes);
            item.RelativePath.Returns(file);

            var writer = new BlobWriter(to, false, cache);
            await writer.Store(new[] { item });

            var result = await to.Properties(file);
            Assert.AreEqual("public, max-age=" + cache, result.CacheControl);

            await to.Delete();
        }
    }
}