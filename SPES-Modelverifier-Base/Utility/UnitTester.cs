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
        public static void RunUnitVerificationTests(Type pModelnetworkType, String pDirectory)
        {
            //get all files
            var files = Directory.GetFiles(System.IO.Path.Combine(Directory.GetCurrentDirectory(), pDirectory), "*.*", SearchOption.AllDirectories).Where(
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

        public static void RunUnitExportTests(Type pModelnetworkType, String pDirectory)
        {
            //get all files
            var files = Directory.GetFiles(System.IO.Path.Combine(Directory.GetCurrentDirectory(), pDirectory), "*.*", SearchOption.AllDirectories).Where(
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
                        if(!modelnetwork.CanExport())
                            throw new Exception($"Can not export: verification failed in file {file}");
                        modelnetwork.Export(file + ".xml");
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

                //check for exceptions
                if (validationFail != null)
                    throw new Exception(validationFail.Message);
            }
        }
    }
}
