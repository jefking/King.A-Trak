namespace King.ATrak.Test.Models
{
    using King.ATrak.Models;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class DataSourceTests
    {
        [Test]
        public void Constructor()
        {
            new DataSource();
        }

        [Test]
        public void IsIDataSource()
        {
            Assert.IsNotNull(new DataSource() as IDataSource);
        }

        [Test]
        public void Folder()
        {
            var expected = Guid.NewGuid().ToString();
            var c = new DataSource
            {
                Folder = expected,
            };

            Assert.AreEqual(expected, c.Folder);
        }

        [Test]
        public void ContainerName()
        {
            var expected = Guid.NewGuid().ToString();
            var c = new DataSource
            {
                ContainerName = expected,
            };

            Assert.AreEqual(expected, c.ContainerName);
        }

        [Test]
        public void ConnectionString()
        {
            var expected = Guid.NewGuid().ToString();
            var c = new DataSource
            {
                ConnectionString = expected,
            };

            Assert.AreEqual(expected, c.ConnectionString);
        }
    }
}