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
        public List<BaseObject> ObjectList = new List<BaseObject>();

        /// <summary>
        /// the page name
        /// </summary>
        public String PageName { get; }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="pPage">the visio page</param>
        /// <param name="pMapping">the mapping to create the objects</param>
        public Model(Page pPage, MappingList pMapping)
        {
            this.PageName = pPage.Name;
            this.ObjectList = GenerateObjects(pPage, pMapping);
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
                        throw new ValidationFailedException(element, "Object is not allowed on the model.");


            //check if elements exist double on any sheet
            List<BaseObject> duplicates = ObjectList.Where(t => !String.IsNullOrEmpty(t.text)).Select(t => t)
                .GroupBy(x => x)
                .Where(group => group.Count() > 1)
                .Select(group => group.Key)
                .ToList();
            if (duplicates.Any())
                throw new ValidationFailedException(duplicates.First(), this.PageName + " contains elements with duplicate text" );

            //set connections in the connector objects
            ObjectList.ForEach(t =>
            {
                if (t is Connection)
                    (t as Connection).SetConnections(ObjectList);
            });


            //check model type
            //throw new NotImplementedException();

            //check if evry function, external function and dependency has at minimum 1 connector added
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

        static List<BaseObject> GenerateObjects(Page pPage, MappingList pMapping)
        {
            List<BaseObject> ObjectList = new List<BaseObject>();

            //transform all shapes into model objects
            foreach (Shape shape in pPage.Shapes)
            {
                BaseObject modelObject = ModelFactory.GetInstanceFromShape(shape, pMapping);
                if (modelObject != null)
                {
                    ObjectList.Add(modelObject);
                }
                else
                {
                    //exception of no matching model object type is found
                    throw new Exception($"Could not match shape: \"{shape.Name}\"");
                }
            }

            return ObjectList;
        }

    }
}
