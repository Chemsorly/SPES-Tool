using SPES_Modelverifier_Base.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITU_Scenario
{
    internal class BMSCReference : ModelReference
    {
        public override List<Type> AllowedReferenceTypes => new List<Type>() { typeof(BMSCModel) };
    }
}
