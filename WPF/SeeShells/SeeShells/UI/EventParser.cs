namespace SeeShells.UI
{
    public class EventParser : Block, IShellItem
    {
        List<IEvent> listOfEvents = new List<IEvent>();
        public EventParser(List<IShellItem> items)
        {
            foreach(IShellItem i in items)
            {
                Event shell = new Event(i);
                listOfEvents.add(shell); 
            }
            returnEventList(listOfEvents);
        }
        public List<IEvent> returnEventList(List<IEvent> events)
        {
            return events;
        }
        
    }


}