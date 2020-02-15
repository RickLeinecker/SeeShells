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
        /// creates the eventList that will be returned full of events based on ShellItems
        /// </summary>
        List<IEvent> eventList = new List<IEvent>();
       
        /// <summary>
        /// parses through a list of shellitems to create a list of IEvents that consists of events created based
        /// on date information from each ShellItem 
        /// <param name="shells">List of IShellItems</param>
        /// <returns>A list of IEvents</returns>
        public static List<IEvent> Parser(List<IShellItem> shells)
        {
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
