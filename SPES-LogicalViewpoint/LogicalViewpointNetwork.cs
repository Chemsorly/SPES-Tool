using NetOffice.VisioApi;
using SPES_Modelverifier_Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPES_LogicalViewpoint
{
    public class LogicalViewpointNetwork : ModelNetwork
    {
        protected override List<string> ShapeTemplateFiles => new List<string>() {"SMT_Class.vssx"};
        protected override Type MappingListType => null;
        public LogicalViewpointNetwork(Application pApplication) : base(pApplication)
        {
        }
        public override string ToString()
        {
            return "Technical Design";
        }
    }
}
