using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPES_Modelverifier_Base.Items;
using SPES_Modelverifier_Base.ModelChecker.Path;

namespace ITU_Scenario.ModelChecker
{
    class ScenarioPathNode : Node
    {
        public ScenarioPathNode(Item pCurrent, int pDepth) : base(pCurrent, pDepth)
        {
        }
    }
}
