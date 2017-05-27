using SPES_Modelverifier_Base;
using SPES_Zielmodell.Connectors;
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
            //GRL: items
            {"Actor", typeof(Actor) },
            {"Task", typeof(Task) },
            {"Belief", typeof(Belief) },
            {"Goal", typeof(Goal) },
            {"Softgoal", typeof(Goal) },
            {"Indicator", typeof(Indicator) },
            {"Resource", typeof(Resource) },

            //GRL: connections
            {"Decomposition", typeof(Decomposition) },
            {"Contribution Link", typeof(ContributionLink) },
            {"Correlation Link", typeof(CorrelationLink) },
            {"Dependency", typeof(Dependency) },

            //GRL: containers
            {"Actor Boundary", typeof(ActorBoundary) }
        };
    }
}
