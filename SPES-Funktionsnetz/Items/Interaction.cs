using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SPES_Modelverifier_Base.Items;
using SPES_Modelverifier_Base.Models;

namespace SPES_Funktionsnetz.Items
{
    public class Interaction : Connection
    {
        [XmlIgnore]
        public override List<Type> AllowedConnectedTypes => new List<Type>() { typeof(Function), typeof(ContextFunction) };

        public override bool Inverted => false;


    }
}
