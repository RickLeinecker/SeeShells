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
using SeeShells.UI.Windows;

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

        private string GetInfo()
        {
            string text = "";
            
            foreach (KeyValuePair<string, string> property in this.aEvent.Parent.GetAllProperties())
            {
                text += AddSpacesToCamelCase(property.Key) + ": " + property.Value;
                text += "\n";
            };

            return text;
        }

        private string AddSpacesToCamelCase(string value)
        {
            return System.Text.RegularExpressions.Regex.Replace(value, "[A-Z]", " $0");
        }

        /// <summary>
        /// This enlarges the textblock so more information can be seen about the shellItem.
        /// </summary>
        public void ToggleInfo()
        {
            if(this.Height >= 250)
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
                this.Width = Double.NaN;
                this.Height = 250;
                this.Text = GetInfo();
            }
        }

        /// <summary>
        /// This creates a window of the event information so that it can be moved around on the screen.
        /// </summary>
        public void PopOutInfo()
        {
            EventInformationWindow info = new EventInformationWindow()
            {
                EventTitle = this.aEvent.Name,
                EventBody = GetInfo()
            };

            info.Show();
        }
    }
}
