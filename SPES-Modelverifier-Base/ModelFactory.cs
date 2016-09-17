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

            //if no mapping was found, return null
            if (pair.Key == null)
                return null;

            //if mapping was found, create object based on type and return
            return Activator.CreateInstance(pair.Key) as BaseObject;
        }

        static String GetBaseNameFromUniquename(String pName)
        {
            return Regex.Replace(pName, @"(\.\d+)", "");
        }
    }
}
