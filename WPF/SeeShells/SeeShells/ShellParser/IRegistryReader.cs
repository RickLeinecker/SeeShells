using System;
using System.Collections.Generic;
using System.Text;

namespace SeeShells.ShellParser
{
    public interface IRegistryReader
    {
        List<RegistryKeyWrapper> GetRegistryKeys();
    }
}
