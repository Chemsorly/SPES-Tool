using SPES_Modelverifier_Base;
using SPES_Modelverifier_Base.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPES_Funktionsnetz
{
    class DependencyConnection : Connection
    {
        public override List<Type> AllowedConnectionTypes => new List<Type>() { typeof(Dependency), typeof(Function), typeof(ContextFunction) };

        public override bool Inverted => true;

        public override void Validate()
        {
            base.Validate();

            //check if one connected item is a function/context function and one is a dependency
            if ((FromObject is Dependency && ToObject is Dependency) ||
                (FromObject is Function || FromObject is ContextFunction) && (ToObject is Function || ToObject is ContextFunction))
                throw new ValidationFailedException(this, "Connection does not connect a dependency with a function (or vice versa)");
        }
    }
}
