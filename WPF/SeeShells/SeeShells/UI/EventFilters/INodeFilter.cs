using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeeShells.UI.Node;

namespace SeeShells.UI
{
    public interface INodeFilter
    {
        /// <summary>
        /// Preforms a filtering operation. removing elements that don't meet this filter.
        /// </summary>
        /// <param name="nodes">Elements which will be tested in this filter.</param>
        void Apply(ref List<Node.Node> nodes);
    }
}
