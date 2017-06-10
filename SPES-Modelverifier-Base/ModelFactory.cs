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
    internal static class ModelFactory
    {
        public static BaseObject GetInstanceFromShape(Model pParentmodel, Shape pShape, MappingList pMappings)
        {
            //get type mapping for shape
            var pair = pMappings.Mapping.FirstOrDefault(t => t.Key == GetBaseNameFromUniquename(pShape.Name));

            //create object
            BaseObject modelObject;
            if (pair.Key != null)
                modelObject = Activator.CreateInstance(pair.Value) as BaseObject;
            else
                modelObject = new NRO();

            //fill base data
            if (modelObject != null)
            {
                modelObject.ParentModel = pParentmodel;
                modelObject.Type = GetBaseNameFromUniquename(pShape.Name);
                modelObject.Uniquename = pShape.Name;
                modelObject.Visiopage = pShape.ContainingPage.Name;
                modelObject.Visioshape = pShape;
                modelObject.Text = pShape.Text;
                modelObject.Locationx = pShape.Cells("PinX").Result(NetOffice.VisioApi.Enums.VisMeasurementSystem.visMSMetric);
                modelObject.Locationy = pShape.Cells("PinY").Result(NetOffice.VisioApi.Enums.VisMeasurementSystem.visMSMetric);
                modelObject.Width = pShape.Cells("Width").Result(NetOffice.VisioApi.Enums.VisMeasurementSystem.visMSMetric);
                modelObject.Height = pShape.Cells("Height").Result(NetOffice.VisioApi.Enums.VisMeasurementSystem.visMSMetric);
            }
            else
                throw new Exception("oops, something went wrong: could not create model object");

            return modelObject;
        }

        private static String GetBaseNameFromUniquename(String pName)
        {
            return Regex.Replace(pName, @"(\.\d+)", "");
        }
    }
}
