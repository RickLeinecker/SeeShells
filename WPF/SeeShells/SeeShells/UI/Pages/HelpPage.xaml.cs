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
using SeeShells.IO.Networking;
using SeeShells.IO.Networking.JSON;
using XamlReader = System.Windows.Markup.XamlReader;

namespace SeeShells.UI.Pages
{
    /// <summary>
    /// Interaction logic for HelpPage.xaml
    /// </summary>
    public partial class HelpPage : Page
    {

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private const string ReadmeFile = "README.md";
        private static readonly string ReadmeLocation = Directory.GetCurrentDirectory() + '/' + ReadmeFile;

        public HelpPage()
        {
            InitializeComponent();
            Loaded += OnLoad;
        }

        private async void OnLoad(object sender, RoutedEventArgs e)
        {
            //use API call for getting help, if fails use internal resource.
            string updatedHelpResult = string.Empty;

            try
            {
                updatedHelpResult = await API.GetHelp();
            }
            catch (APIException ex)
            {
                logger.Error(ex);
            }

            string markdown = string.Empty;
            try
            {
                markdown = File.ReadAllText(ReadmeLocation);
            }
            catch (IOException ex)
            {
                logger.Warn("Unable to Read cached Help file", ex);
            }
            
            //check if the downloaded content has changed, if so save the file and use the updated help
            
            if ( updatedHelpResult != string.Empty && !updatedHelpResult.Equals(markdown, StringComparison.OrdinalIgnoreCase))
            {
                markdown = updatedHelpResult;

                //update local file
                try
                {
                    File.WriteAllText(ReadmeLocation, markdown);
                }
                catch (IOException ex)
                {
                    logger.Error("Unable to save updated readme. Is the program in a write protected directory?", ex);
                }
            }

            // nothing cached, cant update, use default internal help.
            if (markdown == string.Empty)
            {

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
            }

            HelpViewer.Markdown = markdown;
            LoadingIndicator.IsBusy = false;
        }

        private void OpenHyperlink(object sender, ExecutedRoutedEventArgs e)
        { 
            Process.Start(e.Parameter.ToString());

        }
    }
}
