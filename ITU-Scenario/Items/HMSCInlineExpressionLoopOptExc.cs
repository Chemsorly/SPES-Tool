using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using SPES_Modelverifier_Base.Items;

namespace ITU_Scenario.Items
{
    public class HMSCInlineExpressionLoopOptExc : Container
    {
        /// <summary>
        /// connectable item and has to be checked as path item
        /// </summary>
        public override bool IsPathItem => true;
    }
}
