using SPES_Modelverifier_Base.ModelChecker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPES_Modelverifier_Base.Models;
using System.Diagnostics;
using ITU_Scenario.Models;
using SPES_Modelverifier_Base;

namespace ITU_Scenario.ModelChecker
{
    internal class BmscValidPathChecker : IModelChecker
    {
        public override void Initialize(Model pModel)
        {
            //nullcheck
            Debug.Assert(pModel != null);

            //check if bmsc
            if (pModel is BMSCModel)
            {
                //tree validation
                //create tree
                var tree = new BmscTree();
                tree.ValidationFailedEvent += NotifyValidationFailed;
                tree.Initialize((BMSCModel)pModel);

                //call validate function
                try
                {
                    tree.Validate();
                }
                catch (ValidationFailedException ex)
                {
                    NotifyValidationFailed(new ValidationFailedMessage(4, ex));
                }
            }
        }
    }
}
