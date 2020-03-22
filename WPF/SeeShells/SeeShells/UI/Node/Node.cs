using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls.Primitives;

namespace SeeShells.UI.Node
{
    public class Node : ToggleButton
    {
        public IEvent aEvent;
        public InfoBlock block;

        /// <summary>
        /// The Node is an object that stores event data and has graphical objects to be displayed on a timeline.
        /// </summary>
        /// <param name="aEvent">object that stores event/shellbag data</param>
        /// <param name="block">object to display event details on a timeline</param>
        public Node(IEvent aEvent, InfoBlock block)
        {
            this.aEvent = aEvent;
            this.block = block;
        }

        /// <summary>
        /// This is used to hide and show the block of information connected to each dot of information on the timeline.
        /// </summary>
        public void ToggleBlock()
        {
            if (this.block.Visibility == Visibility.Collapsed)
            {
                this.block.Visibility = Visibility.Visible;
            }
            else if (this.block.Visibility == Visibility.Visible)
            {
                this.block.Visibility = Visibility.Collapsed;
            }
        }
    }
}
