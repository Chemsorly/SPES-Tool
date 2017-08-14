using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SPES_Modelverifier_Base.Items;
using SPES_Modelverifier_Base.Models;

namespace ITU_Scenario.Items
{
    public class ConnectionArrow : Connection
    {
        [XmlIgnore]
        public override List<Type> AllowedConnectedTypes => new List<Type>() {
            typeof(ConnectionPoint),
            typeof(StartSymbol),
            typeof(EndSymbol),
            typeof(HMSCMscReference),
            typeof(HMSCInlineExpressionAltPar),
            typeof(HMSCInlineExpressionLoopOptExc),
            typeof(HMSCCondition),
            typeof(HMSCGuardingCondition)};

        public override bool Inverted => true;
    }
}
