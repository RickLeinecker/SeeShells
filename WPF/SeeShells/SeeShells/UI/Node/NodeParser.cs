using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShells.UI.Node
{
    public static class NodeParser
    {
        /// <summary>
        /// This method creates a list of nodes to be used for the timeline
        /// </summary>
        /// <param name="eventList">a list of events</param>
        /// <returns>a list of nodes</returns>
        public static List<Node> GetNodes(List<IEvent> eventList)
        {
            List<Node> nodeList = new List<Node>();
            
            foreach(IEvent aEvent in eventList)
            {
                Node node = new Node(aEvent);
                //TODO:
                // Node needs a Dot and Box added

                nodeList.Add(node);
            }
            
            return nodeList;
        }
    }
}
