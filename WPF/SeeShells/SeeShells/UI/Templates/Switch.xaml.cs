using Microsoft.Win32;
using SeeShells.IO;
using SeeShells.UI.Node;
using SeeShells.UI.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace SeeShells.UI.Templates
{
    /// <summary>
    /// Interaction logic for Switch.xaml
    /// </summary>
    public partial class Switch : UserControl
    {
        public Switch()
        {
            InitializeComponent();
        }
        
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (Home.timelinePage == null)
            {
                timeline.IsEnabled = false;

            }
            else
            {
                string timelinePageKey = "timelinepage";
                if (App.pages.ContainsKey(timelinePageKey))
                {
                    App.NavigationService.Navigate(App.pages[timelinePageKey]);
                }
            }
        }


        private void Home_Click(object sender, RoutedEventArgs e)
        {
            string homepage = "homepage";
            if (App.pages.ContainsKey(homepage))
            {
                App.NavigationService.Navigate(App.pages[homepage]);
            }

        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            string helppage = "helppage";
            if (App.pages.ContainsKey(helppage))
            {
                App.NavigationService.Navigate(App.pages[helppage]);
            }
        }

        private void About_OnClick(object sender, RoutedEventArgs e)
        {
            new AboutWindow().ShowDialog();
        }
        private void Import_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = Directory.GetCurrentDirectory(),
                Filter = "CSV File (*.csv)|*.csv"
            };

            if (openFileDialog.ShowDialog() != true)
                return;
            var file = openFileDialog.FileName;
            if (App.ShellItems != null)
            {
                App.ShellItems.Clear();
            }
            if (App.nodeCollection.nodeList != null)
            {
                App.nodeCollection.nodeList.Clear();
            }

            App.ShellItems = CsvIO.ImportCSVFile(file);
            List<IEvent> events = EventParser.GetEvents(App.ShellItems);
            App.nodeCollection.ClearAllFilters();
            App.nodeCollection.nodeList.AddRange(NodeParser.GetNodes(events));

            if (Home.timelinePage == null)
            {
                Home.timelinePage = new TimelinePage();
                App.NavigationService.Navigate(Home.timelinePage);
            }
            else
            {
                Home.timelinePage.RebuildTimeline();
                string timelinePageKey = "timelinepage";
                if (App.pages.ContainsKey(timelinePageKey))
                {
                    App.NavigationService.Navigate(App.pages[timelinePageKey]);
                }
            }
        }
    }
}
