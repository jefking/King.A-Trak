namespace King.ATrak.Integration.Test.Windows
{
    using King.ATrak.Windows;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.IO;
    using System.Threading.Tasks;

    [TestFixture]
    public class FolderWriterTests
    {
        [Test]
        public void Initialize()
        {
            var root = string.Format("{0}\\{1}", Environment.CurrentDirectory, Guid.NewGuid());
            var writer = new FolderWriter(root);
            writer.Initialize();

            var exists = Directory.Exists(root);
            Assert.IsTrue(exists);
        }

        [Test]
        public async Task Store()
        {
            var root = string.Format("{0}\\{1}", Environment.CurrentDirectory, Guid.NewGuid());
            var file = string.Format("{0}.bin", Guid.NewGuid());
            Directory.CreateDirectory(root);

            var random = new Random();
            var bytes = new byte[8];
            random.NextBytes(bytes);

            var item = Substitute.For<IStorageItem>();
            item.Load();
            item.Data.Returns(bytes);
            item.RelativePath.Returns(file);

            var writer = new FolderWriter(root);
            await writer.Store(new[] { item });

            Assert.IsTrue(File.Exists(string.Format("{0}\\{1}", root, file)));
        }
    }
}