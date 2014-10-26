namespace King.ATrak.Test.Windows
{
    using King.ATrak.Windows;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class FolderReaderTests
    {
        [Test]
        public void Constructor()
        {
            new FolderReader("C:\\folder");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorFromNull()
        {
            new FolderReader(null);
        }

        [Test]
        public void IsIDataLister()
        {
            Assert.IsNotNull(new FolderReader("C:\\folder") as IDataLister);
        }
    }
}