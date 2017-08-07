using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoreLinq;
using NetOffice.VisioApi;

namespace SPES_Modelverifier_Base.Utility
{
    public static class UnitTester
    {
        public static void RunUnitTests(Type pModelnetworkType)
        {
            //get all files
            var files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.*", SearchOption.AllDirectories).Where(
                t => t.EndsWith(".vsdx", StringComparison.OrdinalIgnoreCase));

            //iterate and test files
            foreach (var file in files)
            {
                Exception validationFail = null;
                List<ValidationFailedMessage> violations = null;

                //start application
                using (Application application = new Application())
                {
                    //add document
                    String path = System.IO.Path.Combine(file);
                    var document = application.Documents.Add(path);

                    //load modelnetwork
                    var modelnetwork = Activator.CreateInstance(pModelnetworkType, application) as ModelNetwork;
                    try
                    {
                        violations = modelnetwork.VerifyModels();
                    }
                    catch (Exception ex)
                    {
                        validationFail = ex;
                    }

                    //close visio
                    application.Documents.ForEach(t =>
                    {
                        t.Saved = true;
                        t.Close();
                    });
                    application.Quit();
                }

                //check test result
                if (violations != null && violations.Any())
                    throw new Exception($"Violations found in {file}");

                //check for exceptions
                if (validationFail != null)
                    throw new Exception(validationFail.Message);
            }
        }
    }
}
