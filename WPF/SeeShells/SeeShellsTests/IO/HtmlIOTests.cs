using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeeShells.IO;
using SeeShells.ShellParser;
using SeeShells.ShellParser.ShellItems;
using SeeShells.UI;
using SeeShellsTests.ShellParser.ShellParserMocks;
using System;
using System.Collections.Generic;
using System.IO;

namespace SeeShellsTests.IO
{
    [TestClass]
    public class HtmlIOTests
    {
        /// <summary>
        /// Tests if a CSV file is otputted.
        /// </summary>
        [TestMethod()]
        public void OutputHtmlFileTest()
        {
            List<IEvent> eventList = new List<IEvent>();

            Dictionary<string, string> shellItemProperties = new Dictionary<string, string>();
            shellItemProperties.Add("Size", "0");
            shellItemProperties.Add("Type", "31");
            shellItemProperties.Add("TypeName", "Some Type Name");
            shellItemProperties.Add("Name", "Some Name");
            shellItemProperties.Add("ModifiedDate", "1/1/0001 12:00:00 AM");
            shellItemProperties.Add("AccessedDate", "1/1/0001 12:00:00 AM");
            shellItemProperties.Add("CreationDate", "1/1/0001 12:00:00 AM");
            CsvParsedShellItem ShellItem = new CsvParsedShellItem(shellItemProperties);

            Event aEvent = new Event("item1", DateTime.Now, ShellItem, "Access");
            eventList.Add(aEvent);

            if (File.Exists("timeline.html"))
            {
                File.Delete("timeline.html");
            }
            HtmlIO.OutputHtmlFile(eventList, "timeline.html");
            Assert.IsTrue(File.Exists("timeline.html"));
        }
    }
}