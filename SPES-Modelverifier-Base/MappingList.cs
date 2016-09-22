using SPES_Modelverifier_Base.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPES_Modelverifier_Base
{
    public abstract class MappingList
    {
        /// <summary>
        /// Overwrite target for the model type
        /// </summary>
        public abstract Type TargetModel { get; }

        /// <summary>
        /// Overwrite target for all model objects
        /// </summary>
        public abstract Dictionary<Type, String> Mapping { get; }

        /* Define the VisioShape <-> BaseObject mapping here
         * 
         * Example:
         * {typeof(Models.Function), "Function" },
         * {typeof(Models.Message), "Message" },
         * {typeof(Models.MSCRef), "MSC-Reference" }
         */

        public List<String> GetAllVisioStrings()
        {
            return Mapping.Select(t => t.Value).ToList();
        }

        public List<Type> GetAllObjectTypes()
        {
            return Mapping.Select(t => t.Key).ToList();
        }
    }
}
