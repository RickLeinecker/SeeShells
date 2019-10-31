using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShellBagsParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeeShells.ShellParser;

namespace ShellBagsParser.Tests
{
    [TestClass()]
    public class ProgramTests
    {
        [TestMethod()]
        public void MainTest()
        {
            Program.Leinecker();
        }
    }
}