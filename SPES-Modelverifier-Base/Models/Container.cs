using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPES_Modelverifier_Base.Models
{
    public abstract class Container : BaseObject
    {
        public List<BaseObject> ContainingItems { get; }

        public Container()
        {
            ContainingItems = new List<BaseObject>();
        }

        public void FindContainingItems()
        {
            var neighbours = this.visioshape.SpatialNeighbors((short)NetOffice.VisioApi.Enums.VisSpatialRelationCodes.visSpatialContain, 0, 0);

            ContainingItems.Clear();
            foreach (var item in neighbours)
                ContainingItems.Add(this.ParentModel.ObjectList.Find(t => t.uniquename == item.Name));
        }
    }
}
