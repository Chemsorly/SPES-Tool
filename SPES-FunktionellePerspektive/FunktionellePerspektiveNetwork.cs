using NetOffice.VisioApi;
using SPES_Modelverifier_Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPES_FunktionellePerspektive
{
    public class FunktionellePerspektiveNetwork : ModelNetwork
    {
        protected override List<string> ShapeTemplateFiles { get; }
        protected override Type MappingListType => null;

        public FunktionellePerspektiveNetwork(Application pApplication) : base(pApplication)
        {

        }
    }
}
