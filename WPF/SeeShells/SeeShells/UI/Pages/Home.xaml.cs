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

namespace SeeShells.UI.Pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        public Home()
        {
            InitializeComponent();
        }

        private void OfflineBrowseButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OSBrowseButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GUIDBrowseButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ScriptBrowseButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ScriptUpdateButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GUIUpdateButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OSUpdateButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ParseButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TimelinePage());
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
