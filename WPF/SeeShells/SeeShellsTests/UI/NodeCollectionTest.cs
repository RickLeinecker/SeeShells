using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeeShells;
using SeeShells.UI;
using SeeShells.UI.Node;
using System;
using System.Collections.Generic;

namespace SeeShellsTests.UI
{
    [TestClass()]
    public class NodeCollectionTest
    {
        [TestMethod()]
        public void NodeCollectionTests()
        {
            List<IEvent> eventList = new List<IEvent>();
            Event aEvent = new Event("item1", DateTime.Now, null, "Access");
            eventList.Add(aEvent);
            App.nodeCollection.nodeList = NodeParser.GetNodes(eventList); //TODO: Will be changed when the Node constructor adds Dot and Block
            Assert.AreNotEqual(App.nodeCollection.nodeList.Count, 0);
        }
    }
}
