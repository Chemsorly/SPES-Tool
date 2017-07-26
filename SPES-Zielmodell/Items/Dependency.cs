using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SPES_Modelverifier_Base.Items;
using SPES_Modelverifier_Base.Models;

namespace SPES_Zielmodell.Items
{
    public class Dependency : Connection
    {
        [XmlIgnore]
        public override List<Type> AllowedConnectedTypes => new List<Type>() { typeof(Actor), typeof(IntentionalObject)};

        public override bool Inverted => false;
    }
}
