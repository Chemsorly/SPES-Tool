using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SPES_Zielmodell.Models
{
    public class ZielmodellModel : SPES_Modelverifier_Base.Models.Model
    {
        [XmlIgnore]
        public override List<Type> AllowedItems => null; //all allowed
    }
}
