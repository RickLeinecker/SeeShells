using SeeShells.ShellParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShellsTests.ShellParser.ShellParserMocks
{
    class OfflineMockConfigParser : IConfigParser
    {

        public List<string> GetRegistryLocations()
        {
            List<String> list = new List<String>();
            list.Add(@"Software\Microsoft\Windows\Shell"); //NTUser path
            list.Add(@"Local Settings\Software\Microsoft\Windows\Shell\BagMRU"); //Usrclass path
            return list;
        }

        public List<string> GetUsernameLocations()
        {
            List<String> list = new List<String>();
            list.Add(@"Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders");
            return list;
        }
    }
}
