using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SPES_Modelverifier_Base;
using SPES_Modelverifier_Base.Items;
using SPES_Modelverifier_Base.Models;

namespace SPES_Funktionsnetz.Items
{
    public class DependencyConnection : Connection
    {
        [XmlIgnore]
        public override List<Type> AllowedConnectedTypes => new List<Type>() { typeof(Dependency), typeof(Function), typeof(ContextFunction) };

        public override bool Inverted => true;

        public override void Verify()
        {
            base.Verify();

            //check if one connected item is a function/context function and one is a dependency
            if ((FromObject is Dependency && ToObject is Dependency) ||
                (FromObject is Function || FromObject is ContextFunction) && (ToObject is Function || ToObject is ContextFunction))
                throw new ValidationFailedException(this, "Connection does not connect a dependency with a function (or vice versa)");
        }
    }
}
