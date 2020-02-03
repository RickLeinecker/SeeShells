using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace SeeShells.UI.Node
{
    public class Node
    {
        IEvent aEvent;
        Ellipse dot;
        Rectangle block;

        /// <summary>
        /// The Node is an object that stores event data and has graphical objects to be displayed on a timeline.
        /// </summary>
        /// <param name="aEvent">object that stores event/shellbag data</param>
        /// <param name="dot">object to represent an event on a timeline</param>
        /// <param name="block">object to display event details on a timeline</param>
        public Node(IEvent aEvent, Ellipse dot, Rectangle block)
        {
            this.aEvent = aEvent;
            this.dot = dot;
            this.block = block;
        }
    }
}
