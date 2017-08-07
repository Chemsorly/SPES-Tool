using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPES_Funktionsnetz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using NetOffice.VisioApi;
using SPES_Modelverifier_Base;
using MoreLinq;
using SPES_Modelverifier_Base.Utility;

namespace SPES_Funktionsnetz.Tests
{
    [TestClass()]
    public class FunktionsnetzNetworkTests
    {
        [TestMethod()]
        [DeploymentItem(@"Testfiles\ControlHighBeamHeadlights_Example.vsdx")]
        public void FunktionsnetzTests()
        {
            try
            {
                UnitTester.RunUnitTests(typeof(FunktionsnetzNetwork));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Assert.Fail(ex.Message);
            }
            
        }

        //xml import/export
    }
}