using System;
using System.Collections.Generic;

namespace SeeShells.UI
{
    public class EventCollection
    {
        /// <summary>
        /// creates a global list of IEvents to be accessed through an Instance of the class created in App.xaml.cs
        /// </summary>
        public List<IEvent> eventList;
    }
}
