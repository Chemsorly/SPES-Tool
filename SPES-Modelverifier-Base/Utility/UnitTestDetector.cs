using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Testing
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Detect if we are running as part of a nUnit unit test.
    /// This is DIRTY and should only be used if absolutely necessary 
    /// as its usually a sign of bad design.
    /// </summary>    
    public static class UnitTestDetector
    {
        private static bool init = false;

        private static bool _runningFromNUnit = false;
        private static bool _runningFromMStest = false;


        private static void Initialize()
        {
            foreach (Assembly assem in AppDomain.CurrentDomain.GetAssemblies())
            {
                // Can't do something like this as it will load the nUnit assembly
                // if (assem == typeof(NUnit.Framework.Assert))
                Console.WriteLine(assem.FullName);

                //case nunit
                if (assem.FullName.ToLowerInvariant().StartsWith("nunit.framework"))
                {
                    _runningFromNUnit = true;
                    break;
                }

                //case mstest
                if (assem.FullName.ToLowerInvariant().Contains("testplatform") || assem.FullName.ToLowerInvariant().Contains("unittest"))
                {
                    _runningFromMStest = true;
                    break;
                }
            }

            init = true;
        }

        public static bool IsRunningFromNUnit
        {
            get
            {
                if (!init) Initialize();
                return _runningFromNUnit;
            }
        }
        public static bool IsRunningFromMStest
        {
            get
            {
                if (!init) Initialize();
                return _runningFromMStest;
            }
        }
        public static bool IsRunningUnittest => IsRunningFromNUnit || IsRunningFromMStest;
    }
}
