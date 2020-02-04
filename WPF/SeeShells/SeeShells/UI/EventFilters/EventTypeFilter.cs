using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeeShells.UI.Node;

namespace SeeShells.UI.EventFilters {
    /// <summary>
    /// Filter's a list of <see cref="Node.Node"/>s by a specific <see cref="IEvent.EventTime"/> criteria.
    /// </summary>
    public class EventTypeFilter : INodeFilter
    {
        private readonly string[] eventTypes;

        /// <summary>
        /// Filter's a list of <see cref="Node.Node"/>s by a specific <see cref="IEvent.EventTime"/> criteria.
        /// </summary>
        /// <param name="eventTypes">One or more acceptable types to filter on. </param>
        public EventTypeFilter(params string[] eventTypes)
        {
            this.eventTypes = eventTypes;
        }
        public void Apply(ref List<Node.Node> nodes)
        {
            for (int i = nodes.Count-1; i >= 0; i--)//iterate backwards because iterating forwards would be an issue with a list of changing size.
            {
                Node.Node node = nodes[i];
                IEvent nEvent = node.aEvent;

                bool acceptableType = false;
                foreach (string type in eventTypes)
                {
                    if (nEvent.EventType.Trim().Equals(type, StringComparison.OrdinalIgnoreCase))
                    {
                        acceptableType = true;
                        break;
                    }
                }

                if (!acceptableType)
                {
                    nodes.Remove(node);
                }
            }
        }
    }

}
