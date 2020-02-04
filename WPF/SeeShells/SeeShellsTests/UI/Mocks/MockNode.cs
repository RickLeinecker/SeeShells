using SeeShells.UI;
using SeeShells.UI.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace SeeShellsTests.UI.Mocks
{
    class MockNode : Node
    {
        public MockNode(IEvent aEvent, Ellipse dot = null, Rectangle block= null) : base(aEvent, dot, block)
        {
        }
    }
}
