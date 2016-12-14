using SPES_Modelverifier_Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITU_Scenario.Items;
using SPES_Modelverifier_Base.Models;

namespace ITU_Scenario
{
    public class ScenarioMapping : MappingList
    {
        protected override Dictionary<String,Type> Mapping => new Dictionary<String, Type>()
        {
            //HMSC
            { "Connection Point", typeof(ConnectionPoint) },
            { "Start Symbol", typeof(StartSymbol) },
            { "End Symbol", typeof(EndSymbol) },
            { "MSC Reference", typeof(BMSCReference) },
            { "Connection Arrow", typeof(ConnectionArrow) },
            { "Inline Expr: alt", typeof(InlineExpressionAltPar) },
            { "Inline Expr: par", typeof(InlineExpressionAltPar) },

            //BMSC            
            { "Line Instance", typeof(Instance) },
            { "Headless Instance", typeof(Instance) },
            { "Message (Left)", typeof(Message) }, //TODO: only one message (current implementation aimed at "Message (Right)"
            { "Message (Right)", typeof(Message) }
        };

        protected override List<Type>TargetModels => new List<Type>() { typeof(HMSCModel), typeof(BMSCModel) };
    }
}
