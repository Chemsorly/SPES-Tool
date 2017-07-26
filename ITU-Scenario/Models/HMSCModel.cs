using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using ITU_Scenario.Items;
using ITU_Scenario.ModelChecker;

namespace ITU_Scenario.Models
{
    public class HMSCModel : SPES_Modelverifier_Base.Models.Model
    {
        [XmlIgnore]
        public override List<Type> AllowedItems => new List<Type>()
        {
            typeof(ConnectionPoint) ,
            typeof(StartSymbol) ,
            typeof(EndSymbol) ,
            typeof(BMSCReference) ,
            typeof(ConnectionArrow) ,
            typeof(BMSCInlineExpressionAltPar),
            typeof(Condition),
            typeof(GuardingCondition)
        };

        public override List<Type> CheckersToRun => new List<Type>() {typeof(GuardingConditionExistenceChecker) };
    }
}
