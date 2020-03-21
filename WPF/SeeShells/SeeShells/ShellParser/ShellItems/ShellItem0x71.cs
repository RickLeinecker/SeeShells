using System.Collections.Generic;

namespace SeeShells.ShellParser.ShellItems
{
    /// <summary>
    /// Control Panel Shell Item, with a constant GUID for identification.
    /// </summary>
    public class ShellItem0x71 : MockShellItem
    {
        public string Guid { get; protected set; }
        public byte Flags { get; protected set; }
        public override string TypeName { get => "Control Panel";}
        public override string Name
        {
            get
            {
                if (KnownGuids.dict.ContainsKey(Guid))
                {
                    return string.Format("{{CONTROL PANEL: {0}}}", KnownGuids.dict[Guid]);
                }
                else
                {
                    return string.Format("{{CONTROL PANEL: {0}}}", Guid);
                }
            }
        }
        public ShellItem0x71(byte[] buf)
            : base(buf)
        {
            Guid = unpack_guid(0x0E);
            Flags = unpack_byte(0x03);
        }
        public override IDictionary<string, string> GetAllProperties()
        {
            var ret = base.GetAllProperties();
            AddPairIfNotNull(ret, "Guid", Guid);
            AddPairIfNotNull(ret, "Flags", Flags);
            return ret;
        }
    }
}