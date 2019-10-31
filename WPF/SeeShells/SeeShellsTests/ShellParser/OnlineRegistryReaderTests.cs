using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeeShells.ShellParser;
using SeeShellsTests.ShellParser.ShellParserMocks;
using System.Collections.Generic;

namespace SeeShellsTests.ShellParser
{
    [TestClass()]
    public class OnlineRegistryReaderTests
    {
        /// <summary>
        /// Tests that the Online Registry Reader can read from a running system's registry.
        /// </summary>
        [TestMethod()]
        public void GetRegistryKeysTest()
        {
            OnlineRegistryReader registryReader = new OnlineRegistryReader(new MockConfigParser());
            List<RegistryKeyWrapper> keys = registryReader.GetRegistryKeys();

            Assert.AreNotEqual(keys.Count, 0);
        }

    }
}
