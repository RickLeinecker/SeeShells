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

                foreach (string location in Parser.GetRegistryLocations())
                {
                    foreach (RegistryKeyWrapper keyWrapper in IterateRegistry(userStore.OpenSubKey(location), location, null))
                    {
                        retList.Add(keyWrapper);
                    }
                }
            }

            return retList;

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
