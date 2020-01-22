using System.Collections.Generic;

namespace SeeShells.ShellParser.ShellItems
{
    /// <summary>
    /// Supposedly corresponds to ShellItem 0x23 (as originally given)- but 
    /// a conflicting signature appears here: https://github.com/libyal/libfwsi/blob/master/documentation/Windows%20Shell%20Item%20format.asciidoc#33-volume-shell-item
    /// </summary>
    public class SHITEM_UNKNOWNENTRY2 : ShellItem
    {
        public string Guid { get; protected set; }
        public byte Flags { get; protected set; }
        public override string Name
        {
            get
            {
                if (KnownGuids.dict.ContainsKey(Guid))
                {
                    return string.Format("{{{0}}}", KnownGuids.dict[Guid]);
                }
                else
                {
                    return string.Format("{{{0}}}", Guid);
                }
            }
        }
        public SHITEM_UNKNOWNENTRY2(byte[] buf, int offset, object parent)
            : base(buf, offset)
        {
            Flags = unpack_byte(0x03);
            Guid = unpack_guid(0x04);
        }

        public override IDictionary<string, string> GetAllProperties()
        {
            
            var ret = base.GetAllProperties();
            AddPairIfNotNull(ret, "Flags", Flags);
            AddPairIfNotNull(ret, "Guid", Guid);
            return ret;
        }

    }
}