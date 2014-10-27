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
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorRootNull()
        {
            new FileItem(null, "temp.csv");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorPathNull()
        {
            new FileItem("C:\\happy", null);
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