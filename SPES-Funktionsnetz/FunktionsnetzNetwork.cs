using SPES_Modelverifier_Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPES_Funktionsnetz
{
    public class FunktionsnetzNetwork : ModelNetwork
    {
        public FunktionsnetzNetwork(NetOffice.VisioApi.Application pApplication) : base(pApplication)
        {
        }

        public override Type MappingListType => typeof(FunktionsnetzMapping);

        public override List<string> ShapeTemplateFiles => new List<string>() { "FunctionalDesign_Function.vssx", "FunctionalDesign_Funktionsnetz.vssx" };
        
    }
}
