namespace SeeShells.ShellParser.Registry
{
    public class RegistryKeyWrapper
    {
        public string RegistryUser { get; internal set; }
        public string RegistryPath { get; internal set; }
        public byte[] Value { get; }

        public RegistryKeyWrapper(byte[] value)
        {
            this.Value = value;
            RegistryUser = string.Empty;
            RegistryPath = string.Empty;
        }

        public RegistryKeyWrapper(byte[] value, string registryUser, string registryPath) : this(value)
        {
            this.RegistryUser = registryUser;
            this.RegistryPath = registryPath;
        }

    }
}
