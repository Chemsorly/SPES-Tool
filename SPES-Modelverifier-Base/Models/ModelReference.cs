using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPES_Modelverifier_Base.Models
{
    internal abstract class ModelReference : Item
    {
        public Model LinkedModel { get; }
        public bool ContextReference { get; }
    }
}
