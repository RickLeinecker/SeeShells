using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
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
                TextBlock block = new TextBlock();
                SetBlockProperties(block, aEvent);

                InformedDot dot = new InformedDot(block);
                SetDotProperties(dot);

                Node node = new Node(aEvent, dot, block);
                nodeList.Add(node);
            }
            
            return nodeList;
        }

        /// <summary>
        /// Sets up initial properties for a graphical dot object
        /// </summary>
        /// <param name="dot">graphical object that represents events on a timeline</param>
        private static void SetDotProperties(InformedDot dot)
        {
            dot.Height = 10;
            dot.Width = 10;
            dot.Click += Pages.TimelinePage.Dot_Press;
        }

        /// <summary>
        /// Sets up initial properties for a graphical block object
        /// </summary>
        /// <param name="block">graphical object that contains event details on a timeline</param>
        private static void SetBlockProperties(TextBlock block, IEvent aEvent)
        {
            //foreach (KeyValuePair<string, string> property in aEvent.Parent.GetAllProperties())
            //{
            //  block.Text += property.Key + "," + property.Value;
            //}
            block.Text += aEvent.Name;
            block.Text += aEvent.EventType;
            block.Text += aEvent.EventTime;
            block.Foreground = Brushes.White;
            block.Background = Brushes.Turquoise; // #5ec0ca
            block.Height = 40;
            block.Width = 100;
            block.MouseDown += Pages.TimelinePage.Block_Press;
        }
    }
}
