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
        protected IShellItem BaseShellItem { get; }
        protected RegistryKeyWrapper RegKey { get; }

        /// <summary>
        /// Wraps a IShellItem with the relevant Registry Metadata from which it was produced.
        /// Adds registry related properties to the <seealso cref="IShellItem.GetAllProperties"/> method return
        /// </summary>
        /// <param name="shellItem">The shellitem to be encapsulated. Cannot be null</param>
        /// <param name="regKey">The Wrapper containing the information to be added to the <see cref="IShellItem"/> Can't be Null.</param>
        public RegistryShellItemDecorator(IShellItem shellItem, RegistryKeyWrapper regKey)
        {
            BaseShellItem = shellItem ?? throw new ArgumentNullException(nameof(shellItem));
            RegKey = regKey ?? throw new ArgumentNullException(nameof(regKey));
        }

        public ushort Size => BaseShellItem.Size;
        public byte Type => BaseShellItem.Type;
        public string TypeName => BaseShellItem.TypeName;
        public string Name => BaseShellItem.Name;
        public DateTime ModifiedDate => BaseShellItem.ModifiedDate;
        public DateTime AccessedDate => BaseShellItem.AccessedDate;
        public DateTime CreationDate => BaseShellItem.CreationDate;

        public IDictionary<string, string> GetAllProperties()
        {
            IDictionary<string, string> baseDict = BaseShellItem.GetAllProperties();
            
            //add all registry key values
            if (RegKey.RegistryUser != string.Empty)
                baseDict.Add("RegistryOwner", RegKey.RegistryUser);
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
    }
}