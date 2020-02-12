using System;

namespace SeeShells.UI
{
    [TestClass]
    public class EventParserTest
    {
        [TestMethod]
	    public EventParserTest()
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
            shellItems.Add(ShellItem);
            List<IEvent> events = EventParserTest(shellItems);
            assertThat(events.size()).is(3);
        }
    }

}
