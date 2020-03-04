using Microsoft.Win32;
using SeeShells.IO;
using SeeShells.UI.EventFilters;
using SeeShells.UI.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

        private Point mouseLocation;
        private Point pointOrig;
        private TranslateTransform transPoint;

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

            //rebuild the timeline according to the new filters
            this.RebuildTimeline();
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
        private void BuildTimeline()
        {
            try
            {
                if(App.nodeCollection.nodeList.Count == 0)
                {
                    logger.Info("No nodes to draw on the timeline.");
                    return;
                }

                List<Node.Node> nodeList = new List<Node.Node>();
                foreach (Node.Node node in App.nodeCollection.nodeList)
                {
                    if(node.dot.Visibility == System.Windows.Visibility.Visible)
                    {
                        nodeList.Add(node);
                    }
                }

                if (nodeList.Count == 0)
                {
                    logger.Info("All nodes are filtered out, no nodes to draw on the timeline.");
                    return;
                }

                List<Node.Node> nodesCluster = new List<Node.Node>(); // Holds events for one timeline at a time.
                nodesCluster.Add(nodeList[0]);
                DateTime previousDate = nodeList[0].aEvent.EventTime;
                DateTime realTimeStart = DateTimeRoundDown(previousDate, maxRealTimeSpan);
                int nodeListSize = nodeList.Count;
                for (int i = 1; i < nodeListSize; i++)
                {
                    // If the event belongs to the timeline
                    if (TimeSpan.Compare(nodeList[i].aEvent.EventTime.Subtract(realTimeStart), maxRealTimeSpan) == -1) // Compare returns -1 if the first argument is less than the second
                    {
                        nodesCluster.Add(nodeList[i]);
                    }
                    else
                    {
                        AddTimeline(nodesCluster);
                        nodesCluster.Clear();

                        nodesCluster.Add(nodeList[i]);
                        previousDate = nodeList[i].aEvent.EventTime;
                        realTimeStart = DateTimeRoundDown(previousDate, maxRealTimeSpan);
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
            DateTime beginDate = DateTimeRoundDown(nodesCluster[0].aEvent.EventTime, maxRealTimeSpan);
            DateTime endDate = beginDate.AddMinutes(1);

            TimelinePanel timelinePanel = new TimelinePanel
            {
                UnitTimeSpan = new TimeSpan(0, 0, 0, 1),
                UnitSize = App.nodeCollection.nodeList[0].dot.Width,
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
            textBlock.Text = beginDate.ToString() + " - " + endDate.ToString();
            textBlock.Height = 20;
            textBlock.Width = endDate.Subtract(beginDate).TotalSeconds * App.nodeCollection.nodeList[0].dot.Width;
            textBlock.Background = Brushes.LightSteelBlue;

            TimeStamps.Children.Add(textBlock);
        }

        /// <summary>
        /// Rounds down a DateTime to the nearest TimeSpan
        /// </summary>
        /// <param name="date">date to round down</param>
        /// <param name="roundingFactor">TimeSpan to round down to</param>
        /// <returns>a rounded down DateTime</returns>
        private DateTime DateTimeRoundDown(DateTime date, TimeSpan roundingFactor)
        {
            long ticks = date.Ticks / roundingFactor.Ticks;
            return new DateTime(ticks * roundingFactor.Ticks);
        }

        /// <summary>
        /// Clears the children of all timeline related UI objects and builds timeline.
        /// </summary>
        public void RebuildTimeline()
        {
            foreach(TimelinePanel timeline in Timelines.Children)
            {
                timeline.Children.Clear();
            }
            Timelines.Children.Clear();
            TimeStamps.Children.Clear();
            BuildTimeline();
        }

        /// <summary>
        /// This checks when the download button is hit, whether the HTML and/or the CSV checkbox is checked or not and calls the creation of HtmlOutput.
        /// </summary>
        private void download_Click(object sender, RoutedEventArgs e)
        {
            if (htmlCheckBox.IsChecked ?? false)
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.DefaultExt = ".html";
                saveFileDialog1.Filter = "Html File (*.html)| *.html";
                saveFileDialog1.AddExtension = true;
                saveFileDialog1.ShowDialog();
                string name = saveFileDialog1.FileName;
                HtmlIO.OutputHtmlFile(App.nodeCollection.nodeList, name);
            }
            if (csvCheckBox.IsChecked ?? false)
            {
                SaveFileDialog saveFileDialog2 = new SaveFileDialog();
                saveFileDialog2.DefaultExt = ".csv";
                saveFileDialog2.Filter = "CSV File (*.csv)| *.csv";
                saveFileDialog2.AddExtension = true;
                saveFileDialog2.ShowDialog();
                string name2 = saveFileDialog2.FileName;
                CsvIO.OutputCSVFile(App.ShellItems, name2);
            }
        }

        /// <summary>
        /// This activates the toggle_block method built into the InformedDot object. 
        /// </summary>
        public static void Dot_Press(object sender, EventArgs e)
        {
            ((InformedDot)sender).toggle_block();            
        }

        /// <summary>
        /// This is a work in progress and at the moment moves the boxes which have been turned off. However this is a away to get the coordinates
        /// of the dot. This is also allowing for a future block_press to work.
        /// </summary>
        public static void Block_Press(object sender, RoutedEventArgs e)
        {
            ((TextBlock)sender).Margin = new Thickness(((TextBlock)sender).TransformToAncestor(Application.Current.MainWindow).Transform(new Point(0, 0)).X+10, ((TextBlock)sender).TransformToAncestor(Application.Current.MainWindow).Transform(new Point(0, 0)).Y, 0, 0);
        }
    }
}