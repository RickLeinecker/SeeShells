using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeeShells.ShellParser;
using SeeShellsTests.ShellParser.ShellParserMocks;
using System;
using System.Collections.Generic;

namespace SeeShellsTests.ShellParser
{
    [TestClass]
    public class OfflineRegistryReaderTests
    {
        /// <summary>
        /// Tests that the Offline Registry Reader can read from a hive file.
        /// </summary>
        [TestMethod]
        public void GetRegistryKeysTest()
        {
            OfflineRegistryReader registryReader = new OfflineRegistryReader(new OfflineMockConfigParser());
            List<RegistryKeyWrapper> keys = registryReader.GetRegistryKeys();
            Assert.AreNotEqual(keys.Count, 0);
        }
    }
}
