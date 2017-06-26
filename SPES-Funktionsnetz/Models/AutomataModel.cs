using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SPES_Funktionsnetz.Items;

namespace SPES_Funktionsnetz.Models
{
    public class AutomataModel : SPES_Modelverifier_Base.Models.Model
    {
        [XmlIgnore]
        public override List<Type> AllowedItems => new List<Type>() { typeof(Step), typeof(NodeConnection) };
    }
}
