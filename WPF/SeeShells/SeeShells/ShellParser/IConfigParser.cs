using System;
using System.Collections.Generic;
using System.Text;

namespace SeeShells.ShellParser
{
    public interface IConfigParser
    {
        void GetConfig();
        void ParseConfig();
        List<String> GetLocations();

    }
}
