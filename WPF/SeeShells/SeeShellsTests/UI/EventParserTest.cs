using SeeShells.UI;
using SeeShells.ShellParser.ShellItems;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using SeeShells.IO;

namespace SeeShellsTests.UI
{
    [TestClass]
    class EventParserTest
    {
        [TestMethod]
        public void EventListOutput()
        {
            List<IShellItem> shellItems = new List<IShellItem>();
            Dictionary<string, string> shellItemProperties = new Dictionary<string, string>();
            shellItemProperties.Add("Size", "0");
            shellItemProperties.Add("Type", "31");
            shellItemProperties.Add("TypeName", "Some Type Name");
            shellItemProperties.Add("Name", "Some Name");
            shellItemProperties.Add("ModifiedDate", "1/1/2010 12:00:00 AM");
            shellItemProperties.Add("AccessedDate", "2/1/2000 12:00:00 AM");
            shellItemProperties.Add("CreationDate", "1/1/2019 12:00:00 AM");
            shellItemProperties.Add("LastAccessedDate", "1/1/2016 12:00:00 AM");
            CsvParsedShellItem ShellItem = new CsvParsedShellItem(shellItemProperties);
            shellItems.Add(ShellItem);
            List<IEvent> newList = EventParser.Parser(shellItems);
            Assert.IsNotNull(newList);
            foreach(var el in newList)
            {
                Assert.AreSame(el.Parent, ShellItem);
            }

            
        }
    }
}
