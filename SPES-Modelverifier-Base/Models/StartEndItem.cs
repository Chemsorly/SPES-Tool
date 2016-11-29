using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPES_Modelverifier_Base.Models
{
    public abstract class StartEndItem : Item
    {
        /// <summary>
        /// define if the item is a start item. if false, it's an end item
        /// </summary>
        public abstract bool IsStart { get; }

        public override void Validate()
        {
            //check base item stuff
            base.Validate();

            //startenditem specific checks
            if (IsStart)
            {
                //check if all connection items are outgoing connections
                if (this.Connections.Any(t => t.FromObject != this))
                    throw new ValidationFailedException(this, "Start Item connections contains an invalid connection (does not equal outgoing)");
            }
            else
                //check if all connection items are ingoing connections
                if (this.Connections.Any(t => t.ToObject != this))
                throw new ValidationFailedException(this, "End Item connections contains an invalid connection (does not equal outgoing)");
        }
    }
}
