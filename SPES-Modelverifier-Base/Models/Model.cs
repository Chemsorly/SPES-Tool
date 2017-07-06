using MoreLinq;
using NetOffice.VisioApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using SPES_Modelverifier_Base.Items;
using SPES_Modelverifier_Base.ModelChecker;

namespace SPES_Modelverifier_Base.Models
{
    public abstract class Model
    {
        /// <summary>
        /// list of allowed items on the model. leave null to allow everything (e.g. when you only have 1 model and don't need to restrict types to models). 
        /// needs EXPLICIT XmlIgnore in derived class!
        /// </summary>
        [XmlIgnore]
        public abstract List<Type> AllowedItems { get; }

        /// <summary>
        /// defines a list of checkers which are supposed to run on the model
        /// needs EXPLICIT XmlIgnore in derived class!
        /// </summary>
        [XmlIgnore]
        public virtual List<Type> CheckersToRun => new List<Type>() { typeof(ModelChecker.Path.ValidPathChecker) };

        /// <summary>
        /// the list of all shapes on a sheet
        /// </summary>
        public List<BaseObject> ObjectList { get; set; }

        /// <summary>
        /// the page name
        /// </summary>
        public String PageName { get; set; }

        /// <summary>
        /// event to throw in case of validation exception
        /// </summary>
        public event ValidationFailedDelegate ValidationFailedEvent;

        /// <summary>
        /// set true if all objects on the model have been initialized
        /// </summary>
        [XmlIgnore]
        public bool ObjectsInitialized { get; set; }

        /// <summary>
        /// set true if all connections have been initialized
        /// </summary>
        [XmlIgnore]
        public bool ConnectionsInitialized { get; set; }

        /// <summary>
        /// constructor
        /// </summary>
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
            //var init
            this.PageName = pPage.Name;

            //generate and initialize objects
            this.ObjectList = GenerateObjects(this, pPage, pMapping);
            this.ObjectList.ForEach(t => t.Initialize());
            this.ObjectsInitialized = true;

            //set connections
            this.ObjectList.ForEach(t =>
            {
                var connection = t as Connection;
                if (connection != null)
                {
                    try
                    {
                        connection.SetConnections(ObjectList);
                    }
                    catch (ValidationFailedException ex)
                    {
                        ValidationFailedEvent?.Invoke(new ValidationFailedMessage(2, ex));
                    }
                }
            });
            this.ConnectionsInitialized = true;
        }

        /// <summary>
        /// validates basic intra model spezifications. Can be overwritten and extended by calling base.Validate()
        /// </summary>
        public virtual void Validate()
        {
            //check if sheet is not empty
            if (ObjectList.Count < 1)
            {
                ValidationFailedEvent?.Invoke(new ValidationFailedMessage(1, $"model {this.PageName} is empty"));
                return;
            }

            //check if elements are allowed on model
            if (AllowedItems != null)
                foreach (var element in ObjectList)
                    if (AllowedItems.All(t => t != element.GetType()) && element.GetType() != typeof(NRO))
                    {
                        ValidationFailedEvent?.Invoke(new ValidationFailedMessage(1, "element not allowed", element));
                    }

            //check if elements exist double on any sheet
            List<BaseObject> objects = ObjectList.Where(t => t is Item && !String.IsNullOrEmpty(t.Text) && !((t as Item).CanHaveDuplicateText)).ToList();
            foreach (var obj in objects)
                if (objects.Count(t => t.Text == obj.Text) > 1)
                {
                    ValidationFailedEvent?.Invoke(new ValidationFailedMessage(2, $"{this.PageName} contains elements with duplicate text", obj));
                }

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

            //run checkers if any specified
            foreach (var checkertype in CheckersToRun)
            {
                //check checker
                Debug.Assert(checkertype.IsSubclassOf(typeof(IModelChecker)));

                //create defined checker
                var checker = (IModelChecker)Activator.CreateInstance(checkertype);
                checker.ValidationFailedEvent += delegate (ValidationFailedMessage pMessage) { ValidationFailedEvent?.Invoke(pMessage); };

                //run initialize method
                checker.Initialize(this);
            }
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
