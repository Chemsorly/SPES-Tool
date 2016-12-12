using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPES_Modelverifier_Base.Models
{
    public abstract class Container : BaseObject
    {
        private List<BaseObject> containingItems;

        public IEnumerable<BaseObject> ContainingItems => containingItems ?? (containingItems = GetContainingItems());

        public Container()
        {

        }

        public List<BaseObject> GetContainingItems()
        {
            var neighbours = this.visioshape.SpatialNeighbors((short)NetOffice.VisioApi.Enums.VisSpatialRelationCodes.visSpatialContain, 0, 0);
            return neighbours.Select(item => this.ParentModel.ObjectList.Find(t => t.uniquename == item.Name)).ToList();
        }

        public Container GetTopContainer()
        {
            throw new NotImplementedException();
        }

        public List<Container> GetChildContainers()
        {
            return ContainingItems.Where(t => t is Container).Cast<Container>().ToList();
        }

        public Container GetParentContainer()
        {
            throw new NotImplementedException();
        }
    }
}
