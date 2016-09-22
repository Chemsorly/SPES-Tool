using SPES_Modelverifier_Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPES_Modelverifier_Base.Models;

namespace ITU_Scenario
{
    public class ScenarioMapping : MappingList
    {
        public override Dictionary<Type, string> Mapping => new Dictionary<Type, string>()
        {

        };

        public override Type TargetModel => typeof(ScenarioModel);
    }
}
