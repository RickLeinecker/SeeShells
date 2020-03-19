using System.Collections.Generic;
using System.Linq;
using System.Windows;
using SeeShells.UI.EventFilters;
using SeeShells.UI.Node;

namespace SeeShellsTests.UI.Mocks
{
    /// <summary>
    /// Filters out all but the specified nodes in the constructor
    /// </summary>
    public class MockNodeFilter : INodeFilter
    {
        private readonly Node[] acceptableNodes;

        public MockNodeFilter(params Node[] acceptableNodes)
        {
            this.acceptableNodes = acceptableNodes;
        }

        public void Apply(ref List<Node> nodes)
        {
            foreach (Node node in nodes)
            {
                if (!acceptableNodes.Contains(node))
                {
                    node.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}