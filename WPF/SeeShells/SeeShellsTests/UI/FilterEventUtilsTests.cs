using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeeShells.UI;
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
            var testList = new List<MockEvent>()
            {
                new MockEvent("item1", DateTime.Now, null, "Access"),
                new MockEvent("item2", DateTime.MaxValue, null, "Access"),
                new MockEvent("item3", DateTime.MinValue, null, "Access"),
            };

            //test set startdate and enddate
            DateTime startLimit = new DateTime(DateTime.MinValue.Ticks + 10000000000000, DateTimeKind.Utc);
            DateTime endLimit = new DateTime(DateTime.MaxValue.Ticks - 10000, DateTimeKind.Utc);
            var test1 = FilterEventUtils.FilterDateRange(testList, startLimit, endLimit);
            Assert.AreEqual(1, test1.Count());

            //test the startdate limit
            var test2 = FilterEventUtils.FilterDateRange(testList, startLimit, null);
            Assert.AreEqual(2, test2.Count());

            //test enddate limit
            var test3 = FilterEventUtils.FilterDateRange(testList, null, endLimit);
            Assert.AreEqual(2, test3.Count());

            //test full range (aka pointless filter)
            var test4 = FilterEventUtils.FilterDateRange(testList, null, null);
            Assert.AreEqual(3, test4.Count());
        }

        [TestMethod()]
        public void FilterEventTypeTest()
        {
            var testList = new List<MockEvent>()
            {
                new MockEvent("item1", DateTime.Now, null, "Access"),
                new MockEvent("item2", DateTime.MaxValue, null, "Create"),
                new MockEvent("item3", DateTime.MinValue, null, "Modified"),
            };

            //basic filter
            var test1 = FilterEventUtils.FilterEventType(testList, "Access");
            Assert.AreEqual(1, test1.Count());

            //test multifilter
            var test2 = FilterEventUtils.FilterEventType(testList, "Access", "Create");
            Assert.AreEqual(2, test2.Count());

            //test no filter
            var test3 = FilterEventUtils.FilterEventType(testList, "");
            Assert.AreEqual(0, test3.Count());
        }

        [TestMethod()]
        public void FilterParentTest()
        {
            var parent1 = new MockShellItem();
            var parent2 = new MockShellItem();
            var testList = new List<MockEvent>()
            {
                new MockEvent("item1", DateTime.Now, parent1, "Access"),
                new MockEvent("item2", DateTime.MaxValue, parent1, "Create"),
                new MockEvent("item3", DateTime.MinValue, parent2, "Modified"),
            };

            //check same parent
            var test1 = FilterEventUtils.FilterParent(testList, parent1);
            Assert.AreEqual(2, test1.Count());

            //check diff parent
            var test2 = FilterEventUtils.FilterParent(testList, parent2);
            Assert.AreEqual(1, test2.Count());

            //check unknown parent
            var test3 = FilterEventUtils.FilterParent(testList, new MockShellItem());
            Assert.AreEqual(0, test3.Count());
        }

        [TestMethod()]
        public void FilterNameTest()
        {
            var testList = new List<MockEvent>()
            {
                new MockEvent("item1", DateTime.Now, null, "Access"),
                new MockEvent("item1", DateTime.MaxValue, null, "Create"),
                new MockEvent("item3", DateTime.MinValue, null, "Modified"),
            };

            //check same name
            var test1 = FilterEventUtils.FilterName(testList, "item1");
            Assert.AreEqual(2, test1.Count());

            //check no name
            var test2 = FilterEventUtils.FilterName(testList, "item2");
            Assert.AreEqual(0, test2.Count());

            //check multi names
            var test3 = FilterEventUtils.FilterName(testList, "item1", "item3");
            Assert.AreEqual(3, test3.Count());

        }

        [TestMethod()]
        public void FilterAnyStringTest()
        {
            var parent1 = new MockShellItem("item1", 0x02);
            var parent2 = new MockShellItem("item3", 0x02);

            var testList = new List<MockEvent>()
            {
                new MockEvent(parent1.Name, DateTime.Now, parent1, "Access"),
                new MockEvent(parent1.Name, DateTime.MaxValue, parent1, "Create"),
                new MockEvent(parent2.Name, DateTime.MinValue, parent2, "Modified"),
            };


            //check for unique attribute to some items
            var test1 = FilterEventUtils.FilterAnyString(testList, "item1", false);
            Assert.AreEqual(2, test1.Count());

            //check for partial match to all items
            var test2 = FilterEventUtils.FilterAnyString(testList, "item", false);
            Assert.AreEqual(3, test2.Count());

            //check for regex expression ( .* = match all)
            var test3 = FilterEventUtils.FilterAnyString(testList, ".*", true);
            Assert.AreEqual(3, test3.Count());

            //check for external attribute matching outside of IEvent (test type byte value)
            var test4 = FilterEventUtils.FilterAnyString(testList, "02", false);
            Assert.AreEqual(3, test4.Count());

            //test for no results
            var test5 = FilterEventUtils.FilterAnyString(testList, "SeeShell", false);
            Assert.AreEqual(0, test5.Count());

            //test for regex timeout 
            //TODO Find hard enough regex that actually takes time no matter the PC?
        }
    }
}