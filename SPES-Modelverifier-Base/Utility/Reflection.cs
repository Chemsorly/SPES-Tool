using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SPES_Modelverifier_Base.Items;

namespace SPES_Modelverifier_Base.Utility
{
    public class Reflection
    {
        public static List<Type> GetAllModelreferenceTypesInModule(Type pType)
        {
            return Assembly.GetAssembly(pType).GetTypes().Where(t =>
                    t.IsClass &&
                    !t.GetTypeInfo().IsDefined(typeof(CompilerGeneratedAttribute)) &&
                    (t.Namespace.EndsWith("Items")) &&
                    t.IsSubclassOf(typeof(ModelReference)))
                .ToList();
        }
    }
}
