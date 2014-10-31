namespace King.ATrak.Integration.Test.Windows
{
    using King.ATrak.Windows;
    using NUnit.Framework;
    using System;
    using System.IO;
    using System.Threading.Tasks;

    [TestFixture]
    public class FileItemTests
    {
        [Test]
        public async Task Load()
        {
            var c = new ContentTypes();
            var root = Environment.CurrentDirectory;
            var path = string.Format("{0}\\{1}.bin", root, Guid.NewGuid());

            var random = new Random();
            var bytes = new byte[64];
            random.NextBytes(bytes);

            string md5;
            using (var createHash = System.Security.Cryptography.MD5.Create())
            {
                var hash = createHash.ComputeHash(bytes);
                md5 = System.Convert.ToBase64String(hash);
            }

            File.WriteAllBytes(path, bytes);

            var item = new FileItem(root, path);
            await item.Load();

            Assert.AreEqual(bytes, item.Data);
            Assert.AreEqual(c.ContentType(path), item.ContentType);
            Assert.AreEqual(md5, item.MD5);
        }

        [Test]
        public async Task LoadMD5()
        {
            var c = new ContentTypes();
            var root = Environment.CurrentDirectory;
            var path = string.Format("{0}\\{1}.bin", root, Guid.NewGuid());

            var random = new Random();
            var bytes = new byte[64];
            random.NextBytes(bytes);

            string md5;
            using (var createHash = System.Security.Cryptography.MD5.Create())
            {
                var hash = createHash.ComputeHash(bytes);
                md5 = System.Convert.ToBase64String(hash);
            }

            File.WriteAllBytes(path, bytes);

            var item = new FileItem(root, path);
            await item.LoadMD5();

            Assert.AreEqual(bytes, item.Data);
            Assert.AreEqual(c.ContentType(path), item.ContentType);
            Assert.AreEqual(md5, item.MD5);
        }
    }
}