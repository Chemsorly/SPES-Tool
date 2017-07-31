using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPES_Modelverifier_Base;
using SPES_Modelverifier_Base.ModelChecker;
using SPES_Modelverifier_Base.Models;

namespace ITU_Scenario.ModelChecker
{
    class ScenarioPathChecker : IModelNetworkChecker
    {
        public override void Initialize(ModelNetwork pModelNetwork)
        {
            //get all models that are a valid starting points: all models with no parents
            var models = pModelNetwork.ModelList.Where(t => t.ParentModel == null);

            //create a tree for each model
            foreach (var model in models)
            {
                //create tree
            }

            //create paths from tree and verify
            //todo
        }
    }
}
