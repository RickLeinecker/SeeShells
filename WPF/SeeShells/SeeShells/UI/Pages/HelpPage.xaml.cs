using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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
using System.Xaml;
using Markdig.Renderers;
using Newtonsoft.Json;
using SeeShells.IO.Networking.JSON;
using XamlReader = System.Windows.Markup.XamlReader;

namespace SeeShells.UI.Pages
{
    /// <summary>
    /// Interaction logic for HelpPage.xaml
    /// </summary>
    public partial class HelpPage : Page
    {
        private const string ReadmeFile = "README.md";

        public HelpPage()
        {
            InitializeComponent();
            Loaded += OnLoad;
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {

            string markdown;

            //todo use API call for getting help, if fails use internal resource.

            //internal resource retrieval, see: https://stackoverflow.com/a/3314213
            Assembly assembly = Assembly.GetExecutingAssembly();
            string internalResourcePath = assembly.GetManifestResourceNames().Single(str => str.EndsWith(ReadmeFile));
            using (Stream fileStream = assembly.GetManifestResourceStream(internalResourcePath))
            {
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    markdown = reader.ReadToEnd();
                }
            }


            HelpViewer.Markdown = markdown;
        }

        private void OpenHyperlink(object sender, ExecutedRoutedEventArgs e)
        { 
            Process.Start(e.Parameter.ToString());

        }
    }
}
