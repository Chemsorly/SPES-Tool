using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITU_Scenario.Items
{
    public class FoundMessage : BaseLostFoundMessage
    {
        public override void Verify()
        {
            //todo: check if corresponding lost message exists

            base.Verify();
        }
    }
}
