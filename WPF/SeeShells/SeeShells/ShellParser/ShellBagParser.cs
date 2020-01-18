using SeeShells.ShellParser.ShellItems;
using System;
using System.Collections.Generic;

namespace SeeShells.ShellParser
{
    /// <summary>
    /// This class is used to obtain and identify ShellBag items. 
    /// </summary>
    public class ShellBagParser
    {
        IRegistryReader registryReader;
        public ShellBagParser(IRegistryReader registryReader)
        {
            this.registryReader = registryReader;
        }

        /// <summary>
        /// Identifies and gathers ShellBag items from raw binary registry data.
        /// </summary>
        /// <returns>a list of different variety ShellBag items</returns>
        public List<IShellItem> GetShellItems()
        {
            List<IShellItem> shellItems = new List<IShellItem>();
            foreach (RegistryKeyWrapper keyWrapper in registryReader.GetRegistryKeys())
            {
                if(keyWrapper.getValue() != null) // Some Registry Keys are null
                {
                    ShellItemList shellItemList = new ShellItemList(keyWrapper.getValue());
                    foreach (IShellItem shellItem in shellItemList.Items())
                    {
                        shellItems.Add(shellItem);
                    }
                }
            }
            return shellItems;
        }
    }
}
