namespace SeeShells.ShellParser.ShellItems
{
    /// <summary>
    /// Supposedly corresponds to ShellItem 0x23 (as originally given)- but 
    /// a conflicting signature appears here: https://github.com/libyal/libfwsi/blob/master/documentation/Windows%20Shell%20Item%20format.asciidoc#33-volume-shell-item
    /// </summary>
    public class SHITEM_UNKNOWNENTRY2 : ShellItem
    {
        public string guid { get; protected set; }
        public byte flags { get; protected set; }
        public override string Name
        {
            get
            {
                if (KnownGuids.dict.ContainsKey(guid))
                {
                    return string.Format("{{{0}}}", KnownGuids.dict[guid]);
                }
                else
                {
                    return string.Format("{{{0}}}", guid);
                }
            }
        }
        public SHITEM_UNKNOWNENTRY2(byte[] buf, int offset, object parent)
            : base(buf, offset)
        {
            flags = unpack_byte(0x03);
            guid = unpack_guid(0x04);
        }

    }
}