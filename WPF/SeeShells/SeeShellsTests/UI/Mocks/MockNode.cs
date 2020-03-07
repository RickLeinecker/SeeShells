using SeeShells.UI;
using SeeShells.UI.Node;
using System.Windows.Controls;

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
