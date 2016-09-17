using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPES_Modelverifier_Base
{
    public abstract class MappingList
    {
        public abstract Dictionary<Type, String> Mapping { get; }

           /* Define the VisioShape <-> BaseObject mapping here
            * 
            * Example:
            * {typeof(Models.Function), "Function" },
            * {typeof(Models.Message), "Message" },
            * {typeof(Models.MSCRef), "MSC-Reference" }
            */        
    }
}
