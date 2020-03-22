using Microsoft.Win32;
using SeeShells.IO;
using SeeShells.ShellParser.ShellItems;
using SeeShells.UI.EventFilters;
using SeeShells.UI.Node;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private void ClearEventContentFilter_Click(object sender, RoutedEventArgs e)
        {
            AllStringFilterTextBlock.Clear();
            RegexCheckBox.IsChecked = false;
        }

        private void UpdateDateFilter(object sender, SelectionChangedEventArgs e)
        {
            UpdateFilter("DateFilter", new DateRangeFilter(startDatePicker.SelectedDate, endDatePicker.SelectedDate));

        }

        private void ClearDateFilter_Click(object sender, RoutedEventArgs e)
        {
            startDatePicker.SelectedDate = null;
            endDatePicker.SelectedDate = null;
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

        private void ClearEventTypeFilter_Click(object sender, RoutedEventArgs e)
        {
            EventTypeFilter.SelectedItems.Clear();
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

        private void ClearUserFilter_Click(object sender, RoutedEventArgs e)
        {
            EventUserFilter.SelectedItems.Clear();
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

        private void ClearEventNameFilter_Click(object sender, RoutedEventArgs e)
        {
            EventNameFilter.Clear();
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
                List<TextBlock> blockList = new List<TextBlock>();
                foreach (Node.Node node in App.nodeCollection.nodeList)
                {
                    node.Style = (Style)Resources["Node"];
                    if (node.Visibility == System.Windows.Visibility.Visible)
                    {
                        nodeList.Add(node);
                        blockList.Add(node.block);
                    }
                    node.block.Visibility = Visibility.Collapsed;
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

            TimelinePanel timelinePanel = MakeTimelinePanel(beginDate, endDate);
            TimelinePanel blockPanel = MakeTimelinePanel(beginDate, endDate);

            // Add all blocks onto a timeline
            foreach (Node.Node node in nodesCluster)
            {
                node.block.Style = (Style)Resources["TimelineBlock"];
                TimelinePanel.SetDate(node.block, node.aEvent.EventTime);
                blockPanel.Children.Add(node.block);
            }

            List<StackedNodes> stackedNodesList = GetStackedNodes(nodesCluster);
            // Add all nodes that stack onto a timeline
            foreach (StackedNodes stackedNode in stackedNodesList)
            {
                stackedNode.Click += DotPress;
                stackedNode.Style = (Style)Resources["StackedNode"];
                TimelinePanel.SetDate(stackedNode, stackedNode.events[0].EventTime);
                stackedNode.Content = stackedNode.events.Count.ToString();
                timelinePanel.Children.Add(stackedNode);
                ConnectNodeToTimeline(timelinePanel, stackedNode.events[0].EventTime);
            }
            // Add all other nodes onto a timeline
            foreach (Node.Node node in nodesCluster)
            {
                TimelinePanel.SetDate(node, node.aEvent.EventTime);
                timelinePanel.Children.Add(node);
                ConnectNodeToTimeline(timelinePanel, node.aEvent.EventTime);
            }

            Timelines.Children.Add(timelinePanel);
            Blocks.Children.Add(blockPanel);
            Line separationLine = MakeTimelineSeparatingLine();
            Line blockSeperation = MakeBlockPanelSeparation();
            Blocks.Children.Add(blockSeperation);
            Timelines.Children.Add(separationLine);
            AddTicks(beginDate, endDate);
            AddTimeStamp(beginDate, endDate);
        }


        /// <summary>
        /// Creates a TimelinePanel
        /// </summary>
        /// <param name="beginDate">begin date of timeline</param>
        /// <param name="endDate">end date of timeline</param>
        /// <returns>TimelinePanel that can space graphical objects according to time</returns>
        private TimelinePanel MakeTimelinePanel(DateTime beginDate, DateTime endDate)
        {
            TimelinePanel timelinePanel = new TimelinePanel
            {
                UnitTimeSpan = new TimeSpan(0, 0, 0, 1),
                UnitSize = App.nodeCollection.nodeList[0].Width,
                BeginDate = beginDate,
                EndDate = endDate,
                KeepOriginalOrderForOverlap = true
            };

            return timelinePanel;
        }

        /// <summary>
        /// Gets all nodes that have the same EventTime out of a list that gets passed into the method and returns them in a list of StackedNodes.
        /// The list that gets passed in has the nodes that would stack deleted from it.
        /// </summary>
        /// <param name="nodesCluster">a list of nodes</param>
        /// <returns>list of StackedNodes and modifies the list of nodes that gets passed in</returns>
        private List<StackedNodes> GetStackedNodes(List<Node.Node> nodesCluster)
        {
            List<StackedNodes> stackedNodesList = new List<StackedNodes>();
            Node.Node previousNode = nodesCluster[0];
            int i = 1;
            while (i < nodesCluster.Count)
            {
                if (previousNode.aEvent.EventTime.Equals(nodesCluster[i].aEvent.EventTime))
                {
                    StackedNodes stackedNodes = new StackedNodes();
                    stackedNodes.events.Add(previousNode.aEvent);
                    stackedNodes.blocks.Add(previousNode.block);
                    while (i < nodesCluster.Count && previousNode.aEvent.EventTime.Equals(nodesCluster[i].aEvent.EventTime))
                    {
                        stackedNodes.events.Add(nodesCluster[i].aEvent);
                        stackedNodes.blocks.Add(nodesCluster[i].block);
                        previousNode = nodesCluster[i];
                        nodesCluster.RemoveAt(i - 1);

                    }
                    stackedNodesList.Add(stackedNodes);

                    if (i < nodesCluster.Count) // If haven't reached the end of the list.
                    {
                        previousNode = nodesCluster[i];
                        nodesCluster.RemoveAt(i - 1);
                    }
                    else
                    {
                        nodesCluster.RemoveAt(i - 1); 
                    }
                }
                else
                {
                    previousNode = nodesCluster[i];
                    i++;
                }
            }
            return stackedNodesList;
        }

        /// <summary>
        /// Draws a line to connect a node to a timeline
        /// </summary>
        /// <param name="timelinePanel">timeline panel to hold and position connection lines</param>
        /// <param name="eventTime">time used as position for where to draw a connecting line</param>
        private void ConnectNodeToTimeline(TimelinePanel timelinePanel, DateTime eventTime)
        {
            Line connectorLine = new Line();
            connectorLine.Stroke = Brushes.LightSteelBlue;
            connectorLine.X1 = 10;
            connectorLine.X2 = 10;
            connectorLine.Y1 = 0;
            connectorLine.Y2 = 15;
            connectorLine.StrokeThickness = 1;

            TimelinePanel.SetDate(connectorLine, eventTime);
            timelinePanel.Children.Add(connectorLine);
        }

        /// <summary>
        /// Draws ticks below the nodes to represent the seconds of each timeline interval
        /// </summary>
        /// <param name="beginDate">begin date of a timeline period</param>
        /// <param name="endDate"> end date of a timeline period</param>
        private void AddTicks(DateTime beginDate, DateTime endDate)
        {
            TimelinePanel timelinePanel = MakeTimelinePanel(beginDate, endDate);

            AddTicksBar(beginDate, endDate);
            int timePeriod = (int)endDate.Subtract(beginDate).TotalSeconds;
            for (int i = 0; i < timePeriod; i++)
            {
                Line tick = new Line();
                tick.Stroke = Brushes.Black;
                tick.X1 = 10;
                tick.X2 = 10;
                tick.Y1 = 0;
                tick.Y2 = 20;
                tick.StrokeThickness = 1;

                TimelinePanel.SetDate(tick, beginDate);
                timelinePanel.Children.Add(tick);
                beginDate = beginDate.AddSeconds(1);
            }
            Ticks.Children.Add(timelinePanel);

            Line separationLine = MakeTimelineSeparatingLine();
            separationLine.Visibility = Visibility.Hidden; // Hidden since this line is added only for proper spacing
            Ticks.Children.Add(separationLine);
        }

        /// <summary>
        /// Adds a rectangular bar behind the timeline ticks.
        /// </summary>
        /// <param name="beginDate">begin date of the time interval used to decide the size of the bar</param>
        /// <param name="endDate">end date of the time interval used to decide the size of the bar</param>
        private void AddTicksBar(DateTime beginDate, DateTime endDate)
        {
            Rectangle bar = new Rectangle();
            bar.Height = 20;
            bar.Width = (endDate.Subtract(beginDate).TotalSeconds * App.nodeCollection.nodeList[0].Width) + 12; // The added integer is to compensate for the margins and thickness of the line that separates the timelines. 
            bar.Fill = Brushes.LightSteelBlue;
            bar.Margin = new Thickness(0, 0, -1, 0);

            TicksBar.Children.Add(bar);
        }

        /// <summary>
        /// Adds time stamp that tells the time period of a timeline
        /// </summary>
        /// <param name="beginDate">the begin date of the timeline</param>
        /// <param name="endDate">the end date of the timeline</param>
        private void AddTimeStamp(DateTime beginDate, DateTime endDate)
        {
            TextBlock timeStamp = new TextBlock();
            timeStamp.Text = beginDate.ToString() + " - " + endDate.ToString();
            timeStamp.Foreground = Brushes.White;
            timeStamp.Height = 20;
            timeStamp.Width = (endDate.Subtract(beginDate).TotalSeconds * App.nodeCollection.nodeList[0].Width) + 12;
            timeStamp.Margin = new Thickness(0, 0, -1, 0);

            TimeStamps.Children.Add(timeStamp);
        }

        /// <summary>
        /// Creates a line to be used as separation between timelines
        /// </summary>
        /// <returns>a line to visually separate timelines</returns>
        private Line MakeTimelineSeparatingLine()
        {
            Line separatingLine = new Line();
            separatingLine.Stroke = Brushes.LightSteelBlue;
            separatingLine.X1 = 0;
            separatingLine.X2 = 0;
            separatingLine.Y1 = 43;
            separatingLine.Y2 = 150;
            separatingLine.StrokeThickness = 2;
            separatingLine.HorizontalAlignment = HorizontalAlignment.Left;
            separatingLine.VerticalAlignment = VerticalAlignment.Center;
            separatingLine.Margin = new Thickness(5, 0, 5, 0);

            return separatingLine;
        }

        /// <summary>
        /// Creates a space to seperate the multiple timelines
        /// </summary>
        /// <returns>A appropriate space to separate the block panels on the timelines</returns>
        private Line MakeBlockPanelSeparation()
        {
            Line separatingSpace = new Line();
            separatingSpace.X1 = 0;
            separatingSpace.X2 = 0;
            separatingSpace.Y1 = 43;
            separatingSpace.Y2 = 150;
            separatingSpace.StrokeThickness = 2;
            separatingSpace.HorizontalAlignment = HorizontalAlignment.Left;
            separatingSpace.VerticalAlignment = VerticalAlignment.Center;
            separatingSpace.Margin = new Thickness(5, 0, 5, 0);

            return separatingSpace;
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
        /// Clears all children of UI objects and rebuilds the timeline.
        /// </summary>
        public void RebuildTimeline()
        {
            foreach (Object child in Timelines.Children)
            {
                if(child is TimelinePanel) // Only TimelinePanel objects since Timelines also contains separating lines
                {
                    TimelinePanel timeline = (TimelinePanel)child;
                    timeline.Children.Clear();
                }
            }
            foreach (Object child in Blocks.Children)
            {
                if (child is TimelinePanel) // Only TimelinePanel objects since Timelines also contains separating lines
                {
                    TimelinePanel blockPanel = (TimelinePanel)child;
                    blockPanel.Children.Clear();
                }
            }
            Timelines.Children.Clear();
            Ticks.Children.Clear();
            TicksBar.Children.Clear();
            TimeStamps.Children.Clear();
            Blocks.Children.Clear();
            BuildTimeline();
        }

        /// <summary>
        /// This checks when the download button is hit, whether the HTML and/or the CSV checkbox is checked or not and calls the creation of HtmlOutput.
        /// </summary>
        private void DownloadClick(object sender, RoutedEventArgs e)
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
        /// This activates the toggle_block method built into the Node object. 
        /// </summary>
        public static void DotPress(object sender, EventArgs e)
        {
            if(sender.GetType() == typeof(Node.Node))
            {
                ((Node.Node)sender).ToggleBlock();
            }
            else if(sender.GetType() == typeof(StackedNodes))
            {
                ((StackedNodes)sender).ToggleBlock();
            }
        }

        public static void HoverBlock(object sender, EventArgs e)
        {
            ((InfoBlock)sender).ToggleInfo();
        }
    }
}