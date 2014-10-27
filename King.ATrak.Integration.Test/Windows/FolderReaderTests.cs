namespace King.ATrak.Integration.Test.Windows
{
    using King.ATrak.Windows;
    using NUnit.Framework;
    using System;
    using System.IO;
    using System.Linq;

    [TestFixture]
    public class FolderReaderTests
    {
        [Test]
        public void List()
        {
            var random = new Random();
            var count = random.Next(1, 25);
            var root = string.Format("{0}\\{1}", Environment.CurrentDirectory, Guid.NewGuid());
            Directory.CreateDirectory(root);

            for (var i = 0; i < count; i++)
            {
                var bytes = new byte[8];
                random.NextBytes(bytes);

                var folder = string.Format("{0}\\{1}", root, Guid.NewGuid());
                Directory.CreateDirectory(folder);

                var file = string.Format("{0}\\{1}.bin", folder, Guid.NewGuid());
                File.WriteAllBytes(file, bytes);
            }

            var reader = new FolderReader(root);
            var results = reader.List();

            Assert.IsNotNull(results);
            Assert.AreEqual(count, results.Count());
        }
    }
}