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
            new Disk(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
        }

        [TestMethod]
        public void IsIStorageItem()
        {
            var d = new Disk(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            Assert.IsNotNull(d as IStorageItem);
        }
    }
}