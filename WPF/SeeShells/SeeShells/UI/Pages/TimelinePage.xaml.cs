﻿using Microsoft.Win32;
using SeeShells.IO;
using SeeShells.UI.EventFilters;
using SeeShells.UI.Node;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
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

        // This flag is used in order to prevent the time span slider from writing to the time span slider text box after the text box has been written to.
        // Even though flags are not the best practice, it is necessary since there is no way to write to a text box without triggering a text change event.
        // A potential alternative is to create a custom silent text box class that extends text box and allows for changing of text without triggering a
        // a text change event.
        private bool unitTimeSpanSliderCanWriteToTextBox = true;
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
        /// When the time span slider is moved, it sets a new UnitTimeSpan for the timeline.
        /// </summary>
        /// <param name="sender">Slider</param>
        /// <param name="e">event args</param>
        private void TimeSpanSliderControl_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var slider = sender as Slider;
            this.unitTimeSpan = TimeSpan.FromSeconds(slider.Value);
            if(unitTimeSpanSliderCanWriteToTextBox)
            {
                SetTimeSpanSliderControlTextBoxValue(slider.Value);
            }
            unitTimeSpanSliderCanWriteToTextBox = true;
            if(Nodeline != null)
            {
                Nodeline.UnitTimeSpan = this.unitTimeSpan;
            }
        }

        /// <summary>
        /// When the time span slider text box text is changed, it moves the slider control to desired position, which in turn sets a new UnitTimeSpan for the timeline.
        /// </summary>
        /// <param name="sender">TextBox</param>
        /// <param name="e">event args</param>
        private void TimeSpanSliderControlTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            unitTimeSpanSliderCanWriteToTextBox = false;
            UnpdateTimeSpanSliderControl();
        }

        /// <summary>
        /// When an item from the time span slider combo box is selected, it moves the slider control to desired position, which in turn sets a new UnitTimeSpan for the timeline.
        /// </summary>
        /// <param name="sender">ComboBox</param>
        /// <param name="e">event args</param>
        private void TimeSpanSliderControlComboBox_DropDownClosed(object sender, EventArgs e)
        {
            unitTimeSpanSliderCanWriteToTextBox = false;
            UnpdateTimeSpanSliderControl();
        }

        /// <summary>
        /// When an item from the time span slider combo box is selected (with arrow keys), it moves the slider control to desired position, which in turn rebuilds the timeline.
        /// </summary>
        /// <param name="sender">ComboBox</param>
        /// <param name="e">event args</param>
        private void TimeSpanSliderControlComboBox_KeyUp(object sender, EventArgs e)
        {
            unitTimeSpanSliderCanWriteToTextBox = false;
            UnpdateTimeSpanSliderControl();
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
            //for (int i = 0; i < 100; i++)
            //{
            //    eventList.Add(new Event("item1", time, null, "Access"));
            //    time = time.AddHours(12);
            //}
            //App.nodeCollection.nodeList = NodeParser.GetNodes(eventList);

            try
            {
                Nodeline.UnitSize = 10; // Default size of a dot
                Nodeline.UnitTimeSpan = this.unitTimeSpan;
                Nodeline.BeginDate = GetBeginDate();
                Nodeline.EndDate = GetEndDate() + this.unitTimeSpan; // EndDate should be one UnitTimeSpan more than the max value from the data to display properly
                Nodeline.Children.Clear();

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
        /// Move the time span slider control based on the value of the text box and the unit of time from the combo box.
        /// </summary>
        private void UnpdateTimeSpanSliderControl()
        {
            if (double.TryParse(TimeSpanSliderControlTextBox.Text, out double num))
            {
                string unitOfTime = TimeSpanSliderControlComboBox.Text;

                if (unitOfTime.Equals("Years"))
                {
                    double AmountOfSecondsInAYear = 31536000.0;
                    TimeSpanSliderControl.Value = num * AmountOfSecondsInAYear;
                }
                else if (unitOfTime.Equals("Days"))
                {
                    double AmountOfSecondsInADay = 86400.0;
                    TimeSpanSliderControl.Value = num * AmountOfSecondsInADay;
                }
                else if (unitOfTime.Equals("Hours"))
                {
                    double AmountOfSecondsInAnHour = 3600.0;
                    TimeSpanSliderControl.Value = num * AmountOfSecondsInAnHour;
                }
                else if (unitOfTime.Equals("Minutes"))
                {
                    double AmountOfSecondsInAMinute = 60.0;
                    TimeSpanSliderControl.Value = num * AmountOfSecondsInAMinute;
                }
                else if (unitOfTime.Equals("Seconds"))
                {
                    TimeSpanSliderControl.Value = num;
                }
            }
        }

        /// <summary>
        /// Change the value of the time span text box based on the value change of the time span slider.
        /// </summary>
        /// <param name="sliderValue">value of the time span slider</param>
        private void SetTimeSpanSliderControlTextBoxValue(double sliderValue)
        {
            string unitOfTime = TimeSpanSliderControlComboBox.Text;

            if (unitOfTime.Equals("Years"))
            {
                decimal AmountOfSecondsInAYear = (decimal)31536000.0;
                TimeSpanSliderControlTextBox.Text = String.Format("{0:0.00}", ((decimal)sliderValue / AmountOfSecondsInAYear));
            }
            else if (unitOfTime.Equals("Days"))
            {
                decimal AmountOfSecondsInADay = (decimal)86400.0;
                TimeSpanSliderControlTextBox.Text = String.Format("{0:0.00}", ((decimal)sliderValue / AmountOfSecondsInADay));
            }
            else if (unitOfTime.Equals("Hours"))
            {
                double AmountOfSecondsInAnHour = 3600.0;
                TimeSpanSliderControlTextBox.Text = String.Format("{0:0.00}", (sliderValue / AmountOfSecondsInAnHour));
            }
            else if (unitOfTime.Equals("Minutes"))
            {
                double AmountOfSecondsInAMinute = 60.0;
                TimeSpanSliderControlTextBox.Text = String.Format("{0:0.00}", (sliderValue / AmountOfSecondsInAMinute));

            }
            else if (unitOfTime.Equals("Seconds"))
            {
                TimeSpanSliderControlTextBox.Text = String.Format("{0:0.00}", sliderValue);
            }
        }

        /// <summary>
        /// This checks when the download button is hit, whether the HTML and/or the CSV checkbox is checked or not and calls the creation of HtmlOutput.
        /// </summary>
        private void download_Click(object sender, RoutedEventArgs e)
        {
            if(htmlCheckBox.IsChecked ?? false)
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.DefaultExt = ".html";
                saveFileDialog1.Filter = "Html File (*.html)| *.html";
                saveFileDialog1.AddExtension = true;
                saveFileDialog1.ShowDialog();
                string name = saveFileDialog1.FileName;
                HtmlIO.OutputHtmlFile(App.nodeCollection.nodeList, name);
            }
            if(csvCheckBox.IsChecked ?? false)
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
    }
}