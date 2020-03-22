using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace SeeShells.UI.Node
{
    public class StackedNodes : Button
    {
        public List<IEvent> events = new List<IEvent>();
        public List<InfoBlock> blocks = new List<InfoBlock>();

        public StackedNodes()
        {
            this.Width = 20;
            this.Height = 20;
            this.FontSize = 10;
            this.FontWeight = FontWeights.Bold;
        }

        public void Add(IEvent aEvent)
        {
            events.Add(aEvent);
        }

        public void ToggleBlock()
        {
            foreach(InfoBlock block in this.blocks)
            {
                if (block.Visibility == Visibility.Collapsed)
                {
                    block.Visibility = Visibility.Visible;
                }
                else if (block.Visibility == Visibility.Visible)
                {
                    block.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}
