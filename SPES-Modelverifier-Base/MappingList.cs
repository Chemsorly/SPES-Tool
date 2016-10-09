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
        /// Overwrite target for the model types. Types have to derive from Model
        /// </summary>
        protected internal abstract List<Type> TargetModels { get; }
        /*Define the list of model types. The program picks the right one based on a probability check
         * 
         * Example:
         * typeof(HMSCmodel),
         * typeof(BMSCmodel)
         */


        /// <summary>
        /// Overwrite target for all model objects
        /// </summary>
        protected internal abstract Dictionary<String,Type> Mapping { get; }

        /* Define the VisioShape <-> BaseObject mapping here
         * 
         * Example:
         * {"Function", typeof(Models.Function) },
         * {"Message", typeof(Models.Message) },
         * {"MSC-Reference", typeof(Models.MSCRef)  }
         */

        public List<String> GetAllVisioStrings()
        {
            return Mapping.Select(t => t.Key).ToList();
        }

        public List<Type> GetAllObjectTypes()
        {
            return Mapping.Select(t => t.Value).ToList();
        }
    }
}
