using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPES_Modelverifier_Base.Models
{
    public abstract class Item : BaseObject
    {
        public List<Connection> Connections { get; }

        public Item()
        {
            Connections = new List<Connection>();
        }

        public override void Validate()
        {
            base.Validate();

            if (Connections.Count == 0)
                throw new ValidationFailedException(this, "Item has no connections");
        }
    }
}
