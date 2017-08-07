using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPES_Funktionsnetz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPES_Funktionsnetz.Tests
{
    [TestClass()]
    public class FunktionsnetzNetworkTests
    {
        [DeploymentItem(@"Testfiles\ControlHighBeamHeadlights_Example.vsdx")]
        [TestMethod()]
        public void FN_ControlHighBeamHeadlightsExample()
        {
            Assert.Fail();
        }


    }
}