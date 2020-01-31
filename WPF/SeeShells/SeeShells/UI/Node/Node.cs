using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShells.UI.Node
{
    public class Node
    {
        IEvent aEvent;
        // TODO:
        // Add Dot and Block member variables

        // TODO:
        // Add Dot and Block to constructor
        public Node(IEvent aEvent)
        {
            this.aEvent = aEvent;
        }
    }
}
