using SeeShells.ShellParser.ShellItems.ExtensionBlocks;
using System;
using System.Collections.Generic;

namespace SeeShells.ShellParser.ShellItems
{
    /// <summary>
    /// Delegate Shell Item
    /// </summary>
    public class ShellItem0x74 : ShellItemWithExtensions
    {

        protected ExtensionBlockBEEF0004 ExtensionBlock { get; }

        public uint Signature { get; protected set; }
        public FILEENTRY_FRAGMENT SubItem { get; protected set; }
        public string DelegateItemIdentifier { get; protected set; }
        public string ItemClassIdentifier { get; protected set; }
        public override string TypeName { get => "Delegate Item"; }
        public override string Name
        {
            get
            {
                if (ExtensionBlock.LongName.Length > 0)
                    return ExtensionBlock.LongName;
                return SubItem.short_name;
            }
        }

        public override DateTime ModifiedDate
        {
            get
            {
                return SubItem.ModifiedDate;
            }
        }
        public override DateTime CreationDate
        {
            get
            {
                if (ExtensionBlock != null)
                    return ExtensionBlock.CreationDate;
                return base.CreationDate;
            }
        }
        public override DateTime AccessedDate
        {
            get
            {
                if (ExtensionBlock != null)
                    return ExtensionBlock.AccessedDate;
                return base.AccessedDate;
            }
        }

        public ShellItem0x74(byte[] buf)
            : base(buf)
        {
            // Unknown - Empty ( 1 byte)
            // Unknown - size? - 2 bytes
            // CFSF - 4 bytes
            // sub shell item data size - 2 bytes
            Signature = unpack_dword(0x06);

            int off = 0x0A;
            SubItem = new FILEENTRY_FRAGMENT(buf, offset + off, this, 0x04);
            off += SubItem.Size;

            off += 2; // Empty extension block?

            // 5e591a74-df96-48d3-8d67-1733bcee28ba
            DelegateItemIdentifier = unpack_guid(off);
            off += 10;
            ItemClassIdentifier = unpack_guid(off);
            off += 10;
            ExtensionBlock = new ExtensionBlockBEEF0004(buf, offset + off);
            ExtensionBlocks.Add(ExtensionBlock);            
            
        }

        public override IDictionary<string, string> GetAllProperties()
        {
            var ret = base.GetAllProperties();
            ret.Add("Signature", Signature.ToString());
            ret.Add("DelegateItemIdentifier", DelegateItemIdentifier.ToString());
            ret.Add("ItemClassIdentifier", ItemClassIdentifier.ToString());
            return ret;
        }
    }
}