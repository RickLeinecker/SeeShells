using System.Collections.Generic;

namespace SeeShells.ShellParser.Registry
{
    public interface IRegistryReader
    {
        List<RegistryKeyWrapper> GetRegistryKeys();
    }
}
