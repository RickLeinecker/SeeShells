using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Registry;
using Registry.Abstractions;

namespace SeeShells.ShellParser
{
    /// <summary>
    /// This class is used to read an offline regisry hive file.
    /// Using the Registry API, from EricZimmerman, it reads registry values from an hive file. 
    /// </summary>
    public class OfflineRegistryReader : IRegistryReader
    {
        IConfigParser Parser { get; }

        public OfflineRegistryReader(IConfigParser parser)
        {
            Parser = parser;
        }

        public List<RegistryKeyWrapper> GetRegistryKeys()
        {
            List<RegistryKeyWrapper> retList = new List<RegistryKeyWrapper>();

            foreach (string location in Parser.GetLocations())
            {
                // Location of Offline Hive (change to local dir)
                var hive = new RegistryHiveOnDemand(@"C:\Users\Aleksandar\Desktop\CurrUser");

                foreach (byte[] keyValue in IterateRegistry(hive.GetKey(@"ROOT\" + location), hive, location, 0, ""))
                {
                    retList.Add(new RegistryKeyWrapper(keyValue));
                }
            }

            return retList;
        }

        /// <summary>
        /// Recursively iterates over the a registry key and its subkeys for enumerating all values of the keys and subkeys
        /// </summary>
        /// <param name="rk">The root registry key to start iterating over</param>
        /// <param name="hive">The offline registry hive</param>
        /// <param name="subKey">the path of the first subkey under the root key</param>
        /// <param name="indent"></param>
        /// <param name="path_prefix">the header to the current root key, needed for identification of the registry store</param>
        /// <returns></returns>
        static List<byte[]> IterateRegistry(RegistryKey rk, RegistryHiveOnDemand hive, string subKey, int indent, string path_prefix)
            {
                List<byte[]> retList = new List<byte[]>();
                if (rk == null)
                {
                    return retList;
                }

                foreach (KeyValue keyValue in rk.Values)
                {
                    if (keyValue.ValueName.ToUpper() == "ASSOCIATIONS")
                    {
                        continue;
                    }

                string sk = getSubkeyString(subKey, keyValue.ValueName);
                    Console.WriteLine("{0}", sk);
                    RegistryKey rkNext;
                    try
                    {
                        rkNext = hive.GetKey(getSubkeyString(rk.KeyPath, keyValue.ValueName));
                    }
                    catch (System.Security.SecurityException ex)
                    {
                        Console.WriteLine("ACCESS DENIED: " + ex.Message);
                        continue;
                    }
                /// The offline parser cannot GetValue() for any given gavlue, instead it has to get the data directly from a key value as it iterates over it
                /// This code needs to be reworked
/*                    int slot = 0;
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
                    }*/

                    int intVal = 0;
                    string path = path_prefix;
                    bool isNumeric = int.TryParse(keyValue.ValueName, out intVal);
                    if (isNumeric)
                    {
                        try
                        {
                            byte[] byteVal = (byte[])keyValue.ValueDataRaw;
                            retList.Add(byteVal);
                        }

                        catch (OverrunBufferException ex)
                        {
                            Console.WriteLine("OverrunBufferException: " + keyValue.ValueName);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(keyValue.ValueName);
                        }
                    }

                    retList.AddRange(IterateRegistry(rkNext, hive, sk, indent + 2, path));
                }

                return retList;

            }

        static string getSubkeyString(string subKey, string addOn)
        {
            return string.Format("{0}{1}{2}", subKey, subKey.Length == 0 ? "" : @"\", addOn);
        }
    }
}
