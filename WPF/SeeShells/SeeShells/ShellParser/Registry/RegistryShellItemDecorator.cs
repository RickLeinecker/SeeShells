using System;
using System.Collections.Generic;
using SeeShells.ShellParser.ShellItems;

namespace SeeShells.ShellParser.Registry
{
    /// <summary>
    /// Wraps a IShellItem with the relevant Registry Metadata from which it was produced.
    /// Adds registry related properties to the <seealso cref="IShellItem.GetAllProperties"/> method return
    /// </summary>
    public class RegistryShellItemDecorator : IShellItem
    {
        private const string AbsolutePathIdentifier = "AbsolutePath";
        protected IShellItem BaseShellItem { get; }
        protected RegistryKeyWrapper RegKey { get; }

        /// <summary>
        /// Wraps a IShellItem with the relevant Registry Metadata from which it was produced.
        /// Adds registry related properties to the <seealso cref="IShellItem.GetAllProperties"/> method return
        /// </summary>
        /// <param name="shellItem">The Shellitem to be encapsulated. Cannot be null</param>
        /// <param name="regKey">The Wrapper containing the information to be added to the <see cref="IShellItem"/> Can't be Null.</param>
        /// <param name="parentShellItem">The Shellitem which when hierarchically organized, contains the Shellitem. Can be null </param>
        public RegistryShellItemDecorator(IShellItem shellItem, RegistryKeyWrapper regKey, IShellItem parentShellItem = null)
        {
            BaseShellItem = shellItem ?? throw new ArgumentNullException(nameof(shellItem));
            RegKey = regKey ?? throw new ArgumentNullException(nameof(regKey));
            AbsolutePath = SetAbsolutePath(parentShellItem);
        }

        public ushort Size => BaseShellItem.Size;
        public byte Type => BaseShellItem.Type;
        public string TypeName => BaseShellItem.TypeName;
        public string Name => BaseShellItem.Name;
        public DateTime ModifiedDate => BaseShellItem.ModifiedDate;
        public DateTime AccessedDate => BaseShellItem.AccessedDate;
        public DateTime CreationDate => BaseShellItem.CreationDate;

        //RegistryDecorator Specific properties
        public string AbsolutePath { get; }


        public IDictionary<string, string> GetAllProperties()
        {
            IDictionary<string, string> baseDict = BaseShellItem.GetAllProperties();
            
            baseDict.Add(AbsolutePathIdentifier, AbsolutePath);

            //add all registry key values
            if (RegKey.RegistryUser != string.Empty)
                baseDict.Add("RegistryOwner", RegKey.RegistryUser);
            if (RegKey.RegistryUser != string.Empty)
                baseDict.Add("RegistrySID", RegKey.RegistrySID);
            if (RegKey.RegistryPath != string.Empty)
                baseDict.Add("RegistryPath", RegKey.RegistryPath);
            if (RegKey.ShellbagPath != string.Empty)
                baseDict.Add("ShellbagPath", RegKey.ShellbagPath);
            if (RegKey.LastRegistryWriteDate != DateTime.MinValue)
                baseDict.Add("LastRegistryWriteDate", RegKey.LastRegistryWriteDate.ToString());
            if (RegKey.SlotModifiedDate != DateTime.MinValue)
                baseDict.Add("SlotModifiedDate", RegKey.SlotModifiedDate.ToString());


            return baseDict;
        }

        protected string SetAbsolutePath(IShellItem parentShellItem)
        {
            if (parentShellItem == null) 
                return Name;
            
            IDictionary<string, string> parentProperties = parentShellItem.GetAllProperties();
            if (parentProperties.TryGetValue(AbsolutePathIdentifier, out string parentPath))
            {
                //replace instances of \\\ because thats cascading, \\ can exist in network paths and \ is normal.
                return $@"{parentPath}\{Name}".Replace("\\\\\\", "\\");
            }

            return Name;
        }
    }
}