using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeeShells.ShellParser;
using SeeShellsTests.ShellParser.ShellParserMocks;
using System;
using System.Collections.Generic;
using System.IO;
using SeeShells.ShellParser.Registry;

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
            String registryFilePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\TestResource\NTUSER.DAT";
            OfflineRegistryReader registryReader = new OfflineRegistryReader(new OfflineMockConfigParser(), registryFilePath);
            List<RegistryKeyWrapper> keys = registryReader.GetRegistryKeys();
            Assert.AreNotEqual(keys.Count, 0);
        }
    }
}
