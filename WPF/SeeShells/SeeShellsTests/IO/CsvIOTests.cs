using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeeShells.IO;
using SeeShells.ShellParser;
using SeeShells.ShellParser.ShellItems;
using SeeShellsTests.ShellParser.ShellParserMocks;
using System;
using System.Collections.Generic;
using System.IO;

namespace SeeShellsTests.IO
{
    [TestClass]
    public class CsvIOTests
    {
        /// <summary>
        /// Tests if a CSV file is otputted.
        /// </summary>
        [TestMethod()]
        public void OutputCSVFileTest()
        {
            List<IShellItem> shellItems = new List<IShellItem>();
            Dictionary<string, string> shellItemProperties = new Dictionary<string, string>();
            shellItemProperties.Add("Size", "0");
            shellItemProperties.Add("Type", "31");
            shellItemProperties.Add("TypeName", "Some Type Name");
            shellItemProperties.Add("Name", "Some Name");
            shellItemProperties.Add("ModifiedDate", "1/1/0001 12:00:00 AM");
            shellItemProperties.Add("AccessedDate", "1/1/0001 12:00:00 AM");
            shellItemProperties.Add("CreationDate", "1/1/0001 12:00:00 AM");
            CsvParsedShellItem ShellItem = new CsvParsedShellItem(shellItemProperties);
            shellItems.Add(ShellItem);

            if (File.Exists("raw.csv"))
            {
                File.Delete("raw.csv");
            }
            CsvIO.OutputCSVFile(shellItems, "raw.csv");
            Assert.IsTrue(File.Exists("raw.csv"));
        }

        /// <summary>
        /// Tests if a CSV file is imported, parsed and converted to a list of ShellItems.
        /// </summary>
        [TestMethod()]
        public void ImportCSVFileTest()
        {
            List<IShellItem> shellItems = CsvIO.ImportCSVFile(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\TestResource\raw.csv");
            Assert.AreNotEqual(shellItems.Count, 0);
        }
    }
}