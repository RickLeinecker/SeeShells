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
        /// Tests that the Offline Registry Reader can read from a NTUSER hive file.
        /// </summary>
        [TestMethod]
        public void GetRegistryKeys_NTUSERTest()
        {
            String registryFilePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\TestResource\NTUSER.DAT";
            OfflineRegistryReader registryReader = new OfflineRegistryReader(new OfflineMockConfigParser(), registryFilePath);
            List<RegistryKeyWrapper> keys = registryReader.GetRegistryKeys();
            Assert.AreNotEqual(keys.Count, 0);
        }
        /// <summary>
        /// Tests that the Offline Registry Reader can read from a USRCLASS hive file.
        /// </summary>
        [TestMethod]
        public void GetRegistryKeys_USRCLASSTest()
        {
            String registryFilePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\TestResource\UsrClass.dat";
            OfflineRegistryReader registryReader = new OfflineRegistryReader(new OfflineMockConfigParser(), registryFilePath);
            List<RegistryKeyWrapper> keys = registryReader.GetRegistryKeys();
            Assert.AreNotEqual(keys.Count, 0);
        }
    }
}
