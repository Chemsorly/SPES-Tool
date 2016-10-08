using NetOffice.VisioApi;
using SPES_Modelverifier_Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITU_Scenario
{
    public class ScenarioNetwork : ModelNetwork
    {
        public override List<string> ShapeTemplateFiles => new List<String> { "Basic MSC with Inline Expressions.vsx", "HMSC.vsx" };
        public override Type MappingListType => typeof(ScenarioMapping);

        public ScenarioNetwork(Application pApplication) : base(pApplication)
        {
            
        }
                
    }
}
