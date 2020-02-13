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
        /// <summary>
        /// Tests that a list of Nodes is created and saved in the NodeCollection object in App.xml.cs
        /// </summary>
        [TestMethod()]
        public void NodeCollectionTests()
        {
            List<IEvent> eventList = new List<IEvent>();
            Event aEvent = new Event("item1", DateTime.Now, null, "Access");
            eventList.Add(aEvent);
            App.nodeCollection.nodeList = NodeParser.GetNodes(eventList);
            Assert.AreNotEqual(App.nodeCollection.nodeList.Count, 0);
        }
    }
}
