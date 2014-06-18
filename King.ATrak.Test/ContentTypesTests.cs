namespace King.ATrak.Test
{
    using Abc.ATrak;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class ContentTypesTests
    {

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ContentTypeFilePathInvalid()
        {
            ContentTypes.ContentType(null);
        }
    }
}