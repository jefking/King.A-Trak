namespace King.ATrak.Test.Azure
{
    using King.ATrak.Azure;
    using King.Azure.Data;
    using NSubstitute;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class BlobItemTests
    {
        [Test]
        public void Constructor()
        {
            var container = Substitute.For<IContainer>();
            new BlobItem(container, new Uri("http://google.com"));
        }
    }
}