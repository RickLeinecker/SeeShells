using System;
using System.Collections.Generic;
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
        private String RegistryFilePath;

        public OfflineRegistryReader(IConfigParser parser, String registryFilePath)
        {
            Parser = parser;
            RegistryFilePath = registryFilePath;
        }

        public List<RegistryKeyWrapper> GetRegistryKeys()
        {
            List<RegistryKeyWrapper> retList = new List<RegistryKeyWrapper>();

            foreach (string location in Parser.GetLocations())
            {
                var hive = new RegistryHiveOnDemand(RegistryFilePath);

                foreach (byte[] keyValue in IterateRegistry(hive.GetKey(location), hive, location, 0, ""))
                {
                    retList.Add(new RegistryKeyWrapper(keyValue));
                }
            }

            return retList;
        }

        /// <summary>
        /// Recursively iterates over the a registry key and its subkeys for enumerating all values of the keys and subkeys
        /// </summary>
        /// <param name="rk">the root registry key to start iterating over</param>
        /// <param name="hive">the offline registry hive</param>
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

                foreach (RegistryKey valueName in rk.SubKeys)
                {
                    if (valueName.KeyName.ToUpper() == "ASSOCIATIONS")
                    {
                        continue;
                    }

                string sk = getSubkeyString(subKey, valueName.KeyName);
                    Console.WriteLine("{0}", sk);
                    RegistryKey rkNext;
                    try
                    {
                        rkNext = hive.GetKey(getSubkeyString(rk.KeyPath, valueName.KeyName));
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
                    bool isNumeric = int.TryParse(valueName.KeyName, out intVal);
                    if (isNumeric)
                    {
                        try
                        {
                        var k = hive.GetKey(valueName.KeyPath);
                        foreach (KeyValue keyValue in k.Values)
                        {
                            byte[] byteVal = keyValue.ValueDataRaw;
                            retList.Add(byteVal);
                        }
                        }

                        catch (OverrunBufferException ex)
                        {
                            Console.WriteLine("OverrunBufferException: " + valueName.KeyName);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(valueName.KeyName);
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
