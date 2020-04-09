using System.Collections.Generic;

namespace SeeShells.ShellParser.ShellItems
{
    public class ShellItem0x1F : ShellItem
    {
        private int off_folderid;
        public string guid { get; protected set; }
        public string folder_id { get; protected set; }
        public override string TypeName { get => "Root Folder"; }
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
                    return string.Format("{{{0}: {1}}}", folder_id, guid);
                }
            }
        }
        public ShellItem0x1F(byte[] buf)
            : base(buf)
        {
            off_folderid = 0x03;
            guid = unpack_guid(0x04);

            byte id = unpack_byte(off_folderid);

            switch (id)
            {
                case 0x00:
                    folder_id = "INTERNET_EXPLORER";
                    break;
                case 0x42:
                    folder_id = "LIBRARIES";
                    break;
                case 0x44:
                    folder_id = "USERS";
                    break;
                case 0x48:
                    folder_id = "MY_DOCUMENTS";
                    break;
                case 0x50:
                    folder_id = "MY_COMPUTER";
                    break;
                case 0x58:
                    folder_id = "NETWORK";
                    break;
                case 0x60:
                    folder_id = "RECYCLE_BIN";
                    break;
                case 0x68:
                    folder_id = "INTERNET_EXPLORER";
                    break;
                case 0x70:
                    folder_id = "UNKNOWN";
                    break;
                case 0x80:
                    folder_id = "MY_GAMES";
                    break;
                default:
                    folder_id = "";
                    break;
            }
        }

        public override IDictionary<string, string> GetAllProperties()
        {
            var dict = base.GetAllProperties();
            AddPairIfNotNull(dict, Constants.GUID, guid);
            AddPairIfNotNull(dict, Constants.FOLDER_ID, folder_id);
            return dict;
        }
    }
}