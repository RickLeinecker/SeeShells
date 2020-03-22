using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
                InfoBlock block = new InfoBlock();
                SetBlockProperties(block, aEvent);

                Node node = new Node(aEvent, block);
                SetNodeProperties(node);
                nodeList.Add(node);
            }
            
            return nodeList;
        }

        /// <summary>
        /// Sets up initial properties for a graphical dot object
        /// </summary>
        /// <param name="node">Acta as a graphical object (button) to be placed on the timeline.</param>
        private static void SetNodeProperties(Node node)
        {
            node.Width = 20;
            node.Height = 20;
            node.Click += Pages.TimelinePage.DotPress;
        }

        /// <summary>
        /// Sets up initial properties for a graphical block object
        /// </summary>
        /// <param name="block">graphical object that contains event details on a timeline</param>
        private static void SetBlockProperties(InfoBlock block, IEvent aEvent)
        {
            block.aEvent = aEvent;
            block.Text += aEvent.Name + "\n";
            block.Text += aEvent.EventTime + "\n";
            block.Text += aEvent.EventType + "\n";
            block.MouseEnter += Pages.TimelinePage.HoverBlock;
            block.MouseLeave += Pages.TimelinePage.HoverBlock;
        }
    }
}
