using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoreLinq;
using SPES_Modelverifier_Base.Items;
using SPES_Modelverifier_Base.ModelChecker.Path;

namespace ITU_Scenario.ModelChecker
{
    internal class HmscNode : Node
    {
        public HmscNode(Item pCurrent, int pDepth) : base(pCurrent, pDepth)
        {
            //no changes to basic behaviour
        }

    }
}
