using SPES_Modelverifier_Base;
using SPES_Zielmodell.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPES_Zielmodell
{
    class ZielmodellMapping : MappingList
    {
        protected override List<Type> TargetModels => new List<Type>() { typeof(ZielmodellModel) };

        protected override Dictionary<string, Type> Mapping => new Dictionary<string, Type>()
        {
            //GRL
            {"Actor", typeof(Actor) },
            {"Task", typeof(Task) },
            {"Belief", typeof(Belief) },
            {"Goal", typeof(Goal) },
            {"Softgoal", typeof(Goal) }
        };
    }
}
