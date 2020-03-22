using SeeShells.UI.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                string message = "Warning: Please press the parse button at least once";
                MessageBoxImage image = MessageBoxImage.Information;
                string caption = "Error";
                MessageBox.Show(message, caption, MessageBoxButton.OK, image);
            }
            else
            {
                
            }
        }
    }
}
