namespace King.ATrak.Test
{
    using King.ATrak.Models;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class SynchronizerTests
    {
        [Test]
        public void Constructor()
        {
            var config = new ConfigValues
            {
                ConnectionString = "UseDevelopmentStorage=true;",
                ContainerName = "test",
                Folder = "C:\\happy",
                SyncDirection = Direction.FolderToBlob,
            };

            new Synchronizer(config);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorConfigNull()
        {
            new Synchronizer(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorDirectionUnknown()
        {
            var config = new ConfigValues
            {
                ConnectionString = "UseDevelopmentStorage=true;",
                ContainerName = "test",
                Folder = "C:\\happy",
                SyncDirection = Direction.Unknown,
            };

            new Synchronizer(config);
        }
    }
}