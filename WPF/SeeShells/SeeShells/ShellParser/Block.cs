using System;
using System.Collections.Generic;
using System.Text;

namespace ShellBagsParser
{
    class Block
    {
        public byte[] buf { get; set; }
        public int offset { get; set; }
        public object parent { get; set; }
        public Block(byte[] buf, int offset, object parent)
        {
            this.buf = buf;
            this.offset = offset;
            this.parent = parent;
        }

        public byte unpack_byte(int off)
        {
            try
            {
                return buf[offset + off];
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new OverrunBufferException(offset + off, buf.Length);
            }
        }

        public ushort unpack_word(int off)
        {
            try
            {
                return BitConverter.ToUInt16(buf, offset + off);
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new OverrunBufferException(offset + off, buf.Length);
            }
        }

        public string unpack_guid(int off)
        {
            try
            {
                return string.Format("{0:x2}{1:x2}{2:x2}{3:x2}-{4:x2}{5:x2}-{6:x2}{7:x2}-{8:x2}{9:x2}-{10:x2}{11:x2}{12:x2}{13:x2}{14:x2}{15:x2}",
                    buf[offset + off + 3], buf[offset + off + 2], buf[offset + off + 1], buf[offset + off],
                    buf[offset + off + 5], buf[offset + off + 4],
                    buf[offset + off + 7], buf[offset + off + 6],
                    buf[offset + off + 8], buf[offset + off + 9],
                    buf[offset + off + 10], buf[offset + off + 11], buf[offset + off + 12], buf[offset + off + 13], buf[offset + off + 14], buf[offset + off + 15]);
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new OverrunBufferException(offset + off, buf.Length);
            }
        }

        public string unpack_wstring(int off, int length = 0)
        {
            try
            {
                if (length == 0)
                {
                    int end = offset + off;
                    for (int ind = offset + off; ind + 1 < buf.Length; ind += 2)
                    {
                        if (buf[ind] == 0 && buf[ind + 1] == 0)
                        {
                            end = ind;
                            break;
                        }
                    }
                    length = end - offset - off;
                }
                while (buf[offset + off + length - 2] == 0 && buf[offset + off + length - 1] == 0) length -= 2;
                return Encoding.Unicode.GetString(buf, offset + off, length);
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new OverrunBufferException(offset + off, buf.Length);
            }
        }

        public string unpack_string(int off, int length = 0)
        {
            try
            {
                if (length == 0)
                {
                    int end = Array.IndexOf(buf, (byte)0, offset + off);
                    length = end - offset - off;
                }
                while (buf[offset + off + length - 1] == 0) --length;
                return Encoding.ASCII.GetString(buf, offset + off, length);
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new OverrunBufferException(offset + off, buf.Length);
            }
        }

        public uint unpack_dword(int off)
        {
            try
            {
                return BitConverter.ToUInt32(buf, offset + off);
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new OverrunBufferException(offset + off, buf.Length);
            }
        }

        public DateTime unpack_dosdate(int off)
        {
            try
            {
                ushort dosdate = (ushort)((buf[offset + off + 1] << 8) | buf[offset + off]);
                ushort dostime = (ushort)((buf[offset + off + 3] << 8) | buf[offset + off + 2]);
                int day = dosdate & 0x1F;
                int month = (dosdate & 0x1E0) >> 5;
                int year = (dosdate & 0xFE00) >> 9;
                year += 1980;

                int sec = (dostime & 0x1F) * 2;
                int minute = (dostime & 0x7E0) >> 5;
                int hour = (dostime & 0xF800) >> 11;
                return new DateTime(year, month, day, hour, minute, sec);
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new OverrunBufferException(offset + off, buf.Length);
            }
        }

        public int align(int off, int alignment)
        {
            if (off % alignment == 0)
                return off;
            return off + (alignment - (off % alignment));
        }
    }
}
