using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeeShells.UI;
using SeeShells.UI.EventFilters;
using SeeShells.UI.Node;
using SeeShellsTests.UI.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShellsTests.UI.Tests
{
    [TestClass()]
    public class FilterEventUtilsTests
    {
        [TestMethod()]
        public void FilterDateRangeTest()
        {
            var eventList = new List<MockEvent>()
            {
                new MockEvent("item1", DateTime.Now, null, "Access"),
                new MockEvent("item2", DateTime.MaxValue, null, "Access"),
                new MockEvent("item3", DateTime.MinValue, null, "Access"),
            };

            var testList = new List<Node>()
            {
                new MockNode(eventList.ElementAt(0)),
                new MockNode(eventList.ElementAt(1)),
                new MockNode(eventList.ElementAt(2))
            };

            List<Node> testListCopy; //reset this list each test because we are removing references from this list.


            DateTime startLimit = new DateTime(DateTime.MinValue.Ticks + 10000000000000, DateTimeKind.Utc);
            DateTime endLimit = new DateTime(DateTime.MaxValue.Ticks - 10000, DateTimeKind.Utc);

            //test set startdate and enddate
            testListCopy = new List<Node>(testList);
            new DateRangeFilter(startLimit, endLimit).Apply(ref testListCopy);
            Assert.AreEqual(1, testListCopy.Count());

            //test the startdate limit
            testListCopy = new List<Node>(testList);
            new DateRangeFilter(startLimit, null).Apply(ref testListCopy);
            Assert.AreEqual(2, testListCopy.Count());

            //test enddate limit
            testListCopy = new List<Node>(testList);
            new DateRangeFilter(null, endLimit).Apply(ref testListCopy);
            Assert.AreEqual(2, testListCopy.Count());

            //test full range (aka pointless filter)
            testListCopy = new List<Node>(testList);
            new DateRangeFilter(null, null).Apply(ref testListCopy);
            Assert.AreEqual(3, testListCopy.Count());
        }

        [TestMethod()]
        public void FilterEventTypeTest()
        {
            var eventList = new List<MockEvent>()
            {
                new MockEvent("item1", DateTime.Now, null, "Access"),
                new MockEvent("item2", DateTime.MaxValue, null, "Create"),
                new MockEvent("item3", DateTime.MinValue, null, "Modified"),
            };

            var testList = new List<Node>()
            {
                new MockNode(eventList.ElementAt(0)),
                new MockNode(eventList.ElementAt(1)),
                new MockNode(eventList.ElementAt(2))
            };

            List<Node> testListCopy; //reset this list each test because we are removing references from this list.


            //basic filter
            testListCopy = new List<Node>(testList);
            new EventTypeFilter("Access").Apply(ref testListCopy);
            Assert.AreEqual(1, testListCopy.Count());

            //test multifilter
            testListCopy = new List<Node>(testList);
            new EventTypeFilter("Access", "Create").Apply(ref testListCopy);
            Assert.AreEqual(2, testListCopy.Count());

            //test no filter
            testListCopy = new List<Node>(testList);
            new EventTypeFilter("").Apply(ref testListCopy);
            Assert.AreEqual(0, testListCopy.Count());
        }

        [TestMethod()]
        public void FilterParentTest()
        {
            var parent1 = new MockShellItem();
            var parent2 = new MockShellItem();
            var eventList = new List<MockEvent>()
            {
                new MockEvent("item1", DateTime.Now, parent1, "Access"),
                new MockEvent("item2", DateTime.MaxValue, parent1, "Create"),
                new MockEvent("item3", DateTime.MinValue, parent2, "Modified"),
            };

            var testList = new List<Node>()
            {
                new MockNode(eventList.ElementAt(0)),
                new MockNode(eventList.ElementAt(1)),
                new MockNode(eventList.ElementAt(2))
            };

            List<Node> testListCopy; //reset this list each test because we are removing references from this list.


            //check same parent
            testListCopy = new List<Node>(testList);
            new EventParentFilter(parent1).Apply(ref testListCopy);
            Assert.AreEqual(2, testListCopy.Count());

            //check diff parent
            testListCopy = new List<Node>(testList);
            new EventParentFilter(parent2).Apply(ref testListCopy);
            Assert.AreEqual(1, testListCopy.Count());

            //check unknown parent
            testListCopy = new List<Node>(testList);
            new EventParentFilter(new MockShellItem()).Apply(ref testListCopy);
            Assert.AreEqual(0, testListCopy.Count());
        }

        [TestMethod()]
        public void FilterNameTest()
        {
            var eventList = new List<MockEvent>()
            {
                new MockEvent("item1", DateTime.Now, null, "Access"),
                new MockEvent("item1", DateTime.MaxValue, null, "Create"),
                new MockEvent("item3", DateTime.MinValue, null, "Modified"),
            };

            var testList = new List<Node>()
            {
                new MockNode(eventList.ElementAt(0)),
                new MockNode(eventList.ElementAt(1)),
                new MockNode(eventList.ElementAt(2))
            };

            List<Node> testListCopy; //reset this list each test because we are removing references from this list.


            //check same name
            testListCopy = new List<Node>(testList);
            new EventNameFilter("item1").Apply(ref testListCopy);
            Assert.AreEqual(2, testListCopy.Count());

            //check no name
            testListCopy = new List<Node>(testList);
            new EventNameFilter("item2").Apply(ref testListCopy);
            Assert.AreEqual(0, testListCopy.Count());

            //check multi names
            testListCopy = new List<Node>(testList);
            new EventNameFilter("item1", "item3").Apply(ref testListCopy);
            Assert.AreEqual(3, testListCopy.Count());

        }

        [TestMethod()]
        public void FilterAnyStringTest()
        {
            var parent1 = new MockShellItem("item1", 0x02);
            var parent2 = new MockShellItem("item3", 0x02);

            var eventList = new List<MockEvent>()
            {
                new MockEvent(parent1.Name, DateTime.Now, parent1, "Access"),
                new MockEvent(parent1.Name, DateTime.MaxValue, parent1, "Create"),
                new MockEvent(parent2.Name, DateTime.MinValue, parent2, "Modified"),
            };

            var testList = new List<Node>()
            {
                new MockNode(eventList.ElementAt(0)),
                new MockNode(eventList.ElementAt(1)),
                new MockNode(eventList.ElementAt(2))
            };

            List<Node> testListCopy; //reset this list each test because we are removing references from this list.



            //check for unique attribute to some items
            testListCopy = new List<Node>(testList);
            new AnyStringFilter("item1",false).Apply(ref testListCopy);
            Assert.AreEqual(2, testListCopy.Count());

            //check for partial match to all items
            testListCopy = new List<Node>(testList);
            new AnyStringFilter("item", false).Apply(ref testListCopy);
            Assert.AreEqual(3, testListCopy.Count());

            //check for regex expression ( .* = match all)
            testListCopy = new List<Node>(testList);
            new AnyStringFilter(".*", true).Apply(ref testListCopy);
            Assert.AreEqual(3, testListCopy.Count());

            //check for external attribute matching outside of IEvent (test type byte value)
            testListCopy = new List<Node>(testList);
            new AnyStringFilter("02", false).Apply(ref testListCopy);
            Assert.AreEqual(3, testListCopy.Count());

            //test for no results
            testListCopy = new List<Node>(testList);
            new AnyStringFilter("SeeShell", false).Apply(ref testListCopy);
            Assert.AreEqual(0, testListCopy.Count());

            //test for regex timeout 
            //TODO Find hard enough regex that actually takes time no matter the PC?
        }
    }
}