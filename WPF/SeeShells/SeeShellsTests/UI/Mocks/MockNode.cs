using SeeShells.UI;
using SeeShells.UI.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace SeeShellsTests.UI.Mocks
{
    class MockNode : Node
    {
        public MockNode(IEvent aEvent) : this(aEvent, new InfoBlock())
        {
        }
        public MockNode(IEvent aEvent, InfoBlock block) : base(aEvent, block)
        {
            AEvent = aEvent;
            Block = block;

            this.Visibility = System.Windows.Visibility.Visible;
        }

        public IEvent AEvent { get; }
        public TextBlock Block { get; }
    }

}
