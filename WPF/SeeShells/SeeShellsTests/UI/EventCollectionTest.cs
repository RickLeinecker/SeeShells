using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeeShells;
using SeeShells.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShellsTests.UI
{
    [TestClass()]
    class EventCollectionTest
    {
        [TestMethod()]
        public void EventCollectionTests()
        {
            List<IEvent> eventList = new List<IEvent>();
            Event aEvent = new Event("item1", DateTime.Now, null, "Access");
            eventList.Add(aEvent);
            App.eventCollection.eventList = eventList;
            Assert.AreNotEqual(App.eventCollection.eventList.Count, 0);

        }
    }
}
