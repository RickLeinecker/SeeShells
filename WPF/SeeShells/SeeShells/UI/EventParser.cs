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
        /// <summary>
        /// Constructor for the EventParser that loops through each ShellItem and creates an event 
        /// for each Modified,Accessed and Creation date for that shellItem.
        /// </summary>
        /// <param name="shells"></param>
        public List<IEvent> EventParser(List<IShellItem> shells)
        {
            foreach (IShellItem item in shells)
            {
                Event eM = createModifiedEvent(item);
                Event eA = createAccessedEvent(item);
                Event eC = createCreationEvent(item);
                eventList.Add(eM);
                eventList.Add(eA);
                eventList.Add(eC);
            }
            return eventList;
        }

        /// <summary>
        /// Creates an Event for the ShellItem based on its modified date
        /// </summary>
        /// <param name="mItem"></param>
        /// <returns></returns>
        public Event createModifiedEvent(IShellItem mItem)
        {
            String name = mItem.Name;
            String typename = mItem.TypeName;
            DateTime modified = mItem.ModifiedDate;
            String eventType = "";
            IDictionary<String, String> parser = mItem.GetAllProperties();
            foreach (var el in parser)
            {
                if (el.Key == "ModifiedDate")
                {
                    String temp = el.Key;
                    String[] type = temp.Split('D');
                    eventType = type[0];
                }
            }
            Event newEventM = new Event(name, modified, mItem, eventType);
            return newEventM;

        }
        /// <summary>
        /// Creates an Event for the ShellItem based on its Accessed Date
        /// </summary>
        /// <param name="aItem"></param>
        /// <returns></returns>
        public Event createAccessedEvent(IShellItem aItem)
        {
            String name = aItem.Name;
            String typename = aItem.TypeName;
            DateTime accessed = aItem.AccessedDate;
            String eventType = "";
            IDictionary<String, String> parser = aItem.GetAllProperties();
            foreach (var el in parser)
            {
                if (el.Key == "AccessedDate")
                {
                    String temp = el.Key;
                    String[] type = temp.Split('D');
                    eventType = type[0];
                }
            }
            Event newEventA = new Event(name, accessed, aItem, eventType);
            return newEventA;

        }
        /// <summary>
        /// Creates an Event for the ShellItem based on its Creation Date
        /// </summary>
        /// <param name="cItem"></param>
        /// <returns></returns>
        public Event createCreationEvent(IShellItem cItem)
        {
            String name = cItem.Name;
            String typename = cItem.TypeName;
            DateTime created = cItem.CreationDate;
            String eventType = "";
            IDictionary<String, String> parser = cItem.GetAllProperties();
            foreach (var el in parser)
            {
                if (el.Key == "CreationDate")
                {
                    String temp = el.Key;
                    String[] type = temp.Split('D');
                    eventType = type[0];
                }
            }
            Event newEventC = new Event(name, created, cItem, eventType);
            return newEventC;

        }
    }
}
