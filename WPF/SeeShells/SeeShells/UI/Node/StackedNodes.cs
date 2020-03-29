using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace SeeShells.UI.Node
{
    public class StackedNodes : ToggleButton
    {
        public List<IEvent> events = new List<IEvent>();
        public List<InfoBlock> blocks = new List<InfoBlock>();
        public List<Node> nodes = new List<Node>();
        public TextBlock alignmentBlock { get; set; }

        public StackedNodes()
        {
            this.Width = 20;
            this.Height = 20;
            this.FontSize = 10;
            this.FontWeight = FontWeights.Bold;
        }

        public DateTime GetBlockTime()
        {
            return events[0].EventTime;
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

            if (alignmentBlock.Visibility == Visibility.Collapsed)
                alignmentBlock.Visibility = Visibility.Hidden;
            else
                alignmentBlock.Visibility = Visibility.Collapsed;
        }
    }
}
