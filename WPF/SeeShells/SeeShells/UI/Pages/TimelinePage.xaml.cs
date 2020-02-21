using SeeShells.IO;
using SeeShells.UI.EventFilters;
using SeeShells.UI.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.Toolkit.Primitives;

namespace SeeShells.UI.Pages
{
    /// <summary>
    /// Interaction logic for TimelinePage.xaml
    /// </summary>
    public partial class TimelinePage : Page
    {
        private const string EVENT_PARENT_IDENTIFER = "EventParent";
        private Dictionary<string, string> eventTypeFilterList = new Dictionary<string, string>();
        private HashSet<string> eventTypeList = new HashSet<string>();

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private TimeSpan unitTimeSpan = new TimeSpan(0, 12, 0, 0);

        public TimelinePage()
        {
            InitializeComponent();

            BuildTimeline();
        }

        private void AllStringFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (RegexCheckBox != null) //null check to stop initalization NPEs
            {
                TextBox emitter = (TextBox)sender;
                bool useRegex = RegexCheckBox.IsChecked.GetValueOrDefault(false);
                UpdateFilter("AnyString", new AnyStringFilter(emitter.Text, useRegex));
            }
        }

        private void UpdateDateFilter(object sender, SelectionChangedEventArgs e)
        {
            UpdateFilter("DateFilter", new DateRangeFilter(startDatePicker.SelectedDate, endDatePicker.SelectedDate));

        }

        private void UpdateFilter(string filterIdentifer, INodeFilter newFilter)
        {
            //remove the current filter that exists
            App.nodeCollection.RemoveEventFilter(filterIdentifer);

            //add a new filter with our date restrictions
            App.nodeCollection.AddEventFilter(filterIdentifer, newFilter);

        }

        /// <summary>
        /// Updates the list of <seealso cref="EventFilters.EventTypeFilter"/>s when a change occurs
        /// </summary>
        private void EventTypeFilter_OnItemSelectionChanged(object sender, ItemSelectionChangedEventArgs e)
        {
            CheckComboBox emitter = (CheckComboBox)sender;

            string[] items = emitter.SelectedItems.Cast<string>().ToArray();
            UpdateFilter("EventType", new EventTypeFilter(items));

        }

        private void EventTypeFilter_DropDownOpened(object sender, EventArgs e)
        {
            CheckComboBox emitter = (CheckComboBox)sender;

            if (eventTypeList.Count == 0)
            {
                foreach (Node.Node node in App.nodeCollection.nodeList)
                {
                    eventTypeList.Add(node.aEvent.EventType);
                }
            }

            emitter.ItemsSource = eventTypeList;
        }

        private void EventParentTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //TODO can only be implememented from a context menu option (e.g. right click) which allows us to get the actual event
            TextBox emitter = (TextBox)sender;
            UpdateFilter(EVENT_PARENT_IDENTIFER, new EventParentFilter());
        }

        private void RegexCheckBox_Click(object sender, RoutedEventArgs e)
        {
            //force fire a text changed event for the textbox so that the filter gets updated
            AllStringFilter_TextChanged(AllStringFilterTextBlock, new TextChangedEventArgs(e.RoutedEvent, UndoAction.None));
        }

        private void EventParentClearButton_Click(object sender, RoutedEventArgs e)
        {
            App.nodeCollection.RemoveEventFilter(EVENT_PARENT_IDENTIFER);
            EventParentTextBox.Text = string.Empty;
        }

        private void EventNameFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox emitter = (TextBox)sender;
            UpdateFilter("EventName", new EventNameFilter(emitter.Text));

        }

        /// <summary>
        /// Builds a timeline dynamically.
        /// </summary>
        public void BuildTimeline()
        {
            /// TO BE REMOVED
            /// Uncomment this to see a timeline draw (it builds the App.nodeCollection.nodeList)
            /// This will be removed when all aplication components are connected
            //List<IEvent> eventList = new List<IEvent>();
            //DateTime time = new DateTime(2007, 1, 1);
            //for (int i = 0; i < 3; i++)
            //{
            //    eventList.Add(new Event("item1", time, null, "Access"));
            //    //time = time.AddHours(12);
            //    //time = time.AddMinutes(4);
            //    time = time.AddSeconds(1);
            //}
            //time = time.AddMinutes(10);
            //eventList.Add(new Event("item1", time, null, "Access"));
            //time = time.AddMinutes(2);
            //eventList.Add(new Event("item1", time, null, "Access"));
            //App.nodeCollection.nodeList = NodeParser.GetNodes(eventList);

            try
            {
                //Nodeline.UnitSize = 10; // Default size of a dot
                //Nodeline.UnitTimeSpan = this.unitTimeSpan;
                //Nodeline.BeginDate = GetBeginDate();
                //Nodeline.EndDate = GetEndDate() + this.unitTimeSpan; // EndDate should be one UnitTimeSpan more than the max value from the data to display properly
                //Nodeline.Children.Clear();

                List<Node.Node> nodesCluster = new List<Node.Node>();
                nodesCluster.Add(App.nodeCollection.nodeList[0]);
                DateTime startDate = App.nodeCollection.nodeList[0].aEvent.EventTime;
                int nodeListSize = App.nodeCollection.nodeList.Count;
                for (int i = 1; i < nodeListSize; i++)
                {
                    double timeGap = Math.Abs(startDate.Subtract(App.nodeCollection.nodeList[i].aEvent.EventTime)
                        .TotalMinutes);
                    if (timeGap <= 1)
                    {
                        nodesCluster.Add(App.nodeCollection.nodeList[i]);
                        startDate = App.nodeCollection.nodeList[i].aEvent.EventTime;
                    }
                    else
                    {
                        AddTimeline(nodesCluster);
                        nodesCluster.Clear();

                        nodesCluster.Add(App.nodeCollection.nodeList[i]);
                        startDate = App.nodeCollection.nodeList[i].aEvent.EventTime;
                        if(i == nodeListSize - 1) // If it's the last element
                        {
                            AddTimeline(nodesCluster);
                            nodesCluster.Clear();
                        }
                    }
                }
                if(nodesCluster.Count != 0)
                {
                    AddTimeline(nodesCluster);
                }

                //foreach (Node.Node node in App.nodeCollection.nodeList)
                //{
                //    TimelinePanel.SetDate(node.dot, node.aEvent.EventTime);
                //    Nodeline.Children.Add(node.dot);
                //}
            }
            catch (System.NullReferenceException ex)
            {
                logger.Error(ex, "Null nodeList" + "\n" + ex.ToString());
                return;
            }
        }

        private void AddTimeline(List<Node.Node> nodesCluster)
        {
            TimelinePanel timelinePanel = new TimelinePanel
            {
                UnitTimeSpan = new TimeSpan(0, 0, 0, 1),
                UnitSize = 10,
                BeginDate = nodesCluster[0].aEvent.EventTime,
                EndDate = nodesCluster[nodesCluster.Count - 1].aEvent.EventTime + new TimeSpan(0, 0, 0, 1),
                KeepOriginalOrderForOverlap = true
            };
            foreach (Node.Node node in nodesCluster)
            {
                TimelinePanel.SetDate(node.dot, node.aEvent.EventTime);
                timelinePanel.Children.Add(node.dot);
            }
            //Timeline.Children.Add(timelinePanel);
            Timelines.Children.Add(timelinePanel);
            Line myLine = new Line();
            myLine.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
            myLine.X1 = 1;
            myLine.X2 = 50;
            myLine.Y1 = 1;
            myLine.Y2 = 50;
            myLine.HorizontalAlignment = HorizontalAlignment.Left;
            myLine.VerticalAlignment = VerticalAlignment.Center;
            myLine.StrokeThickness = 2;
            Timelines.Children.Add(myLine);
        }

        /// <summary>
        /// Finds the earliest date from the list of events that is represented on the timeline.
        /// </summary>
        /// <returns>the earliest date</returns>
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

        /// <summary>
        /// Finds the latest date from the list of events that is represented on the timeline.
        /// </summary>
        /// <returns>the latest date</returns>
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

        /// <summary>
        /// This checks when the download button is hit, whether the HTML checkbox is checked or not and calls the creation of HtmlOutput.
        /// </summary>
        private void download_Click(object sender, RoutedEventArgs e)
        {
            if (htmlCheckBox.IsChecked ?? false)
            {
                System.Windows.MessageBox.Show("helps");
                HtmlIO.OutputHtmlFile(App.nodeCollection.nodeList, "timeline.html");
            }
        }
    }
}