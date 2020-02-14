using SeeShells.UI;
using SeeShellsTests.UI.Mocks;
using SeeShells.ShellParser;
using SeeShells.ShellParser.ShellItems;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            shellItemProperties.Add("ModifiedDate", "1/1/0001 12:00:00 AM");
            shellItemProperties.Add("AccessedDate", "1/1/0001 12:00:00 AM");
            shellItemProperties.Add("CreationDate", "1/1/0001 12:00:00 AM");
            shellItemProperties.Add("LastAccessedDate", "1/1/001 12:00:00 AM");
            CsvParsedShellItem ShellItem = new CsvParsedShellItem(shellItemProperties);
            shellItems.Add(ShellItem);
            EventParser eventParser = new EventParser(shellItems);
            List<IEvent> newList = eventParser.Parser();
            Assert.IsNotNull(newList);
            foreach(var el in newList)
            {
                Assert.AreSame(el.Parent, ShellItem);
            }

            
        }
    }
}
