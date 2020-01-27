using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeeShells;
using SeeShells.ShellParser.ShellItems;
using SeeShells.UI;
using SeeShellsTests.UI.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShellsTests.UI
{
    [TestClass()]
    public class EventCollectionTests
    {
        /// <summary>
        /// Test that the list of IEvents created in the EventCollection class can be accessed and modified from any class.  
        /// </summary>
        [TestMethod()]
        public void EventCollectionTest()
        {
            List<IEvent> eventList = new List<IEvent>();
            Event aEvent = new Event("item1", DateTime.Now, null, "Access");
            eventList.Add(aEvent);
            App.eventCollection.eventList = eventList;
            Assert.AreNotEqual(App.eventCollection.eventList.Count, 0);
        }

    }
}
