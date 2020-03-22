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

namespace SeeShells.UI.Pages
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {

        public AboutWindow()
        {
            InitializeComponent();
            System.Version version2 = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            VersionLabel.Content = ($" Version {version2.Major}.{version2.Minor}.{version2.Revision}.{version2.Build}");

        }

        private void HyperlinkNavigate(object sender, RequestNavigateEventArgs e)
        {
            // Open web browser to link (https://stackoverflow.com/a/10238715)
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
