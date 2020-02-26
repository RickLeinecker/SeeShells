using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using SeeShells.IO.Networking.JSON;
using SeeShells.UI.ViewModels;
using Microsoft.Win32;
using SeeShells.ShellParser.Scripting;

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
        /// Scripts are optional, so use this constructor when there is a selected scripts file.
        /// </summary>
        /// <param name="guidsFile"></param>
        /// <param name="OSRegistryFile"></param>
        /// <param name="scriptsFile"></param>
        public ConfigParser(string guidsFile, string OSRegistryFile, string scriptsFile)
        {
            UpdateKnownGUIDS(guidsFile);
            UpdateScripts(scriptsFile);
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

        private void UpdateScripts(string file)
        {
            IList<DecodedScriptPair> scriptPairs = JsonConvert.DeserializeObject<IList<DecodedScriptPair>>(File.ReadAllText(file));
            foreach(DecodedScriptPair pair in scriptPairs)
            {
                ScriptHandler.scripts[pair.getScript().Key] = pair.getScript().Value;   
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
