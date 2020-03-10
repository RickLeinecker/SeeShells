using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace SeeShells.UI.Node
{
    public class StackedNodes : Button
    {
        public List<IEvent> events = new List<IEvent>();

        public StackedNodes()
        {
            this.Width = 10;
            this.Height = 10;
            this.FontSize = 5;
            this.FontWeight = FontWeights.Bold;
        }

        public void Add(IEvent aEvent)
        {
            events.Add(aEvent);
        }
    }
}
