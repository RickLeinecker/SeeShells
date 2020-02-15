using SeeShells.ShellParser.ShellItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShells.UI
{
    public static class EventParser
    {
        /// <summary> 
        /// Parses a list of IShellItems and creates events based on each date of a shellitem
        /// </summary>
        /// <param name="shells">the list of IShellitems to be parsed</param>
        /// <returns>A list of IEvents</returns>
        public static List<IEvent> GetEvents(List<IShellItem> shells)
        {
            List<IEvent> eventList = new List<IEvent>();
            foreach (IShellItem item in shells)
            {
                IDictionary<String, String> parser = item.GetAllProperties();
                foreach(var el in parser)
                {
                    if (el.Key.Contains("Date") && Convert.ToDateTime(el.Value) != DateTime.MinValue)
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
