using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITU_Scenario.Items;
using SPES_Modelverifier_Base;
using SPES_Modelverifier_Base.ModelChecker;
using SPES_Modelverifier_Base.Models;

namespace ITU_Scenario.ModelChecker
{
    class GuardingConditionExistenceChecker : IModelChecker
    {
        public override void Initialize(Model pModel)
        {
            //nullcheck
            Debug.Assert(pModel != null);

            //check if any guarding condition exist
            var gcs = pModel.ObjectList.Where(t => t is HMSCGuardingCondition || t is BMSCGuardingCondition).Cast<BaseCondition>();

            //init parent model list
            List<Model> modelstocheck = new List<Model>() { pModel };

            //iterate through the hierachical structure
            var whilemodel = pModel;
            int i = 0;
            while (whilemodel.ParentModel != null)
            {
                modelstocheck.Add(whilemodel.ParentModel);
                whilemodel = whilemodel.ParentModel;
                i++;

                //emergency abort in case someone constructs a circular model reference
                if (i > 100)
                    throw new Exception("circular model reference found. Please inform the developer.");
            }

            //check everys gc for their key-value pair and check if a condition in the model or any parent model with a corresponding key-value pair exist
            foreach (var gc in gcs)
            {
                var correspondingCondition = modelstocheck.SelectMany(t => t.ObjectList).Where(t => t is HMSCCondition || t is BMSCCondition).Cast<BaseCondition>().Where(t => t.Key == gc.Key);
                if (correspondingCondition.Any())
                {
                    //if value is empty, just check if key has been touched anywhere before
                    if (string.IsNullOrWhiteSpace(gc.Value))
                        break;

                    //compare values; check if a condition with same value exists
                    if (!correspondingCondition.Any(t => t.Value == gc.Value))
                        NotifyValidationFailed(new ValidationFailedMessage(4, $"no corresponding condition with the key {gc.Key} and value {gc.Value} found", gc));
                }
                else
                {
                    NotifyValidationFailed(new ValidationFailedMessage(4, $"no corresponding condition with the key {gc.Key} found", gc));
                }
            }
        }
    }
}
