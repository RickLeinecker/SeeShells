using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

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
                /// TODO:
                /// Customize dot and block objects
                Ellipse dot = new Ellipse();
                dot.Stroke = System.Windows.Media.Brushes.Black;
                dot.Fill = System.Windows.Media.Brushes.DarkBlue;
                dot.Width = 10;
                dot.Height = 10;
                Rectangle block = new Rectangle();

                Node node = new Node(aEvent, dot, block);

                nodeList.Add(node);
            }
            
            return nodeList;
        }
    }
}
