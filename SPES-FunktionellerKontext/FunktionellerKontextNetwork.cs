using NetOffice.VisioApi;
using SPES_Modelverifier_Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPES_FunktionellerKontext
{
    public class FunktionellerKontextNetwork : ModelNetwork
    {
        protected override List<string> ShapeTemplateFiles => new List<String> { "SMT_FuC.vssx", "SMT_SoC.vssx" };

        protected override Type MappingListType => null;

        public FunktionellerKontextNetwork(Application pApplication) : base(pApplication)
        {
        }

        public override string ModelName => "Functional Context";

        //public override string ToString()
        //{
        //    return "Functional Context";
        //}
    }
}
