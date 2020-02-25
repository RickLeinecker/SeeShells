using SeeShells.IO;
using SeeShells.UI.EventFilters;
using SeeShells.UI.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
        private HashSet<string> eventTypeList = new HashSet<string>();
        private HashSet<string> eventUserList = new HashSet<string>();

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private TimeSpan maxRealTimeSpan = new TimeSpan(0, 0, 1, 0); // Max time in one timeline (1 min).

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

        private void EventUserFilter_OnItemSelectionChanged(object sender, ItemSelectionChangedEventArgs e)
        {
            CheckComboBox emitter = (CheckComboBox)sender;

            string[] items = emitter.SelectedItems.Cast<string>().ToArray();
            UpdateFilter("EventUser", new EventUserFilter(items));
        }

        private void EventUserFilter_DropDownOpened(object sender, EventArgs e)
        {
            CheckComboBox emitter = (CheckComboBox)sender;
            if (eventUserList.Count == 0)
            {
                //check if the owner property is on the node, then pull the user
                foreach (Node.Node node in App.nodeCollection.nodeList)
                {
                    string userID;
                    if (node.aEvent.Parent.GetAllProperties().TryGetValue("RegistryOwner", out userID))
                    {
                        eventUserList.Add(userID);
                    }
                }
            }

            emitter.ItemsSource = eventUserList;
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
        /// Builds a timeline dynamically. Creates one timeline for each cluster of events.
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
                List<Node.Node> nodesCluster = new List<Node.Node>(); // Holds events for one timeline at a time.
                nodesCluster.Add(App.nodeCollection.nodeList[0]);
                DateTime previousDate = App.nodeCollection.nodeList[0].aEvent.EventTime;

                int nodeListSize = App.nodeCollection.nodeList.Count;
                for (int i = 1; i < nodeListSize; i++)
                {
                    long ticks = previousDate.Ticks / maxRealTimeSpan.Ticks;
                    DateTime realTimeStart = new DateTime(ticks * maxRealTimeSpan.Ticks); // This is the time of the first event rounded down to the nearest maxRealTimeSpan

                    // If the event belongs to the timeline
                    if (App.nodeCollection.nodeList[i].aEvent.EventTime.Subtract(realTimeStart) < maxRealTimeSpan)
                    {
                        nodesCluster.Add(App.nodeCollection.nodeList[i]);
                    }
                    else
                    {
                        AddTimeline(nodesCluster);
                        nodesCluster.Clear();

                        nodesCluster.Add(App.nodeCollection.nodeList[i]);
                        previousDate = App.nodeCollection.nodeList[i].aEvent.EventTime;
                        if (i == nodeListSize - 1) // If it's the last event of nodeList.
                        {
                            AddTimeline(nodesCluster);
                            nodesCluster.Clear();
                        }
                    }
                }
                if (nodesCluster.Count != 0) // If all events belong to the same timeline.
                {
                    AddTimeline(nodesCluster);
                }
            }
            catch (System.NullReferenceException ex)
            {
                logger.Error(ex, "Null nodeList" + "\n" + ex.ToString());
                return;
            }
        }


        /// <summary>
        /// Creates a timeline with the given events and adds it to the UI.
        /// </summary>
        /// <param name="nodesCluster">list of events that belong in 1 timeline</param>
        private void AddTimeline(List<Node.Node> nodesCluster)
        {
            long ticks = nodesCluster[0].aEvent.EventTime.Ticks / maxRealTimeSpan.Ticks;
            DateTime beginDate = new DateTime(ticks * maxRealTimeSpan.Ticks);
            ticks = (nodesCluster[nodesCluster.Count - 1].aEvent.EventTime.Ticks + maxRealTimeSpan.Ticks - 1) / maxRealTimeSpan.Ticks;
            DateTime endDate = new DateTime(ticks * maxRealTimeSpan.Ticks);

            TimelinePanel timelinePanel = new TimelinePanel
            {
                UnitTimeSpan = new TimeSpan(0, 0, 0, 1),
                UnitSize = 10,
                BeginDate = beginDate,
                EndDate = endDate,
                KeepOriginalOrderForOverlap = true
            };

            foreach (Node.Node node in nodesCluster)
            {
                TimelinePanel.SetDate(node.dot, node.aEvent.EventTime);
                timelinePanel.Children.Add(node.dot);
            }

            Timelines.Children.Add(timelinePanel);
            AddTextBlockTimeStamp(beginDate, endDate);
        }

        /// <summary>
        /// Adds a timestamp for timeline.
        /// </summary>
        /// <param name="beginDate">the begin date of the time interval</param>
        /// <param name="endDate">the end date of the time interval</param>
        private void AddTextBlockTimeStamp(DateTime beginDate, DateTime endDate)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = beginDate.ToString();
            textBlock.Height = 20;
            textBlock.Width = endDate.Subtract(beginDate).TotalSeconds * 10;
            textBlock.Background = Brushes.LightSteelBlue;

            TimeStamps.Children.Add(textBlock);
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