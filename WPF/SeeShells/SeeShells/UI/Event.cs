using SeeShells.ShellParser.ShellItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShells.UI
{
    public class Event : IEvent
    {
        private TimeZone curTimeZone = TimeZone.CurrentTimeZone;
        /// <summary>
        /// Constructor for the Event class that takes in the parameters 
        /// listed below in order to create the elements of an event object. 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="eventTime"></param>
        /// <param name="parent"></param>
        /// <param name="eventType"></param>
        public Event(string name, DateTime eventTime, IShellItem parent, string eventType)
        {
            Name = name;
            EventTime = eventTime;
            Parent = parent;
            EventType = eventType;
        }
        /// <summary>
        /// Identifier for the entity which was modified at this particular <see cref="EventTime"/>
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Details the time in which an event took place.
        /// </summary>
        public DateTime EventTime { get; set; }
        /// <summary>
        /// The ShellItem which this event was derived from.
        /// Provides optional extra information to the formulation of this event.
        /// </summary>
        public IShellItem Parent { get; set; }
        /// <summary>
        /// Categorizes the action which was preformed.
        /// </summary>
        public string EventType { get; set; }

        public TimeZone timeZone { get
            {
                return curTimeZone;
            }
        }
    }
}
