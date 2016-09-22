using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetOffice.VisioApi;
using SPES_Modelverifier_Base;

namespace ITU_Scenario
{
    class ScenarioModel : SPES_Modelverifier_Base.Models.Model
    {
        public ScenarioModel(Page pPage, MappingList pMapping) : base(pPage, pMapping)
        {
        }
    }
}
