﻿using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using ITU_Scenario.Items;

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
            typeof(InlineExpressionAltPar)
        };
    }
}
