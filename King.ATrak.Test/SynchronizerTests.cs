namespace King.ATrak.Test
{
    using King.ATrak.Models;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class SynchronizerTests
    {
        [Test]
        public void ConstructorFolderToBlob()
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
        public void ConstructorFolderToFolder()
        {
            var config = new ConfigValues
            {
                ConnectionString = "UseDevelopmentStorage=true;",
                ContainerName = "test",
                Folder = "C:\\happy",
                SyncDirection = Direction.FolderToFolder,
            };

            new Synchronizer(config);
        }

        [Test]
        public void ConstructorBlobToBlob()
        {
            var config = new ConfigValues
            {
                ConnectionString = "UseDevelopmentStorage=true;",
                ContainerName = "test",
                Folder = "C:\\happy",
                SyncDirection = Direction.BlobToBlob,
            };

            new Synchronizer(config);
        }

        [Test]
        public void ConstructorBlobToFolder()
        {
            var config = new ConfigValues
            {
                ConnectionString = "UseDevelopmentStorage=true;",
                ContainerName = "test",
                Folder = "C:\\happy",
                SyncDirection = Direction.BlobToFolder,
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

        [Test]
        public void IsISynchronizer()
        {
            var config = new ConfigValues
            {
                ConnectionString = "UseDevelopmentStorage=true;",
                ContainerName = "test",
                Folder = "C:\\happy",
                SyncDirection = Direction.FolderToBlob,
            };

            Assert.IsNotNull(new Synchronizer(config) as ISynchronizer);
        }
    }
}