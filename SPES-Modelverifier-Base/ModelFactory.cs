using NetOffice.VisioApi;
using SPES_Modelverifier_Base.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SPES_Modelverifier_Base
{
    class ModelFactory
    {
        MappingList Mappings { get; }

        internal ModelFactory(MappingList pMappings)
        {
            Mappings = pMappings;
        } 

        public BaseObject GetInstanceFromShape(Shape pShape)
        {
            //get type mapping for shape
            var pair = Mappings.Mapping.FirstOrDefault(t => t.Value == GetBaseNameFromUniquename(pShape.Name));

            //create object
            BaseObject modelObject;
            if (pair.Key != null)
                modelObject = Activator.CreateInstance(pair.Key) as BaseObject;
            else
                modelObject = new NRO();

            //fill base data
            if (modelObject != null)
            {
                modelObject.uniquename = pShape.Name;
                modelObject.visiopage = pShape.ContainingPage.Name;
                modelObject.visioshape = pShape;
                modelObject.text = pShape.Text;
                modelObject.locationx = pShape.Cells("PinX").Result(NetOffice.VisioApi.Enums.VisMeasurementSystem.visMSMetric);
                modelObject.locationy = pShape.Cells("PinY").Result(NetOffice.VisioApi.Enums.VisMeasurementSystem.visMSMetric);
                modelObject.width = pShape.Cells("Width").Result(NetOffice.VisioApi.Enums.VisMeasurementSystem.visMSMetric);
                modelObject.height = pShape.Cells("Height").Result(NetOffice.VisioApi.Enums.VisMeasurementSystem.visMSMetric);
            }
            else
                throw new Exception("oops, something went wrong: could not create model object");

            return modelObject;
        }

        static String GetBaseNameFromUniquename(String pName)
        {
            return Regex.Replace(pName, @"(\.\d+)", "");
        }
    }
}
