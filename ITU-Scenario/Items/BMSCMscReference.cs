using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using ITU_Scenario.Models;
using SPES_Modelverifier_Base.Items;
using SPES_Modelverifier_Base.Models;

namespace ITU_Scenario.Items
{
    public class BMSCMscReference : ModelReference
    {
        [XmlIgnore]
        public override List<Type> AllowedReferenceTypes => new List<Type>() { typeof(BMSCModel) };

        public override bool CanHaveDuplicateText => true;

        public override bool IsPathItem => false;
    }
}
