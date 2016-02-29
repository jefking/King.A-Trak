namespace King.ATrak.Test.Windows
{
    using King.ATrak.Windows;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class FileItemTests
    {
        [Test]
        public void Constructor()
        {
            new FileItem("C:\\happy", "temp.csv");
        }

        [Test]
        public void ConstructorRootNull()
        {
            Assert.That(() => new FileItem(null, "temp.csv"), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void ConstructorPathNull()
        {
            Assert.That(() => new FileItem("C:\\happy", null), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void IsIStorageItem()
        {
            Assert.IsNotNull(new FileItem("C:\\happy", "temp.csv") as IStorageItem);
        }

        [Test]
        public void RelativePath()
        {
            var f = new FileItem("C:\\happy", "C:\\happy\\temp.csv");
            Assert.AreEqual("temp.csv", f.RelativePath);
        }

        [Test]
        public void RelativePathBackSlashes()
        {
            var f = new FileItem("C:\\happy\\", "C:\\happy\\temp.csv");
            Assert.AreEqual("temp.csv", f.RelativePath);
        }
    }
}