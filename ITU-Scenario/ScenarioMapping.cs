using SPES_Modelverifier_Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITU_Scenario.Items;
using ITU_Scenario.Models;
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
            { "Inline Expr: alt", typeof(HMSCInlineExpressionAltPar) },
            { "Inline Expr: par", typeof(HMSCInlineExpressionAltPar) },
            { "Inline Expr: loop", typeof(HMSCInlineExpressionLoopOptExc) },
            { "Inline Expr: opt", typeof(HMSCInlineExpressionLoopOptExc) },
            { "Inline Expr: exc", typeof(HMSCInlineExpressionLoopOptExc) },

            //BMSC            
            { "Line Instance", typeof(Instance) },
            { "Message (Left)", typeof(Message) },
            { "Message (Right)", typeof(Message) },
            { "Lost Message", typeof(LostMessage) },
            { "Found Message", typeof(FoundMessage) },
            { "Coregion Box", typeof(CoregionBox) },
            { "Inline Expr: alt (bmsc)", typeof(BMSCInlineExpressionAltPar) },
            { "Inline Expr: par (bmsc)", typeof(BMSCInlineExpressionAltPar) },
            //no function in bmsc atm
            //{ "Inline Expr: loop (bmsc)", typeof(BMSCInlineExpressionLoopOptExc) },
            //{ "Inline Expr: opt (bmsc)", typeof(BMSCInlineExpressionLoopOptExc) },
            //{ "Inline Expr: exc (bmsc)", typeof(BMSCInlineExpressionLoopOptExc) },

            //both
            { "Condition", typeof(Condition) },
            { "Guarding Condition", typeof(GuardingCondition) }
        };

        protected override List<Type>TargetModels => new List<Type>() { typeof(HMSCModel), typeof(BMSCModel) };
    }
}
