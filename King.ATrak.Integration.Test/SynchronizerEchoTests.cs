namespace King.ATrak.Integration.Test
{
    using King.ATrak.Models;
    using King.Azure.Data;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    [TestFixture]
    public class SynchronizerEchoTests
    {
        private readonly string ConnectionString = "UseDevelopmentStorage=true;";

        private class Validation
        {
            public string FileName;
            public byte[] Data;
        }

        [Test]
        public async Task BlobToFolder()
        {
            var random = new Random();
            var containerName = 'a' + Guid.NewGuid().ToString().Replace("-", "");
            var from = new Container(containerName, ConnectionString);
            await from.CreateIfNotExists();

            var to = string.Format("{0}\\{1}", Environment.CurrentDirectory, Guid.NewGuid());

            //Extra files for echo to clean-up
            Directory.CreateDirectory(to);
            var extraCount = random.Next(1, 25);
            var extra = new List<Validation>(extraCount);
            for (var i = 0; i < extraCount; i++)
            {
                var v = new Validation
                {
                    Data = Guid.NewGuid().ToByteArray(),
                    FileName = Guid.NewGuid().ToString(),
                };

                File.WriteAllBytes(string.Format("{0}\\{1}", to, v.FileName), v.Data);
                extra.Add(v);
            }

            var count = random.Next(1, 25);
            var toValidate = new List<Validation>(count);
            for (var i = 0; i < count; i++)
            {
                var v = new Validation
                {
                    Data = new byte[64],
                    FileName = string.Format("{0}.{1}", Guid.NewGuid(), i),
                };

                var bytes = new byte[64];
                random.NextBytes(v.Data);

                await from.Save(v.FileName, v.Data);

                toValidate.Add(v);
            }

            var config = new ConfigValues
            {
                Echo = true,
                Source = new DataSource
                {
                    ConnectionString = ConnectionString,
                    ContainerName = containerName,
                },
                Destination = new DataSource
                {
                    Folder = to,
                },
                Direction = Direction.BlobToFolder,
            };

            var s = new Synchronizer(config);
            await s.Run();

            foreach (var v in toValidate)
            {
                var data = File.ReadAllBytes(string.Format("{0}\\{1}", to, v.FileName));
                Assert.AreEqual(v.Data, data);
            }

            foreach (var v in extra)
            {
                var exists = File.Exists(string.Format("{0}\\{1}", to, v.FileName));
                Assert.IsFalse(exists);
            }

            Directory.Delete(to, true);

            await from.Delete();
        }

        [Test]
        public async Task FolderToBlob()
        {
            var random = new Random();

            var containerName = 'a' + Guid.NewGuid().ToString().Replace("-", "");
            var from = string.Format("{0}\\{1}", Environment.CurrentDirectory, Guid.NewGuid());
            Directory.CreateDirectory(from);

            //Extra files for echo to clean-up
            var to = new Container(containerName, ConnectionString);
            await to.CreateIfNotExists();
            var extraCount = random.Next(1, 25);
            var extra = new List<Validation>(extraCount);
            for (var i = 0; i < extraCount; i++)
            {
                var v = new Validation
                {
                    Data = Guid.NewGuid().ToByteArray(),
                    FileName = Guid.NewGuid().ToString(),
                };

                await to.Save(v.FileName, v.Data);
                extra.Add(v);
            }

            var count = random.Next(1, 25);
            var toValidate = new List<Validation>(count);
            for (var i = 0; i < count; i++)
            {
                var v = new Validation
                {
                    Data = new byte[64],
                    FileName = string.Format("{0}.{1}", Guid.NewGuid(), i),
                };

                var bytes = new byte[64];
                random.NextBytes(v.Data);

                File.WriteAllBytes(string.Format("{0}\\{1}", from, v.FileName), v.Data);

                toValidate.Add(v);
            }

            var config = new ConfigValues
            {
                Echo = true,
                Source = new DataSource
                {
                    Folder = from,
                },
                Destination = new DataSource
                {
                    ConnectionString = ConnectionString,
                    ContainerName = containerName,
                },
                Direction = Direction.FolderToBlob,
            };

            var s = new Synchronizer(config);
            await s.Run();

            foreach (var v in toValidate)
            {
                var data = await to.Get(v.FileName);
                Assert.AreEqual(v.Data, data);
            }

            foreach (var v in extra)
            {
                var exists = await to.Exists(v.FileName);
                Assert.IsFalse(exists);
            }

            await to.Delete();
            Directory.Delete(from, true);
        }

        [Test]
        public async Task FolderToFolder()
        {
            var random = new Random();
            var from = string.Format("{0}\\{1}", Environment.CurrentDirectory, Guid.NewGuid());
            Directory.CreateDirectory(from);
            var to = string.Format("{0}\\{1}", Environment.CurrentDirectory, Guid.NewGuid());

            //Extra files for echo to clean-up
            Directory.CreateDirectory(to);
            var extraCount = random.Next(1, 25);
            var extra = new List<Validation>(extraCount);
            for (var i = 0; i < extraCount; i++)
            {
                var v = new Validation
                {
                    Data = Guid.NewGuid().ToByteArray(),
                    FileName = Guid.NewGuid().ToString(),
                };

                File.WriteAllBytes(string.Format("{0}\\{1}", to, v.FileName), v.Data);
                extra.Add(v);
            }

            var count = random.Next(1, 25);
            var toValidate = new List<Validation>(count);
            for (var i = 0; i < count; i++)
            {
                var v = new Validation
                {
                    Data = new byte[64],
                    FileName = string.Format("{0}.{1}", Guid.NewGuid(), i),
                };

                var bytes = new byte[64];
                random.NextBytes(v.Data);

                File.WriteAllBytes(string.Format("{0}\\{1}", from, v.FileName), v.Data);

                toValidate.Add(v);
            }

            var config = new ConfigValues
            {
                Echo = true,
                Source = new DataSource
                {
                    Folder = from,
                },
                Destination = new DataSource
                {
                    Folder = to,
                },
                Direction = Direction.FolderToFolder,
            };

            var s = new Synchronizer(config);
            await s.Run();

            // Validate
            foreach (var v in toValidate)
            {
                var data = File.ReadAllBytes(string.Format("{0}\\{1}", to, v.FileName));
                Assert.AreEqual(v.Data, data);
            }

            foreach (var v in extra)
            {
                var exists = File.Exists(string.Format("{0}\\{1}", to, v.FileName));
                Assert.IsFalse(exists);
            }

            Directory.Delete(to, true);
            Directory.Delete(from, true);
        }

        [Test]
        public async Task BlobToBlob()
        {
            var random = new Random();
            var containerName = 'a' + Guid.NewGuid().ToString().Replace("-", "");
            var destContainerName = 'b' + Guid.NewGuid().ToString().Replace("-", "");
            var from = new Container(containerName, ConnectionString);
            await from.CreateIfNotExists();

            //Extra files for echo to clean-up
            var to = new Container(destContainerName, ConnectionString);
            await to.CreateIfNotExists();
            var extraCount = random.Next(1, 25);
            var extra = new List<Validation>(extraCount);
            for (var i = 0; i < extraCount; i++)
            {
                var v = new Validation
                {
                    Data = Guid.NewGuid().ToByteArray(),
                    FileName = Guid.NewGuid().ToString(),
                };

                await to.Save(v.FileName, v.Data);
                extra.Add(v);
            }

            var count = random.Next(1, 25);
            var toValidate = new List<Validation>(count);
            for (var i = 0; i < count; i++)
            {
                var v = new Validation
                {
                    Data = new byte[64],
                    FileName = string.Format("{0}.{1}", Guid.NewGuid(), i),
                };

                var bytes = new byte[64];
                random.NextBytes(v.Data);

                await from.Save(v.FileName, v.Data);

                toValidate.Add(v);
            }

            var config = new ConfigValues
            {
                Echo = true,
                Source = new DataSource
                {
                    ConnectionString = ConnectionString,
                    ContainerName = containerName,
                },
                Destination = new DataSource
                {
                    ConnectionString = ConnectionString,
                    ContainerName = destContainerName,
                },
                Direction = Direction.BlobToBlob,
            };

            var s = new Synchronizer(config);
            await s.Run();

            //Validate
            foreach (var v in toValidate)
            {
                var data = await to.Get(v.FileName);
                Assert.AreEqual(v.Data, data);
            }

            foreach (var v in extra)
            {
                var exists = await to.Exists(v.FileName);
                Assert.IsFalse(exists);
            }

            //Clean-Up
            await from.Delete();
            await to.Delete();
        }
    }
}