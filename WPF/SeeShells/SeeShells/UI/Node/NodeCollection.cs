using SeeShells.UI.EventFilters;
using SeeShells.UI.Pages;
using System.Collections.Generic;

namespace SeeShells.UI.Node
{
    public class NodeCollection
    {
        /// <summary>
        /// Creates a global list of Nodes to be accessed through an Instance of the class created in App.xaml.cs
        /// </summary>
        public List<Node> nodeList = new List<Node>();

        Dictionary<string, INodeFilter> filterList = new Dictionary<string, INodeFilter>();


        /// <summary>
        ///  applies a filter to the nodeList 
        /// recalculates the <see cref="nodeList"/> upon adding a filter
        /// </summary>
        /// <param name="identifier">A unique identifer that will be used to identify the filtering instance</param>
        /// <param name="filter">a filter that will be applied to remove nodes from a collection. </param>

        public void AddEventFilter(string identifier, INodeFilter filter)
        {
            filterList.Add(identifier, filter);
            filter.Apply(ref nodeList);
            TimelinePage.timelinePage.RebuildTimeline();
        }

        /// <summary>
        /// Removes a previously added <see cref="INodeFilter"/>
        /// recalculates the <see cref="nodeList"/> upon removing a filter
        /// </summary>
        /// <param name="identifier">A unique identifer that will be used to identify the filtering instance</param>
        public void RemoveEventFilter(string filterIdentifer)
        {
            bool didRemove = filterList.Remove(filterIdentifer);

            if (didRemove)
            {
                //restore visibility of previously filtered events
                foreach (var node in nodeList)
                {
                    node.dot.Visibility = System.Windows.Visibility.Visible;
                }
                //reapply all remaining filters
                foreach (INodeFilter filter in filterList.Values)
                {
                    filter.Apply(ref nodeList);
                }
                TimelinePage.timelinePage.RebuildTimeline();
            }
        }

    }
}
