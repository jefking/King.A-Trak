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
            new FolderReader("C:\\happy");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorFromNull()
        {
            new FolderReader(null);
        }
    }
}