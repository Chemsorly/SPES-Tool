using NetOffice.VisioApi;
using SPES_Modelverifier_Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPES_Wissenskontext
{
    public class WissenskontextNetwork : ModelNetwork
    {
        protected override List<string> ShapeTemplateFiles => new List<String> { "SMT_CoK.vssx" };

        protected override Type MappingListType => null;

        public WissenskontextNetwork(Application pApplication) : base(pApplication)
        {
        }
    }
}
