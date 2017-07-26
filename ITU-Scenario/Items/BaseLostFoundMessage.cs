using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using SPES_Modelverifier_Base.Items;

namespace ITU_Scenario.Items
{
    abstract class BaseLostFoundMessage : Connection
    {
        [XmlIgnore]
        public override List<Type> AllowedConnectedTypes => new List<Type>() {typeof(Instance)};

        public override bool Inverted { get; }
    }
}
