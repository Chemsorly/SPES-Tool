using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using ITU_Scenario.Items;
using ITU_Scenario.ModelChecker;

namespace ITU_Scenario.Models
{
    public class BMSCModel : SPES_Modelverifier_Base.Models.Model
    {
        [XmlIgnore]
        public override List<Type> AllowedItems => new List<Type>()
        {
            typeof(Instance) ,
            typeof(Message) ,
            typeof(BMSCInlineExpressionAltPar),
            typeof(BMSCCondition),
            typeof(BMSCGuardingCondition),
            typeof(BMSCMscReference),
            typeof(LostMessage),
            typeof(FoundMessage),
            typeof(CoregionBox)
        };

        [XmlIgnore]
        public override List<Type> CheckersToRun => new List<Type>() {typeof(BmscValidPathChecker), typeof(GuardingConditionExistenceChecker)};
    }
}
