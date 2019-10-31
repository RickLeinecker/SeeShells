using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;


namespace SeeShells.ShellParser
{
    enum SHITEMTYPE
    {
        UNKNOWN0 = 0x00,
        UNKNOWN1 = 0x01,
        UNKNOWN2 = 0x2E,
        FILE_ENTRY = 0x30,
        FOLDER_ENTRY = 0x1F,
        VOLUME_NAME = 0x20,
        NETWORK_LOCATION = 0x40,
        URI = 0x61,
        CONTROL_PANEL = 0x71,
        DELETEGATE_ITEM = 0x74
    };
    class SHITEM_FOLERENTRY : SHITEM
    {
        private int off_folderid;
        public string guid { get; protected set; }
        public string folder_id { get; protected set; }
        public override string name
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
        public SHITEM_FOLERENTRY(byte[] buf, int offset, object parent)
            : base(buf, offset, parent)
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
    }
    class SHITEM_UNKNOWNENTRY0 : SHITEM
    {
        public string guid { get; protected set; }
        public override string name
        {
            get
            {
                if (size == 0x20)
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
                else
                {
                    return "??";
                }
            }
        }
        public SHITEM_UNKNOWNENTRY0(byte[] buf, int offset, object parent)
            : base(buf, offset, parent)
        {
            if (size == 0x20)
            {
                guid = unpack_guid(0x0E);
            }
        }
    }

    class SHITEM_UNKNOWNENTRY2 : SHITEM
    {
        public string guid { get; protected set; }
        public byte flags { get; protected set; }
        public override string name
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
            : base(buf, offset, parent)
        {
            flags = unpack_byte(0x03);
            guid = unpack_guid(0x04);
        }

    }

    class SHITEM_URIENTRY : SHITEM
    {
        public string uri { get; protected set; }
        public uint flags { get; protected set; }
        public override string name
        {
            get
            {
                return uri;
            }
        }

        public SHITEM_URIENTRY(byte[] buf, int offset, object parent)
            : base(buf, offset, parent)
        {
            flags = unpack_dword(0x03);
            uri = unpack_wstring(0x08);
        }
    }

    class SHITEM_CONTROLPANELENTRY : SHITEM
    {
        public string guid { get; protected set; }
        public byte flags { get; protected set; }
        public override string name
        {
            get
            {
                if (KnownGuids.dict.ContainsKey(guid))
                {
                    return string.Format("{{CONTROL PANEL: {0}}}", KnownGuids.dict[guid]);
                }
                else
                {
                    return string.Format("{{CONTROL PANEL: {0}}}", guid);
                }
            }
        }
        public SHITEM_CONTROLPANELENTRY(byte[] buf, int offset, object parent)
            : base(buf, offset, parent)
        {
            guid = unpack_guid(0x0E);
            flags = unpack_byte(0x03);
        }
    }
    class SHITEM_VOLUMEENTRY : SHITEM
    {
        public override string name { get; protected set; }
        public SHITEM_VOLUMEENTRY(byte[] buf, int offset, object parent)
            : base(buf, offset, parent)
        {
            if ((type & 0x01) != 0)
            {
                name = unpack_string(0x03);
            }
        }
    }
    class SHITEM_NETWORKLOCATIONENTRY : SHITEM
    {
        public string guid { get; protected set; }
        public byte flags { get; protected set; }
        public string location { get; protected set; }
        public string description { get; protected set; }
        public string comments { get; protected set; }
        public override string name
        {
            get
            {
                if ((type & 0x0F) == 0x0D)
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
                else
                {
                    return location;
                }
            }
        }
        public SHITEM_NETWORKLOCATIONENTRY(byte[] buf, int offset, object parent)
            : base(buf, offset, parent)
        {
            if ((type & 0x0F) == 0x0D)
            {
                guid = unpack_guid(0x04);
                return;
            }
            flags = unpack_byte(0x04);
            location = unpack_string(0x05);
            int off = 0x05;
            off += name.Length + 1;
            if ((flags & 0x80) != 0)
            {
                description = unpack_string(off);
                off += description.Length + 1;
            }
            if ((flags & 0x40) != 0)
            {
                comments = unpack_string(off);
            }
        }
    }
    class ExtensionBlock_BEEF0004 : Block
    {
        public ushort ext_size { get; protected set; }
        public ushort ext_version { get; protected set; }
        public DateTime cr_date { get; protected set; }
        public DateTime a_date { get; protected set; }
        public ushort long_name_size { get; protected set; }
        public string long_name { get; protected set; }
        public string localized_name { get; protected set; }
        public ExtensionBlock_BEEF0004(byte[] buf, int offset, object parent)
            : base(buf, offset, parent)
        {
            int off = 0;
            ext_size = unpack_word(off);
            off += 2;
            ext_version = unpack_word(off);
            off += 2;
            if (ext_version >= 0x03)
            {
                off += 4; // 0xbeef0004
                cr_date = unpack_dosdate(off);
                off += 4;
                a_date = unpack_dosdate(off);
                off += 4;
                off += 2; // unknown
            }
            else
            {
                cr_date = DateTime.MinValue;
                a_date = DateTime.MinValue;
            }

            if (ext_version >= 0x07)
            {
                off += 2;
                off += 8; // fileref
                off += 8; // unknown
            }

            if (ext_version >= 0x03)
            {
                long_name_size = unpack_word(off);
                off += 2;
            }

            if (ext_version >= 0x09)
                off += 4; // unknown

            if (ext_version >= 0x08)
                off += 4; // unknown

            if (ext_version >= 0x03)
            {
                long_name = unpack_wstring(off);
                off += 2 * (long_name.Length + 1);
            }
            if (ext_version >= 0x03 && ext_version < 0x07 && long_name_size > 0)
            {
                localized_name = unpack_string(off);
            }
            else if (ext_version >= 0x07 && long_name_size > 0)
            {
                localized_name = unpack_wstring(off);
            }
        }
    }

    class SHITEM_WITH_EXTENSION : SHITEM
    {
        public ExtensionBlock_BEEF0004 extension_block { get; set; }
        public ushort ext_size
        {
            get
            {
                if (extension_block != null)
                    return extension_block.ext_size;
                return 0;
            }
        }
        public ushort ext_version
        {
            get
            {
                if (extension_block != null)
                    return extension_block.ext_size;
                return 0;
            }
        }
        public override DateTime cr_date
        {
            get
            {
                if (extension_block != null)
                    return extension_block.cr_date;
                return base.cr_date;
            }
        }
        public override DateTime a_date
        {
            get
            {
                if (extension_block != null)
                    return extension_block.a_date;
                return base.a_date;
            }
        }
        public ushort long_name_size
        {
            get
            {
                if (extension_block != null)
                    return extension_block.ext_size;
                return 0;
            }
        }
        public string long_name
        {
            get
            {
                if (extension_block != null)
                    return extension_block.long_name;
                return "";
            }
        }
        public string localized_name
        {
            get
            {
                if (extension_block != null)
                    return extension_block.localized_name;
                return "";
            }
        }
        public SHITEM_WITH_EXTENSION(byte[] buf, int offset, object parent)
            : base(buf, offset, parent)
        {
            extension_block = null;
        }
    }

    class Fileentry : SHITEM_WITH_EXTENSION
    {
        public uint filesize { get; protected set; }
        public ushort fileattrs { get; protected set; }
        public ushort ext_offset { get; protected set; }
        public string short_name { get; protected set; }
        public override DateTime m_date { get; protected set; }
        public override string name
        {
            get
            {
                if (long_name.Length > 0)
                    return long_name;
                return short_name;
            }
        }
        public Fileentry(byte[] buf, int offset, object parent, int filesize_offset)
            : base(buf, offset, parent)
        {
            int off = filesize_offset;
            filesize = unpack_dword(off);
            off += 4;
            m_date = unpack_dosdate(off);
            off += 4;
            fileattrs = unpack_word(off);
            off += 2;
            ext_offset = unpack_word(size - 2);

            if (ext_offset > size)
                throw new OverrunBufferException(ext_offset, size);

            if ((type & 0x04) != 0)
                short_name = unpack_wstring(off, ext_offset - off);
            else
                short_name = unpack_string(off, ext_offset - off);
            extension_block = new ExtensionBlock_BEEF0004(buf, ext_offset + offset, this);
        }
    }

    class SHITEM_FILEENTRY : Fileentry
    {
        public byte flags { get; protected set; }
        public SHITEM_FILEENTRY(byte[] buf, int offset, object parent)
            : base(buf, offset, parent, 0x04)
        {
            flags = unpack_byte(0x03);
        }
    }

    class FILEENTRY_FRAGMENT : SHITEM
    {
        public uint filesize { get; protected set; }
        public ushort fileattrs { get; protected set; }
        public string short_name { get; protected set; }
        public override DateTime m_date { get; protected set; }
        public override string name
        {
            get
            {
                return short_name;
            }
        }
        public FILEENTRY_FRAGMENT(byte[] buf, int offset, object parent, int filesize_offset)
            : base(buf, offset, parent)
        {
            int off = filesize_offset;
            filesize = unpack_dword(off);
            off += 4;
            m_date = unpack_dosdate(off);
            off += 4;
            fileattrs = unpack_word(off);
            off += 2;
            short_name = unpack_string(off);
            off += short_name.Length + 1;
            off = align(off, 2);
        }
    }

    class SHITEM_DELEGATE : SHITEM_WITH_EXTENSION
    {
        public uint signature { get; protected set; }
        public FILEENTRY_FRAGMENT sub_item { get; protected set; }
        public string delegate_item_identifier { get; protected set; }
        public string item_class_identifier { get; protected set; }
        public override string name
        {
            get
            {
                if (long_name.Length > 0)
                    return long_name;
                return sub_item.short_name;
            }
        }

        public override DateTime m_date
        {
            get
            {
                return sub_item.m_date;
            }
        }
        public SHITEM_DELEGATE(byte[] buf, int offset, object parent)
            : base(buf, offset, parent)
        {
            // Unknown - Empty ( 1 byte)
            // Unknown - size? - 2 bytes
            // CFSF - 4 bytes
            // sub shell item data size - 2 bytes
            signature = unpack_dword(0x06);

            int off = 0x0A;
            sub_item = new FILEENTRY_FRAGMENT(buf, offset + off, this, 0x04);
            off += sub_item.size;

            off += 2; // Empty extension block?

            // 5e591a74-df96-48d3-8d67-1733bcee28ba
            delegate_item_identifier = unpack_guid(off);
            off += 10;
            item_class_identifier = unpack_guid(off);
            off += 10;
            extension_block = new ExtensionBlock_BEEF0004(buf, offset + off, this);
        }
    }
}