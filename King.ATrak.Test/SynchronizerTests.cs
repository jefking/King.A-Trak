namespace King.ATrak.Test
{
    using King.ATrak.Models;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

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
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task SyncUnknownDirection()
        {
            var config = new ConfigValues
            {
                ConnectionString = "UseDevelopmentStorage=true;",
                ContainerName = "test",
                Folder = "C:\\happy",
                SyncDirection = Direction.FolderToBlob,
            };

            var s = new Synchronizer(config);
            await s.Run(Direction.Unknown);
        }
    }
}