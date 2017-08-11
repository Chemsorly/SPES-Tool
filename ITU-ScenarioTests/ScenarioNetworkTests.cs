﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using ITU_Scenario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPES_Modelverifier_Base.Utility;

namespace ITU_Scenario.Tests
{
    [TestClass()]
    public class ScenarioNetworkTests
    {
        private const string Subfolder = "Szenariomodelle";

        [TestMethod()]
        public void ScenarioTests()
        {
            try
            {
                UnitTester.RunUnitVerificationTests(typeof(ScenarioNetwork), Subfolder);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void ScenarioExport()
        {
            try
            {
                UnitTester.RunUnitExportTests(typeof(ScenarioNetwork), Subfolder);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Assert.Fail(ex.Message);
            }

        }
    }
}