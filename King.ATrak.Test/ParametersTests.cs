namespace King.ATrak.Test
{
    using King.ATrak;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class ParametersTests
    {
        [TestMethod]
        public void Constructor()
        {
            new Parameters(new string[] { Guid.NewGuid().ToString(), Guid.NewGuid().ToString() });
        }

        [TestMethod]
        public void ConstructorParametersNull()
        {
            new Parameters(null);
        }
    }
}