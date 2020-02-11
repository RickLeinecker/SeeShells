using SeeShells.UI.Node;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;

namespace SeeShells.UI.Pages
{
    /// <summary>
    /// Interaction logic for TimelinePage.xaml
    /// </summary>
    public partial class TimelinePage : Page
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public double UnitSize = 10;
        public TimeSpan UnitTimeSpan = new TimeSpan(0, 12, 0, 0);

        public TimelinePage()
        {
            InitializeComponent();

            BuildTimeline();
        }
        /// <summary>
        /// Builds a timeline dynamically
        /// </summary>
        public void BuildTimeline()
        {
            /// Uncomment this to see a timeline draw (it builds the App.nodeCollection.nodeList)
            /// This will be removed when all aplication components are connected
            //List<IEvent> eventList = new List<IEvent>();
            //Event Event1 = new Event("item1", new DateTime(2007, 1, 1, 0, 0, 0), null, "Access");
            //Event Event2 = new Event("item1", new DateTime(2007, 1, 2, 0, 0, 0), null, "Access");
            //Event Event3 = new Event("item1", new DateTime(2007, 1, 2, 12, 0, 0), null, "Access");
            //Event Event4 = new Event("item1", new DateTime(2007, 1, 2, 23, 0, 0), null, "Access");
            //Event Event5 = new Event("item1", new DateTime(2007, 12, 31, 0, 0, 0), null, "Access");
            //eventList.Add(Event1);
            //eventList.Add(Event2);
            //eventList.Add(Event3);
            //eventList.Add(Event4);
            //eventList.Add(Event5);
            //App.nodeCollection.nodeList = NodeParser.GetNodes(eventList);

            try
            {
                Nodeline.UnitSize = this.UnitSize;
                Nodeline.UnitTimeSpan = this.UnitTimeSpan;
                Nodeline.BeginDate = GetBeginDate();
                // EndDate should be one UnitTimeSpan more than the max value from the data to display properly
                Nodeline.EndDate = GetEndDate() + this.UnitTimeSpan;

                foreach (Node.Node node in App.nodeCollection.nodeList)
                {
                    TimelinePanel.SetDate(node.dot, node.aEvent.EventTime);
                    Nodeline.Children.Add(node.dot);
                }
            }
            catch (System.NullReferenceException ex)
            {
                logger.Error(ex, "Null nodeList" + "\n" + ex.ToString());
                return;
            }
        }

        private DateTime GetBeginDate()
        {
            DateTime minDate = DateTime.MaxValue;
            foreach (Node.Node node in App.nodeCollection.nodeList)
            {
                if (minDate > node.aEvent.EventTime)
                {
                    minDate = node.aEvent.EventTime;
                }
            }
            return minDate;
        }
        private DateTime GetEndDate()
        {
            DateTime maxDate = DateTime.MinValue;
            foreach (Node.Node node in App.nodeCollection.nodeList)
            {
                if (maxDate < node.aEvent.EventTime)
                {
                    maxDate = node.aEvent.EventTime;
                }
            }
            return maxDate;
        }
    }
}