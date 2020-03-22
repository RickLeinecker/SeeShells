using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using SeeShells.IO.Networking;
using Microsoft.Win32;
using SeeShells.UI.ViewModels;
using Newtonsoft.Json;
using SeeShells.IO.Networking.JSON;
using SeeShells.ShellParser.ShellItems;
using SeeShells.ShellParser;
using SeeShells.UI.Node;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using NLog.Time;
using SeeShells.ShellParser.Registry;
using SeeShells.UI.Templates;

namespace SeeShells.UI.Pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        private readonly FileLocations locations;
        private GridLength visibleRow = new GridLength(2, GridUnitType.Star);
        private GridLength hiddenRow = new GridLength(0);

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static TimelinePage timelinePage;

        public Home()
        {
            InitializeComponent();

            string currentDirectory = Directory.GetCurrentDirectory();
            locations = new FileLocations(
                os: currentDirectory + @"\OS.json",
                guid: currentDirectory + @"\GUIDs.json",
                script: currentDirectory + @"\Scripts.json"
            );

            this.DataContext = locations;
            UpdateOSVersionList();
            HideOfflineRows();

        }
        

        private void OfflineBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Dat files (*.dat)|*.dat|Registry files (*.reg)|*.reg|All files|*.*",
                InitialDirectory = Directory.GetCurrentDirectory()
            };

            if (openFileDialog.ShowDialog() != true)
                return;

            locations.OfflineFileLocations = openFileDialog.FileNames;
        }

        private void UpdateOSVersionList()
        {
            OSVersion.SelectedIndex = -1;
            OSVersion.Items.Clear();

            IList<RegistryLocations> registryLocations = ConfigParser.GetDefaultRegistryLocations();

            if (File.Exists(locations.OSFileLocation))
            {
                string json = File.ReadAllText(locations.OSFileLocation);
                try
                {
                    registryLocations = JsonConvert.DeserializeObject<IList<RegistryLocations>>(json);
                }
                catch (JsonSerializationException)
                {
                    showErrorMessage("The OS file selected is not formatted properly. Will proceed with default OS configurations.",
                        "Incorrect OS Configuration File Format");
                }
            }

            foreach (RegistryLocations location in registryLocations)
            {
                if (!OSVersion.Items.Contains(location.OperatingSystem))
                    OSVersion.Items.Add(location.OperatingSystem);
            }

        }

        private void OSBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            string location = GetFileFromBrowsing();
            if(UserSelectedFile(location))
            {
                locations.OSFileLocation = location;
                UpdateOSVersionList();
            }
        }

        private async void OSUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            string location = locations.OSFileLocation;
            if (!ContinueAfterSendingOverwriteWarning(location))
                return;

            string message = "File saved at\n" + location;
            string caption = "Success";
            MessageBoxImage image = MessageBoxImage.Information;

            try
            {
                Mouse.OverrideCursor = Cursors.Wait;

                await Task.Run(() => API.GetOSRegistryLocations(location));

            }
            catch (APIException)
            {
                message = "Failed to save the OS configuration file.";
                caption = "Fail";
                image = MessageBoxImage.Error;
            }

            Mouse.OverrideCursor = Cursors.Arrow;
            MessageBox.Show(message, caption, MessageBoxButton.OK, image);
            UpdateOSVersionList();

        }

        private void GUIDBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            string location = GetFileFromBrowsing();
            if (UserSelectedFile(location))
            {
                locations.GUIDFileLocation = location;
            }
        }

        private async void GUIDUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            string location = locations.GUIDFileLocation;
            if (!ContinueAfterSendingOverwriteWarning(location))
                return;

            string message = "File saved at\n" + location;
            string caption = "Success";
            MessageBoxImage image = MessageBoxImage.Information;

            try
            {
                Mouse.OverrideCursor = Cursors.Wait;

                await Task.Run(() => API.GetGuids(location));

            }
            catch (APIException)
            {
                message = "Failed to save the GUID configuration file.";
                caption = "Fail";
                image = MessageBoxImage.Error;
            }

            Mouse.OverrideCursor = Cursors.Arrow;
            MessageBox.Show(message, caption, MessageBoxButton.OK, image);
        }

        private void ScriptBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            string location = GetFileFromBrowsing();
            if (UserSelectedFile(location))
            {
                locations.ScriptFileLocation = location;
            }
        }

        private async void ScriptUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            string location = locations.ScriptFileLocation;
            if (!ContinueAfterSendingOverwriteWarning(location))
                return;

            string message = "File saved at\n" + location;
            string caption = "Success";
            MessageBoxImage image = MessageBoxImage.Information;

            try
            {
                Mouse.OverrideCursor = Cursors.Wait;

                await Task.Run(() => API.GetScripts(location));

            }
            catch (APIException)
            {
                message = "Failed to save the Scripts configuration file.";
                caption = "Fail";
                image = MessageBoxImage.Error;
            }

            Mouse.OverrideCursor = Cursors.Arrow;
            MessageBox.Show(message, caption, MessageBoxButton.OK, image);
        }

        private bool ContinueAfterSendingOverwriteWarning(string path)
        {
            MessageBoxResult answer;

            if (File.Exists(path))
            {
                answer = MessageBox.Show("This button will overwrite the file currently at\n" + path + "\nwith the most recent configuration data.\nWould you like to continue?",
                    "Overwrite file?",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if(answer == MessageBoxResult.Yes)
                    return true;

                return false;
            }

            answer = MessageBox.Show("This button will create a file at\n" + path + "\nwith the most recent configuration data.\nWould you like to continue?",
                    "Create file?",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);
            if (answer == MessageBoxResult.Yes)
                return true;

            return false;

        }


        private bool UserSelectedFile(string result)
        {
            if (result == string.Empty)
                return false;

            return true;
        }

        private string GetFileFromBrowsing()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "JSON files (*.json)|*.json",
                InitialDirectory = Directory.GetCurrentDirectory()
            };

            if (openFileDialog.ShowDialog() != true)
                return string.Empty;

            return openFileDialog.FileName;
        }


        private async void ParseButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ConfigurationFilesAreValid())
                return;

            if (OfflineCheck.IsChecked == true)
                if (!OfflineSelectionsAreValid())
                    return;

            //cover UI
            Mouse.OverrideCursor = Cursors.Wait;
            //TODO spinner over screen to show operation in progress?
            ParseButton.Content = "Parsing...";
            EnableUIElements(false);

            //begin the parsing process
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            App.ShellItems = await ParseShellBags();
            List<IEvent> events = EventParser.GetEvents(App.ShellItems);
            App.nodeCollection.ClearAllFilters();
            App.nodeCollection.nodeList.AddRange(NodeParser.GetNodes(events));
            stopwatch.Stop();
            logger.Info("Parsing Complete. ShellItems Parsed: " + App.ShellItems.Count + ". Time Elapsed: " + stopwatch.ElapsedMilliseconds / 1000 + " seconds");
            
            //Restore UI
            ParseButton.Content = "Parse";
            EnableUIElements(true);

            //Go to Timeline            
            Mouse.OverrideCursor = Cursors.Arrow;
            if(timelinePage == null)
            {
                timelinePage = new TimelinePage();
                NavigationService.Navigate(timelinePage);

            }
            else
            {
                timelinePage.RebuildTimeline();
                NavigationService.Navigate(timelinePage);
            }

            App.NavigationService = NavigationService;
            App.pages.Add("timelinepage", timelinePage);


        }

        private async Task<List<IShellItem>> ParseShellBags()
        {
            bool useRegistryHiveFiles = OfflineCheck.IsChecked.GetValueOrDefault(false);
            string osVersion = OSVersion.SelectedItem == null ? string.Empty : OSVersion.SelectedItem.ToString();
            //potentially long running operation, operate in another thread.
            return await Task.Run(() => 
            { 

                List<IShellItem> retList = new List<IShellItem>();

                ConfigParser parser = new ConfigParser(locations.GUIDFileLocation, locations.OSFileLocation, locations.ScriptFileLocation);

                //perform offline shellbag parsing
                if (useRegistryHiveFiles)
                {
                    parser.OsVersion = osVersion;
                    string[] registryFilePaths = locations.OfflineFileLocations;
                    foreach (string registryFile in registryFilePaths)
                    {
                        OfflineRegistryReader offlineReader = new OfflineRegistryReader(parser, registryFile);
                        retList.AddRange(ShellBagParser.GetShellItems(offlineReader));
                    }

                }
                else //perform online shellbag parsing
                {
                    OnlineRegistryReader onlineReader = new OnlineRegistryReader(parser);
                    retList.AddRange(ShellBagParser.GetShellItems(onlineReader));
                }

                return retList;
            });
        }

        private bool ConfigurationFilesAreValid()
        {
            string messageBoxTitle = "Invalid configuration selected";
            string question = "The currently selected {0} file is invalid or missing.\n" +
                              "Would you like to proceed anyway?";

            if (!ConfigParser.IsValidOsFile(locations.OSFileLocation))
            {
                MessageBoxResult result = AskYesNoQuestion(string.Format(question, "OS Configuration"), messageBoxTitle);
                if (result != MessageBoxResult.Yes)
                    return false;
            }
            if (!ConfigParser.IsValidGuidFile(locations.GUIDFileLocation))
            {
                MessageBoxResult result = AskYesNoQuestion(string.Format(question, "GUID Configuration"), messageBoxTitle);
                if (result != MessageBoxResult.Yes)
                    return false;
            }
            if (!ConfigParser.IsValidScriptFile(locations.ScriptFileLocation))
            {
                MessageBoxResult result = AskYesNoQuestion(string.Format(question, "Script Configuration"), messageBoxTitle);
                if (result != MessageBoxResult.Yes)
                    return false;

            }

            return true;
        }

        private bool OfflineSelectionsAreValid()
        {
            if (locations.OfflineFileLocations.Length == 0)
            {
                showErrorMessage("Select a registry hive file.", "Missing Hive");
                return false;
            }

            foreach (string location in locations.OfflineFileLocations)
            {
                if (!File.Exists(location))
                {
                    showErrorMessage(location + " is an invalid location.", "Invalid Hive");
                    return false;
                }
            }          

            if (OSVersion.SelectedItem is null)
            {
                showErrorMessage("Select what OS Version the offline hive is.", "No OS Version selected.");
                return false;
            }

            return true;
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new HelpPage());
        }

        private void OfflineChecked(object sender, EventArgs e)
        {
            ShowOfflineRows();
        }

        private void OfflineUnchecked(object sender, EventArgs e)
        {
            HideOfflineRows();
        }

        private void HideOfflineRows()
        {
            OfflineLocationRow.Height = hiddenRow;
            OSVersionRow.Height = hiddenRow;
        }

        private void ShowOfflineRows()
        {
            OfflineLocationRow.Height = visibleRow;
            OSVersionRow.Height = visibleRow;
        }

        private static void showErrorMessage(string message, string messageBoxTitle = "An Error Occurred")
        {
            logger.Warn(message);
            MessageBox.Show(message, messageBoxTitle, MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private static MessageBoxResult AskYesNoQuestion(string message, string messageBoxTitle = "SeeShells")
        {
            return MessageBox.Show(message, messageBoxTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
        }
        
        private void EnableUIElements(bool value)
        {
            ParseButton.IsEnabled = value;
            OSBrowseButton.IsEnabled = value;
            OSUpdateButton.IsEnabled = value;
            GUIDBrowseButton.IsEnabled = value;
            GUIDUpdateButton.IsEnabled = value;
            ScriptBrowseButton.IsEnabled = value;
            ScriptUpdateButton.IsEnabled = value;
            OfflineCheck.IsEnabled = value;
            OfflineBrowseButton.IsEnabled = value;
            OSVersion.IsEnabled = value;
            HelpButton.IsEnabled = value;
        }
    }
}
