namespace SeeShells.ShellParser
{
    public class RegistryKeyWrapper
    {
        private readonly byte[] value;

        public RegistryKeyWrapper(byte[] Value)
        {
            value = Value;
        }


        byte[] getValue()
        {
            return value;
        }
    }
}
