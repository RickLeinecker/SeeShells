using SeeShells.ShellParser.ShellItems;
using SeeShells.UI;
using SeeShells.UI.Node;
using SeeShells.UI.Pages;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Reflection;
using System.Windows.Resources;
using SeeShells.UI.Pages;

namespace SeeShells
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Creates an instance of the EventCollection class so that the whole program can access the list of events. 
        /// </summary>
        public static EventCollection eventCollection = new EventCollection();

        /// <summary>
        /// Creates an instance of the NodeCollection class so that the whole program can access the list of nodes. 
        /// </summary>
        public static NodeCollection nodeCollection = new NodeCollection();

        /// <summary>
        /// Collection of <see cref="ShellItem"/> which is populated after a parsing operation.
        /// </summary>
        public static List<IShellItem> ShellItems { get; set; }

        public static Dictionary<string, Page> pages = new Dictionary<string, Page>();


        public static NavigationService NavigationService;

        /// <summary>
        /// Removes the toolbar overflow side button.
        /// <see cref="https://stackoverflow.com/a/1051264"/>
        /// </summary>
        private void Toolbar_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.ToolBar toolBar = sender as System.Windows.Controls.ToolBar;
            var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;
            if (overflowGrid != null)
            {
                overflowGrid.Visibility = Visibility.Collapsed;
            }
        }

        private void About_OnClick(object sender, RoutedEventArgs e)
        {
            new AboutWindow().ShowDialog();
        }
    }
}
