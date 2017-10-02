using NetOffice.VisioApi;
using SPES_Modelverifier_Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPES_StrukturellerKontext
{
    public class StrukturellerKontextNetwork : ModelNetwork
    {
        protected override List<string> ShapeTemplateFiles => new List<String> { "SMT_SoC.vssx", "SMT_BeC.vssx" };

        protected override Type MappingListType => null;

        public StrukturellerKontextNetwork(Application pApplication) : base(pApplication)
        {
        }
    }
}
