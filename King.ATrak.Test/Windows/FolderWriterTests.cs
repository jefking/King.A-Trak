namespace King.ATrak.Test.Windows
{
    using King.ATrak.Windows;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class FolderWriterTests
    {
        [Test]
        public void Constructor()
        {
            new FolderWriter("C:\\happy");
        }

        [Test]
        public void ConstructorToNull()
        {
            Assert.That(() => new FolderWriter(null), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void IsIDataWriter()
        {
            Assert.IsNotNull(new FolderWriter("C:\\happy") as IDataWriter);
        }
    }
}