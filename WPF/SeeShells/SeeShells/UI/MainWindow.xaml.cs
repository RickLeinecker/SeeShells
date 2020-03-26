using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NLog;
using SeeShells.UI.Pages;

namespace SeeShells
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        private Home home = new Home();
        private HelpPage helpPage = new HelpPage();
        public MainWindow()
        {
            InitializeComponent();
            mainframe.Navigate(home);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            App.NavigationService = mainframe.NavigationService;
            App.pages.Add("homepage", home);
            App.pages.Add("helppage", helpPage);
        }
    }
}
