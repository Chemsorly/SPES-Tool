using NetOffice.VisioApi;
using SPES_Modelverifier_Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPES_SzenarioUseCases
{
    public class SzenarioUseCasesNetwork : ModelNetwork
    {
        protected override List<string> ShapeTemplateFiles => new List<String> { "SMT_UCM.vssx" };

        protected override Type MappingListType => null;

        public SzenarioUseCasesNetwork(Application pApplication) : base(pApplication)
        {
        }
        public override string ToString()
        {
            return "Scenario-UseCase Maps";
        }
    }
}
