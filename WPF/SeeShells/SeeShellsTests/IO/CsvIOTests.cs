using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeeShells.IO;
using SeeShells.ShellParser;
using SeeShells.ShellParser.ShellItems;
using SeeShellsTests.ShellParser.ShellParserMocks;
using System;
using System.Collections.Generic;

namespace SeeShellsTests.IO
{
    [TestClass]
    public class CsvIOTests
    {
        /// <summary>
        /// Tests if a CSV file is otputted. The live registry is used for this test.
        /// </summary>
        [TestMethod()]
        public void OutputCSVFileTest()
        {
            ShellBagParser shellBagParser = new ShellBagParser(new OnlineRegistryReader(new MockConfigParser()));
            List<IShellItem> shellItems = shellBagParser.GetShellItems();

            CsvIO.OutputCSVFile(shellItems, "raw.csv");
        }

        /// <summary>
        /// Tests if a CSV file is imported, parsed and converted to a list of ShellItems.
        /// </summary>
        [TestMethod()]
        public void ImportCSVFileTest()
        {

        }
    }
}
