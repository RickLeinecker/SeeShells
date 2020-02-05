using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeeShells.ShellParser.ShellItems;
using SeeShells.UI.Node;

namespace SeeShells.UI.EventFilters
{
    /// <summary>
    /// Filter's <see cref="Node"/>s by an <see cref="IEvent.Parent"/> property.
    /// If multiple <see cref="IEvent.Parent"/> are specified, returned <see cref="Node.Node"/> are one of the specified types.
    /// </summary>
    public class EventParentFilter : INodeFilter
    {
        private readonly IShellItem[] shellItems;

        /// <summary>
        /// Filter's <see cref="Node.Node"/>s by their <see cref="IEvent.Parent"/> property.
        /// If multiple <see cref="IEvent.Parent"/> are specified, returned <see cref="Node.Node"/> are one of the specified types.
        /// </summary>
        /// <param name="shellItems">One or more acceptable <see cref="IShellItem"/> to filter on. </param>
        public EventParentFilter(params IShellItem[] shellItems)
        {
            this.shellItems = shellItems;
        }

        public void Apply(ref List<Node.Node> nodes)
        {
            for (int i = nodes.Count-1; i >= 0; i--)//iterate backwards because iterating forwards would be an issue with a list of changing size.
            {
                Node.Node node = nodes[i];
                IEvent nEvent = node.aEvent;

                bool acceptableParent = false;
                foreach (var parent in shellItems)
                {
                    if (nEvent.Parent.Equals(parent))
                    {
                        acceptableParent = true;
                        break;
                    }
                }

                if (!acceptableParent)
                {
                    //nodes.Remove(node);
                    node.dot.Visibility = System.Windows.Visibility.Collapsed;
                }
            }

        }
    }
}
