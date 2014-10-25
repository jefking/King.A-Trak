namespace King.ATrak.Test
{
    using King.ATrak;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class DiskTests
    {
        [TestMethod]
        public void Constructor()
        {
            new FileItem(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
        }

        [TestMethod]
        public void IsIStorageItem()
        {
            var d = new FileItem(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            Assert.IsNotNull(d as IStorageItem);
        }
    }
}