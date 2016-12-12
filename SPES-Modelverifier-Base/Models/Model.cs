using MoreLinq;
using NetOffice.VisioApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPES_Modelverifier_Base.Models
{
    public abstract class Model
    {
        /// <summary>
        /// list of allowed items on the model. leave null to allow everything
        /// </summary>
        public abstract List<Type> AllowedItems { get; }

        /// <summary>
        /// the list of all shapes on a sheet
        /// </summary>
        public List<BaseObject> ObjectList { get; private set; }

        /// <summary>
        /// the page name
        /// </summary>
        public String PageName { get; private set; }

        /// <summary>
        /// event to throw in case of validation exception
        /// </summary>
        public event ValidationFailedDelegate ValidationFailedEvent;

        /// <summary>
        /// signals if all objects on the model have been initialized
        /// </summary>
        public bool ObjectsInitialized { get; private set; }

        /// <summary>
        /// signals if all connections have been initialized
        /// </summary>
        public bool ConnectionsInitialized { get; private set; }

        public Model()
        {
            ObjectsInitialized = false;
            ConnectionsInitialized = false;
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="pPage">the visio page</param>
        /// <param name="pMapping">the mapping to create the objects</param>
        public void Initialize(Page pPage, MappingList pMapping)
        {
            this.PageName = pPage.Name;

            //generate objects
            this.ObjectList = GenerateObjects(this, pPage, pMapping);

            //initialize objects
            this.ObjectList.ForEach(t => t.Initialize());
        }

        /// <summary>
        /// validates basic intra model spezifications. Can be overwritten and extended by calling base.Validate()
        /// </summary>
        public virtual void Validate()
        {
            //check if sheet is not empty
            if (ObjectList.Count < 1)
                throw new Exception(this.PageName + " is an empty model.");

            //check if elements are allowed on model
            if (AllowedItems != null)
                foreach (var element in ObjectList)                
                    if (!AllowedItems.Any(t => t == element.GetType()) && element.GetType() != typeof(NRO))
                        ValidationFailedEvent?.Invoke(new ValidationFailedMessage(2, "element not allowed", element));

            //check if elements exist double on any sheet
            List<BaseObject> objects = ObjectList.Where(t => t is Item && !String.IsNullOrEmpty(t.text) && !((t as Item).CanHaveDuplicateText)).ToList();
            foreach(var obj in objects)
                if(objects.Count(t => t.text == obj.text) > 1)
                    ValidationFailedEvent?.Invoke(new ValidationFailedMessage(2, $"{this.PageName} contains elements with duplicate text", obj));

            //ONLY CHECK IF AT LEAST ONE OF THOSE OBJECTS EXIST; deadlock check here
            var startenditems = this.ObjectList.Where(t => t is StartEndItem);
            if (startenditems.Any())
            {
                //check if start item is unique; check if minimum one end item exists;
                if (startenditems.Count(t => (t as StartEndItem).IsStart) > 1)
                    ValidationFailedEvent?.Invoke(new ValidationFailedMessage(2, "Model contains more than one start item.", startenditems.First(t => (t as StartEndItem).IsStart)));
                if (startenditems.Count(t => !(t as StartEndItem).IsStart) == 0)
                    ValidationFailedEvent?.Invoke(new ValidationFailedMessage(2, "Model contains no enditems", startenditems.First()));
            }

            //set connections in the connector objects
            ObjectList.ForEach(t =>
            {
                if (t is Connection)
                {
                    try
                    {
                        (t as Connection).SetConnections(ObjectList);
                    }
                    catch(ValidationFailedException ex)
                    {
                        ValidationFailedEvent?.Invoke(new ValidationFailedMessage(2, ex));
                    }
                }
            });

            //do checks on objects, if implemented
            ObjectList.ForEach(t =>
            {
                try
                {
                    t.Validate();
                }
                catch(ValidationFailedException ex)
                {
                    ValidationFailedEvent?.Invoke(new ValidationFailedMessage(2, ex));
                }
            });
        }

        /// <summary>
        /// calculates a plausibility rating based on the amount of allowed items on the model
        /// </summary>
        /// <returns>amount of allowed items</returns>
        public int CalculateRating()
        {
            if (AllowedItems != null)
                return ObjectList.Count(t => AllowedItems.Exists(x => x == t.GetType()));
            else
                return ObjectList.Count;
        }

        private List<BaseObject> GenerateObjects(Model pParentmodel, Page pPage, MappingList pMapping)
        {
            List<BaseObject> ObjectList = new List<BaseObject>();

            //transform all shapes into model objects
            foreach (Shape shape in pPage.Shapes)
            {
                BaseObject modelObject = ModelFactory.GetInstanceFromShape(pParentmodel,shape, pMapping);
                if (modelObject != null)
                {
                    ObjectList.Add(modelObject);
                }
                else
                {
                    //exception of no matching model object type is found
                    ValidationFailedEvent?.Invoke(new ValidationFailedMessage(1, $"could not match shape {shape.Name}", null));
                }
            }

            return ObjectList;
        }

    }
}
