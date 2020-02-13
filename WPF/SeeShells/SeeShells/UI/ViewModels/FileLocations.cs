using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShells.UI.ViewModels
{
    public class FileLocations : INotifyPropertyChanged
    {
        private string osLocation = string.Empty;
        private string guidLocation = string.Empty;
        private string scriptLocation = string.Empty;
        private string offlineLocation = string.Empty;

        public event PropertyChangedEventHandler PropertyChanged;

        public string OSFileLocation
        {
            get { return osLocation; }
            set
            {
                if (osLocation != value)
                {
                    osLocation = value;
                    OnPropertyChanged("OSFileLocation");
                }
            }
        }

        public string GUIDFileLocation
        {
            get { return guidLocation; }
            set
            {
                if (guidLocation != value)
                {
                    guidLocation = value;
                    OnPropertyChanged("GUIDFileLocation");
                }
            }
        }

        public string ScriptFileLocation
        {
            get { return scriptLocation; }
            set
            {
                if (scriptLocation != value)
                {
                    scriptLocation = value;
                    OnPropertyChanged("ScriptFileLocation");
                }
            }
        }

        public string OfflineFileLocation
        {
            get { return offlineLocation; }
            set
            {
                if (offlineLocation != value)
                {
                    offlineLocation = value;
                    OnPropertyChanged("OfflineFileLocation");
                }
            }
        }

        public FileLocations(string os, string guid, string script)
        {
            OSFileLocation = os;
            guidLocation = guid;
            scriptLocation = script;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
