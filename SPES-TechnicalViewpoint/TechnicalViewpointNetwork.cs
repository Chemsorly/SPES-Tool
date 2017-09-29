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
        public TechnicalViewpointNetwork(Application pApplication) : base(pApplication)
        {
        }

        protected override List<string> ShapeTemplateFiles { get; }
        protected override Type MappingListType { get; }
    }
}
