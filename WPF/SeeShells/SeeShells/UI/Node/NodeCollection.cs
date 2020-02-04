using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShells.UI.Node
{
    public class NodeCollection
    {
        /// <summary>
        /// Creates a global list of Nodes to be accessed through an Instance of the class created in App.xaml.cs
        /// </summary>
        public List<Node> nodeList;

        List<Node> filteredNodeList;
        Dictionary<string, INodeFilter> filterList;


        /// <summary>
        ///  applies a filter to the nodeList 
        /// recalculates the <see cref="filteredNodeList"/> upon adding a filter
        /// </summary>
        /// <param name="identifier">A unique identifer that will be used to identify the filtering instance</param>
        /// <param name="filter">a filter that will be applied to remove nodes from a collection. </param>

        public void AddEventFilter(string identifier, INodeFilter filter)
        {
            filterList.Add(identifier, filter);
            filter.Apply(ref filteredNodeList);
        }

        /// <summary>
        /// Removes a previously added <see cref="INodeFilter"/>
        /// recalculates the <see cref="filteredNodeList"/> upon removing a filter
        /// </summary>
        /// <param name="identifier">A unique identifer that will be used to identify the filtering instance</param>
        public void RemoveEventFilter(string filterIdentifer)
        {
            bool didRemove = filterList.Remove(filterIdentifer);

            if (didRemove)
            {
                //reapply all remaining filters
                filteredNodeList = new List<Node>(nodeList);
                foreach (INodeFilter filter in filterList.Values)
                {
                    filter.Apply(ref filteredNodeList);
                }
            }
        }

    }
}
