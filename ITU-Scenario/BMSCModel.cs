using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITU_Scenario.Items;
using ITU_Scenario.ModelChecker;
using NetOffice.VisioApi;
using SPES_Modelverifier_Base;

namespace ITU_Scenario
{
    public class BMSCModel : SPES_Modelverifier_Base.Models.Model
    {
        public override List<Type> AllowedItems => new List<Type>()
        {
            typeof(Instance) ,
            typeof(Message) ,
            typeof(InlineExpressionAltPar)
        };
        public override List<Type> CheckersToRun => new List<Type>() {typeof(BmscValidPathChecker)};
    }
}
