using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MoreLinq;

namespace SPES_Modelverifier_Base.Items
{
    public abstract class Container : BaseObject
    {
        /// <summary>
        /// All BaseObjects in the container item
        /// </summary>
        public List<BaseObject> ContainingItems => _containingItems ?? (_containingItems = GetContainingItems());
        private List<BaseObject> _containingItems;

        /// <summary>
        /// most top/parent container in hierarchy. returns itself if already top
        /// </summary>
        public Container TopContainer => _topContainer ?? (_topContainer = GetTopContainer());
        private Container _topContainer;
        
        /// <summary>
        /// direct parent container. null if no parent
        /// </summary>
        public Container ParentContainer => _parentContainer ?? (_parentContainer = GetParentContainer());
        private Container _parentContainer;
        
        /// <summary>
        /// all childcontainers
        /// </summary>
        public List<Container> ChildContainers => _childContainers ?? (_childContainers = GetAllChildContainers());
        private List<Container> _childContainers;
        
        /// <summary>
        /// all direct descendend childcontainers
        /// </summary>
        public List<Container> FirstLevelChildContainers => _firstLevelChildContainers ?? (_firstLevelChildContainers = GetFirstLevelChildContainers());
        private List<Container> _firstLevelChildContainers;

        public override void Initialize()
        {
            base.Initialize();

            //notify the BaseObjects they are inside a container
            ContainingItems.ForEach(t =>
            {
                if (!t.Containers.Contains(this))
                    t.Containers.Add(this);
            }
            );
        }

        /// <summary>
        /// validates container specifics
        /// </summary>
        public override void Validate()
        {
            base.Validate();

            //check if overlapping
            if(DoParentAndChildrenCrossBorders())
                throw new ValidationFailedException(this,"Borders are overlapping with child container");
        }

        /// <summary>
        /// returns all containing items
        /// </summary>
        /// <returns>all baseobjects in the vicinity of the container</returns>
        protected virtual List<BaseObject> GetContainingItems()
        {
            //get containing items
            var items = ParentModel.ObjectList.Where(t =>
                    t.Locationtopleft.IsContainedIn(Locationtopleft, Locationtopright, Locationbottomleft,
                        Locationbottomright) ||
                    t.Locationtopright.IsContainedIn(Locationtopleft, Locationtopright, Locationbottomleft,
                        Locationbottomright) ||
                    t.Locationbottomleft.IsContainedIn(Locationtopleft, Locationtopright, Locationbottomleft,
                        Locationbottomright) ||
                    t.Locationbottomright.IsContainedIn(Locationtopleft, Locationtopright, Locationbottomleft,
                        Locationbottomright)
            ).ToList();

            //remove itself
            items.Remove(this);

            return items;
            //var neighbours = this.Visioshape.SpatialNeighbors((short)NetOffice.VisioApi.Enums.VisSpatialRelationCodes.visSpatialContain, 0, 0);
            //return neighbours.Select(item => this.ParentModel.ObjectList.Find(t => t.Uniquename == item.Name)).ToList();
        }

        /// <summary>
        /// gets the most parent container
        /// </summary>
        /// <returns>highest hierarchy container. null if no parent</returns>
        private Container GetTopContainer()
        {
            //check if model has correctly been initialized in advance
            Debug.Assert(ParentModel.ObjectsInitialized);

            var containers = ParentModel.ObjectList.Where(t => t is Container).Cast<Container>();
            return containers.Where(t => t.ChildContainers.Contains(this))
                .MaxBy(t => t._containingItems.Count(v => v is Container));
        }

        /// <summary>
        /// returns all child containers
        /// </summary>
        /// <returns></returns>
        private List<Container> GetAllChildContainers()
        {
            //check if model has correctly been initialized in advance
            Debug.Assert(ParentModel.ObjectsInitialized);

            return ContainingItems.Where(t => t is Container).Cast<Container>().ToList();
        }

        /// <summary>
        /// returns only the direct child containers
        /// </summary>
        /// <returns></returns>
        private List<Container> GetFirstLevelChildContainers()
        {
            //check if model has correctly been initialized in advance
            Debug.Assert(ParentModel.ObjectsInitialized);

            //get all childcontainers
            var children = new List<Container>();
            children.AddRange(ChildContainers);

            //get the child-child containers
            var childchildren = children.SelectMany(t => t.ChildContainers).Distinct();

            //remove child-child containers from childcontainers to only leave level 1 children behind
            childchildren.ForEach(t => children.Remove(t));

            return children;
        }

        /// <summary>
        /// gets the direct parent container
        /// </summary>
        /// <returns></returns>
        private Container GetParentContainer()
        {
            //check if model has correctly been initialized in advance
            Debug.Assert(ParentModel.ObjectsInitialized);

            //get top and iterate through all children
            var top = GetTopContainer();
            var children = top.ChildContainers;
            foreach (var child in children)
            {
                //if node found where first level child = this container, return
                var firstlevel = child.FirstLevelChildContainers;
                var potentialparent = firstlevel.FirstOrDefault(v => v == this);
                if (potentialparent != null)
                    return potentialparent;
            }

            //if none found, return null
            return null;
        }

        /// <summary>
        /// assuming containers are 90° rectangles and not rotated
        /// </summary>
        /// <returns>true if a border is crossed</returns>
        private bool DoParentAndChildrenCrossBorders()
        {
            //check if model has correctly been initialized in advance
            Debug.Assert(ParentModel.ObjectsInitialized);

            //get all containers to check
            List<Container> containerstocheck = new List<Container>();
            containerstocheck.AddRange(ChildContainers);

            //check every container if horizontal and vertical borders cross by checking that coordinates of childs are smaller than current container
            foreach (var cont in containerstocheck)
            {
                //topleft
                if((cont.Locationtopleft.X < this.Locationtopleft.X || (cont.Locationtopleft.Y > this.Locationtopleft.Y)))
                    return true;

                //topright
                if ((cont.Locationtopright.X > this.Locationtopright.X || (cont.Locationtopright.Y > this.Locationtopright.Y)))
                    return true;

                //bottomleft
                if ((cont.Locationbottomleft.X < this.Locationbottomleft.X || (cont.Locationbottomleft.Y < this.Locationbottomleft.Y)))
                    return true;

                //bottomright
                if ((cont.Locationbottomright.X > this.Locationbottomright.X || (cont.Locationbottomright.Y < this.Locationbottomright.Y)))
                    return true;
            }
            return false;
        }
    }
}
