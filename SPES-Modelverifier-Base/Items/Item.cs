using System.Collections.Generic;
using System.Xml.Serialization;

namespace SPES_Modelverifier_Base.Items
{
    public abstract class Item : BaseObject
    {
        [XmlIgnore]
        public List<Connection> Connections { get; }
        public virtual bool CanHaveDuplicateText => false;

        public Item()
        {
            Connections = new List<Connection>();
        }

        public override void Verify()
        {
            base.Verify();

            if (Connections.Count == 0)
                throw new ValidationFailedException(this, "Item has no connections");
        }
    }
}
