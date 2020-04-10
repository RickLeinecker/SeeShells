using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;

namespace SeeShells.ShellParser.Registry
{
    /// <summary>
    /// This class is used to read the Registry on a live/running Windows Operating system.
    /// Using the Win32 API it accesses the Registry and reads registry values. 
    /// </summary>
    public class OnlineRegistryReader : IRegistryReader
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        IConfigParser Parser { get; }

        public OnlineRegistryReader(IConfigParser parser)
        {
            Parser = parser;
        }


        public List<RegistryKeyWrapper> GetRegistryKeys()
        {
            List<RegistryKeyWrapper> retList = new List<RegistryKeyWrapper>();

            //get relevant online registry
            RegistryKey store = Microsoft.Win32.Registry.Users;

            List<RegistryKey> userStores = new List<RegistryKey>();
            foreach (string userStoreName in store.GetSubKeyNames())
            {
                userStores.Add(store.OpenSubKey(userStoreName));
            }

            //iterate over each user's registry
            foreach (RegistryKey userStore in userStores)
            {
                logger.Debug("NEW USER SID: " + userStore.Name);
                string userOfStore = FindOnlineUsername(userStore);

                foreach (string location in Parser.GetRegistryLocations())
                {
                    foreach (RegistryKeyWrapper keyWrapper in IterateRegistry(userStore.OpenSubKey(location), location, null))
                    {
                        if (userOfStore != string.Empty)
                        {
                            keyWrapper.RegistryUser = userOfStore;
                        }

                        retList.Add(keyWrapper);
                    }
                }
            }

            return retList;

        }

        private string FindOnlineUsername(RegistryKey userStore)
        {
            string retval = string.Empty;

            List<string> usernameLocations = Parser.GetUsernameLocations();

            Dictionary<string, int> likelyUsernames = new Dictionary<string, int>();
            foreach (string usernameLocation in usernameLocations)
            {
                if (userStore.OpenSubKey(usernameLocation) != null)
                {
                    //based on the values in '...\Explorer\Shell Folders' the [2] value in the string may not always be the username, but it does appear the most.
                    foreach (string value in userStore.OpenSubKey(usernameLocation).GetValueNames())
                    {
                        string valueData = (string)userStore.OpenSubKey(usernameLocation).GetValue(value);
                        string[] pathParts = valueData.Split('\\');
                        if (pathParts.Length > 2)
                        {
                            string username = pathParts[2]; //usually in the form of C:\Users\username
                            if (!likelyUsernames.ContainsKey(username))
                            {
                                likelyUsernames[username] = 1;
                            }
                            else
                            {
                                likelyUsernames[username]++;
                            }
                        }
                    }
                }
                else
                {
                    return retval;
                }
            }
            //most occurred value is probably the username.
            if (likelyUsernames.Count >= 1)
            {
                retval = likelyUsernames.OrderByDescending(pair => pair.Value).First().Key;
            }

            return retval;
        }

        /// <summary>
        /// Recursively iterates over the a registry key and its subkeys for enumerating all values of the keys and subkeys
        /// </summary>
        /// <param name="rk">The root registry key to start iterating over</param>
        /// <param name="subKey">the path of the first subkey under the root key</param>
        /// <param name="parent">The Parent Key of the Registry Key currently being iterated. Can be null</param>
        /// <returns></returns>
        static List<RegistryKeyWrapper> IterateRegistry(RegistryKey rk, string subKey, RegistryKeyWrapper parent)
        {
            List<RegistryKeyWrapper> retList = new List<RegistryKeyWrapper>();
            if (rk == null)
            {
                return retList;
            }

            string[] subKeys = rk.GetSubKeyNames();
            string[] values = rk.GetValueNames();

            logger.Trace("**" + subKey);

            foreach (string valueName in subKeys)
            {
                if (valueName.ToUpper() == "ASSOCIATIONS")
                {
                    continue;
                }

                string sk = getSubkeyString(subKey, valueName);
                logger.Trace("{0}", sk);
                RegistryKey rkNext;
                try
                {
                    rkNext = rk.OpenSubKey(valueName);
                }
                catch (System.Security.SecurityException ex)
                {
                    logger.Warn("ACCESS DENIED: " + ex.Message);
                    continue;
                }

                RegistryKeyWrapper rkNextWrapper = null;

                //shellbags only have their numerical identifer for the value name, not a shellbag otherwise
                bool isNumeric = int.TryParse(valueName, out _);
                if (isNumeric)
                {
                    try
                    {
                        byte[] byteVal = (byte[])rk.GetValue(valueName);
                        rkNextWrapper = new RegistryKeyWrapper(rkNext, byteVal, parent);
                        retList.Add(rkNextWrapper);
                    }
                    catch (OverrunBufferException ex)
                    {
                        logger.Warn("OverrunBufferException: " + valueName);
                    }
                    catch (Exception ex)
                    {
                        logger.Warn(valueName + '\n' + ex);
                    }
                }

                retList.AddRange(IterateRegistry(rkNext, sk, rkNextWrapper));
            }

            return retList;

        }

        static string getSubkeyString(string subKey, string addOn)
        {
            return string.Format("{0}{1}{2}", subKey, subKey.Length == 0 ? "" : @"\", addOn);
        }
    }
}
