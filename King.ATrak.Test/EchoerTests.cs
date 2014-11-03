namespace King.ATrak.Test
{
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

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
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorDestinationNull()
        {
            new Echoer(null);
        }

        [Test]
        public void IsIEchoer()
        {
            var destination = Substitute.For<IDataLister>();
            Assert.IsNotNull(new Echoer(destination) as IEchoer);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CleanDestinationSourceItemsNull()
        {
            var destination = Substitute.For<IDataLister>();
            var e = new Echoer(destination);
            e.CleanDestination(null);
        }

        [Test]
        public void CleanDestinationSourceItemsEmpty()
        {
            var destination = Substitute.For<IDataLister>();
            var e = new Echoer(destination);
            e.CleanDestination(new List<IStorageItem>());
        }

        [Test]
        public void CleanDestinationDestinationItems()
        {
            var sItems = new List<IStorageItem>();
            sItems.Add(Substitute.For<IStorageItem>());
            var destination = Substitute.For<IDataLister>();
            destination.List().Returns((IEnumerable<IStorageItem>)null);

            var e = new Echoer(destination);
            e.CleanDestination(sItems);
        }

        [Test]
        public void CleanDestinationDestinationItemsEmpty()
        {
            var sItems = new List<IStorageItem>();
            sItems.Add(Substitute.For<IStorageItem>());
            var dItems = new List<IStorageItem>();
            var destination = Substitute.For<IDataLister>();
            destination.List().Returns(dItems);

            var e = new Echoer(destination);
            e.CleanDestination(sItems);
        }

        [Test]
        public void CleanDestinationNothingToDelete()
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
            e.CleanDestination(sItems);
        }
    }
}