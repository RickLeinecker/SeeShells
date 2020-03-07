using System.Windows;
using System.Windows.Controls;

namespace SeeShells.UI.Node
{   
    public class InformedDot : System.Windows.Controls.Button
    {
        public TextBlock block;

        /// <summary>
        /// The InformedDot is an object that holds the graphical elements of the dots on the timeline.
        /// </summary>
        /// <param name="block">object to display event details on a timeline</param>
        
        public InformedDot()
        {
            TextBlock block = new TextBlock();
            this.block = block;
        }
        public InformedDot(TextBlock block)
        {
            this.block = block;
        }

        /// <summary>
        /// This is used to hide and show the block of information connected to each dot of information on the timeline.
        /// </summary>
        public void toggle_block()
        {
            MessageBox.Show(block.Text);
            if (this.block.Visibility == Visibility.Collapsed)
            {
                this.block.Visibility = Visibility.Visible;
            }
            else if(this.block.Visibility == Visibility.Visible)
            {
                this.block.Visibility = Visibility.Collapsed;
            }
        }
    }
}
