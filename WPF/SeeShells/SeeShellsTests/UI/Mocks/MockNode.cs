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
        public MockNode(IEvent aEvent) : this(aEvent, new Ellipse(), new Rectangle())
        {
        }
        public MockNode(IEvent aEvent, Ellipse dot, Rectangle block) : base(aEvent, dot, block)
        {
            AEvent = aEvent;
            Dot = dot;
            Block = block;

            dot.Visibility = System.Windows.Visibility.Visible;
        }

        public IEvent AEvent { get; }
        public Ellipse Dot { get; }
        public Rectangle Block { get; }
    }

}
