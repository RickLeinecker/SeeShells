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
        public MockNode(IEvent aEvent) : this(aEvent, new InformedDot(),  new TextBlock())
        {
        }
        public MockNode(IEvent aEvent, InformedDot dot, TextBlock block) : base(aEvent, dot, block)
        {
            AEvent = aEvent;
            Dot = dot;
            dot.block = block;
            Block = block;

            dot.Visibility = System.Windows.Visibility.Visible;
        }

        public IEvent AEvent { get; }
        public InformedDot Dot { get; }
        public TextBlock Block { get; }
    }

}
