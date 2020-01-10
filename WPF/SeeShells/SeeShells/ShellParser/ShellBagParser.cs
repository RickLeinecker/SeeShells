using SeeShells.ShellParser.ShellItems;
using System.Collections.Generic;

namespace SeeShells.ShellParser
{
    /// <summary>
    /// This class is used to obtain and identify ShellBag items. 
    /// </summary>
    class ShellBagParser
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
            // TODO: actual logic
            return new List<IShellItem>();
        }
    }
}
