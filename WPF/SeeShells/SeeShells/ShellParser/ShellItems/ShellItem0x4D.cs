using System.Collections.Generic;

namespace SeeShells.ShellParser.ShellItems
{
    public class ShellItem0x4D : ShellItem0x40
    {
        //this existed in the code before, no mention of a GUID for type 0x4D has been seen otherwise
        public string GUID { get; protected set; }

        public override string TypeName { get => "Network Location - NetworkPlaces"; }
        public override string Name
        {
            get
            {
                if ((Type & 0x0F) == 0x0D)
                {
                    if (KnownGuids.dict.ContainsKey(GUID))
                    {
                        return string.Format("{{{0}}}", KnownGuids.dict[GUID]);
                    }
                    else
                    {
                        return string.Format("{{{0}}}", GUID);
                    }
                }
                else
                {
                    return Location;
                }
            }
        }

        public ShellItem0x4D(byte[] buf) : base(buf)
        {
            GUID = unpack_guid(0x04);
        }

        public override IDictionary<string, string> GetAllProperties()
        {
            var ret = base.GetAllProperties();
            ret.Add("GUID", GUID.ToString());
            return ret;
        }
    }
}