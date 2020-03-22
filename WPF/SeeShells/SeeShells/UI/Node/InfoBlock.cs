using SeeShells.ShellParser.ShellItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SeeShells.UI.Node
{
    public class InfoBlock : TextBlock
    {
        public IEvent aEvent;

        /// <summary>
        /// The InfoBlock displays the information held in an IEvent graphically on the timeline.
        /// </summary>
        public InfoBlock()
        {
            this.aEvent = null;
        }

        /// <summary>
        /// The InfoBlock displays the information held in an IEvent graphically on the timeline.
        /// </summary>
        /// <param name="aEvent">object that stores shellbag data and current event information</param>
        public InfoBlock(IEvent aEvent)
        {
            this.aEvent = aEvent;
        }

        /// <summary>
        /// This enlarges the textblock so more information can be seen about the shellItem.
        /// </summary>
        public void ToggleInfo()
        {
            if(this.Width > 300)
            {
                this.Text = "";
                this.Width = 200;
                this.Height = 70;
                this.Text += this.aEvent.Name + "\n";
                this.Text += this.aEvent.EventTime + "\n";
                this.Text += this.aEvent.EventType + "\n";
            }
            else
            {
                this.Width = 450;
                this.Height = 250;
                this.Text = "";
                foreach (KeyValuePair<string, string> property in this.aEvent.Parent.GetAllProperties())
                {
                    this.Text += property.Key + " : " + property.Value;
                    this.Text += "\n";
                }
            }
        }
    }
}
