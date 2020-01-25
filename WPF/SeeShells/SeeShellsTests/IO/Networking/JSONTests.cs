using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SeeShells.IO.Networking.JSON;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShellsTests.IO.Networking
{
    [TestClass()]
    public class JSONTests
    {

        [TestMethod]
        public void APIError_DeserializationTest()
        {
            var json = "{\"success\": 0, \"error\": \"Error Occurred\"}";
            var obj = JsonConvert.DeserializeObject<APIError>(json);
            Assert.AreEqual(obj.GetType(), new APIError().GetType());
        }

        [TestMethod]
        public void GUIDPair_DeserializationTest()
        {
            string json = File.ReadAllText(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\TestResource\sampleGUIDs.json");
            var obj = JsonConvert.DeserializeObject<IList<GUIDPair>>(json);
            Assert.AreEqual(obj[0].GetType(), new GUIDPair().GetType());
        }


        [TestMethod]
        public void RegistryLocations_DeserializationTest()
        {
            string json = File.ReadAllText(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\TestResource\sampleOSRegistryLocations.json");
            var obj = JsonConvert.DeserializeObject<IList<RegistryLocations>>(json);
            Assert.AreEqual(obj[0].GetType(), new RegistryLocations().GetType());
        }

    }
}
