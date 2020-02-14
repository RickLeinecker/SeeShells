using SeeShells.ShellParser.ShellItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShells.UI
{
    public class EventParser
    {
        /// <summary>
        /// creates the eventList that will be returned full of events based on ShellItems
        /// </summary>
        List<IEvent> eventList = new List<IEvent>();
        List<IShellItem> shells;
        /// <summary>
        /// Constructor for the EventParser that loops through each ShellItem and creates an event 
        /// for each Modified,Accessed and Creation date for that shellItem.
        /// </summary>
        /// <param name="shells"></param>
        public EventParser(List<IShellItem> shells)
        {
            this.shells = shells;
        }
        public List<IEvent> Parser()
        {
            foreach (IShellItem item in shells)
            {
                IDictionary<String, String> parser = item.GetAllProperties();
                foreach(var el in parser)
                {
                    String check = el.Key;
                    String date = "Date";
                    if (check.Contains(date) && el.Value != DateTime.MinValue)
                    {
                        String name = item.Name;
                        String typename = item.TypeName;
                        DateTime eventDate = Convert.ToDateTime(el.Value);
                        String[] type = check.Split('D');
                        String eventType = type[0];
                        Event e = new Event(name, eventDate, item, eventType);
                        eventList.Add(e);
                    }
                }
            }
            return eventList;
        }
    }
}
