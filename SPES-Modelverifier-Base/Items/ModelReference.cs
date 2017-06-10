using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SPES_Modelverifier_Base.Models;

namespace SPES_Modelverifier_Base.Items
{
    public abstract class ModelReference : Item
    {
        /// <summary>
        /// lists the model types which this object can point to
        /// needs EXPLICIT XmlIgnore in derived class!
        /// </summary>
        [XmlIgnore]
        public abstract List<Type> AllowedReferenceTypes { get; }
        public Model LinkedModel { get; set; }
    }
}
