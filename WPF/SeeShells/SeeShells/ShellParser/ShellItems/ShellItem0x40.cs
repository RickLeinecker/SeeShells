using System.Collections.Generic;

namespace SeeShells.ShellParser.ShellItems
{
    /// <summary>
    /// Network Location Shell Item
    /// </summary>
    /// https://github.com/libyal/libfwsi/blob/master/documentation/Windows%20Shell%20Item%20format.asciidoc#35-network-location-shell-item
    public class ShellItem0x40 : MockShellItem
    {
        public byte Flags { get; protected set; }
        public string Location { get; protected set; }
        public string Description { get; protected set; }
        public string Comments { get; protected set; }
        public override string TypeName { get => "Network Location"; }
        public override string Name => Location;
        public ShellItem0x40(byte[] buf) : base(buf)
        {
            Flags = unpack_byte(0x04);
            Location = unpack_string(0x05);
            int off = 0x05;
            off += Name.Length + 1;
            if ((Flags & 0x80) != 0)
            {
                Description = unpack_string(off);
                off += Description.Length + 1;
            }
            if ((Flags & 0x40) != 0)
            {
                Comments = unpack_string(off);
            }
        }

        public override IDictionary<string, string> GetAllProperties()
        {
            var ret =  base.GetAllProperties();
            AddPairIfNotNull(ret, "Flags", Flags);
            AddPairIfNotNull(ret, "Location", Location);
            AddPairIfNotNull(ret, "Description", Description);
            AddPairIfNotNull(ret, "Comments", Comments);
            return ret;
        }
    }
}