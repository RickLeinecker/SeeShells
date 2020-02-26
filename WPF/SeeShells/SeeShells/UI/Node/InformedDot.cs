using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SeeShells.UI.Node
{   
    public class InformedDot : System.Windows.Controls.Button
    {
        public TextBlock block;

        public InformedDot(TextBlock block)
        {
            this.block = block;
        }

        public void toggle_block()
        {
            MessageBox.Show(this.block.Text);
            if (this.block.Visibility == System.Windows.Visibility.Collapsed)
            {
                this.block.Visibility = System.Windows.Visibility.Visible;
            }
            else if(this.block.Visibility == System.Windows.Visibility.Visible)
            {
                this.block.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
    }
}
