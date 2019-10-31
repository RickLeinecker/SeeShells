using System;
using System.Collections.Generic;
using System.Text;

namespace SeeShells.ShellParser
{
    interface IRegistryReader
    {
        List<RegistryKeyWrapper> GetRegistryKeys();
    }
}
