using SPES_Modelverifier_Base;
using SPES_Zielmodell.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPES_Zielmodell.Models;

namespace SPES_Zielmodell
{
    class ZielmodellMapping : MappingList
    {
        protected override List<Type> TargetModels => new List<Type>() { typeof(ZielmodellModel) };

        protected override Dictionary<string, Type> Mapping => new Dictionary<string, Type>()
        {
            //GRL: items
            {"Actor", typeof(Actor) },
            {"Task", typeof(IntentionalObject) },
            {"Belief", typeof(IntentionalObject) },
            {"Goal", typeof(IntentionalObject) },
            {"Softgoal", typeof(IntentionalObject) },
            {"Indicator", typeof(IntentionalObject) },
            {"Resource", typeof(IntentionalObject) },

            //GRL: connections
            {"Decomposition", typeof(IntentionalObjectConnection) },
            {"Contribution Link", typeof(IntentionalObjectConnection) },
            {"Correlation Link", typeof(IntentionalObjectConnection) },
            {"Dependency", typeof(Dependency) },

            //GRL: containers
            {"Actor Boundary", typeof(ActorBoundary) }
        };
    }
}
