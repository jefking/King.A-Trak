namespace King.ATrak.Test
{
    using Abc.ATrak;
    using Amazon.S3;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NSubstitute;

    [TestClass]
    public class S3Tests
    {
        [TestMethod]
        public void Constructor()
        {
            var client = Substitute.For<IAmazonS3>();
            new S3(client, "bucket", "path");
        }

        [TestMethod]
        public void IsIStorageItem()
        {
            var client = Substitute.For<IAmazonS3>();
            var s = new S3(client, "bucket", "path");
            Assert.IsNotNull(s as IStorageItem);
        }
    }
}