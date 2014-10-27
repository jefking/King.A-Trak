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
                Source = new DataSource
                {
                    Folder = "C:\\testing",
                },
                Destination = new DataSource
                {
                    ConnectionString = "UseDevelopmentStorage=true;",
                    ContainerName = "test",
                },
                Direction = Direction.FolderToBlob,
            };

            new Synchronizer(config);
        }

        [Test]
        public void ConstructorFolderToFolder()
        {
            var config = new ConfigValues
            {
                Source = new DataSource
                {
                    Folder = "C:\\testing",
                },
                Destination = new DataSource
                {
                    Folder = "C:\\copy",
                },
                Direction = Direction.FolderToFolder,
            };

            new Synchronizer(config);
        }

        [Test]
        public void ConstructorBlobToBlob()
        {
            var config = new ConfigValues
            {
                Source = new DataSource
                {
                    ConnectionString = "UseDevelopmentStorage=true;",
                    ContainerName = "copy",
                },
                Destination = new DataSource
                {
                    ConnectionString = "UseDevelopmentStorage=true;",
                    ContainerName = "test",
                },
                Direction = Direction.BlobToBlob,
            };

            new Synchronizer(config);
        }

        [Test]
        public void ConstructorBlobToFolder()
        {
            var config = new ConfigValues
            {
                Source = new DataSource
                {
                    ConnectionString = "UseDevelopmentStorage=true;",
                    ContainerName = "test",
                },
                Destination = new DataSource
                {
                    Folder = "C:\\testing",
                },
                Direction = Direction.BlobToFolder,
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
                Source = new DataSource(),
                Destination = new DataSource(),
                Direction = Direction.Unknown,
            };

            new Synchronizer(config);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorSourceNull()
        {
            var config = new ConfigValues
            {
                Source = null,
                Destination = new DataSource(),
                Direction = Direction.FolderToBlob,
            };

            new Synchronizer(config);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorDestinationNull()
        {
            var config = new ConfigValues
            {
                Source = new DataSource(),
                Destination = null,
                Direction = Direction.BlobToBlob,
            };

            new Synchronizer(config);
        }

        [Test]
        public void IsISynchronizer()
        {
            var config = new ConfigValues
            {
                Source = new DataSource
                {
                    Folder = "C:\\From",
                },
                Destination = new DataSource
                {
                    Folder = "C:\\To",
                },
                Direction = Direction.FolderToFolder,
            };

            Assert.IsNotNull(new Synchronizer(config) as ISynchronizer);
        }
    }
}