using NetOffice.VisioApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPES_Modelverifier_Base.Models
{
    abstract class Model
    {
        public List<BaseObject> ObjectList = new List<BaseObject>();
        public ModelFactory Factory { get; }
        public String PageName { get; }

        public Model(Page pPage, MappingList pMapping)
        {
            this.PageName = pPage.Name;
            this.Factory = new ModelFactory(pMapping);

            //transform all shapes into model objects
            foreach (Shape shape in pPage.Shapes)
            {
                BaseObject modelObject = Factory.GetInstanceFromShape(shape);
                if (modelObject != null)
                {
                    this.ObjectList.Add(modelObject);
                }
                else
                {
                    //exception of no matching model object type is found
                    throw new Exception($"Could not match shape: \"{shape.Name}\"");
                }
            }
        }

        public virtual void Validate()
        {
            //check if sheet is not empty
            if (ObjectList.Count < 1)
                throw new Exception(this.PageName + " is an empty model.");

            //check if elements exist double on any sheet
            List<String> duplicates = ObjectList.Where(t => !String.IsNullOrEmpty(t.text)).Select(t => t.text)
                .GroupBy(x => x)
                .Where(group => group.Count() > 1)
                .Select(group => group.Key)
                .ToList();
            if (duplicates.Any())
            {
                String dups = String.Empty;
                duplicates.ForEach(t => dups += t + "; ");
                throw new Exception(this.PageName + " contains elements with duplicate text: " + dups);
            }

            //set connections in the connector objects
            ObjectList.ForEach(t =>
            {
                if (t is Connection)
                    (t as Connection).SetConnections(ObjectList);
            });


            //check model type

            //check if evry function, external function and dependency has at minimum 1 connector added

        }
    }
}
