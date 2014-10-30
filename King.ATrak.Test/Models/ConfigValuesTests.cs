namespace King.ATrak.Test.Models
{
    using King.ATrak.Models;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class ConfigValuesTests
    {
        [Test]
        public void Constructor()
        {
            new ConfigValues();
        }

        [Test]
        public void IsIConfigValues()
        {
            Assert.IsNotNull(new ConfigValues() as IConfigValues);
        }

        [Test]
        public void Echo()
        {
            var expected = true;
            var c = new ConfigValues
            {
                Echo = expected,
            };

            Assert.AreEqual(expected, c.Echo);
        }

        [Test]
        public void CreateSnapshot()
        {
            var expected = true;
            var c = new ConfigValues
            {
                CreateSnapshot = expected,
            };

            Assert.AreEqual(expected, c.CreateSnapshot);
        }

        [Test]
        public void CacheControl()
        {
            var random = new Random();
            var expected = random.Next();
            var c = new ConfigValues
            {
                CacheControlDuration = expected,
            };

            Assert.AreEqual(expected, c.CacheControlDuration);
        }

        [Test]
        public void Destination()
        {
            var expected = new DataSource();
            var c = new ConfigValues
            {
                Destination = expected,
            };

            Assert.AreEqual(expected, c.Destination);
        }

        [Test]
        public void Source()
        {
            var expected = new DataSource();
            var c = new ConfigValues
            {
                Source = expected,
            };

            Assert.AreEqual(expected, c.Source);
        }

        [Test]
        public void SyncDirection()
        {
            var expected = Direction.FolderToBlob;
            var c = new ConfigValues
            {
                Direction = expected,
            };

            Assert.AreEqual(expected, c.Direction);
        }
    }
}