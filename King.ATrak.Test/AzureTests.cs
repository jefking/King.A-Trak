namespace King.ATrak.Test
{
    using Abc.ATrak;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.WindowsAzure.Storage.Blob;
    using System;

    [TestClass]
    public class AzureTests
    {
        [TestMethod]
        public void Constructor()
        {
            var c = new CloudBlobContainer(new Uri("http://myblob.com"));
            new Azure(c, "happy-blob");
        }

        [TestMethod]
        public void IsIStorageItem()
        {
            var c = new CloudBlobContainer(new Uri("http://myblob.com"));
            var d = new Azure(c, "happy-blob");
            Assert.IsNotNull(d as IStorageItem);
        }
    }
}