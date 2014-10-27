namespace King.ATrak.Integration.Test
{
    using King.ATrak.Models;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [TestFixture]
    public class SynchronizerTests
    {
        [Test]
        public async Task BlobToFolder()
        {
            var config = new ConfigValues
            {
            };

            var s = new Synchronizer(config);
            await s.Run(Direction.BlobToFolder);

            Assert.Inconclusive();
        }

        [Test]
        public async Task FolderToBlob()
        {
            var config = new ConfigValues
            {
            };

            var s = new Synchronizer(config);
            await s.Run(Direction.BlobToFolder);

            Assert.Inconclusive();
        }
    }
}