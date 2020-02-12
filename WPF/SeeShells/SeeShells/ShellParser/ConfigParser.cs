using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using SeeShells.IO.Networking.JSON;
using SeeShells.UI.ViewModels;
using Microsoft.Win32;
namespace SeeShells.ShellParser
{
    class ConfigParser : IConfigParser
    {
        private readonly string OSRegistryFile;

        public string OsVersion {private get; set;}

        public ConfigParser(string guidsFile, string OSRegistryFile)
        {
            UpdateKnownGUIDS(guidsFile);
            OsVersion = getLiveOSVersion();
            this.OSRegistryFile = OSRegistryFile;
        }


        /// <summary>
        /// Overrides (or Adds) existing GUID entries into <see cref="KnownGuids.dict"/>
        /// </summary>
        /// <param name="guidsFile">A JSON file that contains <see cref="GUIDPair"/></param>
        private void UpdateKnownGUIDS(string guidsFile)
        {
            IList<GUIDPair> guidPairs = JsonConvert.DeserializeObject<IList<GUIDPair>>(File.ReadAllText(guidsFile));
            foreach(GUIDPair pair in guidPairs)
            {   
                KnownGuids.dict[pair.getKnownGUID().Key] = pair.getKnownGUID().Value;
            }

        }


        public List<string> GetRegistryLocations()
        {
            List<string> locations = new List<string>();

            IList<RegistryLocations> registryLocations = JsonConvert.DeserializeObject<IList<RegistryLocations>>(File.ReadAllText(OSRegistryFile));
            foreach (RegistryLocations regLocation in registryLocations)
            {
                if (OsVersion.Contains(regLocation.OperatingSystem))
                {
                    foreach (IList<string> registryPaths in regLocation.GetRegistryFilePaths().Values)
                    {
                        locations.AddRange(registryPaths);
                    }
                    return locations;
                }
            }

            return locations;
        }
        
        private string getLiveOSVersion()
        {
            RegistryKey registryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion");
            return (string)registryKey.GetValue("productName");   
        }

    }
}
