using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPES_Modelverifier_Base.Models
{
    public abstract class ModelReference : Item
    {
        public abstract List<Type> AllowedReferenceTypes { get; }
        public Model LinkedModel { get; set; }
    }
}
