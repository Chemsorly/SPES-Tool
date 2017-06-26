using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SPES_Modelverifier_Base.Items;
using SPES_Modelverifier_Base.Models;

namespace ITU_Scenario.Items
{
    public class Message : Connection
    {
        [XmlIgnore]
        public override List<Type> AllowedConnectedTypes => new List<Type>() { typeof(Instance) };        

        public override bool Inverted => true;
        
    }
}
