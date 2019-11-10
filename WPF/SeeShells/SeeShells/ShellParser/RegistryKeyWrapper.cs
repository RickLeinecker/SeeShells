namespace SeeShells.ShellParser
{
    public class RegistryKeyWrapper
    {
        private readonly byte[] value;

        public RegistryKeyWrapper(byte[] Value)
        {
            value = Value;
        }


        public byte[] getValue()
        {
            return value;
        }
    }
}
