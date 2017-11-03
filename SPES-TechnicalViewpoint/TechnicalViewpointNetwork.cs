using NetOffice.VisioApi;
using SPES_Modelverifier_Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPES_TechnicalViewpoint
{
    public class TechnicalViewpointNetwork : ModelNetwork
    {

        protected override List<string> ShapeTemplateFiles => new List<String>() { "SMT_SM.vssx", "SMT_IA.vssx" };
        protected override Type MappingListType => null;
        public TechnicalViewpointNetwork(Application pApplication) : base(pApplication)
        {
        }
        public override string ToString()
        {
            return "Technical Design";
        }
    }
}
