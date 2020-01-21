using SeeShells.ShellParser.ShellItems;
using SeeShells.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShellsTests.UI.Mocks
{
    class MockEvent : IEvent
    {

        public MockEvent(string name, DateTime eventTime, IShellItem parent, string eventType)
        {
            Name = name;
            EventTime = eventTime;
            Parent = parent;
            EventType = eventType;
        }

        public string Name { get; set; }

        public DateTime EventTime { get; set; }

        public IShellItem Parent { get; set; }

        public string EventType { get; set; }
    }
}
