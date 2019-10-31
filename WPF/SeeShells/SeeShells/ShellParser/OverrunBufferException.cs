using System;
using System.Collections.Generic;
using System.Text;


namespace SeeShells.ShellParser
{
    class OverrunBufferException : Exception
    {
        public int ext_offset { get; set; }
        public int size { get; set; }
        public OverrunBufferException(int ext_offset, int size)
        {
            this.ext_offset = ext_offset;
            this.size = size;
        }
    }
}
