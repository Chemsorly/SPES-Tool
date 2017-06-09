using SPES_Modelverifier_Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPES_Zielmodell
{
    public class ZielmodellNetwork : ModelNetwork
    {
        public ZielmodellNetwork(NetOffice.VisioApi.Application pApplication) : base(pApplication)
        {
        }

        protected override List<string> ShapeTemplateFiles => new List<String> { "GRL.vssx" };

        protected override Type MappingListType => typeof(ZielmodellMapping);

        public override List<ValidationFailedMessage> Validate()
        {
            //step 1-3: meta-model
            base.Validate();

            //step 4

            //return
            return CollectedValidationMessages;
        }
    }
}
