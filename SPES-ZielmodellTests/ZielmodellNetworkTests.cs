using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPES_Modelverifier_Base.Utility;
using SPES_Zielmodell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPES_Zielmodell.Tests
{
    [TestClass()]
    public class ZielmodellNetworkTests
    {
        private const string Subfolder = "Zielmodelle";

        [TestMethod()]
        public void ZielmodellTests()
        {
            try
            {
                UnitTester.RunUnitVerificationTests(typeof(ZielmodellNetwork), Subfolder);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void ZielmodellExport()
        {
            try
            {
                UnitTester.RunUnitExportTests(typeof(ZielmodellNetwork), Subfolder);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Assert.Fail(ex.Message);
            }

        }
    }
}