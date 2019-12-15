using Microsoft.Win32;
using System;
using System.Collections.Generic;

namespace SeeShells.ShellParser
{
    /// <summary>
    /// This class is used to read the Registry on a live/running Windows Operating system.
    /// Using the Win32 API it accesses the Registry and reads registry values. 
    /// </summary>
    public class OnlineRegistryReader : IRegistryReader
    {
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
                Console.WriteLine("NEW USER SID: " + userStore.Name);

                foreach (string location in Parser.GetLocations())
                {
                    foreach (byte[] keyValue in IterateRegistry(userStore.OpenSubKey(location), location, 0, ""))
                    {
                        retList.Add(new RegistryKeyWrapper(keyValue));
                    }
                }
                //todo: this is the ideal spot for labeling information by each user if needed.
                //possible separation of RegistryKeyWrappers by user is also possible above.
            }

            return retList;

        }

        /// <summary>
        /// Recursively iterates over the a registry key and its subkeys for enumerating all values of the keys and subkeys
        /// </summary>
        /// <param name="rk">The root registry key to start iterating over</param>
        /// <param name="subKey">the path of the first subkey under the root key</param>
        /// <param name="indent"></param>
        /// <param name="path_prefix">the header to the current root key, needed for identification of the registry store</param>
        /// <returns></returns>
        static List<byte[]> IterateRegistry(RegistryKey rk, string subKey, int indent, string path_prefix)
        {
            List<byte[]> retList = new List<byte[]>();
            if (rk == null)
            {
                return retList;
            }

            string[] subKeys = rk.GetSubKeyNames();
            string[] values = rk.GetValueNames();

            Console.WriteLine("**" + subKey);

            foreach (string valueName in subKeys)
            {
                if (valueName.ToUpper() == "ASSOCIATIONS")
                {
                    continue;
                }

                string sk = getSubkeyString(subKey, valueName);
                Console.WriteLine("{0}", sk);
                RegistryKey rkNext;
                try
                {
                    rkNext = rk.OpenSubKey(valueName);
                }
                catch (System.Security.SecurityException ex)
                {
                    Console.WriteLine("ACCESS DENIED: " + ex.Message);
                    continue;
                }
                int slot = 0;
                DateTime slotModified = DateTime.MinValue;
                string slotKeyName = "";
                try
                {
                    slot = (int)rk.GetValue("NodeSlot");
                    slotKeyName = string.Format("{0}{1}\\{2}", rk.Name.Substring(0, rk.Name.IndexOf("BagMRU")), "Bags", slot);
                    if (rk.Name.StartsWith("HKEY_USERS"))
                    {
                        slotModified = RegistryHelper.GetDateModified(RegistryHive.Users, slotKeyName.Replace("HKEY_USERS\\", "")) ?? DateTime.MinValue;
                    }
                    else if (rkNext.Name.StartsWith("HKEY_CURRENT_USER"))
                    {
                        slotModified = RegistryHelper.GetDateModified(RegistryHive.CurrentUser, slotKeyName.Replace("HKEY_CURRENT_USER\\", "")) ?? DateTime.MinValue;
                    }
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("NodeSlot was not found");
                }

                int intVal = 0;
                string path = path_prefix;
                bool isNumeric = int.TryParse(valueName, out intVal);
                if (isNumeric)
                {
                    try
                    {
                        byte[] byteVal = (byte[])rk.GetValue(valueName);
                        retList.Add(byteVal);
                    }
                    catch (OverrunBufferException ex)
                    {
                        Console.WriteLine("OverrunBufferException: " + valueName);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(valueName);
                    }
                }

                retList.AddRange(IterateRegistry(rkNext, sk, indent + 2, path));
            }

            return retList;

        }

        static string getSubkeyString(string subKey, string addOn)
        {
            return string.Format("{0}{1}{2}", subKey, subKey.Length == 0 ? "" : @"\", addOn);
        }
    }
}
