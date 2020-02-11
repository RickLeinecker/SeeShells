﻿using SeeShells.UI.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;

namespace SeeShells.UI.Pages
{
    /// <summary>
    /// Interaction logic for TimelinePage.xaml
    /// </summary>
    public partial class TimelinePage : Page
    {
        public TimelinePage()
        {
            InitializeComponent();

            BuildTimeline();
        }

        public void BuildTimeline()
        {
            List<IEvent> eventList = new List<IEvent>();
            Event Event1 = new Event("item1", new DateTime(2007, 1, 1, 0, 0, 0), null, "Access");
            Event Event2 = new Event("item1", new DateTime(2007, 1, 2, 0, 0, 0), null, "Access");
            Event Event3 = new Event("item1", new DateTime(2007, 1, 2, 12, 0, 0), null, "Access");
            Event Event4 = new Event("item1", new DateTime(2007, 1, 2, 23, 0, 0), null, "Access");
            Event Event5 = new Event("item1", new DateTime(2007, 12, 31, 0, 0, 0), null, "Access");
            eventList.Add(Event1);
            eventList.Add(Event2);
            eventList.Add(Event3);
            eventList.Add(Event4);
            eventList.Add(Event5);

            NodeCollection nodeCollection = new NodeCollection();
            nodeCollection.nodeList = NodeParser.GetNodes(eventList);

            Nodeline.BeginDate = new DateTime(2007, 1, 1, 0, 0, 0);
            Nodeline.EndDate = new DateTime(2007, 12, 31, 12, 0, 0); // Should be one UnitTimeSpan more than the max value from the data
            Nodeline.UnitSize = 10.0;
            Nodeline.UnitTimeSpan = new TimeSpan(0, 12, 0, 0);

            foreach (Node.Node node in nodeCollection.nodeList)
            {
                TimelinePanel.SetDate(node.dot, node.aEvent.EventTime);
                Nodeline.Children.Add(node.dot);
            }
        }
    }
}
