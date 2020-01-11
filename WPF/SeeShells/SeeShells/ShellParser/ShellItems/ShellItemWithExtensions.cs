using SeeShells.ShellParser.ShellItems.ExtensionBlocks;
using System;
using System.Collections.Generic;

namespace SeeShells.ShellParser.ShellItems
{
    /// <summary>
    /// Indicates that this Shell Type can have 0 or more <see cref="IExtensionBlock"/> types included.
    /// </summary>
    public class ShellItemWithExtensions : ShellItem
    {
        public List<IExtensionBlock> ExtensionBlocks { get; private set; }
        
        public ShellItemWithExtensions(byte[] buf) : base(buf)
        {
            ExtensionBlocks = new List<IExtensionBlock>();
        }

        public override IDictionary<string, string> GetAllProperties()
        {
            var ret =  base.GetAllProperties();
            foreach (IExtensionBlock block in ExtensionBlocks)
            {
                var props = block.GetAllProperties();
                //TODO how do we populate Extension Blocks in addition to the shell Item Key-Value Pairs?
            }
            ret.Add("ExtensionBlockCount", ExtensionBlocks.Count.ToString()); //TODO REMOVE ME WHEN THE ABOVE IS ANSWERED.
            return ret;
        }
    }
}