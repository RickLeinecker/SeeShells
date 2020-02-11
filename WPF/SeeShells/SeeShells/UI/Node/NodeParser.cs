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
                Ellipse dot = new Ellipse();
                SetDotProperties(dot);
                Rectangle block = new Rectangle();
                SetBlockProperties(block);

                Node node = new Node(aEvent, dot, block);
                nodeList.Add(node);
            }
            
            return nodeList;
        }

        /// <summary>
        /// Sets up initial properties for a graphical dot object
        /// </summary>
        /// <param name="dot">graphical object that represents events on a timeline</param>
        private static void SetDotProperties(Ellipse dot)
        {
            dot.Stroke = System.Windows.Media.Brushes.Black;
            dot.Fill = System.Windows.Media.Brushes.DarkBlue;
            dot.Width = 10;
            dot.Height = 10;
        }

        /// <summary>
        /// Sets up initial properties for a graphical block object
        /// </summary>
        /// <param name="block">graphical object that contains event details on a timeline</param>
        private static void SetBlockProperties(Rectangle block)
        {
            /// TODO:
            /// Set Properties.
        }
    }
}
