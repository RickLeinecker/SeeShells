using Microsoft.Win32;
using Newtonsoft.Json;
using SeeShells.IO.Networking;
using SeeShells.IO.Networking.JSON;
using SeeShells.ShellParser;
using SeeShells.ShellParser.ShellItems;
using SeeShells.UI.Node;
using SeeShells.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace SeeShells.UI.Pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        private readonly FileLocations locations;
        private FileLocations defaultLocations;
        private GridLength visibleRow = new GridLength(2, GridUnitType.Star);
        private GridLength hiddenRow = new GridLength(0);

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public Home()
        {
            InitializeComponent();

            string currentDirectory = Directory.GetCurrentDirectory();
            locations = new FileLocations(
                os: currentDirectory + @"\OS.json",
                guid: currentDirectory + @"\GUID.json",
                script: currentDirectory + @"\Scripts.json"
            );


            this.DataContext = locations;
            UpdateOSVersionList();
            HideOfflineRows();
        }
        private void defaultSettings()
        {

            string defaultdir = Directory.GetCurrentDirectory();
            string supporter = Path.GetFullPath(Path.Combine(defaultdir, @"..\..\..\"));

            defaultLocations = new FileLocations(
                os: supporter + @"\SeeShells\IO\Networking\JSON\defaultOS.json",
                guid: supporter + @"\SeeShells\IO\Networking\JSON\defaultGUID.json",
                script: supporter + @"\SeeShells\IO\Networking\JSON\Scripts.json"
                );
            this.DataContext = defaultLocations;
        }

        private void OfflineBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Registry files (*.reg)|*.reg|Dat files (*.dat)|*.dat|All files|*.*",
                InitialDirectory = Directory.GetCurrentDirectory()
            };

            if (openFileDialog.ShowDialog() != true)
                return;

            locations.OfflineFileLocation = openFileDialog.FileName;
        }

        private void UpdateOSVersionList()
        {
            OSVersion.SelectedIndex = -1;
            OSVersion.Items.Clear();
            OSVersion.Items.Add("Generic Windows");

            if (!File.Exists(locations.OSFileLocation))
                return;

            string json = File.ReadAllText(locations.OSFileLocation);
            try
            {
                IList<RegistryLocations> registryLocations = JsonConvert.DeserializeObject<IList<RegistryLocations>>(json);
                foreach (RegistryLocations location in registryLocations)
                {
                    if (!OSVersion.Items.Contains(location.OperatingSystem))
                        OSVersion.Items.Add(location.OperatingSystem);
                }
            }
            catch (JsonSerializationException)
            {
                showErrorMessage("The OS file you selected is not formatted properly.", "Incorrect OS Configuration File Format");
            }
        }

        private void OSBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            string location = GetFileFromBrowsing();
            if (UserSelectedFile(location))
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

        }

        private void GUIDBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            string location = GetFileFromBrowsing();
            if (UserSelectedFile(location))
            {
                locations.GUIDFileLocation = location;
            }
        }

        private async void GUIUpdateButton_Click(object sender, RoutedEventArgs e)
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

        private void ScriptUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Scripting is still a work in progress.", "Check back later.", MessageBoxButton.OK, MessageBoxImage.Information);
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

                if (answer == MessageBoxResult.Yes)
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
            ConfigurationFilesAreValidAsync();


            if (OfflineCheck.IsChecked == true)
                if (!OfflineSelectionsAreValid())
                    return;

            //cover UI
            Mouse.OverrideCursor = Cursors.Wait;
            //TODO spinner over screen to show operation in progress?
            ParseButton.Content = "Parsing...";
            ParseButton.IsEnabled = false;

            //begin the parsing process
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            App.ShellItems = await ParseShellBags();
            List<IEvent> events = EventParser.GetEvents(App.ShellItems);
            App.nodeCollection.nodeList = new List<Node.Node>(); //TODO REMOVE ME once merged with filtering code, unneccesarly NPE stopping
            App.nodeCollection.nodeList.AddRange(NodeParser.GetNodes(events));
            stopwatch.Stop();
            logger.Info("Parsing Complete. ShellItems Parsed: " + App.ShellItems.Count + ". Time Elapsed: " + stopwatch.ElapsedMilliseconds / 1000 + " seconds");
            //Restore UI
            ParseButton.Content = "Parse";
            ParseButton.IsEnabled = true;

            //Go to Timeline            
            Mouse.OverrideCursor = Cursors.Arrow;
            NavigationService.Navigate(new TimelinePage());

        }

        private async Task<List<IShellItem>> ParseShellBags()
        {
            bool useRegistryHiveFiles = OfflineCheck.IsChecked.GetValueOrDefault(false);
            string osVersion = OSVersion.SelectedItem == null ? string.Empty : OSVersion.SelectedItem.ToString();
            ConfigParser parser = new ConfigParser(defaultLocations.GUIDFileLocation, defaultLocations.OSFileLocation);
            //potentially long running operation, operate in another thread.
            return await Task.Run(() =>
            {

                List<IShellItem> retList = new List<IShellItem>();
                if (File.Exists(locations.GUIDFileLocation))
                    parser = new ConfigParser(locations.GUIDFileLocation, locations.OSFileLocation);

                //perform offline shellbag parsing
                if (useRegistryHiveFiles)
                {
                    parser.OsVersion = osVersion;
                    List<string> registryFilePaths = new List<string>() { locations.OfflineFileLocation };
                    //TODO handle multiple offline registry files (locations only serves one so far)
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

        private async void ConfigurationFilesAreValidAsync()
        {
            defaultSettings();
            if (!File.Exists(locations.OSFileLocation))
            {
                string defaultOs = defaultLocations.OSFileLocation;
                Mouse.OverrideCursor = Cursors.Wait;
                await Task.Run(() => API.GetOSRegistryLocations(defaultOs));

            }
            if (!File.Exists(locations.GUIDFileLocation))
            {
                string defaultGuid = defaultLocations.GUIDFileLocation;
                Mouse.OverrideCursor = Cursors.Wait;
                await Task.Run(() => API.GetGuids(defaultGuid));
            }
            if (!File.Exists(locations.ScriptFileLocation))
            {
                string defaultScript = defaultLocations.ScriptFileLocation;
            }

        }

        private bool OfflineSelectionsAreValid()
        {
            if (!File.Exists(locations.OfflineFileLocation))
            {
                showErrorMessage("Select a registry hive file.", "Missing Hive");
                return false;
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

        private void showErrorMessage(string message, string messageBoxTitle = "An Error Occurred")
        {
            logger.Warn(message);
            MessageBox.Show(message, messageBoxTitle, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
