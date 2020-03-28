
using System;
using Microsoft.Win32;
using NLog;

namespace SeeShells.ShellParser.Registry
{
    public class RegistryKeyWrapper
    {

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public string RegistryUser { get; internal set; }
        public string RegistryPath { get; internal set; }
        public byte[] Value { get; }
        public DateTime SlotModifiedDate { get; internal set; }
        public DateTime LastRegistryWriteDate { get; internal set; }
        public string ShellbagPath { get; internal set; }
        public RegistryKeyWrapper Parent { get; }

        public RegistryKeyWrapper(byte[] value)
        {
            this.Value = value;
            RegistryUser = string.Empty;
            RegistryPath = string.Empty;
        }

        public RegistryKeyWrapper(byte[] value, string registryUser, string registryPath) : this(value)
        {
            this.RegistryUser = registryUser;
            this.RegistryPath = registryPath;
        }

        /// <summary>
        /// Adapts a ShellBag RegistryKey to a common standard for retrieval of important information independent of key retrieval methodologies
        /// </summary>
        /// <param name="registryKey">A Registry Key associated with a Shellbag, retrieved via Win32 API </param>
        /// <param name="keyValue">The Value of a Registry key containing Shellbag information. Found in the Parent of the registryKey being inspected</param>
        /// <param name="parent">The parent of the currently inspected registryKey. Can be null.</param>
        public RegistryKeyWrapper(Microsoft.Win32.RegistryKey registryKey, byte[] keyValue, RegistryKeyWrapper parent = null) : this(keyValue)
        {
            Parent = parent;
            RegistryPath = registryKey.Name;
            AdaptWin32Key(registryKey);
        }

        /// <summary>
        /// Adapts a ShellBag RegistryKey to a common standard for retrieval of important information independent of key retrieval methodologies
        /// </summary>
        /// <param name="registryKey">A Registry Key associated with a Shellbag, retrieved from a offline registry reader API</param>
        /// <param name="keyValue">The Value of a Registry key containing Shellbag information. Found in the Parent of the registryKey being inspected</param>
        /// <param name="parent">The parent of the currently inspected registryKey. Can be null.</param>
        public RegistryKeyWrapper(global::Registry.Abstractions.RegistryKey registryKey, byte[] keyValue, RegistryKeyWrapper parent = null) : this(keyValue)
        {
            //TODO adapt offline hive properties 
        }

        private void AdaptWin32Key(Microsoft.Win32.RegistryKey registryKey)
        {

            //obtain NodeSlot (Shellbag Path in registry)
            SlotModifiedDate = DateTime.MinValue;
            ShellbagPath = string.Empty;
            try
            {
                int slot = (int)registryKey.GetValue("NodeSlot");
                ShellbagPath = string.Format("{0}{1}\\{2}", registryKey.Name.Substring(0, registryKey.Name.IndexOf("BagMRU", StringComparison.Ordinal)), "Bags", slot);
                
                if (registryKey.Name.StartsWith("HKEY_USERS"))
                {
                    SlotModifiedDate = RegistryHelper.GetDateModified(RegistryHive.Users, ShellbagPath.Replace("HKEY_USERS\\", "")) ?? DateTime.MinValue;
                }
            }
            catch (Exception ex)
            {
                logger.Info(ex, $"NodeSlot was not found for registry key at {RegistryPath}");
            }

            //obtain the date the registry last wrote this key
            LastRegistryWriteDate = RegistryHelper.GetDateModified(RegistryHive.Users, registryKey.Name.Replace("HKEY_USERS\\", "")) ?? DateTime.MinValue;

        }
    }
}
