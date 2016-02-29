namespace King.ATrak.Test
{
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [TestFixture]
    public class EchoerTests
    {
        [Test]
        public void Constructor()
        {
            var destination = Substitute.For<IDataLister>();
            new Echoer(destination);
        }

        [Test]
        public void ConstructorDestinationNull()
        {
            Assert.That(() => new Echoer(null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void IsIEchoer()
        {
            var destination = Substitute.For<IDataLister>();
            Assert.IsNotNull(new Echoer(destination) as IEchoer);
        }

        [Test]
        public async Task CleanDestinationSourceItemsNull()
        {
            var destination = Substitute.For<IDataLister>();
            var e = new Echoer(destination);
            Assert.That(async () => await e.CleanDestination(null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public async Task CleanDestinationSourceItemsEmpty()
        {
            var destination = Substitute.For<IDataLister>();
            destination.List().Returns(new List<IStorageItem>());

            var e = new Echoer(destination);
            await e.CleanDestination(new List<IStorageItem>());
        }

        [Test]
        public async Task CleanDestinationDestinationItems()
        {
            var sItems = new List<IStorageItem>();
            sItems.Add(Substitute.For<IStorageItem>());
            var destination = Substitute.For<IDataLister>();
            destination.List().Returns((IEnumerable<IStorageItem>)null);

            var e = new Echoer(destination);
            await e.CleanDestination(sItems);

            destination.Received().List();
        }

        [Test]
        public async Task CleanDestinationDestinationItemsEmpty()
        {
            var sItems = new List<IStorageItem>();
            sItems.Add(Substitute.For<IStorageItem>());
            var dItems = new List<IStorageItem>();
            var destination = Substitute.For<IDataLister>();
            destination.List().Returns(dItems);

            var e = new Echoer(destination);
            await e.CleanDestination(sItems);

            destination.Received().List();
        }

        [Test]
        public async Task CleanDestinationNothingToDelete()
        {
            var random = new Random();
            var count = random.Next(1, 64);

            var sItems = new List<IStorageItem>(count);
            var dItems = new List<IStorageItem>(count);

            for (var i = 0; i < count; i++)
            {
                var item = Substitute.For<IStorageItem>();
                item.RelativePath.Returns(Guid.NewGuid().ToString());

                sItems.Add(item);
                dItems.Add(item);
            }

            var destination = Substitute.For<IDataLister>();
            destination.List().Returns(dItems);

            var e = new Echoer(destination);
            await e.CleanDestination(sItems);

            destination.Received().List();

            foreach (var di in dItems)
            {
                di.Received(0).Delete();
            }
        }

        [Test]
        public async Task CleanDestinationDeleteAll()
        {
            var random = new Random();
            var count = random.Next(1, 64);

            var dItems = new List<IStorageItem>(count);

            for (var i = 0; i < count; i++)
            {
                var item = Substitute.For<IStorageItem>();
                item.RelativePath.Returns(Guid.NewGuid().ToString());

                dItems.Add(item);
            }

            var destination = Substitute.For<IDataLister>();
            destination.List().Returns(dItems);

            var e = new Echoer(destination);
            await e.CleanDestination(new List<IStorageItem>(count));

            destination.Received().List();

            foreach (var di in dItems)
            {
                di.Received().Delete();
            }
        }

        [Test]
        public async Task CleanDestinationNoDestinationDelete()
        {
            var random = new Random();
            var count = random.Next(1, 64);

            var sItems = new List<IStorageItem>(count);

            for (var i = 0; i < count; i++)
            {
                var item = Substitute.For<IStorageItem>();
                item.RelativePath.Returns(Guid.NewGuid().ToString());

                sItems.Add(item);
            }

            var destination = Substitute.For<IDataLister>();
            destination.List().Returns(new List<IStorageItem>());

            var e = new Echoer(destination);
            await e.CleanDestination(sItems);

            destination.Received().List();

            foreach (var si in sItems)
            {
                si.Received(0).Delete();
            }
        }

        [Test]
        public async Task CleanDestinationMoreInSource()
        {
            var random = new Random();
            var count = random.Next(1, 64);

            var toDelete = new List<IStorageItem>(count);
            var sItems = new List<IStorageItem>(count);
            var dItems = new List<IStorageItem>(count);

            for (var i = 0; i < count; i++)
            {
                var item = Substitute.For<IStorageItem>();
                item.RelativePath.Returns(Guid.NewGuid().ToString());

                dItems.Add(item);
                toDelete.Add(item);
            }
            for (var i = 0; i < count; i++)
            {
                var item = Substitute.For<IStorageItem>();
                item.RelativePath.Returns(Guid.NewGuid().ToString());

                sItems.Add(item);
            }

            var destination = Substitute.For<IDataLister>();
            destination.List().Returns(dItems);

            var e = new Echoer(destination);
            await e.CleanDestination(sItems);

            destination.Received().List();

            foreach (var i in toDelete)
            {
                i.Received().Delete();
            }
        }
    }
}