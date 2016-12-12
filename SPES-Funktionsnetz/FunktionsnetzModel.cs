using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetOffice.VisioApi;
using SPES_Modelverifier_Base;

namespace SPES_Funktionsnetz
{
    internal class FunktionsnetzModel : SPES_Modelverifier_Base.Models.Model
    {
        public override List<Type> AllowedItems => new List<Type>() { typeof(Function), typeof(ContextFunction), typeof(Interaction), typeof(Dependency), typeof(DependencyConnection) };
    }
}
