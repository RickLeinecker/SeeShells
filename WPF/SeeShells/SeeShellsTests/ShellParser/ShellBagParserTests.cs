﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeeShells.ShellParser;
using SeeShellsTests.ShellParser.ShellParserMocks;
using SeeShells.ShellParser.ShellItems;
using System.Collections.Generic;
using System.IO;
using System;

namespace SeeShellsTests.ShellParser
{
    [TestClass()]
    public class ShellBagParserTests
    {
        /// <summary>
        /// Tests if shell itmes can be obtained from a live registry.
        /// </summary>
        [TestMethod()]
        [TestCategory("OnlineTest")]
        public void GetShellItemsOnlineTest()
        {
            ShellBagParser shellBagParser = new ShellBagParser(new OnlineRegistryReader(new MockConfigParser()));
            List<IShellItem> shellItems = shellBagParser.GetShellItems();

            Assert.AreNotEqual(shellItems.Count, 0);
        }

        /// <summary>
        /// Tests if shell itmes can be obtained from an offline hive.
        /// </summary>
        [TestMethod()]
        public void GetShellItemsOfflineTest()
        {
            String registryFilePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\TestResource\NTUSER.DAT";
            ShellBagParser shellBagParser = new ShellBagParser(new OfflineRegistryReader(new OfflineMockConfigParser(), registryFilePath));
            List<IShellItem> shellItems = shellBagParser.GetShellItems();

            Assert.AreNotEqual(shellItems.Count, 0);
        }
    }
}