using System;
using System.Collections.Generic;
using System.Text;

namespace SeeShells.ShellParser
{
    public interface IConfigParser
    {
        List<string> GetRegistryLocations();

        List<string> GetUsernameLocations();

    }
}
